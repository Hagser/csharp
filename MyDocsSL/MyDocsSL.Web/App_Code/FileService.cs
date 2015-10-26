using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.Linq;

[ServiceContract]
public interface IFileService
{
    [OperationContract]
    List<Fileinfo> ListFiles();
    [OperationContract]
    bool DeleteFile(string name);
    [OperationContract]
    bool RenameFile(string oldname, string newname);
    [OperationContract]
    List<Fileinfo> ListVersions(string filename);
    [OperationContract]
    bool DeleteVersion(string name);
    [OperationContract]
    ExcelDocument GetExcelDocument(string filename);
    
}
public class FileService : IFileService
{
    public static string GetPath()
    {
        AppSettingsReader asr = new AppSettingsReader();
        string v = asr.GetValue("path", typeof(string)) as string;
        return v;
    }
    private string getASRV(string key)
    {
        AppSettingsReader asr = new AppSettingsReader();
        string v = asr.GetValue(key, typeof(string)) as string;
        return v;
    }
    private string getPath()
    {
        return getASRV("path");
    }
    private string getBaseUrl()
    {
        return getASRV("url");
    }

    public ExcelDocument GetExcelDocument(string filename)
    {
        string path = getPath();
        try
        {
            ExcelDocument ed = new ExcelDocument(path + "\\" + filename);
            return ed;
        }
        catch (Exception ex)
        { 
            string ssdfsdf = ex.Message;
        }
        return null;
    }
    public List<Fileinfo> ListFiles()
    {
        List<Fileinfo> list = new List<Fileinfo>();
        string path = getPath();
        foreach (string strFile in Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly))
        {
            if (!strFile.Replace(path,"").Replace("\\","").StartsWith("v_"))
            {
                Fileinfo fi = new Fileinfo(strFile);
                fi.Uri = getUri(fi.Name);
                list.Add(fi);
            }
        }
        return list.OrderBy(x=>x.Name).ToList<Fileinfo>();
    }
    private Uri getUri(string name)
    {
        string url = getBaseUrl();
        Uri u = new Uri(string.Format(url, "sender.ashx", name));
        return u;
    }
    public List<Fileinfo> ListVersions(string filename)
    {
        List<Fileinfo> list = new List<Fileinfo>();
        string path = getPath();
        foreach (string strFile in Directory.GetFiles(path, "v_*" + filename, SearchOption.TopDirectoryOnly))
        {
            if (strFile.Replace(path, "").Replace("\\", "").StartsWith("v_"))
            {
                Fileinfo fi = new Fileinfo(strFile);
                fi.Uri = getUri(fi.Name);
                list.Add(fi);
            }
        }

        return list.OrderBy(x => x.Name).ToList<Fileinfo>();
    }
    public bool DeleteFile(string name)
    {
        string path = getPath();
        if (File.Exists(path + "\\" + name))
        {
            int i = 0;
            string strDest = path + "\\deleted\\" + i + name;
            while (File.Exists(strDest))
            {
                i++;
                File.Move(path + "\\" + name, strDest);
            }
            foreach (string strfile in Directory.GetFiles(getPath(), "v_*" + name, SearchOption.TopDirectoryOnly))
            {
                FileInfo fi = new FileInfo(strfile);
                
                strDest = fi.DirectoryName + "\\deleted\\" + i + fi.Name;
                i = 0;
                while (File.Exists(strDest))
                {
                    i++;
                    File.Move(fi.FullName, strDest);
                }

                
            }

            return true;
        }
        return false;
    }
    public bool DeleteVersion(string name)
    {
        string path = getPath();
        if (File.Exists(path + "\\" + name))
        {
            int i = 0;
            string strDest = path + "\\deleted\\" + i + name;
            while (File.Exists(strDest))
            {
                i++;
                File.Move(path + "\\" + name, strDest);
            }
            return true;
        }
        return false;
    }
    public bool RenameFile(string oldname, string newname)
    {
        string path = getPath();
        if (File.Exists(path + "\\" + oldname) && !File.Exists(path + "\\" + newname))
        {
            File.Move(path + "\\" + oldname, path + "\\" + newname);
            return true;
        }
        return false;
    }
}
