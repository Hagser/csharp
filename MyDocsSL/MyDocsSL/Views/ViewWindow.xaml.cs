using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MyDocsSL.FileService;
using System.Windows.Media;
using System.Windows.Browser;

namespace MyDocsSL.Views
{
    public partial class ViewWindow : ChildWindow
    {
        ExcelDocument Exceldocument = new ExcelDocument();
        FileService.FileServiceClient fsc = new FileService.FileServiceClient();
        public ViewWindow(file filename)
        {
            InitializeComponent();
            if (filename.Name.EndsWith("xlsx"))
            {
                SheetList.Visibility = Visibility.Visible;
                dataGrid.Visibility = Visibility.Visible;

                fsc.GetExcelDocumentCompleted += (a, b) =>
                {
                    Exceldocument = b.Result;
                    SheetList.SetBinding(ListBox.ItemsSourceProperty, new Binding("WorkSheets") { Source = Exceldocument, BindsDirectlyToSource = true, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.Default });
                };

                fsc.GetExcelDocumentAsync(filename.Name);
            }
            else if (filename.Name.EndsWith("pdf"))
            {
                UriBuilder ub = new UriBuilder(fsc.Endpoint.Address.Uri.AbsoluteUri.Replace("FileService.svc", "sender.ashx"));
                ub.Query = string.Format("filename={0}", filename.Name);

                webbrowser.Visibility = Visibility.Visible;
                webbrowser.NavigateToString("<html><body><iframe src='"+ ub.Uri.AbsoluteUri +"&mime=true' id='ifrm' style='width:100%;height:100%;border:solid 10px green;margin:0;padding:0;'></iframe></body></html>");
                
            }
            else if (filename.Name.ToLower().EndsWith("jpg"))
            {
                UriBuilder ub = new UriBuilder(fsc.Endpoint.Address.Uri.AbsoluteUri.Replace("FileService.svc", "sender.ashx"));
                ub.Query = string.Format("filename={0}", filename.Name);

                ImageSourceConverter isc = new ImageSourceConverter();
                image.Source= isc.ConvertFromString(ub.Uri.OriginalString) as ImageSource;

                image.Visibility = Visibility.Visible;

            }
            
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button bu = (sender as Button);
            string sheet = bu.Content.ToString();
            WorkSheet wsheet = Exceldocument.WorkSheets.Where(x => x.SheetName.Equals(sheet)).FirstOrDefault();
            if (wsheet != null) {
                /*
                dataGrid.Columns.Clear();
                foreach (ExcelRow row in wsheet.Rows)
                {
                    if (dataGrid.Columns.Count == 0)
                    {
                        foreach (ExcelCell cell in row.Cells)
                        {
                            DataGridTextColumn dgtc = new DataGridTextColumn();
                            dgtc.Header = cell.Coord;
                            dataGrid.Columns.Add(dgtc);
                        }
                    }
                    
                    DataGridRow dr = new DataGridRow();
                    
                    dr.ItemArray = new object[row.Cells.Count];
                    int ic = 0;
                    foreach (ExcelCell cell in row.Cells)
                    {
                        dr.ItemArray.SetValue(cell.Value, ic);
                        ic++;
                    }
                    dt.Rows.Add(dr);
                    
                }
                */
                dataGrid.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Rows") { Source = wsheet, BindsDirectlyToSource = true, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.Default });
            }
        }



    }

}

