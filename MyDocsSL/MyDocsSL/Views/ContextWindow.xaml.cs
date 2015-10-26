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

namespace MyDocsSL.Views
{
    public partial class ContextWindow : ChildWindow
    {
        FileService.FileServiceClient fsc = new FileService.FileServiceClient();
        ListBox lb = new ListBox();
        string filename = "";
        public ContextWindow(string strLink)
        {
            InitializeComponent();

            StackPanel sp = new StackPanel();
            sp.Orientation=Orientation.Vertical;
            sp.VerticalAlignment = VerticalAlignment.Stretch;
            sp.HorizontalAlignment = HorizontalAlignment.Stretch;
            
            HyperlinkButton btn = new HyperlinkButton();
            btn.NavigateUri = new Uri(strLink);
            btn.Content = "Download";
            btn.FontSize = 24;
            btn.Margin = new Thickness(5);
            btn.VerticalAlignment = VerticalAlignment.Center;
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            sp.Children.Add(btn);

            Button btnCopy = new Button();
            btnCopy.Content = "Copy Link";
            btnCopy.Tag = strLink;
            btnCopy.FontSize = 24;
            btnCopy.Margin = new Thickness(5);
            btnCopy.VerticalAlignment = VerticalAlignment.Center;
            btnCopy.HorizontalAlignment = HorizontalAlignment.Center;
            btnCopy.Click += new RoutedEventHandler(btnCopy_Click);
            sp.Children.Add(btnCopy);

            lb.ItemTemplate = this.Resources["VersionTemplate"] as DataTemplate;

            lb.VerticalAlignment = VerticalAlignment.Stretch;
            lb.HorizontalAlignment = HorizontalAlignment.Stretch;

            fsc.ListVersionsCompleted += (a, b) => {
                if (b.Result != null)
                {
                    lb.Items.Clear();
                    foreach (FileService.Fileinfo fi in b.Result)
                    {
                        lb.Items.Add(fi);
                    }
                }
            };
            filename = strLink.Split('=')[1];
            fsc.ListVersionsAsync(filename);
            sp.Children.Add(lb);
            LayoutRoot.Children.Add(sp);

            
        }

        void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string file = (sender as Button).Tag.ToString();
                Clipboard.SetText(file);
            }
            catch { }
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            string file = (sender as Button).Tag.ToString();
            
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            string file = (sender as Button).Tag.ToString();
            if (MessageBox.Show("Are you sure you want to delete the selected version?", "Delete", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                
                fsc.DeleteVersionCompleted += (a, b) =>
                {
                    fsc.ListVersionsAsync(filename);
                };
                fsc.DeleteVersionAsync(file, null);
                
            }

        }

    }
}

