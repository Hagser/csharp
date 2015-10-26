using System.Windows.Controls;
using System.Collections.ObjectModel;
using PhotoSlideshow.Classes;
using System.Windows.Media;
using System;
using System.Windows.Browser;
using System.Net;
using System.IO;
using System.Linq;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Threading;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Json;
using Newtonsoft.Json.Linq;


namespace PhotoSlideshow
{
    public partial class PhotoNavigator : UserControl
    {
        public delegate void ImageIsChangedEventHandler(object sender, ImageIsChangedEventArgs e);
        public delegate void PlayPauseEventHandler(object sender, PlayPauseEventArgs e);
        public delegate void SpeedChangedEventHandler(object sender, SpeedEventArgs e);
        public delegate void MouseEnterImageEventHandler(object sender, MouseEnterImageEventArgs e);
        public delegate void MouseLeaveImageEventHandler(object sender, MouseLeaveImageEventArgs e);
        public delegate void MouseDownImageEventHandler(object sender, MouseDownImageEventArgs e);


        public Size ThumbnailSize { get; set; }
        public bool AutoGotoNextDate { get; set; }
        public bool RotateOnLoading { get; set; }
        public ImagePlacements ImagePlacements { get; set; }
        

        private string receiverURL = "";
        private Dispatcher UIDispatcher;
        private string screenWidth = "";
        private string screenHeight = "";
        string rssURL = "";
        int iPosSlide = -1;
        int iBigImageCnt = 0;
        DispatcherTimer dt = new DispatcherTimer();

        ObservableCollection<Image> images = new ObservableCollection<Image>();
        public PhotoNavigator()
        {
            InitializeComponent();
            
            UIDispatcher = this.Dispatcher;
            rssURL = HtmlPage.Document.GetElementById("rssURL").GetAttribute("value");
            receiverURL = HtmlPage.Document.GetElementById("receiverURL").GetAttribute("value");
            screenWidth = HtmlPage.Document.GetElementById("screenWidth").GetAttribute("value");
            screenHeight = HtmlPage.Document.GetElementById("screenHeight").GetAttribute("value");

            //ImageStackPanel.Width = LayoutRoot.ActualWidth; //int.Parse(screenWidth) - 4;
            ImageWrapPanel.Width = LayoutRoot.ActualWidth;
            //ISPScroller.MaxHeight = int.Parse(screenHeight) - 700;

            changeDate(DateTime.Today.AddDays(-2));

            dt.Interval = new TimeSpan(0, 0, 0, 3, 0);
            dt.Tick += new EventHandler(dt_Tick);
            ThumbnailSize = new Size(75, 75);
            ImagePlacements = ImagePlacements.ZeroToEnd;
        }


        private void GetPhotos(DateTime dtMonth)
        {
            UriBuilder ub = new UriBuilder(receiverURL + "LocalPhotos.ashx");
            if(dtMonth>DateTime.MinValue)
            {
                ub.Query = string.Format("month={0}", dtMonth.ToString("yyyy-MM-dd"));
            }

        }

        private void GetImages(string in_rss)
        {
            UriBuilder ub = new UriBuilder(in_rss + "&format=json");
            
            WebClient wc = new WebClient();
            wc.OpenReadCompleted += (a, b) =>
                {
                    if (b.Result != null)
                    {
                        Stream s = (Stream)b.Result;
                        using (StreamReader sr = new StreamReader(s))
                        {
                            string json = sr.ReadToEnd().Replace("jsonFlickrApi(", "");
                            json = json.Substring(0, json.Length - 1);
                            JObject wlbobj = JObject.Parse(json);
                            if (wlbobj != null && wlbobj["photos"] != null)
                            {
                                foreach (JProperty obj in wlbobj["photos"])
                                {
                                    if (obj != null && obj.Name.Equals("photo"))
                                    {
                                        foreach (JObject p in obj.Value as JArray)
                                        {
                                            if (p != null)
                                            {
                                                string strImg = p["id"] + "#" + p["owner"] + "#" + p["secret"] + "#" + p["server"] + "#" + p["farm"] + "#" + p["title"];
                                                AddImage(strImg);
                                            }
                                        }

                                        UIDispatcher.BeginInvoke(delegate()
                                        {
                                            bIsChanging = false;
                                        });
                                    }
                                }
                            }
                        }
                        s.Close();
                        s.Dispose();
                    }
                };
            wc.OpenReadAsync(ub.Uri);
        }

