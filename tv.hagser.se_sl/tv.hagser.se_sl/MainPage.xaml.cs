using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using tv.hagser.se_sl.TvWebService;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;


namespace tv.hagser.se_sl
{
    public partial class MainPage : UserControl
    {
        TvWebService.Service1SoapClient sc = new Service1SoapClient();
        Style sOrg;
        Style sAlt;
        Style sOld;
        Dictionary<string, ProgammeResponse> progresplist = new Dictionary<string, ProgammeResponse>();
        public MainPage()
        {
            InitializeComponent();
            LayoutRoot.Loaded += new RoutedEventHandler(LayoutRoot_Loaded);

            sOrg = this.Resources["sOrg"] as Style;
            sAlt = this.Resources["sAlt"] as Style;
            sOld = this.Resources["sOld"] as Style;
            sc.GetChannelsCompleted += new EventHandler<GetChannelsCompletedEventArgs>(sc_GetChannelsCompleted);

            sc.GetProgrammesCompleted += new EventHandler<GetProgrammesCompletedEventArgs>(sc_GetProgrammesCompleted);


            if (iss.Contains(strFavChanKey))
            {
                FavoriteChannels = (iss[strFavChanKey] as List<string>);
            }
            else
            { 
                FavoriteChannels=strMainChannels.Split(';').ToList<string>();
                iss.Add(strFavChanKey, FavoriteChannels);
                iss.Save();
            }
        }

        void sc_GetChannelsCompleted(object sender, GetChannelsCompletedEventArgs b)
        {
            if (b.Error == null)
            {
                if (b.Result != null)
                {
                    foreach (Channel chn in b.Result)
                    {
                        TextBlock tb = new TextBlock();
                        tb.Tag = chn.id;
                        tb.Text = chn.name;
                        Border bo = new Border();
                        bo.Child = tb;
                        bo.HorizontalAlignment = HorizontalAlignment.Stretch;
                        bo.VerticalAlignment = VerticalAlignment.Stretch;
                        bo.Width = 150;
                        bo.Margin = new Thickness(0);
                        if (FavoriteChannels.Contains(chn.id))
                        {
                            bo.Background = new SolidColorBrush(Colors.Green);
                        }
                        channels.Items.Add(bo);
                    }
                }
            }
        }

        void sc_GetProgrammesCompleted(object sender, GetProgrammesCompletedEventArgs b)
        {
            if (b.Error == null)
            {
                if (b.Result != null)
                {
                    programmes.Children.Clear();
                    if (chkMain.IsChecked.HasValue && chkMain.IsChecked.Value)
                    {
                        progresplist.Remove(strProgRespListMainKey);
                        progresplist.Add(strProgRespListMainKey, new ProgammeResponse() { resp = b, lastrun = DateTime.Now });
                        showMain(b);
                    }
                    else if (chkNow.IsChecked.HasValue && chkNow.IsChecked.Value)
                    {
                        progresplist.Remove(strProgRespListNowKey);
                        progresplist.Add(strProgRespListNowKey, new ProgammeResponse() { resp = b, lastrun = DateTime.Now });
                        showNow(b);
                    }
                    else if (chkChannel.IsChecked.HasValue && chkChannel.IsChecked.Value)
                    {
                        progresplist.Remove(strProgRespListChannelKey);
                        progresplist.Add(strProgRespListChannelKey, new ProgammeResponse() { resp = b, lastrun = DateTime.Now });
                        showChannel(b);
                    }
                    else if (chkMatrix.IsChecked.HasValue && chkMatrix.IsChecked.Value)
                    {
                        progresplist.Remove(strProgRespListMatrixKey);
                        progresplist.Add(strProgRespListMatrixKey, new ProgammeResponse() { resp = b, lastrun = DateTime.Now });
                        showMatrix(b);
                    }

                }
            }
        }

