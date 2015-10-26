using System;
using System.IO;

/// <summary>
/// Summary description for Fileinfo
/// </summary>
public class Fileinfo
{
	public Fileinfo()
	{
	}
    public Fileinfo(string file)
    {
        FileInfo fi = new FileInfo(file);
        Name = fi.Name;
        Size = fi.Length;
        Changed = fi.CreationTime;
    }
    public Fileinfo(FileInfo file)
    {
        Name = file.Name;
        Size = file.Length;
        Changed = file.CreationTime;
    }
    public string Name { get; set; }
    public long Size { get; set; }
    public DateTime Changed { get; set; }
    public Uri Uri { get; set; }

}