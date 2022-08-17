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
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Globalization;
using System.Threading;

namespace FFXIV_ACT_ViewUnlocker {
	public partial class MainPage : UserControl {
		public MainPage() {
			InitializeComponent();
		}

		private delegate void SetRemoteInfoDelegate(string str);
		public void SetRemoteInfo(string str) {
			if (InvokeRequired)
				Invoke(new SetRemoteInfoDelegate(SetRemoteInfo), str);
			else
				RemoteInfoLable.Text = str;
		}

		private delegate void SetPullDataButtonActiveDelegate(bool active);
		public void SetPullDataButtonActive(bool active) {
			if (InvokeRequired)
				Invoke(new SetPullDataButtonActiveDelegate(SetPullDataButtonActive), active);
			else
				PullData.Enabled = active;
		}

		private delegate void SetStatusDelegate(string str);
		public void SetStatus(string status) {
			if (InvokeRequired)
				Invoke(new SetStatusDelegate(SetStatus), status);
			else
				StatusLable.Text = status;
		}

		private delegate void SetOffsetDelegate(int offset);
		public void SetOffsetInputfield(int offset) {
			if (InvokeRequired)
				Invoke(new SetOffsetDelegate(SetOffsetInputfield), offset);
			else
				offsetInputField.Text = offset.ToString("X8");
		}

		private delegate void SetOffsetActiveDelegate(bool active);
		public void SetOffsetInputfieldActive(bool active) {
			if (InvokeRequired)
				Invoke(new SetOffsetActiveDelegate(SetOffsetInputfieldActive), active);
			else
				offsetInputField.Enabled = active;
		}

		private delegate void SetLocalGameVersionDelegate(string version);
		public void SetLocalGameVersion(string version) {
			if (InvokeRequired)
				Invoke(new SetLocalGameVersionDelegate(SetLocalGameVersion), version);
			else
				GameVersionLable.Text = "本地游戏版本:\n" + version;
		}

		private delegate void SetFovDelegate(float fov);
		public void SetFov(float fov) {
			if (InvokeRequired)
				Invoke(new SetFovDelegate(SetFov), fov);
			else
				FovInputfield.Text = fov.ToString();
		}

		private delegate void SetZoomDelegate(float fov);
		public void SetZoom(float zoom) {
			if (InvokeRequired)
				Invoke(new SetZoomDelegate(SetZoom), zoom);
			else
				ZoomInputField.Text = zoom.ToString();
		}

		private delegate void SetProgressDelegate(int progress);
		public void SetProgress(int progress) {
			if (InvokeRequired)
				Invoke(new SetProgressDelegate(SetProgress), progress);
			else
				MemoryScanProgress.Value = progress;
		}

		private void textKeyPress(object sender, KeyPressEventArgs e) {
			if ((Convert.ToInt32(e.KeyChar) == 8)) {
				e.Handled = false;
			} else {
				Regex numRegex = new Regex(@"^(-?[0-9]*[.]*[0-9]*)$");
				Match Result = numRegex.Match(Convert.ToString((sender as TextBox).Text + e.KeyChar));
				e.Handled = !Result.Success;
			}
		}

		private void zoom_TextChanged(object sender, EventArgs e) {
			float z = 20f;
			if (float.TryParse(ZoomInputField.Text, out z))
				View_Unlocker.instance.SetZoom(z);
		}

		private void fov_TextChanged(object sender, EventArgs e) {
			float f = 0.73f;
			if (float.TryParse(FovInputfield.Text, out f))
				View_Unlocker.instance.SetFov(f);
		}

		private void offsetInputField_TextChanged(object sender, EventArgs e) {
			int offset = 0;
			if (int.TryParse(offsetInputField.Text, NumberStyles.HexNumber, null, out offset))
				View_Unlocker.instance.SetOffset(offset);
		}

		private void SetToDefault_Click(object sender, EventArgs e) {
			ZoomInputField.Text = "20";
			FovInputfield.Text = "0.78";
			View_Unlocker.instance.SetZoom(20);
			View_Unlocker.instance.SetFov(0.78f);
		}

		private void ScanOffsetLocally_Click(object sender, EventArgs e) {
			View_Unlocker.instance.ScanOffset();
		}

		private void PullData_Click(object sender, EventArgs e) {
			View_Unlocker.instance.PullOffsetFromServer();
		}

		private void cbAutoCheck_CheckStateChanged(object sender, EventArgs e) {
			PullData.Enabled = !cbAutoCheck.Checked;
		}

		private void cbUpload_CheckStateChanged(object sender, EventArgs e) {

		}
	}
}
