using System;
using System.Windows.Media;
using System.Net;
using System.IO;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;

namespace PhotoSlideshow.Classes
{
    public delegate void ISEventHandler(object sender, EventArgs e);
    public delegate void DPEventHandler(object sender, DownloadProgressChangedEventArgs e);
    public class PhotoInfo
    {
        public event ISEventHandler ImageSourceIsLoaded;
        public event ISEventHandler SmallImageSourceIsLoaded;
        public event DPEventHandler DownloadProgressChanged;
        ImageSourceConverter isc = new ImageSourceConverter();
        WebClient wc = new WebClient() { AllowReadStreamBuffering = true };
        WebClient wcSI = new WebClient() { AllowReadStreamBuffering = true };
        System.IO.IsolatedStorage.IsolatedStorageSettings iss = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;

        public PhotoInfo(string strImg,bool bVisible)
        {
            string[] strSplit = strImg.Split("#".ToCharArray()[0]);

            id = strSplit[0];
            owner = strSplit.Length > 1 ? strSplit[1] : "";
            secret = strSplit.Length > 2 ? strSplit[2] : "";
            server = strSplit.Length > 3 ? strSplit[3] : "";
            farm = strSplit.Length > 4 ? strSplit[4] : "";
            title = strSplit.Length > 5 ? strSplit[5] : "";


            wc.DownloadProgressChanged += (a, b) =>
                {
                    OnDownloadProgressChanged(b);
                };
            wc.OpenReadCompleted += (a, b) =>
            {
                BitmapSource bs = (BitmapSource)isc.ConvertFromString(SmallImageUrl);
                if (b.Error == null)
                {
                    bs.SetSource(b.Result);
                    _ImageSource = bs;
                    OnImageSourceIsLoaded(EventArgs.Empty);
                }
            };

            wcSI.OpenReadCompleted += (a, b) =>
            {
                BitmapSource bs = (BitmapSource)isc.ConvertFromString(SmallImageUrl);
                if (b.Error == null)
                {
                    bs.SetSource(b.Result);
                    _SmallImageSource = bs;
                    GetOwner();
                    OnSmallImageSourceIsLoaded(EventArgs.Empty);
                }
            };
            if (bVisible)
            {
                //System.Threading.Thread.Sleep(1);
                wcSI.OpenReadAsync(new Uri(SmallImageUrl), null);
            }
        }


        protected virtual void OnDownloadProgressChanged(DownloadProgressChangedEventArgs e)
        {
            if (DownloadProgressChanged != null)
                DownloadProgressChanged(this, e);
        }

        protected virtual void OnImageSourceIsLoaded(EventArgs e)
        {
            if (ImageSourceIsLoaded != null)
                ImageSourceIsLoaded(this, e);
        }

        protected virtual void OnSmallImageSourceIsLoaded(EventArgs e)
        {
            if (SmallImageSourceIsLoaded != null)
                SmallImageSourceIsLoaded(this, e);
        }

        public string id { get; set; }
        public string owner { get; set; }
        public string secret { get; set; }
        public string server { get; set; }
        public string farm { get; set; }
        public string title { get; set; }
        private bool _IsVisible { get; set; }

        public bool IsVisible {
            get { return _IsVisible; }
            set { _IsVisible = value; if (value) { System.Windows.Media.ImageSource imgs = SmallImageSource; imgs = ImageSource; imgs = null; } }
        }
        public bool SmallIsLoaded {
            get { return (_SmallImageSource != null); }
        }
        public bool BigIsLoaded
        {
            get { return (_BigImageSource != null); }
        }
        public bool ImageIsLoaded
        {
            get { return (_ImageSource != null); }
        }
        private string FixUrl(string in_id)
        {
            return FixUrl(in_id, "med");
        }
        private string FixUrl(string in_id,string size)
        {
            if (iss.Contains("receiverURL"))
            {
                return iss["receiverURL"] + "DownloadPhoto.ashx?size=" + size + "&path=" + in_id;
            }
            return in_id;
        }

        public string BigImageUrl {
            get { return id.StartsWith("file") ? FixUrl(id,"big") : "http://farm" + farm + ".static.flickr.com/" + server + "/" + id + "_" + secret + "_b.jpg"; }
        }

        public string SmallImageUrl
        {
            get { return id.StartsWith("file") ? FixUrl(id,"small") : "http://farm" + farm + ".static.flickr.com/" + server + "/" + id + "_" + secret + "_s.jpg"; }
        }
        public string ImageUrl
        {
            get { return id.StartsWith("file") ? FixUrl(id) : "http://farm" + farm + ".static.flickr.com/" + server + "/" + id + "_" + secret + ".jpg"; }
        }
        private ImageSource _BigImageSource { get; set; }
        public ImageSource BigImageSource {
            get{
                if(_BigImageSource==null){
                    _BigImageSource = (ImageSource)isc.ConvertFromString(BigImageUrl);
                }
                return _BigImageSource;
            }
            set{_BigImageSource=value;}
        }
        private ImageSource _SmallImageSource { get; set; }
        public ImageSource SmallImageSource
        {
            get
            {
                if (_SmallImageSource == null)
                {
                    if (id.Length > 0 && !wcSI.IsBusy)
                    {
                        wcSI.OpenReadAsync(new Uri(SmallImageUrl), null);
                    }
                }
                return _SmallImageSource;
            }
            set { _SmallImageSource = value; }
        }
        private ImageSource _ImageSource { get; set; }
        public ImageSource ImageSource
        {
            get
            {
                if (_ImageSource == null)
                {
                    if (id.Length > 0 && !wc.IsBusy)
                    {   
                        wc.OpenReadAsync(new Uri(ImageUrl), null);
                    }
                }
                return _ImageSource;
            }
            set { _ImageSource = value; }
        }
        private string _owner { get; set; }
        public string GetOwner()
        {

            if (string.IsNullOrEmpty(_owner))
            {
                WebClient wcO = new WebClient() { AllowReadStreamBuffering = true };
                wcO.DownloadStringAsync(new Uri("http://api.flickr.com/services/rest/?method=flickr.people.getInfo&api_key=737ed31f0208c8c4d2ec2bdf1ccfb41a&user_id=" + owner + "&format=json&nojsoncallback=1"));
                wcO.DownloadStringCompleted += (a, b) =>
                {
                    if (b.Result != null)
                    {
                        string json = b.Result;
                        JObject wlbobj = JObject.Parse(json);
                        if (wlbobj != null && wlbobj["person"] != null)
                        {
                            _owner = wlbobj["person"]["username"]["_content"].ToString();
                        }
                    }
                };
            }

            return _owner;
        }

    }
}
