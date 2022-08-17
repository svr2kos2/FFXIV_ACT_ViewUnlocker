using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Windows.Forms;

namespace FFXIV_ACT_ViewUnlocker {
	[DataContract]
	class VUPropertys {
		public static VUPropertys instance;
		[DataMember(Order = 0)]
		public float Zoom = 20;
		[DataMember(Order = 1)]
		public float Fov = 0.78f;

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
			stream.Close();
		}
	}
	static class Utilitys {
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


		public static int KMP(byte[] src, byte[] pattern) {
			int[] next = new int[pattern.Length];
			next[0] = -1;

			int i = 0, j = -1;
			while (i < pattern.Length - 1) {
				if (j == -1 || pattern[i] == pattern[j] || pattern[i] == 0x2e)
					next[++i] = ++j;
				else
					j = next[j];
			}
			i = 0;
			j = 0;
			while (i < src.Length && j < pattern.Length) {
				if (j == -1 || src[i] == pattern[j] || pattern[j] == 0x2e) {
					i++;
					j++;
				} else
					j = next[j];
			}
			return j == pattern.Length ? i - j : -1;
		}
	}
}
