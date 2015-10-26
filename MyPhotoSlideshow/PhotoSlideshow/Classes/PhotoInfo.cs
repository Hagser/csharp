using System;
using System.Windows.Media;
using System.Net;
using System.IO;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;

namespace MyPhotoSlideshow.Classes
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
        dynamic _obj;
        public PhotoInfo(dynamic obj,bool bVisible)
        {
            _obj = obj;
            wc.DownloadProgressChanged += (a, b) =>
                {
                    OnDownloadProgressChanged(b);
                };
            wc.OpenReadCompleted += (a, b) =>
            {
                BitmapSource bs = (BitmapSource)isc.ConvertFromString(SmallImageUrl);
                if (b.Error == null && b.Result.Length > 0)
                {
                    bs.SetSource(b.Result);
                    _ImageSource = bs;
                    OnImageSourceIsLoaded(EventArgs.Empty);
                }
            };

            wcSI.OpenReadCompleted += (a, b) =>
            {
                BitmapSource bs = (BitmapSource)isc.ConvertFromString(SmallImageUrl);
                if (b.Error == null && b.Result.Length>0)
                {
                    bs.SetSource(b.Result);
                    _SmallImageSource = bs;
                    OnSmallImageSourceIsLoaded(EventArgs.Empty);
                }
            };
            if (bVisible)
            {
                //System.Threading.Thread.Sleep(1);
                wcSI.OpenReadAsync(new Uri(SmallImageUrl), null);
            }
        }
        public string lat { get { return _obj.lat; } set { } }
        public string lon { get { return _obj.lon; } set { } }
        public string title { get { return _obj.title; } set { } }
        public string link { get { return _obj.link; } set { } }

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
            get { return _obj.link; }
        }

        public string SmallImageUrl
        {
            get { return ("" + _obj.thumbnail.url).Replace("&amp;", "&"); }
        }
        public string ImageUrl
        {
            get { return _obj.link; }
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
                    if (SmallImageUrl!="" && !wcSI.IsBusy)
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
                    if (_obj.link!="" && !wc.IsBusy)
                    {   
                        wc.OpenReadAsync(new Uri(ImageUrl), null);
                    }
                }
                return _ImageSource;
            }
            set { _ImageSource = value; }
        }

    }
}
