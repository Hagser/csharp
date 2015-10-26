using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Permissions;
using Microsoft.Win32;
using System.Diagnostics;

namespace Trafikverket
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]

    public partial class Map : UserControl
    {
        public List<Weather> List = new List<Weather>();
        public List<Weather> AllList = new List<Weather>();
        public Map(List<Weather> _list)
        {
            InitializeComponent();
            List = _list;
            Map_Load();
        }

        void Map_Load()
        {

#if !DEBUG
            webBrowser1.ScriptErrorsSuppressed = true;
#endif
            webBrowser1.AllowWebBrowserDrop = false;
            webBrowser1.IsWebBrowserContextMenuEnabled = false;
            webBrowser1.WebBrowserShortcutsEnabled = false;
            webBrowser1.ObjectForScripting = this;
            
            webBrowser1.Navigate("file:///" + Application.StartupPath.Replace("\\", "/") + "/map.htm");

        }

        public delegate void PositionEventHandler(object sender, PositionEventArgs e);
        public PositionEventHandler scrollTo;
        public void ScrollTo(string id, double lat, double lng)
        {
            if (scrollTo != null)
                scrollTo.Invoke(this, new PositionEventArgs() { id = id.Split('_')[0], lat = lat, lng = lng });
        }
        public void panTo(Weather wc)
        {
            if (webBrowser1.Document != null)
                webBrowser1.Document.InvokeScript("panTo", new object[] { wc.Lat, wc.Lng });
        }
        public void setText(string txt)
        {
            this.Parent.Parent.Parent.Text = txt + "_" + DateTime.Now.ToString("HH:mm:ss");
        }
        public void addTimes(List<Weather> list)
        {
            Cursor.Current = Cursors.WaitCursor;
            AllList = list;
            if (webBrowser1.Document != null)
            {
                webBrowser1.Document.InvokeScript("clearTimes");
                foreach (Weather wc in AllList)
                {
                    webBrowser1.Document.InvokeScript("addTimes", new object[] { wc.MeasurePoint, wc.AirTemp, wc.PrecipitationIconId, wc.WindIconId, wc.AverageWindIconId, wc.MaxWindIconId, wc.AmountOfPrecipitation, wc.WindForce, wc.AverageWindForce, wc.MaxWindForce, wc.MeasureTime, wc.Lat, wc.Lng });
                }
                webBrowser1.Document.InvokeScript("runTimes");
            }

            Cursor.Current = Cursors.Default;
        }
        public void setBounds(double latNE, double lngNE, double latSW, double lngSW)
        { 
        
        
        }
        public void addWeather()
        {
            Cursor.Current = Cursors.WaitCursor;
            if (webBrowser1.Document != null)
            {
                webBrowser1.Document.InvokeScript("getBounds");
                foreach (Weather wc in List)
                {
                    webBrowser1.Document.InvokeScript("toggleWeather", new object[] { wc.MeasurePoint, wc.Lat, wc.Lng, wc.Id, wc.AirTemp, wc.PrecipitationIconId, wc.WindIconId, wc.AverageWindIconId, wc.MaxWindIconId, wc.AmountOfPrecipitation,wc.WindForce,wc.AverageWindForce,wc.MaxWindForce,wc.MeasureTime.ToString("yyyy-MM-dd HH:mm") });
                }
                webBrowser1.Document.InvokeScript("showHeat");
            }
            Cursor.Current = Cursors.Default;
        }
    }

    public class PositionEventArgs : EventArgs
    {
        public static readonly PositionEventArgs Empty;

        public PositionEventArgs() { }
        public string id { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }
}
