using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Collections.ObjectModel;

namespace MyOpmlPhotoViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            photocanvas.LayoutUpdated += new EventHandler(photocanvas_LayoutUpdated);

            ScrollViewer sv = (photocanvas.Parent as ScrollViewer);
            sv.ScrollChanged += new ScrollChangedEventHandler(sv_ScrollChanged);
             
        }

        void sv_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            foreach (Border img in photocanvas.Children)
            {
                if (img != null)
                {
                    Image photo = (img.Child as Image);
                    if (photo != null && photo.Source == null && photo.Tag != null)
                    {
                        ScrollViewer sv = (photocanvas.Parent as ScrollViewer);
                        double dv = sv.VerticalOffset;
                        double dh = sv.ActualHeight;
                        double mt = double.Parse(img.GetValue(Canvas.TopProperty).ToString());
                        if (mt >= dv && mt < (dh + dv + img.ActualHeight))
                        {
                            string url = photo.Tag.ToString();
                            ImageSourceConverter isc = new ImageSourceConverter();
                            photo.Source = isc.ConvertFromString(url) as ImageSource;
                        }
                    }
                }
            }
        }
        bool canvasUpdating = false;
        double pch = 0;
        double pcw = 0;
        void photocanvas_LayoutUpdated(object sender, EventArgs e)
        {
            if (canvasUpdating || ((photocanvas.Height == pch && photocanvas.Width == pcw) && (pch+pcw)>0))
                return;
            canvasUpdating = true;
            int irow = 0;
            int icol = 0;
            double ih = 0;
            Border oldimg = new Border();
            foreach (Border img in photocanvas.Children)
            {
                if (img != null)
                {
                    img.BorderThickness = new Thickness(1, 1, 0, 0);
                    if (((icol+1) * img.ActualWidth) > photocanvas.ActualWidth)
                    {
                        oldimg.BorderThickness = new Thickness(1, 1, 1, 0);
                        icol = 0;
                        irow++;
                    }
                    if (irow == 0)
                    {
                        img.BorderThickness = new Thickness(1, 1, 0, 0);
                    }
                    img.SetValue(Canvas.LeftProperty, (icol * img.ActualWidth));
                    icol++;
                    img.SetValue(Canvas.TopProperty, (irow * img.ActualHeight));

                    Image photo = (img.Child as Image);
                    if (photo != null && photo.Source == null && photo.Tag!=null)
                    {
                        ScrollViewer sv = (photocanvas.Parent as ScrollViewer);
                        double dv = sv.VerticalOffset;
                        double dh = sv.ActualHeight;
                        double mt = double.Parse(img.GetValue(Canvas.TopProperty).ToString());
                        if (mt>=dv && mt < (dh + dv+img.ActualHeight))
                        {
                            string url = photo.Tag.ToString();
                            ImageSourceConverter isc = new ImageSourceConverter();
                            photo.Source = isc.ConvertFromString(url) as ImageSource;
                        }
                    }
                    oldimg = img;
                }
                ih = img.ActualHeight;
            }
            photocanvas.Height = ((irow+1) * (ih + 1));
            pch = photocanvas.Height;
            pcw = photocanvas.Width;
            canvasUpdating = false;
        }
        public opml _opml = new opml();
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr ip = OCExt.MessageBox(0, "text", "capt", uint.MinValue);

            Uri u = new Uri("http://ubuntu/opml.php");
            XmlDocument xd = new XmlDocument();
            xd.Load(u.AbsoluteUri);
            foreach (XmlElement xe in xd.DocumentElement)
            { 
                if(xe.Name.Equals("body"))
                {
                    _opml.children.AddRange(getChildren(xe)); 
                    break;
                }
            }
            
            treeview.ItemsSource = _opml.children;
        }

        private ObservableCollection<outline> getChildren(XmlElement xe)
        {
            ObservableCollection<outline> list = new ObservableCollection<outline>();
            foreach (XmlNode xn in xe.ChildNodes)
            {
                outline ol = new outline() { text = getAttribute("text", xn), type = getAttribute("type", xn), url = getAttribute("url", xn) };
                ol.children.AddRange(getChildren(xn));
                list.Add(ol);
            }
            return list;
        }

        private ObservableCollection<outline> getChildren(XmlNode xn)
        {
            ObservableCollection<outline> list = new ObservableCollection<outline>();
            foreach (XmlNode xn2 in xn.ChildNodes)
            {                
                outline ol = new outline(){text=getAttribute("text",xn2),type=getAttribute("type",xn2),url=getAttribute("url",xn2)};
                ol.children.AddRange(getChildren(xn2));
                list.Add(ol);
            }
            return list;
        }

        private string getAttribute(string p, XmlNode xn2)
        {
            if (xn2.Attributes[p] != null && xn2.Attributes[p].Value!=null)
            {
                return xn2.Attributes[p].Value;
            }
            return "";
        }

        private void treeview_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null)
            {
                outline o = (e.NewValue as outline);
                if (o != null)
                {
                    if (o.url.StartsWith("http://"))
                    {
                        LoadPhotos(o.url);
                    }
                }
            }
        }

        private void LoadPhotos(string url)
        {
            photocanvas.Children.Clear();
            XmlDocument xd = new XmlDocument();
            xd.Load(url);
            int icnt = 0;
            foreach(XmlNode xn in xd.GetElementsByTagName("item"))
            {
                if (icnt > 120)
                    break;
                Border b = new Border();
                b.BorderThickness = new Thickness(1,1,0,0);
                b.BorderBrush = new SolidColorBrush(Colors.SteelBlue);
                Image img = new Image();
                img.MouseEnter += new MouseEventHandler(img_MouseEnter);
                img.MouseLeave += new MouseEventHandler(img_MouseLeave);
                img.Margin = new Thickness(2);
                string w = getAttribute("width", xn.LastChild);
                string h = getAttribute("height", xn.LastChild);
                if (!string.IsNullOrEmpty(w) && !string.IsNullOrEmpty(h))
                {
                    img.Width = int.Parse(w);
                    img.Height = int.Parse(h);
                    img.MinWidth = int.Parse(w);
                    img.MinHeight = int.Parse(h);
                }
                img.Tag = getAttribute("url", xn.LastChild);
                
                b.Child = img;
                photocanvas.Children.Add(b);
                icnt++;
            }
        }

        void img_MouseLeave(object sender, MouseEventArgs e)
        {
            canvasUpdating = false;
            Image img = (sender as Image);
            Border b = (img.Parent as Border);
            b.Background = new SolidColorBrush(Colors.Transparent);
        }

        void img_MouseEnter(object sender, MouseEventArgs e)
        {
            canvasUpdating = true;
            Image img = (sender as Image);
            Border b = (img.Parent as Border);
            b.Background = new SolidColorBrush(Colors.SteelBlue);
        }

    }

}
