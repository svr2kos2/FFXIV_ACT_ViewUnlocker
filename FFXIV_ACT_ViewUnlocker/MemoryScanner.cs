using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FFXIV_ACT_ViewUnlocker
{
	class MemoryScanner
	{
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
		public struct SYSTEM_INFO
		{
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

		public struct MEMORY_BASIC_INFORMATION64
		{
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

		public static IntPtr Search(Process process, byte[] pattern)
		{
			if(process == null || process.HasExited)
				return IntPtr.Zero;
			SYSTEM_INFO sys_info = new SYSTEM_INFO();
			GetSystemInfo(out sys_info);
			IntPtr proc_min_address = sys_info.minimumApplicationAddress;
			IntPtr proc_max_address = sys_info.maximumApplicationAddress;

			ulong proc_min_address_l = (ulong)proc_min_address;
			ulong proc_max_address_l = (ulong)proc_max_address;

			MEMORY_BASIC_INFORMATION64 mem_basic_info = new MEMORY_BASIC_INFORMATION64();

			IntPtr bytesRead = IntPtr.Zero;

			while (proc_min_address_l < proc_max_address_l)
			{
				VirtualQueryEx(process.Handle, proc_min_address, out mem_basic_info, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION64)));
				if (mem_basic_info.Protect ==
				PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
				{
					byte[] buffer = new byte[mem_basic_info.RegionSize];
					ReadProcessMemory(process.Handle,
					(IntPtr)mem_basic_info.BaseAddress, buffer, (int)mem_basic_info.RegionSize, bytesRead);

					for (ulong i = 0; i < mem_basic_info.RegionSize; i++)
					{
						ulong addr = mem_basic_info.BaseAddress + i;
						byte b = buffer[i];
						for (ulong j = 0; i + j < mem_basic_info.RegionSize; ++j)
						{
							if (j == (ulong)pattern.LongLength)
							{
								return (IntPtr)addr;
							}
							if (buffer[i + j] != pattern[j])
								break;
						}
					}
				}
				proc_min_address_l += mem_basic_info.RegionSize;
				proc_min_address = (IntPtr)proc_min_address_l;
			}
			return IntPtr.Zero;
		}
	}
}
