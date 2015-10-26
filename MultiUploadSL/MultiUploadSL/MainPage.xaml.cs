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
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;

namespace MultiUploadSL
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.lst.ItemsSource = fl.myFileList;
        }

        //public ObservableCollection<MyFileInfo> myFL = new ObservableCollection<MyFileInfo>();
        public FileList fl = new FileList();
        OpenFileDialog ofd = new OpenFileDialog();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            ofd.Multiselect = true;
            ofd.Filter = "Images|*.jpg;*.png;*.tif";

            if ((bool)ofd.ShowDialog())
            {
                foreach (System.IO.FileInfo fi in ofd.Files)
                {
                    AddFileToListBox(new MyFileInfo(fi));
                }
                btnUpload.IsEnabled = (txtName.Text.Length > 0 && txtAdress.Text.Length>0 && txtEmail.Text.Length > 0 && txtGolfID.Text.Length > 0 && lst.Items.Count > 0);
            }
            else
            {
                lbl.Text = "Inga filer valda!";
            }

        }

        private void AddFileToListBox(MyFileInfo fi)
        {
            //lst.Items.Add(CreateSP(fi));
            fi.Progress = "Vald";
            fi.ProgressValue = 0;
            fi.Uploaded = false;
            fl.myFileList.Add(fi);
            //lst.Items.Add(fi);
        }

        private void UploadFiles(MyFileInfo mfi, System.IO.FileStream data)
        {
            UriBuilder ub = new UriBuilder("http://localhost:3793/receiver.ashx");
            ub.Query = string.Format("filename={0}&name={1}&address={2}&email={3}&golfid={4}", mfi.Name, fixText(txtName.Text), fixText(txtAdress.Text), fixText(txtEmail.Text), fixText(txtGolfID.Text));
            //ub.Query = string.Format("filename={0}", mfi.Name);
            WebClient wc = new WebClient();

            wc.OpenWriteCompleted += (sender, e) =>
            {
                PushData(data, e.Result, mfi);
                e.Result.Close();
                data.Close();
                lbl.Text = "Fil(er) uppladdade!";
            };
            wc.OpenWriteAsync(ub.Uri);
        }

        private string fixText(string p)
        {
            return System.Text.RegularExpressions.Regex.Replace(p, "[^_A-Za-z0-9åäöÅÄÖé@\\.]", "");
        }

        private void PushData(System.IO.FileStream input, System.IO.Stream output, MyFileInfo mfi)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            int iRead = 0;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) != 0)
            {
                iRead = iRead + buffer.Length;
                mfi.Progress = Math.Round((double.Parse(input.Length.ToString()) / double.Parse(iRead.ToString()))*100, 0).ToString() + " %";
                mfi.ProgressValue = Math.Round((double.Parse(input.Length.ToString()) / double.Parse(iRead.ToString())) * 100, 0);
                
                output.Write(buffer, 0, bytesRead);
            }
            mfi.Uploaded = true;
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnUpload.IsEnabled = (txtName.Text.Length > 0 && txtAdress.Text.Length >0 && txtEmail.Text.Length > 0 && txtGolfID.Text.Length > 0 && lst.Items.Count > 0);
        }

        private void ListBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                foreach(MyFileInfo mfi in lst.SelectedItems){
                    //lst.Items.Remove(mfi); 
                    this.fl.myFileList.Remove(mfi);
                }

                btnUpload.IsEnabled = lst.Items.Count > 0;
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            btnUpload.IsEnabled = false;
            if (lst.Items.Count > 0)
            {
                lbl.Text = "Laddar...";
                foreach (MyFileInfo mfi in lst.Items)
                {
                    if (!mfi.Uploaded)
                    {
                        UploadFiles(mfi, mfi.OpenRead());
                    }
                }
            }
            btnUpload.IsEnabled = true;
        }


    }
}