        private void AddImage(string strImg)
        {
            if (!string.IsNullOrEmpty(strImg))
            {
                PhotoInfo pi = new PhotoInfo(strImg, true);
                if (pi.id.Length > 0)
                {
                    Image img = new Image { Name = pi.id, Width = ThumbnailSize.Width, Height = ThumbnailSize.Height, Margin = new Thickness(1), Stretch = System.Windows.Media.Stretch.Uniform };
                    img.Opacity = .01;
                    pi.DownloadProgressChanged += (a, b) =>
                    {
                        img.Opacity = b.ProgressPercentage * 1e-2;
                        if(RotateOnLoading)
                            img.RenderTransform = new RotateTransform() { Angle = b.ProgressPercentage * 3.6, CenterX = (img.Width / 2), CenterY = (img.Height / 2) };
                        System.GC.Collect();
                    };
                    pi.ImageSourceIsLoaded += (a, b) =>
                    {
                        img.Opacity = 1;
                        img.RenderTransform = null;
                        if (AutoGotoNextDate)
                        {
                            var list1 = ImageWrapPanel.Children.Cast<Image>().Where(x => x.Tag != null && x.Tag.GetType() == typeof(PhotoInfo));
                            if (list1.All(x => (x.Tag as PhotoInfo).ImageIsLoaded))
                                PlayPauseClick(Statuses.Playing);
                        }

                        System.GC.Collect();
                    };
                    pi.SmallImageSourceIsLoaded += (a, b) =>
                    {
                        img.Source = pi.SmallImageSource;
                        try
                        {
                            //ImageWrapPanel.Children.Add(img);
                            Random rnd = new Random(System.DateTime.Now.Millisecond);
                            double didx = double.Parse(ImageWrapPanel.Children.Count.ToString());
                            didx = Math.Round(didx / 2);
                            int idx = 0;
                            switch (ImagePlacements)
                            {
                                case ImagePlacements.Random:
                                    idx = rnd.Next(0, ImageWrapPanel.Children.Count); //int.Parse(didx.ToString());
                                    ImageWrapPanel.Children.Insert(idx, img);
                                    break;
                                case ImagePlacements.Split:
                                    idx = int.Parse(didx.ToString());
                                    ImageWrapPanel.Children.Insert(idx, img);
                                    break;
                                case ImagePlacements.ZeroToEnd:
                                    ImageWrapPanel.Children.Add(img);
                                    break;
                            
                            }
                                img.MouseLeftButtonDown += (a1, b1) =>
                                {
                                    WrapPanel spp = (WrapPanel)img.Parent;
                                    if (spp == null)
                                        return;
                                    int iThis = spp.Children.GetIndex(img);
                                    Image imgPrev = (iThis - 1) > 0 ? (Image)spp.Children.ElementAt<UIElement>(iThis - 1) : null;
                                    Image imgNext = (iThis + 1) < spp.Children.Count ? (Image)spp.Children.ElementAt<UIElement>(iThis + 1) : null;
                                    OnMouseDownImage(new MouseDownImageEventArgs()
                                    {
                                        image = img,
                                        photoinfo = pi,
                                        nextPhotoInfo = imgNext != null ? imgNext.Tag != null ? (PhotoInfo)imgNext.Tag : null : null,
                                        prevPhotoInfo = imgPrev != null ? imgPrev.Tag != null ? (PhotoInfo)imgPrev.Tag : null : null,
                                        point = b1.GetPosition(a1 as UIElement)
                                    });
                                };

                                img.MouseEnter += (a2, b2) =>
                                {
                                    WrapPanel spp = (WrapPanel)img.Parent;
                                    if (spp == null)
                                        return;
                                    int iThis = spp.Children.GetIndex(img);
                                    Image imgPrev = (iThis - 1) > 0 ? (Image)spp.Children.ElementAt<UIElement>(iThis - 1) : null;
                                    Image imgNext = (iThis + 1) < spp.Children.Count ? (Image)spp.Children.ElementAt<UIElement>(iThis + 1) : null;
                                    OnMouseEnterImage(new MouseEnterImageEventArgs()
                                    {
                                        image = img,
                                        photoinfo = pi,
                                        nextPhotoInfo = imgNext != null ? imgNext.Tag != null ? (PhotoInfo)imgNext.Tag : null : null,
                                        prevPhotoInfo = imgPrev != null ? imgPrev.Tag != null ? (PhotoInfo)imgPrev.Tag : null : null,
                                        point = b2.GetPosition(a2 as UIElement)
                                    });

                                };
                            }
                        
                        catch { }
                        ImageSource isi = pi.ImageSource;
                        
                    };
                    img.Tag = pi;

                    img.MouseLeave += (a, b) =>
                        {
                            OnMouseLeaveImage(new MouseLeaveImageEventArgs() { image = img, photoinfo = pi, point = b.GetPosition(a as UIElement) });                        
                        };

                    
                    
                    img.ImageFailed += new EventHandler<ExceptionRoutedEventArgs>(img_ImageFailed);
                }
            }
        }