        private void showMatrix(GetProgrammesCompletedEventArgs b)
        {
            programmes.Children.Clear();

            programmes.Orientation = Orientation.Vertical;

            StackPanel sptime = new StackPanel();
            sptime.Orientation = Orientation.Horizontal;
            DateTime dtNow = DateTime.Now;

            TextBlock tblog = new TextBlock();
            tblog.Width = 44;
            tblog.Margin = new Thickness(0);
            tblog.Padding = tblog.Margin;
            sptime.Children.Add(tblog);
            DateTime dtMax = b.Result.programmes.Max(x => x.stop);
            while (dtNow < dtMax)
            {
                TextBlock tbtim = new TextBlock();
                tbtim.Margin = new Thickness(0);
                tbtim.Padding = tbtim.Margin;
                tbtim.Width = 6;
                tbtim.FontSize = 9;
                tbtim.TextAlignment = TextAlignment.Center;
                if (dtNow.Minute == 27 || dtNow.Minute == 57)
                {
                    dtNow = dtNow.AddMinutes(3);
                    tbtim.Text = dtNow.ToString("HH:mm");
                    tbtim.Width = 36;
                    dtNow = dtNow.AddMinutes(2);
                }
                dtNow = dtNow.AddMinutes(1);
                Border bor = inBorder(tbtim);
                bor.BorderThickness = new Thickness(0, 0, 0, 1);
                sptime.Children.Add(bor);
            }
            programmes.Children.Add(sptime);


            foreach (string chn in b.Result.programmes.OrderBy(x => FixToNum(x.channel)).Select(x => x.channel).Distinct())
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                sp.Margin = new Thickness(0);
                Image img = new Image();
                ImageSourceConverter isc = new ImageSourceConverter();
                ImageSource imagesource = isc.ConvertFromString("images/" + chn + ".png") as ImageSource;
                img.Source = imagesource;
                img.MaxHeight = 44;
                sp.Children.Add(inBorder(img));
                Programme lastprg = new Programme();
                foreach (Programme prg in b.Result.programmes.Where(x => x.channel.Equals(chn)).OrderBy(x => x.start))
                {
                    if (prg.stop > DateTime.Now)
                    {
                        if (DateTime.Now < prg.start && sp.Children.Count == 1)
                        {
                            Programme pause = new Programme()
                            {
                                start = DateTime.Now,
                                stop = prg.start,
                                title = "Sändningsuppehåll",
                                desc = "Uppehåll i sändning",
                                channel = prg.channel
                            };
                            Border bp = NewProg(pause);
                            AddToSp(sp, bp);
                        }
                        Border bo = NewProg(prg);
                        AddToSp(sp, bo);

                        lastprg = prg;
                    }
                }
                if (lastprg.stop < dtMax)
                {
                    Programme pause = new Programme()
                    {
                        start = lastprg.stop,
                        stop = dtMax,
                        title = "Sändningsuppehåll",
                        desc = "Uppehåll i sändning",
                        channel = lastprg.channel
                    };
                    Border bp = NewProg(pause);
                    AddToSp(sp, bp);
                }
                programmes.Children.Add(sp);
                
            }
        }

        private void showChannel(GetProgrammesCompletedEventArgs b)
        {
            programmes.Children.Clear();

            programmes.Orientation = Orientation.Vertical;
            foreach (Programme prg in b.Result.programmes)
            {

                MyProgramme myp = Translate(prg);
                TextBlock tb = new TextBlock() { Text = myp.start.ToString("HH:mm") + "-" + myp.stop.ToString("HH:mm") + " " + myp.title, Width = 400, FontSize = 9, Tag = myp };
                myp.Parent = tb;
                Border bo = inBorder(tb);
                if (myp.start <= DateTime.Now && DateTime.Now < myp.stop)
                {
                    tb.FontWeight = FontWeights.Bold;
                }
                if (myp.stop < DateTime.Now)
                {
                    bo.Style = sOld;
                }
                tb.MouseLeave += (x, z) =>
                {
                    if (myp.stop < DateTime.Now)
                    {
                        bo.Style = sOld;
                    }
                    else
                    {
                        bo.Style = sOrg;
                    }
                };
                tb.MouseEnter += (x, z) =>
                {
                    bo.Style = sAlt;
                    canvas.Visibility = Visibility.Collapsed;
                    showCanvas(z.GetPosition(LayoutRoot));
                    info.Text = myp.start.ToString("yyyy-MM-dd HH:mm") + "-" + myp.stop.ToString("HH:mm") + "\r" +
                        myp.title + (!string.IsNullOrEmpty(myp.subtitle) ? " - " + myp.subtitle : "") +
                        "\r" + myp.desc;
                };
                programmes.Children.Add(bo);
            }
        }

