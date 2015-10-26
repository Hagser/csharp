using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MySpotify
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Dictionary<string, string> artists = new Dictionary<string, string>();
        List<Track> toptracks = new List<Track>();
        WebClient webclient = new WebClient();
        bool bRun = false;
        string access_token { get; set; }
        int isleep = 50;
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text.Equals("Search"))
            {
                artists.Clear();
                toptracks.Clear();
                btnSearch.Text = "Stop";
                bRun = true;
                string json = webclient.DownloadString("https://api.spotify.com/v1/search?q=" + txtSearchFor.Text.Replace(" ", "%20") + "&type=artist");
                dynamic obj = JsonConvert.DeserializeObject(json);

                foreach (var art in obj.artists.items)
                {
                    string name = art.name;
                    string id = art.id;
                    artists.Add(id, name);
                    GetTopTracks(id);
                    GetRelatedArtists(id, 0);
                    break;
                }
            }
            btnSearch.Text = "Search";
            bRun = false;
            dataGridView1.DataSource = toptracks.OrderBy(x=>x.name).ToList();

            if (toptracks != null)
            {
                btnCreate.Enabled = true;
                WMPLib.IWMPPlaylist pl = mediaPlayer.newPlaylist("New playlist", "");
                foreach (Track itm in toptracks.OrderBy(x => x.name).ToList())
                {
                    pl.appendItem(mediaPlayer.newMedia(itm.preview));
                }
                mediaPlayer.currentPlaylist = pl;
                mediaPlayer.close();
            }
        }

        private string GetPlaylist(string playlist)
        {
            string url = "https://api.spotify.com/v1/users/yohpops/playlists";

            HttpWebRequest req = WebRequest.CreateHttp(url);
            req.Accept = "application/json";
            req.UserAgent = "MySpotify";
            req.Headers.Add("Authorization", "Bearer " + access_token);
            req.Method = "GET";
            try
            {
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                if (res.ContentLength > 0)
                {
                    string stop = "";
                    StreamReader sr = new StreamReader(res.GetResponseStream());
                    dynamic obj = JsonConvert.DeserializeObject(sr.ReadToEnd());
                    foreach (var itm in obj.items)
                    {
                        string name = itm.name + "";
                        if (name.Equals(playlist, StringComparison.InvariantCultureIgnoreCase))
                        {
                            string id = itm.id + "";
                            return id;
                        }
                    }
                }
                else
                {
                    string stop = "";
                }
            }
            catch (Exception ex)
            {
                string stop = ex.Message;
            }
            return null;
        }

        private void GetGrant()
        {
            string url = "https://accounts.spotify.com/api/token";

            HttpWebRequest req = WebRequest.CreateHttp(url);
            req.Accept = "application/json";
            req.UserAgent = "MySpotify";
            req.Headers.Add("Authorization", "Basic " + (Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(client_id + ":" + client_secret))));
            req.Method = "POST";
            byte[] body = System.Text.Encoding.Default.GetBytes("grant_type=authorization_code\rcode=" + Code + "\rredirect_uri=" + redirect_uri);
            Stream ms = req.GetRequestStream();
            ms.Write(body, 0, body.Length);
            try
            {
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                if (res.ContentLength > 0)
                {
                    string stop = "";
                    StreamReader sr = new StreamReader(res.GetResponseStream());
                    dynamic obj = JsonConvert.DeserializeObject(sr.ReadToEnd());
                    access_token = obj.access_token + "";
                }
                else
                {
                    string stop = "";
                }
            }
            catch (Exception ex)
            {
                string stop = ex.Message;
            }

        }

        private string CreatePlaylist(string playlist)
        {
            string url = "https://api.spotify.com/v1/users/yohpops/playlists";            

            HttpWebRequest req = WebRequest.CreateHttp(url);
            req.Accept = "application/json";
            req.UserAgent = "MySpotify";
            req.Headers.Add("Authorization", "Bearer " + access_token);
            req.Method = "POST";
            req.ContentType = "application/json";
            byte[] body = System.Text.Encoding.Default.GetBytes("{\"name\":\"" + playlist + "\", \"public\":false}");
            Stream ms = req.GetRequestStream();
            ms.Write(body, 0, body.Length);
            try
            {
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                if (res.ContentLength > 0)
                {
                    string stop = "";
                    StreamReader sr = new StreamReader(res.GetResponseStream());
                    dynamic obj = JsonConvert.DeserializeObject(sr.ReadToEnd());
                    string id = obj.id + "";
                    return id;
                }
                else
                {
                    string stop = "";
                }
            }
            catch (Exception ex)
            {
                string stop = ex.Message;
            }
            return null;
        }

        private void AddSongs(dynamic id,string uris)
        {
            Action act = new Action(() =>
            {
                Thread.Sleep(isleep);
                bool bDone = false;
                int icnt = 0;
                while (!bDone && icnt<5)
                {
                    string url = "https://api.spotify.com/v1/users/yohpops/playlists/{1}/tracks?uris={0}";

                    HttpWebRequest req = WebRequest.CreateHttp(String.Format(url, uris, id));
                    req.Accept = "application/json";
                    req.UserAgent = "MySpotify";
                    req.Headers.Add("Authorization", "Bearer " + access_token);
                    req.Method = "POST";
                    req.ContentType = "application/json";
                    try
                    {
                        HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                        if (res.ContentLength > 0)
                        {
                            string stop = "";
                        }
                        else
                        {
                            string stop = "";
                        }
                        bDone = true;
                    }
                    catch (Exception ex)
                    {
                        string stop = ex.Message;
                        icnt++;
                        Thread.Sleep(isleep);
                    }
                }
                if (icnt > 0)
                {
                    string ssss = "";
                }
            });
            act.Invoke();
        }

        private void GetRelatedArtists(string id,int lvl)
        {
            Action act = new Action(() =>
            {
                Thread.Sleep(isleep);
                if (!bRun) return;
                if (lvl < 2)
                {
                    string json = webclient.DownloadString("https://api.spotify.com/v1/artists/" + id + "/related-artists");
                    dynamic obj = JsonConvert.DeserializeObject(json);

                    foreach (var art in obj.artists)
                    {
                        if (!bRun) return;
                        string name = art.name;
                        string artid = art.id;
                        if (!artists.ContainsKey(artid))
                        {
                            artists.Add(artid, name);
                            GetTopTracks(artid);
                            GetRelatedArtists(artid, lvl + 1);
                        }
                    }
                }
            });
            act.Invoke();
        }

        private void GetTopTracks(string id)
        {
            Action act = new Action(() =>
            {
                Thread.Sleep(isleep);
                if (!bRun) return;
                string json = webclient.DownloadString("https://api.spotify.com/v1/artists/" + id + "/top-tracks?country=SE");
                dynamic obj = JsonConvert.DeserializeObject(json);
                int icnt = 0;
                foreach (var track in obj.tracks)
                {
                    if (!bRun) return;
                    if (icnt > 1)
                        break;
                    string name = track.name;
                    string href = track.href;
                    string trid = track.id;
                    string artname = track.artists[0].name;
                    string preview = track.preview_url;
                    string uri = track.uri;
                    toptracks.Add(new Track() { name = name, href = href, trid = trid, artname = artname, preview = preview, uri = uri });
                    this.Text = artists.Count() + " " + toptracks.Count() + "";
                    Application.DoEvents();
                    icnt++;
                }
            });
            act.Invoke();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            WMPLib.IWMPMedia media = mediaPlayer.currentPlaylist.Where(w => w.sourceURL.Equals(dataGridView1.Rows[e.RowIndex].Cells["Preview"].Value.ToString())).FirstOrDefault();
            if (media != null)
            {
                mediaPlayer.Ctlcontrols.playItem(media);
            }
        }
        string Code { get; set; }
        //string token = "AQCSa5zjgUSmfo4b9eb8I7me3_8ukaheOsww4Zc97vtIHzXGk7Or1cggrYIB-CFTbN-kUP9OG1Jq9Ej9KCFVyQJyaLbhk2KZqmMaLNFxNFD-shNEdEcqjQYInsvgO7sRa11hvo0Tp563WBGO60ku5v8rWDvC6CPmHfkE50REFPu1ZRr3blz3vhEZkaEHZZqfl6sypTX0KCZL9eTpOMD5iw2NG7lgSX3wQEMn8A";
        string client_id = "56d7039c2f57497a9a796771c4990da5"; // Your client id
        string client_secret = "dbb8d589e5ae47ab8d1c87514e6b7790"; // Your client secret
        string redirect_uri = "http://localhost:8080/callback"; // Your redirect uri
        private void Form1_Load(object sender, EventArgs e)
        {
            
            Logon l = new Logon();
            l.FormClosed += (a, b) => {
                Code = l.Code;
            };
            
            l.ShowDialog();
            if (Code != null)
            {
                access_token = Code;
                //GetGrant();
                return;
            }
            
            
            

        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (toptracks != null)
            {
                btnCreate.Enabled = false;
                string playlistid = GetPlaylist(txtSearchFor.Text + " R.A.T.T");
                if (string.IsNullOrEmpty(playlistid))
                {
                    playlistid = CreatePlaylist(txtSearchFor.Text + " R.A.T.T");
                }
                if (!string.IsNullOrEmpty(playlistid))
                {
                    string uris = "";
                    int icnt = 0;
                    foreach (Track trk in toptracks)
                    {
                        uris += "," + trk.uri;
                        icnt++;
                        if (icnt > 99)
                        {
                            AddSongs(playlistid, uris.Substring(1));
                            uris = "";
                            icnt = 0;
                        }

                    }
                    if(!string.IsNullOrEmpty(uris))
                        AddSongs(playlistid, uris.Substring(1));
                }

            }
        }

        private void txtSearchFor_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void txtSearchFor_TextChanged(object sender, EventArgs e)
        {
            if (txtSearchFor.Text.Length > 3)
            {
                string json = webclient.DownloadString("https://api.spotify.com/v1/search?q=" + txtSearchFor.Text.Replace(" ", "+") + "&type=artist");
                dynamic obj = JsonConvert.DeserializeObject(json);
                List<string> names = new List<string>();
                foreach (var art in obj.artists.items)
                {
                    string name = art.name;
                    names.Add(name);
                }
                txtSearchFor.Items.Clear();
                txtSearchFor.Items.AddRange(names.ToArray());
                txtSearchFor.SelectionStart = txtSearchFor.Text.Length;
            }
        }
    }
    public static class wmpplaylistext
    {
        public static List<WMPLib.IWMPMedia> Where(this WMPLib.IWMPPlaylist source, Func<WMPLib.IWMPMedia, bool> predicate)
        {
            List<WMPLib.IWMPMedia> list = new List<WMPLib.IWMPMedia>();
            for (int i = 0; i < source.count; i++)
            {
                list.Add(source.get_Item(i));
            }
            return list.Where(predicate).ToList();
        }
    }
}
