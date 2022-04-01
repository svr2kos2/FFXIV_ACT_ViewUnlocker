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

		int viewPointerOffset = 0x1E74290;
		int currentZoomOffset = 0X114;
		int maxZoomOffset = 0x11C;
		int currentFovOffset = 0x120;
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
						viewPointerOffset = int.Parse(line);
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
					sw.WriteLine(0x1E74290);
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
		Timer retryTimer = null;
		public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
		{
			screenSpace = pluginScreenSpace;
			statusLabel = pluginStatusText;
			pluginScreenSpace.Controls.Add(this);
			if (retryTimer != null && retryTimer.Enabled)
				retryTimer.Stop();
			SyncConfig();
			retryTimer = new Timer();
			try
			{
				process = Process.GetProcessesByName("ffxiv_dx11").FirstOrDefault();
				if (process == null)
					throw new Exception("You need to start FFXIV(DX11) to initialize this plugin.");
				byte[] viewPointer = new byte[8];
				if (!ReadProcessMemory(process.Handle, IntPtr.Add(process.MainModule.BaseAddress, viewPointerOffset), viewPointer, 8, IntPtr.Zero))
					throw new Exception("ReadProcessMemory failed.");
				baseAddress = new IntPtr(BitConverter.ToInt64(viewPointer, 0));
				if (!WriteProcessMemory(process.Handle, IntPtr.Add(baseAddress, currentZoomOffset), BitConverter.GetBytes(float.Parse(zoom.Text)), 4, IntPtr.Zero))
					throw new Exception("WriteProcessMemory failed.");
				if (!WriteProcessMemory(process.Handle, IntPtr.Add(baseAddress, maxZoomOffset), BitConverter.GetBytes(float.Parse(zoom.Text)), 4, IntPtr.Zero))
					throw new Exception("WriteProcessMemory failed.");
				if (!WriteProcessMemory(process.Handle, IntPtr.Add(baseAddress, currentFovOffset), BitConverter.GetBytes(float.Parse(fov.Text)), 4, IntPtr.Zero))
					throw new Exception("WriteProcessMemory failed.");
				if (!WriteProcessMemory(process.Handle, IntPtr.Add(baseAddress, farFovOffset), BitConverter.GetBytes(float.Parse(fov.Text)), 4, IntPtr.Zero))
					throw new Exception("WriteProcessMemory failed.");
				statusLabel.Text = "Working :D";
			}
			catch (Exception e)
			{
				statusLabel.Text = e.Message;
				process = null;
			}
			retryTimer.Interval = 3000;
			retryTimer.Tick += Refresh;
			retryTimer.Start();
		}

		void Refresh(object sender, EventArgs e)
        {
			if(process == null || process.HasExited)
            {
				ActPluginData actPluginData = ActGlobals.oFormActMain.PluginGetSelfData(this);
				actPluginData.cbEnabled.Checked = false;
				actPluginData.cbEnabled.Checked = true;
            } else
            {
				try
                {
					byte[] currentByte = new byte[4];
					float current = 0.0f;
					if (!ReadProcessMemory(process.Handle, IntPtr.Add(baseAddress, maxZoomOffset), currentByte, 4, IntPtr.Zero))
						throw new Exception("ReadProcessMemory failed.");
					current = BitConverter.ToSingle(currentByte, 0);
					if(current != float.Parse(zoom.Text))
						throw new Exception("Update.");
					if (!ReadProcessMemory(process.Handle, IntPtr.Add(baseAddress, farFovOffset), currentByte, 4, IntPtr.Zero))
						throw new Exception("ReadProcessMemory failed.");
					current = BitConverter.ToSingle(currentByte, 0);
					if (current != float.Parse(fov.Text))
						throw new Exception("Update.");
				}
				catch
                {
					SetZoom(float.Parse(zoom.Text));
					SetFov(float.Parse(fov.Text));
					//ActPluginData actPluginData = ActGlobals.oFormActMain.PluginGetSelfData(this);
					//actPluginData.cbEnabled.Checked = false;
					//actPluginData.cbEnabled.Checked = true;
				}
			}
		}
		public void DeInitPlugin()
		{
			if (retryTimer != null && retryTimer.Enabled)
				retryTimer.Stop();
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
                if (Result.Success)
                {
                    e.Handled = false;
                    switch ((sender as TextBox).Name) 
                    {
                        case "zoom":
                            float zoom = float.Parse(Result.Value);
                            SetZoom(zoom);
                            break;
                        case "fov":
                            float fov = float.Parse(Result.Value);
                            SetFov(fov);
                            break;
                    }
                }
                else
                    e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
			zoom.Text = "20";
			fov.Text = "0.78";
			SetZoom(float.Parse(zoom.Text));
			SetFov(float.Parse(fov.Text));
		}
    }
}
