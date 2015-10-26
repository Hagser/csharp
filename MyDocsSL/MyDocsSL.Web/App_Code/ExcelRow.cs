using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel;

/// <summary>
/// Summary description for ExcelRow
/// </summary>

[DataContract]
public class ExcelRow : INotifyPropertyChanged
{
    [IgnoreDataMember]
    public ObservableCollection<ExcelCell> Cells { get { return _Cells; } set { _Cells = value; InvokePropertyChanged("Cells"); } }
    private ObservableCollection<ExcelCell> _Cells = new ObservableCollection<ExcelCell>();
    [IgnoreDataMember]
    public string Span { get; set; }

    [DataMember(Order = 1)]
    public object A { get; set; }
    [DataMember(Order = 2)]
    public object B { get; set; }
    [DataMember(Order = 3)]
    public object C { get; set; }
    [DataMember(Order = 4)]
    public object D { get; set; }
    [DataMember(Order = 5)]
    public object E { get; set; }
    [DataMember(Order = 6)]
    public object F { get; set; }
    [DataMember(Order = 7)]
    public object G { get; set; }
    [DataMember(Order = 8)]
    public object H { get; set; }
    [DataMember(Order = 9)]
    public object I { get; set; }
    [DataMember(Order = 10)]
    public object J { get; set; }
    [DataMember(Order = 11)]
    public object K { get; set; }
    [DataMember(Order = 12)]
    public object L { get; set; }
    [DataMember(Order = 13)]
    public object M { get; set; }
    [DataMember(Order = 14)]
    public object N { get; set; }
    [DataMember(Order = 15)]
    public object O { get; set; }
    [DataMember(Order = 16)]
    public object P { get; set; }
    [DataMember(Order = 17)]
    public object Q { get; set; }
    [DataMember(Order = 18)]
    public object R { get; set; }
    [DataMember(Order = 19)]
    public object S { get; set; }
    [DataMember(Order = 20)]
    public object T { get; set; }
    [DataMember(Order = 21)]
    public object U { get; set; }
    [DataMember(Order = 22)]
    public object V { get; set; }
    [DataMember(Order = 23)]
    public object W { get; set; }
    [DataMember(Order = 24)]
    public object X { get; set; }
    [DataMember(Order = 25)]
    public object Y { get; set; }
    [DataMember(Order = 26)]
    public object Z { get; set; }

    [DataMember(Order = 0)]
    public int Row { get; set; }


    public void setA2Z(string coord)
    {
        if (coord.Equals("A" + Row))
            try { A = Cells.Where(x => x.Coord.Equals("A" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("B" + Row))
            try { B = Cells.Where(x => x.Coord.Equals("B" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("C" + Row))
            try { C = Cells.Where(x => x.Coord.Equals("C" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("D" + Row))
            try { D = Cells.Where(x => x.Coord.Equals("D" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("E" + Row))
            try { E = Cells.Where(x => x.Coord.Equals("E" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("F" + Row))
            try { F = Cells.Where(x => x.Coord.Equals("F" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("G" + Row))
            try { G = Cells.Where(x => x.Coord.Equals("G" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("H" + Row))
            try { H = Cells.Where(x => x.Coord.Equals("H" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("I" + Row))
            try { I = Cells.Where(x => x.Coord.Equals("I" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("J" + Row))
            try { J = Cells.Where(x => x.Coord.Equals("J" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("K" + Row))
            try { K = Cells.Where(x => x.Coord.Equals("K" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("L" + Row))
            try { L = Cells.Where(x => x.Coord.Equals("L" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("M" + Row))
            try { M = Cells.Where(x => x.Coord.Equals("M" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("N" + Row))
            try { N = Cells.Where(x => x.Coord.Equals("N" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("O" + Row))
            try { O = Cells.Where(x => x.Coord.Equals("O" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("P" + Row))
            try { P = Cells.Where(x => x.Coord.Equals("P" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("Q" + Row))
            try { Q = Cells.Where(x => x.Coord.Equals("Q" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("R" + Row))
            try { R = Cells.Where(x => x.Coord.Equals("R" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("S" + Row))
            try { S = Cells.Where(x => x.Coord.Equals("S" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("T" + Row))
            try { T = Cells.Where(x => x.Coord.Equals("T" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("U" + Row))
            try { U = Cells.Where(x => x.Coord.Equals("U" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("V" + Row))
            try { V = Cells.Where(x => x.Coord.Equals("V" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("W" + Row))
            try { W = Cells.Where(x => x.Coord.Equals("W" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("X" + Row))
            try { X = Cells.Where(x => x.Coord.Equals("X" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("Y" + Row))
            try { Y = Cells.Where(x => x.Coord.Equals("Y" + Row)).FirstOrDefault().Value; }
            catch { }
        if (coord.Equals("Z" + Row))
            try { Z = Cells.Where(x => x.Coord.Equals("Z" + Row)).FirstOrDefault().Value; }
            catch { }
    }
	public ExcelRow()
	{
        Cells = new ObservableCollection<ExcelCell>();
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