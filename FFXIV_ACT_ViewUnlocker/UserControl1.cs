using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Advanced_Combat_Tracker;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace FFXIV_ACT_ViewUnlocker
{
    public partial class ViewUnlocker : UserControl, IActPluginV1
    {
        public ViewUnlocker ()
        {
            InitializeComponent();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(
             IntPtr hProcess,
             IntPtr lpBaseAddress,
             byte[] lpBuffer,
             Int32 nSize,
             IntPtr lpNumberOfBytesWritten);


		Label statusLabel = null;
		TabPage screenSpace = null;
		Process process = null;
		IntPtr baseAddress = IntPtr.Zero;

		byte[] pattern = new byte[] { 0x00, 0x00, 0xA0, 0x41, 
									  0x00, 0x00, 0xC0, 0x3F, 
									  0x00, 0x00, 0xA0, 0x41, 
									  0x14, 0xAE, 0x47, 0x3F, 
									  0xD7, 0xA3, 0x30, 0x3F, 
									  0x14, 0xAE, 0x47, 0x3F };

		string offsetVersion = "";
		int currentZoomOffset = 0X114;
		int minZoomOffset = 0x118;
		int maxZoomOffset = 0x11C;
		int currentFovOffset = 0x120;
		int nearFovOffset = 0x124;
		int farFovOffset = 0x128;

		void SyncConfig(bool write = false)
        {
			ActPluginData actPluginData = ActGlobals.oFormActMain.PluginGetSelfData(this);
			
			var filePath = actPluginData.pluginFile.DirectoryName;
			filePath = filePath + "\\view_unlocker.cfg";
			if (write == false && File.Exists(filePath))
			{
				using (StreamReader sr = new StreamReader(filePath))
				{
					string line;
					if ((line = sr.ReadLine()) != null)
						offsetVersion = line;
					if ((line = sr.ReadLine()) != null)
						offset.Text = line;
					if ((line = sr.ReadLine()) != null)
						zoom.Text = line;
					if ((line = sr.ReadLine()) != null)
						fov.Text = line;
				}
			}
			else
			{
				using (StreamWriter sw = new StreamWriter(filePath))
				{
					sw.WriteLine(offsetVersion);
					sw.WriteLine(offset.Text);
					sw.WriteLine(zoom.Text);
					sw.WriteLine(fov.Text);
				}
			}
		}

		public void SetFov(float fov)
		{
			try
			{
				if (!WriteProcessMemory(process.Handle, IntPtr.Add(baseAddress, farFovOffset), BitConverter.GetBytes(fov), 4, IntPtr.Zero))
					throw new Exception("WriteProcessMemory failed.");
				if (!WriteProcessMemory(process.Handle, IntPtr.Add(baseAddress, currentFovOffset), BitConverter.GetBytes(fov), 4, IntPtr.Zero))
					throw new Exception("WriteProcessMemory failed.");
			}
			catch (Exception e)
			{
				if (statusLabel != null)
					statusLabel.Text = e.Message;
			}
		}

		public void SetZoom(float zoom)
		{
			try
			{
				if (!WriteProcessMemory(process.Handle, IntPtr.Add(baseAddress, maxZoomOffset), BitConverter.GetBytes(zoom), 4, IntPtr.Zero))
					throw new Exception("WriteProcessMemory failed.");
				if (!WriteProcessMemory(process.Handle, IntPtr.Add(baseAddress, currentZoomOffset), BitConverter.GetBytes(zoom), 4, IntPtr.Zero))
					throw new Exception("WriteProcessMemory failed.");
			}
			catch (Exception e)
			{
				if (statusLabel != null)
					statusLabel.Text = e.Message;
			}
		}

		public void Init()
		{
			try
			{
				process = Process.GetProcessesByName("ffxiv_dx11").FirstOrDefault();
				if (process == null)
					throw new Exception("You need to start FFXIV(DX11) to initialize this plugin.");
				byte[] viewPointer = new byte[8];
				if (!ReadProcessMemory(process.Handle, IntPtr.Add(process.MainModule.BaseAddress, int.Parse(offset.Text, System.Globalization.NumberStyles.HexNumber)), viewPointer, 8, IntPtr.Zero))
					throw new Exception("ReadProcessMemory failed.");
				baseAddress = new IntPtr(BitConverter.ToInt64(viewPointer, 0));

				byte[] currentByte = new byte[4];
				if (!ReadProcessMemory(process.Handle, IntPtr.Add(baseAddress, minZoomOffset), currentByte, 4, IntPtr.Zero))
					throw new Exception("错误的Offset, 请在插件页面更新Offset");
				if (BitConverter.ToSingle(currentByte, 0) != 1.50f)
					throw new Exception("错误的Offset, 请在插件页面更新Offset");
				if (!ReadProcessMemory(process.Handle, IntPtr.Add(baseAddress, nearFovOffset), currentByte, 4, IntPtr.Zero))
					throw new Exception("错误的Offset, 请在插件页面更新Offset");
				if (BitConverter.ToSingle(currentByte, 0) != 0.69f)
					throw new Exception("错误的Offset, 请在插件页面更新Offset");


				if (!WriteProcessMemory(process.Handle, IntPtr.Add(baseAddress, currentZoomOffset), BitConverter.GetBytes(float.Parse(zoom.Text)), 4, IntPtr.Zero))
					throw new Exception("WriteProcessMemory failed.");
				if (!WriteProcessMemory(process.Handle, IntPtr.Add(baseAddress, maxZoomOffset), BitConverter.GetBytes(float.Parse(zoom.Text)), 4, IntPtr.Zero))
					throw new Exception("WriteProcessMemory failed.");
				if (!WriteProcessMemory(process.Handle, IntPtr.Add(baseAddress, currentFovOffset), BitConverter.GetBytes(float.Parse(fov.Text)), 4, IntPtr.Zero))
					throw new Exception("WriteProcessMemory failed.");
				if (!WriteProcessMemory(process.Handle, IntPtr.Add(baseAddress, farFovOffset), BitConverter.GetBytes(float.Parse(fov.Text)), 4, IntPtr.Zero))
					throw new Exception("WriteProcessMemory failed.");
				statusLabel.Text = "Working :D";
				offset.Enabled = false;
				RequestInfo.Text = "正常运行中";
			}
			catch (Exception e)
			{
				statusLabel.Text = e.Message;
				offset.Enabled = true;
				RequestInfo.Text = "Offset错误, 请更新Offset";
				//process = null;
			}
		}


		Timer updateTimer = null;
		public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
		{
			screenSpace = pluginScreenSpace;
			statusLabel = pluginStatusText;
			pluginScreenSpace.Controls.Add(this);
			SyncConfig();
			Init();
			updateTimer = new Timer();
			updateTimer.Interval = 3000;
			updateTimer.Tick += Update;
			updateTimer.Start();
		}

		void Update(object sender, EventArgs e)
        {
			if(process == null || process.HasExited)
            {
				Init();
            } else
            {
				if(statusLabel != null && !statusLabel.Text.Contains("Working :D"))
					Init();
			}
		}
		public void DeInitPlugin()
		{
			if (updateTimer != null && updateTimer.Enabled)
				updateTimer.Stop();
			if (process != null && baseAddress != IntPtr.Zero)
				statusLabel.Text = "Exit :|";
			else
				statusLabel.Text = "Error :(";
			SyncConfig(true);
		}
		private void textKeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Convert.ToInt32(e.KeyChar) == 8))
            {
                e.Handled = false;
            }
            else
            {
                Regex numRegex = new Regex(@"^(-?[0-9]*[.]*[0-9]*)$");
                Match Result = numRegex.Match(Convert.ToString((sender as TextBox).Text + e.KeyChar));
				e.Handled = !Result.Success;
            }
        }


		private void zoom_TextChanged(object sender, EventArgs e)
		{
			float z = 20f;
			if(float.TryParse(zoom.Text,out z))
				SetZoom(z);
		}

		private void fov_TextChanged(object sender, EventArgs e)
		{
			float f = 0.73f;
			if (float.TryParse(fov.Text,out f))
				SetFov(f);
		}
		private void button_SetToDefault_Click(object sender, EventArgs e)
        {
			zoom.Text = "20";
			fov.Text = "0.78";
		}

		private void PullOffsetFromeNetwork_Click(object sender, EventArgs e)
		{
			if (process == null)
				return;
			var path = process.MainModule.FileName;
			path = path.Substring(0, path.LastIndexOf("\\")) + "\\ffxivgame.ver";
			string gameVersion = "";
			if (File.Exists(path))
			{
				using (StreamReader sr = new StreamReader(path))
				if ((gameVersion = sr.ReadLine()) == null)
					return;
			}
			var host = "ffxiv_vu.svr.moe";
			string remoteVersion = "";
			ulong remoteOffset = 0;
			var data = new List<byte>(Dns.GetHostAddresses(host).FirstOrDefault().MapToIPv6().GetAddressBytes());
			foreach (var b in data.GetRange(0, 8))
				remoteVersion += b.ToString("X2");
			remoteOffset = BitConverter.ToUInt64(data.GetRange(8, 8).ToArray(), 0);

			if (remoteVersion != gameVersion.Replace(".", ""))
			{
				RequestInfo.Text = "远程的Offset版本与游戏版本不一致,请尝试本地扫描";
				return;
			}
			offsetVersion = gameVersion;
			RequestInfo.Text = "更新成功";
			offset.Text = remoteOffset.ToString("X");
		}

		private void ScanOffsetLocally_Click(object sender, EventArgs e)
		{
			if(statusLabel.Text.Contains("Working"))
			{
				RequestInfo.Text = "正常运行中, 搜索已跳过";
				return;
			}
			RequestInfo.Text = "搜索中...";
			try
			{
				var scannedCurrentZoomOffset =  MemoryScanner.Search(process,pattern);
				if (scannedCurrentZoomOffset == IntPtr.Zero)
					return;
				var pointer = BitConverter.GetBytes((scannedCurrentZoomOffset - currentZoomOffset).ToInt64());

				byte[] moduleData = new byte[process.MainModule.ModuleMemorySize];
				if (!ReadProcessMemory(process.Handle, process.MainModule.BaseAddress, moduleData, process.MainModule.ModuleMemorySize, IntPtr.Zero))
					throw new Exception("ReadProcessMemory failed.");

				for (int i = 0; i < moduleData.Length; ++i)
				{
					for (int j = 0; i + j < moduleData.Length; ++j)
					{
						if (j == pointer.Length)
						{
							RequestInfo.Text = "搜索到位置" + i.ToString("X");
							offset.Text = i.ToString("X");
							return;
						}
						if (pointer[j] != 0x2e && moduleData[i + j] != pointer[j])
							break;
					}
				}
				RequestInfo.Text = "未搜索到目标地址";
			} catch (Exception ex)
			{
				RequestInfo.Text = "搜索失败, 请等待更新";
			}
			
		}

		private void offset_TextChanged(object sender, EventArgs e)
		{
			Init();
		}
	}
}
