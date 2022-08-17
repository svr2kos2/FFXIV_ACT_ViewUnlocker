using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Advanced_Combat_Tracker;

namespace FFXIV_ACT_ViewUnlocker {

	internal class ACT_View_Unlocker : IActPluginV1 {
		public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText) {
			pluginScreenSpace.Text = "视角解锁";
			View_Unlocker.mainpage = new MainPage();
			pluginScreenSpace.Controls.Add(View_Unlocker.mainpage);
			//statusLabel = pluginStatusText;
			ActPluginData actPluginData = ActGlobals.oFormActMain.PluginGetSelfData(this);
			View_Unlocker.instance = new View_Unlocker();
			View_Unlocker.instance.currentPath = actPluginData.pluginFile.DirectoryName;
			View_Unlocker.instance.VUInit();
		}
		public void DeInitPlugin() {
			View_Unlocker.instance.VUDeinit();
		}
	}

	public class View_Unlocker {
		public static View_Unlocker instance;

		public View_Unlocker() {
			instance = this;
		}

		string status = "init";
		public string currentPath = Path.GetTempPath();
		Process ffxivProcess;

		byte[] viewStructurePattern = new byte[] { 0x00, 0x00, 0xA0, 0x41,
									  0x00, 0x00, 0xC0, 0x3F,
									  0x00, 0x00, 0xA0, 0x41,
									  0x14, 0xAE, 0x47, 0x3F,
									  0xD7, 0xA3, 0x30, 0x3F,
									  0x14, 0xAE, 0x47, 0x3F };

		public static class ViewAddr {
			public static IntPtr viewStructureAddr = IntPtr.Zero;
			public static IntPtr currentZoom {
				get {
					return IntPtr.Add(viewStructureAddr, 0X114);
				}
			}
			public static IntPtr minZoom {
				get {
					return IntPtr.Add(viewStructureAddr, 0x118);
				}
			}
			public static IntPtr maxZoom {
				get {
					return IntPtr.Add(viewStructureAddr, 0x11C);
				}
			}
			public static IntPtr currentFov {
				get {
					return IntPtr.Add(viewStructureAddr, 0x120);
				}
			}
			public static IntPtr minFov {
				get {
					return IntPtr.Add(viewStructureAddr, 0x124);
				}
			}
			public static IntPtr maxFov {
				get {
					return IntPtr.Add(viewStructureAddr, 0x128);
				}
			}
		}

		public void SetFov(float fov) {
			try {
				Utilitys.WinAPI.WriteMemory(ffxivProcess.Handle, ViewAddr.maxFov, fov);
				Utilitys.WinAPI.WriteMemory(ffxivProcess.Handle, ViewAddr.currentFov, fov);
				VUPropertys.instance.Fov = fov;
			} catch (Exception e) {

			}
		}

		public void SetZoom(float zoom) {
			try {
				Utilitys.WinAPI.WriteMemory(ffxivProcess.Handle, ViewAddr.maxZoom, zoom);
				Utilitys.WinAPI.WriteMemory(ffxivProcess.Handle, ViewAddr.currentZoom, zoom);
				VUPropertys.instance.Zoom = zoom;
			} catch (Exception e) {

			}
		}

		public bool SetOffset(int offset) {
			if (status == "Working")
				return true;
			try {
				ViewAddr.viewStructureAddr = new IntPtr(Utilitys.WinAPI.ReadMemory<long>(ffxivProcess.Handle,
					ffxivProcess.MainModule.BaseAddress + offset));

				if (Utilitys.WinAPI.ReadMemory<float>(ffxivProcess.Handle, ViewAddr.minZoom) != 1.50f)
					throw new Exception("错误的Offset, 请更新Offset");
				if (Utilitys.WinAPI.ReadMemory<float>(ffxivProcess.Handle, ViewAddr.minFov) != 0.69f)
					throw new Exception("错误的Offset, 请更新Offset");

				mainpage.SetFov(VUPropertys.instance.Fov);
				mainpage.SetZoom(VUPropertys.instance.Zoom);
			} catch (Exception e) {
				status = e.Message;
				return false;
			}
			status = "Working";
			mainpage.SetStatus("Working");
			mainpage.SetOffsetInputfieldActive(false);
			return true;
		}



		public void VUInit() {
			try {
				VUPropertys.ReadConfig(currentPath + "/ViewUnlocker.property");
				Task.Run(() => {
					for (; ; Task.Delay(3000).Wait()) {
						ffxivProcess = Process.GetProcessesByName("ffxiv_dx11").FirstOrDefault();
						if (ffxivProcess != null)
							break;
					}
				}).ContinueWith((t) => {
					mainpage.SetLocalGameVersion(Utilitys.GetLocalGameVersion(ffxivProcess));
					mainpage.SetOffsetInputfield(VUPropertys.instance.ViewStructurePointerOffset);
					if (VUPropertys.instance.autoCheckRemoteOffset)
						PullOffsetFromServer();
				});
			} catch (Exception e) {
				MessageBox.Show(e.Message);
			}

		}

		public void VUDeinit() {
			VUPropertys.WriteConfig(currentPath + "/ViewUnlocker.property");
		}

		public void PullOffsetFromServer() {
			if (ffxivProcess == null)
				return;

			var gameVersion = Utilitys.GetLocalGameVersion(ffxivProcess);
			WebClient client = new WebClient();
			Task.Run(() => {
				try {
					mainpage.SetPullDataButtonActive(false);
					mainpage.SetRemoteInfo("正在获取数据...");
					return client.DownloadData("https://svr.moe/API/ViewUnlocker/GetOffset?version=" + gameVersion);
				} catch (Exception e) {
					return null;
				}

			}).ContinueWith((t) => {

				RemoteInfo info = new RemoteInfo();
				try {
					if (t.Result == null)
						throw new Exception("Failed to pull offset from server");
					var data = t.Result;
					var json = Encoding.UTF8.GetString(data);
					var serializer = new DataContractJsonSerializer(typeof(RemoteInfo));
					var mStream = new MemoryStream(Encoding.Default.GetBytes(json));
					info = serializer.ReadObject(mStream) as RemoteInfo;
					if (info.GameVersion != gameVersion)
						throw new Exception("GameVersion is not match");
				} catch {
					(info.GameVersion, info.Offset) = PullOffsetFromIPv6Record();
				}

				string remoteInfo = "Offset:" + info.Offset.ToString("X8") + "\n游戏版本:\n" + info.GameVersion;

				if (info.GameVersion == gameVersion) {
					mainpage.SetOffsetInputfield(info.Offset);
					VUPropertys.instance.ViewStructurePointerOffset = info.Offset;
					VUPropertys.instance.GameVersion = gameVersion;
				} else {
					remoteInfo += "\n看起来远程数据还没更新哦,\n尝试一下本地搜索吧";
				}
				mainpage.SetRemoteInfo(remoteInfo);
				mainpage.SetPullDataButtonActive(true);
			});
		}

		private (string, int) PullOffsetFromIPv6Record() {
			if (ffxivProcess == null)
				return (null, 0);

			var gameVersion = Utilitys.GetLocalGameVersion(ffxivProcess);

			var host = "ffxiv_vu.svr.moe";
			string remoteVersion = "";
			ulong remoteOffset = 0;
			var data = new List<byte>(Dns.GetHostAddresses(host).FirstOrDefault().MapToIPv6().GetAddressBytes());
			foreach (var b in data.GetRange(0, 8))
				remoteVersion += b.ToString("X2");
			remoteOffset = BitConverter.ToUInt64(data.GetRange(8, 8).ToArray(), 0);

			int[] dotIndexes = { 12, 8, 6, 4 };
			foreach (var i in dotIndexes)
				remoteVersion = remoteVersion.Insert(i, ".");

			return (remoteVersion, (int)remoteOffset);
		}

		public void ScanOffset() {
			if (ffxivProcess == null)
				return;
			var prog = new Progress<float>((t) => { mainpage.SetProgress((int)(Math.Min(1f, Math.Max(0f, t)) * 100)); });

			Task.Run(() => {
				return Utilitys.MemoryScaner.Search(ffxivProcess, viewStructurePattern, prog);
			}).ContinueWith((t) => {
				var addr = t.Result;
				if (addr == IntPtr.Zero) {
					MessageBox.Show("没有找到Offset");
					return;
				}
				var pointer = BitConverter.GetBytes((addr - 0X114).ToInt64());
				byte[] moduleData = new byte[ffxivProcess.MainModule.ModuleMemorySize];
				if (!Utilitys.WinAPI.ReadProcessMemory(ffxivProcess.Handle, ffxivProcess.MainModule.BaseAddress,
					moduleData, ffxivProcess.MainModule.ModuleMemorySize, IntPtr.Zero))
					throw new Exception("ReadProcessMemory failed.");

				for (int i = 0; i < moduleData.Length; ++i) {
					for (int j = 0; i + j < moduleData.Length; ++j) {
						if (j == pointer.Length) {
							mainpage.SetOffsetInputfield(i);
							return;
						}
						if (pointer[j] != 0x2e && moduleData[i + j] != pointer[j])
							break;
					}
				}
			});
		}

		public static MainPage mainpage;
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			var form = new Form();
			mainpage = new MainPage();
			form.Controls.Add(mainpage);
			form.Size = mainpage.Size + new Size(20, 40);
			var vu = new View_Unlocker();
			vu.currentPath = Environment.CurrentDirectory;
			vu.VUInit();
			Application.Run(form);
			vu.VUDeinit();
		}
	}
}
