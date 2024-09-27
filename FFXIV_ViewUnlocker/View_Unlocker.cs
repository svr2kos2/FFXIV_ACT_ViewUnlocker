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
//using Advanced_Combat_Tracker;
using System.Threading;

namespace FFXIV_ACT_ViewUnlocker {
    public static class View_Unlocker {
        public static string currentPath = Path.GetTempPath();
        static Process ffxivProcess;

        //48 8d 0d ? ? ? ? 48 8b 0c c1 48 85 c9 74 0c 83 b9
        static byte[] pattern = new byte[] {
            0x48, 0x8d, 0x0d, 0x2e, 0x2e, 0x2e, 0x2e, //lea  rcx, (view struct ptr relative offset)
            0x48, 0x8b, 0x0c, 0xc1,                   //mov  rcx, [rcx+rax*8]
            0x48, 0x85, 0xc9,                         //test rcx, rcx
            0x74, 0x0c,                               //je   short
            0x83, 0xb9,                               //cmp  [rcx+0x10], (whatever)
        };

        public static class ViewAddr {
            public static IntPtr viewStructureAddr = IntPtr.Zero;

            public static IntPtr currentZoom {
                get { return IntPtr.Add(viewStructureAddr, 0X114); }
            }

            public static IntPtr maxZoom {
                get { return IntPtr.Add(viewStructureAddr, 0x11C); }
            }

            public static IntPtr currentFov {
                get { return IntPtr.Add(viewStructureAddr, 0x120); }
            }

            public static IntPtr maxFov {
                get { return IntPtr.Add(viewStructureAddr, 0x128); }
            }
        }

        public static void SetFov(float fov) {
            try {
                var bDiffrent = Utilitys.ReadMemory<float>(ffxivProcess.Handle, ViewAddr.maxFov) != fov;
                Utilitys.WriteMemory(ffxivProcess.Handle, ViewAddr.maxFov, fov);
                if (bDiffrent)
                    Utilitys.WriteMemory(ffxivProcess.Handle, ViewAddr.currentFov, fov);
                VUPropertys.instance.Fov = fov;
            }
            catch (Exception e) { }
        }

        public static void SetZoom(float zoom) {
            try {
                var bDiffrent = Utilitys.ReadMemory<float>(ffxivProcess.Handle, ViewAddr.maxZoom) != zoom;
                Utilitys.WriteMemory(ffxivProcess.Handle, ViewAddr.maxZoom, zoom);
                if (bDiffrent)
                    Utilitys.WriteMemory(ffxivProcess.Handle, ViewAddr.currentZoom, zoom);
                VUPropertys.instance.Zoom = zoom;
            }
            catch (Exception e) { }
        }

        public static IntPtr GetViewStructureAddress(Process ffxiv) {
            byte[] moduleData = new byte[ffxiv.MainModule.ModuleMemorySize];
            if (!Utilitys.ReadProcessMemory(ffxiv.Handle, ffxiv.MainModule.BaseAddress,
                    moduleData, ffxiv.MainModule.ModuleMemorySize, IntPtr.Zero)) {
                mainpage.SetStatus("Can't find view structure address.");
                MessageBox.Show("Can't find view structure address.");
                Application.Exit();
            }


            var lea = Utilitys.KMP(moduleData, pattern);
            if (lea < 0) {
                mainpage.SetStatus("Can't find view structure address.");
                MessageBox.Show("Can't find view structure address.");
                Application.Exit();
            }
            var relativeOffset = BitConverter.ToInt32(moduleData, lea + 0x3);
            var absoluteOffset = lea + relativeOffset + 7;
            var absoluteAddress = IntPtr.Add(ffxiv.MainModule.BaseAddress, absoluteOffset);
            byte[] pointerBytes = new byte[8];
            Utilitys.ReadProcessMemory(ffxiv.Handle, absoluteAddress, pointerBytes, 8, IntPtr.Zero);
            return (IntPtr)BitConverter.ToUInt64(pointerBytes, 0);
        }

        static bool quit = false;

        public static bool Init() {
            try {
                VUPropertys.ReadConfig(currentPath + "/ViewUnlocker.cfg");
                Task.Run(() => {
                    for (; !quit;) {
                        for (; !quit; Task.Delay(3000).Wait()) {
                            mainpage.SetStatus("等待游戏启动");
                            ffxivProcess = Process.GetProcessesByName("ffxiv_dx11").FirstOrDefault();
                            if (ffxivProcess == null) continue;
                            ViewAddr.viewStructureAddr = GetViewStructureAddress(ffxivProcess);
                            break;
                        }

                        mainpage.SetFov(VUPropertys.instance.Fov);
                        mainpage.SetZoom(VUPropertys.instance.Zoom);
                        for (; !quit && !ffxivProcess.HasExited;) {
                            mainpage.SetStatus("Working");
                            SetFov(VUPropertys.instance.Fov);
                            SetZoom(VUPropertys.instance.Zoom);
                            Task.Delay(3000).Wait();
                        }
                    }
                });
            }
            catch (Exception e) {
                mainpage.SetStatus(e.Message);
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }

        public static void Deinit() {
            quit = true;
            VUPropertys.WriteConfig(currentPath + "/ViewUnlocker.cfg");
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
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = false;
            form.ShowIcon = false;
            form.Text = "FFXIV_ViewUnlocker";
            currentPath = Environment.CurrentDirectory;
            if (!Init()) {
                return;
            }

            Application.Run(form);
            Deinit();
        }
        // internal class ACT_View_Unlocker : IActPluginV1 {
        //     public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText) {
        //         pluginScreenSpace.Text = "视角解锁";
        //         mainpage = new MainPage();
        //         pluginScreenSpace.Controls.Add(View_Unlocker.mainpage);
        //         pluginStatusText.Text = "Started";
        //         ActPluginData actPluginData = ActGlobals.oFormActMain.PluginGetSelfData(this);
        //         currentPath = actPluginData.pluginFile.DirectoryName;
        //         Init();
        //     }
        //     public void DeInitPlugin() {
        //         ActGlobals.oFormActMain.PluginGetSelfData(this).lblPluginStatus.Text = "Stoped";
        //         Deinit();
        //     }
        // }
    }
}