        void img_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image isend = (Image)sender;
            openImage(isend);

        }

        private void openImage(Image isend)
        {
            if (isend.Clip != null)
            {
                Geometry g = isend.Clip;
                Rect re = g.Bounds;
                double b = re.Bottom;
                double t = re.Top;
                double l = re.Left;
                double r = re.Right;
            }
            if (isend.Tag != null)
            {
                PhotoInfo pi = (PhotoInfo)isend.Tag;
                WrapPanel spp = (WrapPanel)isend.Parent;
                int iThis = spp.Children.GetIndex(isend);
                Image imgPrev = iThis > 1 ? (Image)spp.Children.ElementAt<UIElement>(iThis - 1) : null;
                Image imgNext = iThis < spp.Children.Count - 1 ? (Image)spp.Children.ElementAt<UIElement>(iThis + 1) : null;

                OnImageIsChanged(new ImageIsChangedEventArgs() { photoinfo = pi, nextPhotoInfo = imgNext != null ? imgNext.Tag != null ? (PhotoInfo)imgNext.Tag : null : null, prevPhotoInfo = imgPrev != null ? imgPrev.Tag != null ? (PhotoInfo)imgPrev.Tag : null : null, });
            }
        }


        public event MouseDownImageEventHandler MouseDownImage;
        protected virtual void OnMouseDownImage(MouseDownImageEventArgs e)
        {
            if (MouseDownImage != null)
                MouseDownImage(this, e);
        }

        public event MouseLeaveImageEventHandler MouseLeaveImage;
        protected virtual void OnMouseLeaveImage(MouseLeaveImageEventArgs e)
        {
            if (MouseLeaveImage != null)
                MouseLeaveImage(this, e);
        }

        public event MouseEnterImageEventHandler MouseEnterImage;
        protected virtual void OnMouseEnterImage(MouseEnterImageEventArgs e)
        {
            if (MouseEnterImage != null)
                MouseEnterImage(this, e);
        }

        public event SpeedChangedEventHandler SpeedChanged;
        protected virtual void OnSpeedChanged(SpeedEventArgs e)
        {
            if (SpeedChanged != null)
                SpeedChanged(this, e);
        }

        public event ImageIsChangedEventHandler ImageIsChanged;
        protected virtual void OnImageIsChanged(ImageIsChangedEventArgs e)
        {
            if (ImageIsChanged != null)
                ImageIsChanged(this, e);
        }
        public event PlayPauseEventHandler PlayPauseChanged;
        protected virtual void OnPlayPauseChanged(PlayPauseEventArgs e)
        {
            if (PlayPauseChanged != null)
                PlayPauseChanged(this, e);
        }

