using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Taskbar;
using Microsoft.WindowsAPICodePack.Shell;
using System.Runtime.InteropServices;

namespace MyAppRunner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Application.Idle += new EventHandler(Application_Idle);
            foreach(string file in Directory.EnumerateFiles(@"C:\Users\johan\Documents\hcdev\csharp\","*.exe",SearchOption.AllDirectories).OrderByDescending(x=>new FileInfo(x).LastWriteTime))
                if (file.Contains(@"\bin\Release\") && !file.Contains("setup.exe") && !file.Contains(".vshost") && !file.Contains("MyAppRunner"))
                    apps.Add(new MyApps(){Path=file});
        }
        bool bLoaded = false;
        JumpList _jumpList = null;

        List<MyApps> apps = new List<MyApps>();

        void Application_Idle(object sender, EventArgs e)
        {
            try
            {
                Process currentProcess = Process.GetCurrentProcess();
                if (currentProcess != null && currentProcess.MainWindowHandle != IntPtr.Zero && !bLoaded)
                {
                    bLoaded = true;
                    Application.Idle -= Application_Idle;
                    if (_jumpList == null)
                    {
                        _jumpList = JumpList.CreateJumpList();
                        _jumpList.KnownCategoryToDisplay = JumpListKnownCategoryType.Recent;
                        _jumpList.ClearAllUserTasks();
                        _jumpList.Refresh();
                    }
                    var list = new List<string>();
                    foreach (MyApps app in apps.ToList().OrderByDescending(x => x.Fileinfo.LastAccessTime))
                    {

                        if (app.Fileinfo.Exists)
                        {
                            string name = app.Fileinfo.Name.Replace(app.Fileinfo.Extension, "");
                            if (!list.Contains(name))
                            {
                                JumpListTask task = new JumpListLink(app.Path, name)
                                {
                                    IconReference = new IconReference(app.Path, 0),
                                };

                                _jumpList.AddUserTasks(task);
                                list.Add(name);
                            }
                        }
                    }
                    
                    _jumpList.Refresh();
                }
            }
            catch
            { }
        }

        private Process getProcessByMyApp(MyApps app)
        {
            if (app.Handle != null && app.Handle!=IntPtr.Zero)
            {
                Process p = Process.GetProcessById(app.Handle.ToInt32());
                if (p != null)
                    return p;
            }
            string name = app.Fileinfo.Name.Replace(app.Fileinfo.Extension, "");
            Process[] ps = Process.GetProcessesByName(name);
            if (ps.Length > 0)
                return ps[0];
            return null;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            try
            {
                foreach (MyApps app in apps.ToList().OrderBy(x => (x.Fileinfo.Name)))
                {
                    if (app.Fileinfo.Exists)
                    {
                        string name = app.Fileinfo.Name.Replace(app.Fileinfo.Extension, "");
						
                        Process p = getProcessByMyApp(app);

                        /*if (p != null)
                        {
                            if (p.MainWindowHandle != IntPtr.Zero)
                            {

                                if (TaskbarManager.Instance.TabbedThumbnail.IsThumbnailPreviewAdded(p.MainWindowHandle))
                                {
                                    TabbedThumbnail pr = TaskbarManager.Instance.TabbedThumbnail.GetThumbnailPreview(p.MainWindowHandle);
                                    Bitmap bmp = TabbedThumbnailScreenCapture.GrabWindowBitmap(p.MainWindowHandle, new Size(300, 200));
                                    flowLayoutPanel1.Controls.Add(new PictureBox() { SizeMode = PictureBoxSizeMode.AutoSize, Image = bmp });
                                    pr.SetImage(bmp);
                                }
                                else
                                {
                                    TabbedThumbnail pr = new TabbedThumbnail(this.Handle, p.MainWindowHandle);
                                    pr.Title = name;
                                    pr.DisplayFrameAroundBitmap = true;

                                    Bitmap bmp = TabbedThumbnailScreenCapture.GrabWindowBitmap(p.MainWindowHandle, new Size(300, 200));
                                    flowLayoutPanel1.Controls.Add(new PictureBox() { SizeMode = PictureBoxSizeMode.AutoSize, Image = bmp });
                                    pr.SetImage(bmp);
                                    //pr.Tag = p;

                                    pr.TabbedThumbnailActivated += (s1, s2) =>
                                    {
                                        MyShowWindow.ShowWindow(s2.WindowHandle);
                                    };
                                    pr.TabbedThumbnailBitmapRequested += (s1, s2) =>
                                    {
                                        Bitmap bmp2 = TabbedThumbnailScreenCapture.GrabWindowBitmap(s2.WindowHandle, new Size(300, 200));
                                        (s1 as TabbedThumbnail).SetImage(bmp2);
                                    };
                                    TaskbarManager.Instance.TabbedThumbnail.AddThumbnailPreview(pr);
                                }
                            }
							
                        }
                        else if (app.Handle != IntPtr.Zero)
                        {
                            if (TaskbarManager.Instance.TabbedThumbnail.IsThumbnailPreviewAdded(app.Handle))
                                TaskbarManager.Instance.TabbedThumbnail.RemoveThumbnailPreview(app.Handle);

                        }*/
						//else
						{
							Label lbl = new Label() { Text= name,Tag=app.Fileinfo.FullName,AutoSize=true};
							lbl.Click += (s1,s2)=>
							{
								var lbl1 = (Label)s1;
								if(lbl1!=null)
								{
									var fi = lbl1.Tag+"";
									Process.Start(fi);
								}
							};
							flowLayoutPanel1.Controls.Add(lbl);
						}
                    }
                }
            }
            catch { }
        }


        
            /*

            TaskbarManager.Instance.SetProgressValue(50,100);
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);

            ThumbnailToolBarButton btn = new ThumbnailToolBarButton(Icon.FromHandle(this.Handle), "test");
            btn.Click+=(r,g)=>{
            
            };
            TaskbarManager.Instance.ThumbnailToolBars.AddButtons(this.Handle, btn);

            TabbedThumbnail preview = TaskbarManager.Instance.TabbedThumbnail.GetThumbnailPreview(this.Handle);
            */


        
    }
}
