using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Collections.ObjectModel;

/// <summary>
/// Summary description for WorkSheet
/// </summary>

[DataContract]
public class WorkSheet : INotifyPropertyChanged
{
    [DataMember]
    public string SheetName { get; set; }
    [IgnoreDataMember]
    public string SheetXml { get; set; }
    [DataMember]
    public ObservableCollection<ExcelRow> Rows { get { return _Rows; } set { _Rows = value; InvokePropertyChanged("Rows"); } }

    private ObservableCollection<ExcelRow> _Rows = new ObservableCollection<ExcelRow>();

	public WorkSheet()
	{
        SheetName = "";
        SheetXml = "";
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