using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using System.IO;
using zip=ICSharpCode.SharpZipLib.Zip;
using System.Collections;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Xml;
using System.Data;

/// <summary>
/// Summary description for ExcelDocument
/// </summary>
[DataContract]
public class ExcelDocument:INotifyPropertyChanged
{
    [DataMember]
    public ObservableCollection<WorkSheet> WorkSheets { get { return _WorkSheets; } set { _WorkSheets = value; InvokePropertyChanged("WorkSheets"); } }
    private ObservableCollection<WorkSheet> _WorkSheets = new ObservableCollection<WorkSheet>();
    
    private DataSet GetDataSet()
    {
        DataSet ds = new DataSet();
        try
        {
            foreach (WorkSheet s in WorkSheets)
            {
                DataTable dt = new DataTable(s.SheetName);
                foreach (ExcelRow row in s.Rows)
                {
                    if (dt.Columns.Count == 0)
                    {
                        foreach (ExcelCell cell in row.Cells)
                        {
                            dt.Columns.Add(cell.Coord);
                        }
                    }
                    DataRow dr = dt.NewRow();
                    dr.ItemArray = new object[row.Cells.Count];
                    int ic = 0;
                    foreach (ExcelCell cell in row.Cells)
                    {
                        dr.ItemArray.SetValue(cell.Value, ic);
                        ic++;
                    }
                    dt.Rows.Add(dr);
                }

                ds.Tables.Add(dt);
            }
        }
        catch (Exception ex) { string ss = ex.Message; }
        return ds;
    }

	public ExcelDocument(string filename)
	{
        if (!filename.Contains("\\"))
            filename = FileService.GetPath()+"\\"+filename;
        WorkSheets = new ObservableCollection<WorkSheet>();
        try
        {
            using (zip.ZipFile zf = new zip.ZipFile(filename))
            {
                IEnumerator ienum = zf.GetEnumerator();
                int ifld = zf.FindEntry("xl/workbook.xml", true);

                StreamReader srwb = new StreamReader(zf.GetInputStream(ifld));

                XmlDocument xd = new XmlDocument();
                xd.LoadXml(srwb.ReadToEnd());
                foreach (XmlNode xn in xd.DocumentElement.GetElementsByTagName("sheet"))
                {
                    string sheetId = xn.Attributes["sheetId"].Value;
                    string name = xn.Attributes["name"].Value;
                    int intsh = zf.FindEntry("xl/worksheets/sheet" + sheetId + ".xml", true);
                    if (intsh != -1)
                    {
                        StreamReader sr = new StreamReader(zf.GetInputStream(intsh));
                        WorkSheet ws = new WorkSheet() { SheetName = name, SheetXml = sr.ReadToEnd() };
                        
                        XmlDocument xdr = new XmlDocument();
                        xdr.LoadXml(ws.SheetXml);
                        string decsep = (1.5).ToString();
                        string repdecsep = decsep.Contains(",") ? "," : decsep.Contains(".") ? "." : "";
                        decsep = decsep.Contains(",") ? "." : decsep.Contains(".") ? "," : "";

                        foreach (XmlNode xnr in xdr.DocumentElement.GetElementsByTagName("row"))
                        {
                            try
                            {
                                ExcelRow row = new ExcelRow();
                                row.Row = int.Parse(xnr.Attributes["r"].Value.ToString());
                                row.Span = xnr.Attributes["spans"].Value;
                                foreach (XmlNode xnc in xnr.ChildNodes)
                                {
                                    try
                                    {
                                        ExcelCell cell = new ExcelCell();
                                        cell.Coord = xnc.Attributes["r"].Value;
                                        cell.Formula = xnc.ChildNodes.Count > 0 && xnc.FirstChild != null && xnc.FirstChild.Name.Equals("f") ? xnc.FirstChild.InnerText : "";
                                        cell.Value = xnc.ChildNodes.Count == 1 && xnc.FirstChild != null && xnc.FirstChild.Name.Equals("v") && !string.IsNullOrEmpty(xnc.FirstChild.InnerText) ? xnc.FirstChild.InnerText :
                                            xnc.ChildNodes.Count > 1 && xnc.LastChild != null && xnc.LastChild.Name.Equals("v") && !string.IsNullOrEmpty(xnc.LastChild.InnerText) ? xnc.LastChild.InnerText : "0";
                                        row.Cells.Add(cell);
                                        row.setA2Z(cell.Coord);
                                    }
                                    catch (Exception ex) { string ss = ex.Message; }
                                }
                                ws.Rows.Add(row);
                            }
                            catch (Exception ex) { string ss = ex.Message; }
                        }

                        WorkSheets.Add(ws);
                    }
                }
            }
        }
        catch (Exception ex) { string ss = ex.Message; }
	}

    public event PropertyChangedEventHandler PropertyChanged;
    private void InvokePropertyChanged(string propertyname)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}