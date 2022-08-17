using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace FFXIV_ACT_ViewUnlocker {
	[DataContract]
	class RemoteInfo {
		[DataMember(Order = 0)]
		public string GameVersion;
		[DataMember(Order = 1)]
		public int Offset;
	}
	[DataContract]
	class VUPropertys {
		public static VUPropertys instance;
		[DataMember(Order = 0)]
		public string ViewUnlockerVersion = "1.1";
		[DataMember(Order = 1)]
		public string GameVersion = "";
		[DataMember(Order = 2)]
		public int ViewStructurePointerOffset = 0;
		[DataMember(Order = 3)]
		public float Zoom = 20;
		[DataMember(Order = 4)]
		public float Fov = 0.78f;
		[DataMember(Order = 5)]
		public bool autoCheckRemoteOffset = false;
		[DataMember(Order = 6)]
		public bool uploadOffset = false;

		public static void ReadConfig(string path) {
			try {
				var serializer = new DataContractJsonSerializer(typeof(VUPropertys));
				var stream = new StreamReader(path);
				instance = serializer.ReadObject(stream.BaseStream) as VUPropertys;
				stream.Close();
			} catch {
				instance = new VUPropertys();
			}
		}

		public static void WriteConfig(string path) {
			if (!File.Exists(path))
				File.Create(path).Close();
			var serializer = new DataContractJsonSerializer(typeof(VUPropertys));
			var stream = new StreamWriter(path);
			serializer.WriteObject(stream.BaseStream, instance);
			//File.WriteAllText(path, "");
		}

	}
	static class Utilitys {

		public static class WinAPI {
			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern bool ReadProcessMemory(
				IntPtr hProcess,
				IntPtr lpBaseAddress,
				[Out] byte[] lpBuffer,
				int dwSize,
				IntPtr lpNumberOfBytesRead);

			[DllImport("kernel32.dll")]
			public static extern bool WriteProcessMemory(
				 IntPtr hProcess,
				 IntPtr lpBaseAddress,
				 byte[] lpBuffer,
				 Int32 nSize,
				 IntPtr lpNumberOfBytesWritten);

			public static T ReadMemory<T>(IntPtr hProcess, IntPtr lpBaseAddress) {
				int size = Marshal.SizeOf(typeof(T));
				byte[] buffer = new byte[size];
				ReadProcessMemory(hProcess, lpBaseAddress, buffer, size, IntPtr.Zero);
				GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
				T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
				handle.Free();
				return theStructure;
			}

			public static void WriteMemory<T>(IntPtr hProcess, IntPtr lpBaseAddress, T theStructure) {
				int size = Marshal.SizeOf(typeof(T));
				byte[] buffer = new byte[size];
				IntPtr ptr = Marshal.AllocHGlobal(size);
				Marshal.StructureToPtr(theStructure, ptr, true);
				Marshal.Copy(ptr, buffer, 0, size);
				Marshal.FreeHGlobal(ptr);
				WriteProcessMemory(hProcess, lpBaseAddress, buffer, size, IntPtr.Zero);
			}

		}

		public static string GetLocalGameVersion(Process gameProcess) {
			var path = gameProcess.MainModule.FileName;
			path = path.Substring(0, path.LastIndexOf("\\")) + "\\ffxivgame.ver";
			string gameVersion = "";
			if (File.Exists(path)) {
				using (StreamReader sr = new StreamReader(path))
					if ((gameVersion = sr.ReadLine()) == null)
						return null;
			}
			return gameVersion;
		}



		public static long KMP(byte[] src, byte[] pattern) {
			long[] next = new long[pattern.Length];
			next[0] = -1;

			long i = 0, j = -1;
			while (i < pattern.Length - 1) {
				if (j == -1 || pattern[i] == pattern[j])
					next[++i] = ++j;
				else
					j = next[j];
			}

			i = 0;
			j = 0;

			while (i < src.Length && j < pattern.Length) {
				if (j == -1 || src[i] == pattern[j]) {
					i++;
					j++;
				} else
					j = next[j];
			}
			return j == pattern.Length ? i - j : -1;
		}


		public static class MemoryScaner {
			[DllImport("kernel32.dll", SetLastError = true)]
			static extern bool ReadProcessMemory(
				IntPtr hProcess,
				IntPtr lpBaseAddress,
				[Out] byte[] lpBuffer,
				int dwSize,
				IntPtr lpNumberOfBytesRead);

			[DllImport("kernel32.dll")]
			static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

			[DllImport("kernel32.dll", SetLastError = true)]
			static extern int VirtualQueryEx(IntPtr hProcess,
			IntPtr lpAddress, out MEMORY_BASIC_INFORMATION64 lpBuffer, uint dwLength);
			public struct SYSTEM_INFO {
				public ushort processorArchitecture;
				ushort reserved;
				public uint pageSize;
				public IntPtr minimumApplicationAddress;
				public IntPtr maximumApplicationAddress;
				public IntPtr activeProcessorMask;
				public uint numberOfProcessors;
				public uint processorType;
				public uint allocationGranularity;
				public ushort processorLevel;
				public ushort processorRevision;
			}

			public struct MEMORY_BASIC_INFORMATION64 {
				public ulong BaseAddress;
				public ulong AllocationBase;
				public int AllocationProtect;
				public int __alignment1;
				public ulong RegionSize;
				public int State;
				public int Protect;
				public int Type;
				public int __alignment2;
			}

			const long MEM_COMMIT = 0x00001000;
			const long PAGE_READWRITE = 0x04;

			public static IntPtr Search(Process process, byte[] pattern, IProgress<float> progress = null) {
				if (process == null || process.HasExited)
					return IntPtr.Zero;

				DateTime tStart = DateTime.Now;

				SYSTEM_INFO sys_info = new SYSTEM_INFO();
				GetSystemInfo(out sys_info);
				IntPtr proc_min_address = sys_info.minimumApplicationAddress;
				IntPtr proc_max_address = sys_info.maximumApplicationAddress;

				ulong min_addr = (ulong)proc_min_address;
				ulong max_addr = (ulong)proc_max_address;

				MEMORY_BASIC_INFORMATION64 mem_basic_info = new MEMORY_BASIC_INFORMATION64();

				IntPtr bytesRead = IntPtr.Zero;

				progress?.Report(0);
				ulong scannedSize = 0;
				float lastProgess = 0f;
				for (var cur_addr = min_addr; cur_addr < max_addr; cur_addr += mem_basic_info.RegionSize) {
					VirtualQueryEx(process.Handle, (IntPtr)cur_addr, out mem_basic_info,
						(uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION64)));
					if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT) {
						scannedSize += mem_basic_info.RegionSize;

						byte[] buffer = new byte[mem_basic_info.RegionSize];
						ReadProcessMemory(process.Handle,
						(IntPtr)mem_basic_info.BaseAddress, buffer, (int)mem_basic_info.RegionSize, bytesRead);

						var prog = (float)scannedSize / process.PagedMemorySize64;
						if (prog - lastProgess >= 0.01f) {
							progress?.Report(prog);
							lastProgess = prog;
						}

						var res = KMP(buffer, pattern);
						if (res != -1) {
							progress?.Report(1);
							return (IntPtr)(mem_basic_info.BaseAddress + (ulong)res);
						}
					}
				}
				progress?.Report(1);
				return IntPtr.Zero;
			}
		}
	}
}