        void img_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Image img = (Image)sender;
            img.Visibility = Visibility.Collapsed;
        }
        private int getSPSize(double p)
        {
            return int.Parse(Math.Round((p / 80), 0).ToString());
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            slidePrev();
        }

        private void slidePrev()
        {
            if (iPosSlide > 0)
            {
                iPosSlide--;
            }
            else
            {
                iPosSlide = getPhotoCount() - 1;
            }
            showImage(iPosSlide);
        }

        private void showImage(int iPos)
        {
            int iret = 0;

            //foreach (StackPanel sp in ImageStackPanel.Children.Cast<StackPanel>())
            //{
                foreach (Image img in ImageWrapPanel.Children.Cast<Image>())
                {
                    if (iPos == iret)
                    {
                        openImage(img);
                        break;
                    }
                    iret++;
                }
            /*
                if (iPos == iret)
                {
                    break;
                }*/
            //}
        }
        private int getPhotoCount()
        {
            int iret = ImageWrapPanel.Children.Count;
            /*
            foreach (StackPanel sp in ImageStackPanel.Children.Cast<StackPanel>())
            {
                iret += sp.Children.Count;
            }
            */
            return iret;
        }
        private void slideNext()
        {
            if (iPosSlide < (getPhotoCount() - 1))
            {
                iPosSlide++;
            }
            else
            {
                iPosSlide = 0;
                if(AutoGotoNextDate)
                    changeDate(currentDateTime.AddDays(1));
            }
            showImage(iPosSlide);
        }

        public void PlayPauseClick(Statuses status)
        {
            if (status == Statuses.Paused)
            {
                dt.Stop();
                btnPlayPause.Content = ">";
            }
            else if (status == Statuses.Playing)
            {
                dt.Start();
                btnPlayPause.Content = "||";
            }
            OnPlayPauseChanged(new PlayPauseEventArgs() { Status = status });
        }
        public void PlayPauseClick()
        {
            PlayPauseEventArgs ppe = new PlayPauseEventArgs();
            if (dt.IsEnabled)
            {
                dt.Stop();
                btnPlayPause.Content = ">";
                ppe.Status = Statuses.Paused;
            }
            else
            {
                dt.Start();
                btnPlayPause.Content = "||";
                ppe.Status = Statuses.Playing;
            }
            OnPlayPauseChanged(ppe);
        }
        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            PlayPauseClick();
        }

        void dt_Tick(object sender, EventArgs e)
        {
            slideNext();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            slideNext();
        }
        DateTime currentDateTime;
        private void changeDate(DateTime in_dt)
        {
            if (!currentDateTime.Equals(in_dt))
            {
                currentDateTime = in_dt;
                dpDate.Text = in_dt.ToString("yyyy-MM-dd");
                btnPlayPause.Focus();
                dt.Stop();
                btnPlayPause.Content = ">";
                OnPlayPauseChanged(new PlayPauseEventArgs() { Status = Statuses.Paused});
                /*foreach (Image img in ImageWrapPanel.Children.Cast<Image>())
                {
                    for (int i = 100; i > 0; i = i - 10)
                    {
                        img.Opacity = i * 1e-2;
                        img.RenderTransform = new RotateTransform() { Angle = i * 3.6, CenterX = (img.Width / 2), CenterY = (img.Height / 2) };
                        Thread.Sleep(1);
                    }
                    img.Opacity = 0;
                    img.RenderTransform = null;
                }*/
                ImageWrapPanel.Children.Clear();
                System.GC.Collect();
                iBigImageCnt = 0;
                iPosSlide = -1;
                dpDate.IsEnabled = false;

                string per_page = Math.Max(100, getPerPage()).ToString();
                if (!per_page.Equals("NaN"))
                    GetImages(rssURL.Replace("#per_page#", per_page) + "&date=" + dpDate.Text);
                dpDate.IsEnabled = true;
            }
        }

        private void ToggleFullScreen()
        {
            Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
        }

        private void ISPScroller_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ISPScroller.ScrollToVerticalOffset(ISPScroller.VerticalOffset - e.Delta);
        }

        private void imgFullScreen_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ToggleFullScreen();
        }

        private void dpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            changeDate(dpDate.SelectedDate.Value);
        }

        private void dpDate_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string strText = dpDate.Text;
            DateTime dtresult = new DateTime();
            if ((strText.Length == 6 || strText.Length == 10) && DateTime.TryParse(strText, out dtresult))
            {
                changeDate(dtresult);
            }
        }

        private void sInterval_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sInterval != null && dt != null)
            {
                double d = e.NewValue;
                //1 = sakta, typ 5 s
                //10 = snabbt typ 0.5
                double dr = Math.Round(((sInterval.Maximum + 1) - d) / 2, 1);
                string[] sdr = dr.ToString().Split(",".ToCharArray()[0]);
                int s = int.Parse(sdr[0]);
                int ms = sdr.Length > 1 ? int.Parse(sdr[1]) : 0;
                dt.Interval = new TimeSpan(0, 0, 0, s, ms * 100);
                OnSpeedChanged(new SpeedEventArgs() { Speed = d,SpeedTimeSpan =  dt.Interval});

            }
        }
        bool bIsChanging = false;
        private void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (bIsChanging)
                return;
            bIsChanging = true;
            ISPScroller.Height = e.NewSize.Height - 30;
            ISPScroller.Width = e.NewSize.Width;
            ImageWrapPanel.Width = e.NewSize.Width;
            string per_page = Math.Max(100, getPerPage()).ToString();
            if(!per_page.Equals("NaN"))
                GetImages(rssURL.Replace("#per_page#", per_page) + "&date=" + dpDate.Text);
        }

        private double getPerPage()
        {
            double dbl = (ISPScroller.Height * ISPScroller.Width) / (ThumbnailSize.Height * ThumbnailSize.Width);
            dbl = Math.Round(dbl) - 1;
            return dbl;
        }

        private void btnPrevDate_Click(object sender, RoutedEventArgs e)
        {
            changeDate(dpDate.SelectedDate.Value.AddDays(-1));
        }

        private void btnNextDate_Click(object sender, RoutedEventArgs e)
        {
            if (dpDate.SelectedDate.HasValue && dpDate.SelectedDate.Value < DateTime.Today.AddDays(-2))
            {
                changeDate(dpDate.SelectedDate.Value.AddDays(1));
            }
        }
        private bool _IsControlsVisible = true;
        public bool IsControlsVisible
        {
            get {
                return _IsControlsVisible;
            }
            set {
                _IsControlsVisible = value;
                SPControls.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                LayoutRoot.RowDefinitions[1].Height = value ? new GridLength(30) : new GridLength(0);
            }
        }

    }
}
