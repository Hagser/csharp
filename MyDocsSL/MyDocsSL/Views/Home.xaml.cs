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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Data;
using System.ComponentModel;
using MyDocsSL.Views;

namespace MyDocsSL
{
    public partial class Home : Page
    {
        public file dirCur = new file();
        public DataGrid dataCurGrid = new DataGrid();
        FileService.FileServiceClient fsc = new FileService.FileServiceClient();
        public Home()
        {
            InitializeComponent();

            dataCurGrid.MinHeight = 100;
            dataCurGrid.MinWidth = 500;
            dataCurGrid.AllowDrop = true;
            dataCurGrid.Drop += new DragEventHandler(dataGrid_Drop);
            dataCurGrid.VerticalAlignment = VerticalAlignment.Stretch;
            dataCurGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            dataCurGrid.SelectionChanged += new SelectionChangedEventHandler(dataGrid_SelectionChanged);
            dataCurGrid.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Files") { Source = dirCur, BindsDirectlyToSource = true, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.Default });
            
            dataCurGrid.KeyDown += new KeyEventHandler(dataCurGrid_KeyDown);

            ContentStackPanel.AllowDrop = true;
            ContentStackPanel.Orientation = Orientation.Vertical;


            ContentStackPanel.Children.Add(dataCurGrid);
            Button btnUpload = new Button();
            btnUpload.Content = "Upload";
            btnUpload.Width = 70;
            btnUpload.HorizontalAlignment = HorizontalAlignment.Left;
            btnUpload.Click += new RoutedEventHandler(btnUpload_Click);

            ContentStackPanel.Children.Add(btnUpload);

            ContentStackPanel.Children.Add(lbl);
            LayoutRoot.AllowDrop = true;
            LayoutRoot.Loaded += new RoutedEventHandler(LayoutRoot_Loaded);
        }

        void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All files|*.*";
            ofd.Multiselect = true;
            bool? b = ofd.ShowDialog();
            if (b.HasValue && b.Value)
            {
                foreach (FileInfo fi in ofd.Files)
                {
                    if (fi.Exists)
                    {
                        dirCur.Add(fi);
                    }
                }
                Upload();
            }            
        }

        void dataCurGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (MessageBox.Show("Are you sure you want to delete the selected files?", "Delete", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    fsc.DeleteFileCompleted += (a, b) =>
                    {
                        file f = b.UserState as file;
                        dirCur.Files.Remove(f);
                    };
                    foreach (file f in dataCurGrid.SelectedItems)
                    {
                        fsc.DeleteFileAsync(f.Name, f);
                    }
                    ListFiles();
                }
            }
            else if (e.Key==Key.D)
            {
                if (dataCurGrid.SelectedItem != null)
                {
                    file fi = dataCurGrid.SelectedItem as file;
                    UriBuilder ub = new UriBuilder(fsc.Endpoint.Address.Uri.AbsoluteUri.Replace("FileService.svc", "sender.ashx"));
                    ub.Query = string.Format("filename={0}", fi.Name);
                    ContextWindow cw = new ContextWindow(ub.Uri.OriginalString);
                    cw.Show();
                }
            }
            else if (e.Key == Key.V)
            {
                if (dataCurGrid.SelectedItem != null)
                {
                    file fi = dataCurGrid.SelectedItem as file;
                    ViewWindow vw = new ViewWindow(fi);
                    vw.Show();
                }
            }
        }
        CheckBox chk = new CheckBox();

        Label lbl = new Label();
        private void ListFiles()
        {
            fsc.ListFilesCompleted += (a, b) =>
            {
                if (b.Result != null)
                {
                    foreach (FileService.Fileinfo fi in b.Result)
                    {
                        if (!dirCur.Files.Any(x => x.Name == fi.Name && x.Size == fi.Size))
                            dirCur.Add(new file(fi.Name, fi.Size));
                    }
                    if (dirCur.Files.Count > b.Result.Count())
                    {
                        List<file> delete = new List<file>();
                        foreach (file f in dirCur.Files)
                        {
                            if (!b.Result.Any(x => x.Name == f.Name && x.Size == f.Size) || dirCur.Files.Count(x => x.Name == f.Name && x.Size == f.Size) > 1)
                                delete.Add(f);
                        }
                        foreach (file f in delete)
                        {
                            dirCur.Files.Remove(f);
                        }
                    }
                }
            };
            fsc.ListFilesAsync();
        }
        void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Threading.DispatcherTimer tim = new System.Windows.Threading.DispatcherTimer();
            tim.Interval = new TimeSpan(0, 0, 0);
            tim.Tick += (a, b) =>
            {
                ListFiles();
                tim.Interval = new TimeSpan(0, 2, 0);
            };
            tim.Start();

            dataCurGrid.Columns[dataCurGrid.Columns.Count - 1].Visibility = Visibility.Collapsed;
        }

        void Upload()
        {
            List<file> list = new List<file>();
            foreach (file f in dirCur.Files.Where(x=>x.Do))
            {
                list.Add(f);
            }
            foreach (file f in list)
            {
                FileStream fs = f.GetFileStream();
                UploadFile(f, fs);                
            }
        }

        private void UploadFile(file mfi, System.IO.FileStream data)
        {
            UriBuilder ub = new UriBuilder(fsc.Endpoint.Address.Uri.AbsoluteUri.Replace("FileService.svc", "receiver.ashx"));
            ub.Query = string.Format("filename={0}", mfi.Name);
            
            WebClient wc = new WebClient();
            wc.OpenWriteCompleted += (sender, e) =>
            {
                try
                {
                    PushData(data, e.Result, mfi);
                    e.Result.Close();
                    data.Close();
                }
                catch (Exception ex)
                {
                    lbl.Content += ex.Message + "\n" + ex.StackTrace + "\n";
                }
            };
            wc.OpenWriteAsync(ub.Uri);
        }

        private void PushData(System.IO.FileStream input, System.IO.Stream output, file mfi)
        {
            byte[] buffer = new byte[32768];
            int bytesRead;
            int iRead = 0;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) != 0)
            {
                iRead = iRead + buffer.Length;
                mfi.Progress = iRead;//Math.Round((double.Parse(input.Length.ToString()) / double.Parse(iRead.ToString())) * 100, 0);
                output.Write(buffer, 0, bytesRead);
            }
            mfi.Do = false;
            ListFiles();
        }


        void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataCurGrid.SelectedItems.Count == 1)
            {
                dataCurGrid.BeginEdit();
            }
        }


        void dataGrid_Drop(object sender, DragEventArgs e)
        {
            string sfd = "FileDrop";
            if (e.Data.GetDataPresent(sfd))
            {
                foreach (object o in e.Data.GetData(sfd) as object[])
                {
                    if (o.GetType() == typeof(FileInfo))
                    {                            
                        FileInfo fi = o as FileInfo;
                        if (fi.Exists)
                        {
                            dirCur.Add(fi);
                        }
                    }
                }
                Upload();
            }            
        }
        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}