        private void showNow(GetProgrammesCompletedEventArgs b)
        {
            programmes.Children.Clear();

            programmes.Orientation = Orientation.Vertical;
            foreach (Programme prg in b.Result.programmes.OrderBy(x => FixToNum(x.channel)))
            {
                MyProgramme myp = Translate(prg);
                if (myp.start <= DateTime.Now && DateTime.Now < myp.stop)
                {
                    StackPanel sp = new StackPanel();
                    sp.Orientation = Orientation.Horizontal;
                    sp.Margin = new Thickness(0);
                   
                    Image img = new Image();
                    ImageSourceConverter isc = new ImageSourceConverter();
                    ImageSource imagesource = isc.ConvertFromString("images/" + prg.channel + ".png") as ImageSource;
                    img.Source = imagesource;
                    img.MaxHeight = 44;

                    Border boimg = inBorder(img);
                    boimg.MinWidth = 44;
                    sp.Children.Add(boimg);
                    
                    /*
                    Border bochn = inBorder(new TextBlock() { Text = myp.channelname, Width = 100 });
                    sp.Children.Add(bochn);
                    */

                    TextBlock tb = new TextBlock() { Text = myp.start.ToString("HH:mm") + "-" + myp.stop.ToString("HH:mm") + " " + myp.title, Width = 400, FontSize = 9, Tag = myp };
                    myp.Parent = tb;
                    Border bo = inBorder(tb);
                    if (myp.stop < DateTime.Now)
                    {
                        bo.Style = sOld;
                    }
                    tb.MouseLeave += (x, z) =>
                    {
                        if (myp.stop < DateTime.Now)
                        {
                            bo.Style = sOld;
                        }
                        else
                        {
                            bo.Style = sOrg;
                        }
                    };
                    tb.MouseEnter += (x, z) =>
                    {
                        bo.Style = sAlt;
                        canvas.Visibility = Visibility.Collapsed;
                        showCanvas(z.GetPosition(LayoutRoot));

                        info.Text = myp.start.ToString("yyyy-MM-dd HH:mm") + "-" + myp.stop.ToString("HH:mm") + "\r" +
                            myp.title + (!string.IsNullOrEmpty(myp.subtitle) ? " - " + myp.subtitle : "") +
                            "\r" + myp.desc;
                    };
                    sp.Children.Add(bo);

                    programmes.Children.Add(sp);
                }
            }
        }

