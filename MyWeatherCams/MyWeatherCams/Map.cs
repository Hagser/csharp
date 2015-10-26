using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace MyWeatherCams
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class Map : Form
    {
        List<webcam> list = new List<webcam>();
        public Map(List<webcam> _list)
        {
            InitializeComponent();
            list = _list;
            Load += Map_Load;
        }

        void Map_Load(object sender, EventArgs e)
        {
            //webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.AllowWebBrowserDrop = false;
            webBrowser1.IsWebBrowserContextMenuEnabled = false;
            webBrowser1.WebBrowserShortcutsEnabled = false;
            webBrowser1.ObjectForScripting = this;

            webBrowser1.Navigate("file:///" + Application.StartupPath.Replace("\\", "/") + "/map.htm");
            
        }
        public delegate void PositionEventHandler(object sender, PositionEventArgs e);
        public PositionEventHandler scrollTo;
        public void ScrollTo(string id,double lat,double lng)
        {
            if (scrollTo != null)
                scrollTo.Invoke(this, new PositionEventArgs() { id = id.Split('_')[0], lat = lat, lng = lng });
        }
        public void panTo(webcam wc)
        {
            if(webBrowser1.Document!=null)
                webBrowser1.Document.InvokeScript("panTo", new object[] { wc.lat, wc.lng });
        }
        public void addCameras()
        {
            foreach (webcam wc in list)
            {
                webBrowser1.Document.InvokeScript("insertCamera", new object[] { wc.name, wc.lat, wc.lng, wc.url,wc.id });
            }
        }
    }
    
    public class PositionEventArgs:EventArgs
    {
        public static readonly PositionEventArgs Empty;

        public PositionEventArgs() { }
        public string id { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }
}
