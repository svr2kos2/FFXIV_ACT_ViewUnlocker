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
		private delegate void SetStatusDelegate(string str);
		public void SetStatus(string status) {
			if (InvokeRequired)
				Invoke(new SetStatusDelegate(SetStatus), status);
			else
				StatusLable.Text = status;
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
				View_Unlocker.SetZoom(z);
		}

		private void fov_TextChanged(object sender, EventArgs e) {
			float f = 0.73f;
			if (float.TryParse(FovInputfield.Text, out f))
				View_Unlocker.SetFov(f);
		}

		private void SetToDefault_Click(object sender, EventArgs e) {
			ZoomInputField.Text = "20";
			FovInputfield.Text = "0.78";
			View_Unlocker.SetZoom(20);
			View_Unlocker.SetFov(0.78f);
		}
	}
}