        private void showMain(GetProgrammesCompletedEventArgs b)
        {
            programmes.Children.Clear();

            programmes.Orientation = Orientation.Horizontal;
            int imin = 9999;
            /*
            foreach (string chn in b.Result.programmes.Select(x=>x.channel).Distinct())
            {
                int l = b.Result.programmes.Count(x => x.channel == chn);
                imin = Math.Min(imin, l);
            }
            */
            List<StackPanel> sps = new List<StackPanel>();

            foreach (string chn in b.Result.programmes.OrderBy(x => FixToNum(x.channel)).Select(x => x.channel).Distinct())
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Vertical;
                sp.Margin = new Thickness(10);

                Image img = new Image();
                ImageSourceConverter isc = new ImageSourceConverter();
                ImageSource imagesource = isc.ConvertFromString("images/" + chn + ".png") as ImageSource;
                img.Source = imagesource;
                img.HorizontalAlignment = HorizontalAlignment.Left;
                img.MaxHeight = 44;
                Border bimg = inBorder(img);
                bimg.BorderThickness = new Thickness(1);
                sp.Children.Add(bimg);
                var list = b.Result.programmes.Where(x => x.channel.Equals(chn)).OrderBy(x => x.start);
                int icnt = 0;
                
                foreach (Programme prg in list.Where(x=>!x.title.Contains("Sändningsuppehåll")))
                {
                    MyProgramme myp = Translate(prg);

                    TextBlock tb = new TextBlock() { Text = myp.start.ToString("HH:mm") + " " + myp.title, Width = 200,FontFamily=new FontFamily("Verdana"), FontSize = 12, Tag = myp };
                    myp.Parent = tb;

                    Border bo = inBorder(tb);
                    bo.BorderThickness = new Thickness(1, 0, 1, 1);
                    
                    if (myp.start <= DateTime.Now && DateTime.Now < myp.stop)
                    {
                        tb.FontWeight = FontWeights.Bold;
                    }
                    if (myp.stop < DateTime.Now)
                    {
                        bo.Style = sOld;
                    }
                    tb.MouseLeave += (x, z) =>
                    {
                        if (myp.stop < DateTime.Now)
                        {
                            bo.Style = sOld;
                        }
                        else
                        {
                            bo.Style = sOrg;
                        }
                    };
                    tb.MouseEnter += (x, z) =>
                    {
                        bo.Style = sAlt;
                        canvas.Visibility = Visibility.Collapsed;
                        showCanvas(z.GetPosition(LayoutRoot));

                        info.Text = myp.start.ToString("yyyy-MM-dd HH:mm") + "-" + myp.stop.ToString("HH:mm") + "\r" +
                            myp.title + (!string.IsNullOrEmpty(myp.subtitle) ? " - " + myp.subtitle : "") +
                            "\r" + myp.desc;
                    };
                    icnt++;

                    if (icnt == list.Count() || icnt==imin)
                    {
                        bo.BorderThickness = new Thickness(1,0,1,1);
                    }
                    AddToSp(sp, bo);
                    if (icnt == imin)
                    {
                        break;
                    }
                }
                sps.Add(sp);
            }
            programmes.Orientation = Orientation.Vertical;
            List<StackPanel> spvs = new List<StackPanel>();
            string strPerRow = (programmes.ActualWidth / 220).ToString();
            int numperrows = int.Parse(strPerRow.Contains(",")?strPerRow.Substring(0,strPerRow.IndexOf(",")):strPerRow.Contains(".")?strPerRow.Substring(0,strPerRow.IndexOf(".")):strPerRow);
            double cnt = sps.Count;
            //int sqrt = int.Parse(Math.Round((Math.Sqrt(cnt)+1), 0).ToString());
            int sqrt = int.Parse(Math.Round((cnt/numperrows), 0).ToString());
            for (int ix = 0; ix < sqrt; ix++)
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                sp.Margin = new Thickness(0);
                spvs.Add(sp);
            }
            int ispicnt = 0;
            int ispvcnt = 0;
            foreach (StackPanel spi in sps)
            {
                if (ispicnt > 0 && ispicnt>=numperrows) //ispicnt % sqrt == 0)
                {
                    ispicnt = 0;
                    ispvcnt++;
                }
                StackPanel spv = spvs[ispvcnt];
                spv.Children.Add(spi);
                ispicnt++;
            }

            foreach (StackPanel spv in spvs)
            {
                programmes.Children.Add(spv);
            }
            programmes.HorizontalAlignment = HorizontalAlignment.Stretch;
            programmes.VerticalAlignment = VerticalAlignment.Stretch;
        }

        private int FixToNum(string p)
        {
            char[] chValid = "0123456789".ToCharArray();
            char[] cValue = p.Split('.')[0].Replace("sju","7").ToCharArray();
            char[] nValue = cValue.Where(x => chValid.Contains(x)).ToArray<char>();
            int result=0;
            if (int.TryParse(new string(nValue), out result))
            {
                return result;
            }            
            return 99;
        }

        private void AddToSp(StackPanel sp, Border bp)
        {
            if ((bp.Child as TextBlock).Width < 2)
            {
                bp.BorderThickness = new Thickness(0);
            }
            sp.Children.Add(bp);
        }
        private Border NewProg(Programme prg)
        {
            MyProgramme myp = Translate(prg);
            TextBlock tb = new TextBlock() { Text = myp.title, Width = Math.Round((myp.Width>1?myp.Width-1:myp.Width),0), FontSize = 9, Tag = myp };
            myp.Parent = tb;
            if (myp.start < DateTime.Now && myp.stop > DateTime.Now)
            {
                tb.Width = Math.Round(new TimeSpan(myp.stop.Ticks - DateTime.Now.Ticks).TotalMinutes, 0) * 6;
                tb.Width = Math.Round((tb.Width > 1 ? tb.Width - 1 : tb.Width), 0);
            }
            tb.HorizontalAlignment = HorizontalAlignment.Stretch;
            tb.VerticalAlignment = VerticalAlignment.Stretch;
            tb.TextAlignment = TextAlignment.Center;
            Border bo = inBorder(tb);
            if (myp.start <= DateTime.Now && DateTime.Now < myp.stop)
            {
                tb.FontWeight = FontWeights.Bold;
            }
            if (myp.stop < DateTime.Now)
            {
                bo.Style = sOld;
            }
            tb.MouseLeave += (x, z) =>
            {
                if (myp.stop < DateTime.Now)
                {
                    bo.Style = sOld;
                }
                else
                {
                    bo.Style = sOrg;
                }
            };
            tb.MouseEnter += (x, z) =>
            {
                bo.Style = sAlt;
                canvas.Visibility = Visibility.Collapsed;
            };
            tb.MouseLeftButtonDown += (c, d) =>
            {
                showCanvas(d.GetPosition(LayoutRoot));
                info.Text = myp.start.ToString("yyyy-MM-dd HH:mm") + "-" + myp.stop.ToString("HH:mm") + "\r" +
                    myp.title + (!string.IsNullOrEmpty(myp.subtitle) ? " - " + myp.subtitle : "") + 
                    "\r" + myp.desc;
            };
            return bo;
        }
        private void showCanvas(Point p)
        {
//            infoimg.SetValue(Canvas.LeftProperty, p.X + (border.Child as TextBlock).Width+4); //Left
//            infoimg.SetValue(Canvas.TopProperty, p.Y-26); //Top
            infoimg.SetValue(Canvas.LeftProperty, p.X + 6); //Left
            infoimg.SetValue(Canvas.TopProperty, p.Y - 26); //Top

            border.SetValue(Canvas.LeftProperty, p.X+10); //Left
            border.SetValue(Canvas.TopProperty, p.Y-20); //Top
            canvas.Visibility = Visibility.Visible;
        }
        string strFavChanKey = "strFavChanKey_20120129";
        IsolatedStorageSettings iss = IsolatedStorageSettings.ApplicationSettings;
        List<string> FavoriteChannels = new List<string>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (channels.SelectedItem != null)
            {
                Border b = channels.SelectedItem as Border;
                TextBlock tb = b.Child as TextBlock;
                if (tb != null)
                {
                    if (!FavoriteChannels.Contains(tb.Tag.ToString()))
                    {
                        FavoriteChannels.Add(tb.Tag.ToString());
                        iss[strFavChanKey] = FavoriteChannels;
                        iss.Save();
                        var chk = LayoutRoot.Children.Where(x => x.GetType() == typeof(RadioButton) && (x as RadioButton).IsChecked.HasValue && (x as RadioButton).IsChecked.Value).FirstOrDefault();
                        if (chk != null)
                        {
                            chk_Checked(chk, null);
                        }
                    }
                }
            }
        }

        Border inBorder(UIElement el)
        {
            Border bo = new Border();
            bo.BorderBrush = border.BorderBrush;
            bo.BorderThickness = new Thickness(0, 0, 1, 1);
            bo.Child = el;
            bo.Margin = new Thickness(0);
            bo.Padding = bo.Margin;
            return bo;
        }
        void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            sc.GetChannelsAsync();
            chkMatrix.IsChecked = true;
        }

        private void channels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (chkChannel.IsChecked.HasValue && chkChannel.IsChecked.Value)
            {
                sc.GetProgrammesAsync(new GetProgrammesRequest()
                {
                    programme = new Programme()
                    {
                        channel = ((channels.SelectedItem as Border).Child as TextBlock).Tag.ToString(),
                        start = DateTime.Now.AddHours(-6),
                        stop = DateTime.Now.AddHours(8)
                    }
                }
                );
            }
        }
        private MyProgramme Translate(Programme prg)
        {
            return new MyProgramme(sOld)
            {
                categories = prg.categories,
                channel = prg.channel,
                channelname = prg.channelname,
                credits = prg.credits,
                date = prg.date,
                desc = prg.desc,
                episodenum = prg.episodenum,
                start = prg.start,
                stop = prg.stop,
                subtitle = prg.subtitle,
                title = prg.title
            };
        }

        private void chk_Checked(object sender, RoutedEventArgs e)
        {
            if (sender != null && (sender as RadioButton) != null)
            {
                bStopTimer = true;
                canvas.Visibility = Visibility.Collapsed;
                rect.Visibility = Visibility.Collapsed;

                switch ((sender as RadioButton).Name)
                {
                    case "chkMain":
                        getMain();
                        break;
                    case "chkNow":
                        getNow();
                        break;
                    case "chkMatrix":
                        rect.Visibility = Visibility.Visible;
                        getMatrix();
                        break;
                    case "chkChannel":
                        getChannel();
                        break;
                }
            }
        }
        string strProgRespListMainKey = "main";
        private void getMain()
        {

            if (progresplist.ContainsKey(strProgRespListMainKey) && progresplist[strProgRespListMainKey].lastrun.MinutesAgo() < 5)
            {
                showMain(progresplist[strProgRespListMainKey].resp);
            }
            else
            {
                System.Windows.Threading.DispatcherTimer distim = new System.Windows.Threading.DispatcherTimer();
                bStopTimer = false;
                distim.Tick += (a, b) =>
                {
                    DateTime dt = DateTime.Now;
                    distim.Interval = new TimeSpan(0, 10, 0);
                    if (bStopTimer)
                    {
                        distim.Stop();
                        bStopTimer = false;
                        return;
                    }

                    sc.GetProgrammesAsync(new GetProgrammesRequest()
                    {
                        programme = new Programme()
                        {
                            start = DateTime.Today.AddHours(6),
                            stop = DateTime.Today.AddHours(30),
                            channel = FavoriteChannels.Concat(strMainChannels.Split(';')).Distinct().ToList().Flatten()
                        }
                    }
                    );

                };
                distim.Interval = new TimeSpan(0, 0, 1);
                distim.Start();
            }
        }
        string strMainChannels = "svt1.svt.se;svt2.svt.se;tv3.viasat.se;tv4.se;kanal5.se;tv6.viasat.se;sjuan.tv4.se;tv8.se;kanal9.se;tv10.viasat.se;tv11.tv4.se";
        string strProgRespListChannelKey = "channel";
        private void getChannel()
        {
            if (progresplist.ContainsKey(strProgRespListChannelKey) && progresplist[strProgRespListChannelKey].lastrun.MinutesAgo() < 5)
            {
                showChannel(progresplist[strProgRespListChannelKey].resp);
            }
            else
            {
                System.Windows.Threading.DispatcherTimer distim = new System.Windows.Threading.DispatcherTimer();
                bStopTimer = false;
                distim.Tick += (a, b) =>
                {
                    DateTime dt = DateTime.Now;
                    distim.Interval = new TimeSpan(0, 10, 0);
                    if (bStopTimer)
                    {
                        distim.Stop();
                        bStopTimer = false;
                        return;
                    }
                    if (channels.SelectedItem != null)
                    {
                        sc.GetProgrammesAsync(new GetProgrammesRequest()
                        {
                            programme = new Programme()
                            {
                                channel = ((channels.SelectedItem as Border).Child as TextBlock).Tag.ToString(),
                                start = DateTime.Now.AddHours(-6),
                                stop = DateTime.Now.AddHours(8)
                            }
                        }
                        );
                    }
                };
                distim.Interval = new TimeSpan(0, 0, 1);
                distim.Start();
            }
        }
        bool bStopTimer = false;

        string strProgRespListMatrixKey = "matrix";

        private void getMatrix()
        {
            if (progresplist.ContainsKey(strProgRespListMatrixKey) && progresplist[strProgRespListMatrixKey].lastrun.MinutesAgo() < 5)
            {
                showMatrix(progresplist[strProgRespListMatrixKey].resp);
            }
            else
            {
                System.Windows.Threading.DispatcherTimer distim = new System.Windows.Threading.DispatcherTimer();
                System.Windows.Threading.DispatcherTimer linetim = new System.Windows.Threading.DispatcherTimer();
                bStopTimer = false;
                distim.Tick += (a, b) =>
                {
                    DateTime dt = DateTime.Now;
                    distim.Interval = new TimeSpan(0, 40, 0);
                    if (bStopTimer)
                    {
                        distim.Stop();
                        linetim.Stop();
                        bStopTimer = false;
                        return;
                    }
                    sc.GetProgrammesAsync(new GetProgrammesRequest()
                    {
                        programme = new Programme()
                        {
                            start = dt.AddHours(-2),
                            stop = dt.AddHours(8),
                            channel = FavoriteChannels.Concat(strMainChannels.Split(';')).Distinct().ToList().Flatten()
                        }
                    }
                    );
                };
                linetim.Tick += (a, b) => {
                    if (bStopTimer)
                    {
                        distim.Stop();
                        linetim.Stop();
                        bStopTimer = false;
                        return;
                    }

                    LinearGradientBrush lgb = rect.Fill as LinearGradientBrush;
                    
                    GradientStop gs1 = lgb.GradientStops[1];
                    GradientStop gs2 = lgb.GradientStops[2];

                    double ho = scrollviewer.HorizontalOffset;
                    double sw = scrollviewer.ScrollableWidth;

                    double ds = 0.1;
                    if (ho > 0 && sw > 0)
                    {
                        ds = ds - (ho / sw);
                    }

                    gs1.Offset = ds;


                    gs2.Offset = gs1.Offset - 0.006;
                    
                };
                distim.Interval = new TimeSpan(0, 0, 1);
                distim.Start();
                linetim.Interval = new TimeSpan(0, 0, 1);
                linetim.Start();
            }

            
        }

        string strProgRespListNowKey = "now";
        private void getNow()
        {

            if (progresplist.ContainsKey(strProgRespListNowKey) && progresplist[strProgRespListNowKey].lastrun.MinutesAgo() < 5)
            {
                showNow(progresplist[strProgRespListNowKey].resp);
            }
            else
            {
                System.Windows.Threading.DispatcherTimer distim = new System.Windows.Threading.DispatcherTimer();
                bStopTimer = false;
                distim.Tick += (a, b) =>
                {
                    DateTime dt = DateTime.Now;
                    distim.Interval = new TimeSpan(0, 10, 0);
                    if (bStopTimer)
                    {
                        distim.Stop();
                        bStopTimer = false;
                        return;
                    }

                    sc.GetProgrammesAsync(new GetProgrammesRequest()
                    {
                        programme = new Programme()
                        {
                            start = DateTime.Now.AddMinutes(+1),
                            stop = DateTime.Now.AddMinutes(+2),
                            channel = FavoriteChannels.Concat(strMainChannels.Split(';')).Distinct().ToList().Flatten()
                        }
                    }
                    );

                };
                distim.Interval = new TimeSpan(0, 0, 1);
                distim.Start();
            }
        }

        private void ScrollViewer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            canvas.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (channels.SelectedItem != null)
            {
                Border b = channels.SelectedItem as Border;
                TextBlock tb = b.Child as TextBlock;
                if (tb != null)
                {
                    if (FavoriteChannels.Contains(tb.Text))
                    {
                        FavoriteChannels.Remove(tb.Text);
                        iss[strFavChanKey] = FavoriteChannels;
                        iss.Save();
                        var chk = LayoutRoot.Children.Where(x => x.GetType() == typeof(RadioButton) && (x as RadioButton).IsChecked.HasValue && (x as RadioButton).IsChecked.Value).FirstOrDefault();
                        if (chk != null)
                        {
                            chk_Checked(chk, null);
                        }
                    }
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }



    }
    public class MyProgramme:Programme
    {
        public MyProgramme(Style oldStyle)
        {
            if(DateTime.Now<start ||DateTime.Now<stop)
            {
                distim.Tick += (a, b) =>
                {
                    if (DateTime.Now > stop)
                    {
                        if (Parent != null)
                        {
                            Parent.FontWeight = FontWeights.Normal;
                            //Parent.Foreground = new SolidColorBrush(Colors.LightGray);
                            (Parent.Parent as Border).Style = oldStyle;
                        }
                        distim.Stop();
                    }
                    else if (DateTime.Now > start && DateTime.Now < stop)
                    {
                        if (Parent != null)
                        {
                            Parent.FontWeight = FontWeights.Bold;
                        }                    
                    }
                };
                distim.Interval = DateTime.Now > start ? new TimeSpan(stop.Ticks - DateTime.Now.Ticks) : new TimeSpan(start.Ticks - DateTime.Now.Ticks);
                distim.Start();
            }
        }
        private System.Windows.Threading.DispatcherTimer distim = new System.Windows.Threading.DispatcherTimer();
        public double Width { get {return Math.Round(new TimeSpan(this.stop.Ticks-this.start.Ticks).TotalMinutes,0)*6; } }
        public TextBlock Parent { get; set; }
    }
    public class ProgrammeChannelComparer : IEqualityComparer<Programme>
    {

        public bool Equals(Programme x, Programme y)
        {
            return x.channel == y.channel;
        }
        public int GetHashCode(Programme obj)
        {
            return obj.GetHashCode();

        }

    }
    public class ProgammeResponse
    {
        public GetProgrammesCompletedEventArgs resp { get; set; }
        public DateTime lastrun { get;set; }
    }
    public static class DateTimeExt
    { 
        public static double MinutesAgo(this DateTime dt)
        {
            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - dt.Ticks);
            return ts.TotalMinutes;
        }
    }
}
