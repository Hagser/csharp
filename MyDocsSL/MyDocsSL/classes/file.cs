using System.IO;
using System.ComponentModel;
using System.Collections.ObjectModel;
public class file : INotifyPropertyChanged
{
    public file()
    {
    }
    public file(string name, long size)
    {
        _name = name;
        _size = size;
    }
    public file(FileInfo _fi)
    {
        fi=_fi;
    }
    public FileStream GetFileStream()
    {
        return fi.OpenRead();
    }
    public void Add(file fi)
    {
        _files.Add(fi);
        InvokePropertyChanged("Files");
    }
    public void Add(FileInfo fi)
    {
        _files.Add(new file(fi) { Do = true });
        InvokePropertyChanged("Files");
    }
    public bool Do { get { return _do; } set { if (_do != value) { _do = value; InvokePropertyChanged("Do"); } } }
    public string Name { get { return fi != null ? fi.Name : _name; } }//set { if (value != _name) { _name = value; InvokePropertyChanged("Name"); } } }
    public long Size { get { return fi!=null?fi.Length:_size; } }
    public double Progress { get { return _Progress; } set { if (value != _Progress) { _Progress = value; InvokePropertyChanged("Progress"); } } }

    private bool _do { get; set; }
    private double _Progress { get; set; }
    private long _size { get; set; }
    private string _name { get; set; }
    private FileInfo fi { get; set; }


    public ObservableCollection<file> Files { get { return _files; } set { _files = value; InvokePropertyChanged("Files"); } }
    private ObservableCollection<file> _files = new ObservableCollection<file>();

    public event PropertyChangedEventHandler PropertyChanged;
    private void InvokePropertyChanged(string propertyname)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}