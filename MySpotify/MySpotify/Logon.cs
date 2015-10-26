using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MySpotify
{
    public partial class Logon : Form
    {
        public Logon()
        {
            InitializeComponent();
        }

        string client_id = "56d7039c2f57497a9a796771c4990da5"; // Your client id
        string client_secret = "dbb8d589e5ae47ab8d1c87514e6b7790"; // Your client secret
        string redirect_uri = "http://localhost:8080/callback"; // Your redirect uri

        public string Code {get;set;}

        private void Logon_Load(object sender, EventArgs e)
        {
            var state = DateTime.Now.Ticks.ToString();

            var scope = "playlist-modify-private playlist-modify-public";
            string url = "https://accounts.spotify.com/authorize?" +
                "response_type=token" +
                "&client_id=" + client_id +
                "&scope=" + scope +
                "&redirect_uri=" + redirect_uri +
                "&state=" + state;

            webBrowser1.Navigating += webBrowser1_Navigating;
            webBrowser1.Navigate(url);
        }

        void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.Host.Length>0 && redirect_uri.Contains(e.Url.Host))
            {
                string Code = e.Url.Fragment.Replace("#access_token=", "");
                this.Code = Code.Split('&')[0];
                this.Close();
            }
        }
    }
}
