using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace GoodStuff
{
    public class manageFile
    {
        public static string[] openFile(OpenFileDialog ofd)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return ofd.FileNames;
            }
            return ",".Split(",".ToCharArray()[0]);

        }
        public static string[] openFile(string Filter, string InitialDirectory, bool Multiselect, string Title)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = Filter;
            ofd.InitialDirectory = InitialDirectory;
            ofd.Multiselect = Multiselect;
            ofd.Title = Title;
            return openFile(ofd);
        }
        public static string[] openFile(string Filter, string InitialDirectory, bool Multiselect)
        {
            return openFile(Filter, InitialDirectory, Multiselect, "");
        }
        public static string[] openFile(string Filter, string InitialDirectory)
        {
            return openFile(Filter, InitialDirectory, false, "");
        }
        public static string[] openFile(string Filter)
        {
            return openFile(Filter, Application.StartupPath, false, "");
        }
        public static string[] openFile()
        {
            return openFile("All|*.*", Application.StartupPath, false, "");
        }

        public static string saveFile(SaveFileDialog sfd)
        {
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                return sfd.FileName;
            }
            return "";
        }
        public static string saveFile(bool AddExtension, string Filter, string InitialDirectory, bool OverwritePrompt, string Title)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = AddExtension;
            sfd.Filter = Filter;
            sfd.InitialDirectory = InitialDirectory;
            sfd.OverwritePrompt = OverwritePrompt;
            sfd.Title = Title;
            return saveFile(sfd);
        }
        public static string saveFile(bool AddExtension, string Filter, string InitialDirectory, bool OverwritePrompt)
        {
            return saveFile(AddExtension, Filter, InitialDirectory, OverwritePrompt, "");
        }
        public static string saveFile(bool AddExtension, string Filter, string InitialDirectory)
        {
            return saveFile(AddExtension, Filter, InitialDirectory, true, "");
        }
        public static string saveFile(bool AddExtension, string Filter)
        {
            return saveFile(AddExtension, Filter, Application.StartupPath, true, "");
        }
        public static string saveFile(bool AddExtension)
        {
            return saveFile(AddExtension, "All|*.*", Application.StartupPath, true, "");
        }
        public static string saveFile(string Filter, string InitialDirectory)
        {
            return saveFile(true, Filter, InitialDirectory, true, "");
        }
        public static string saveFile(string Filter, string InitialDirectory, bool OverwritePrompt)
        {
            return saveFile(true, Filter, InitialDirectory, OverwritePrompt, "");
        }
        public static string saveFile(string Filter)
        {
            return saveFile(true, Filter, Application.StartupPath, true, "");
        }
        public static string saveFile()
        {
            return saveFile(true, "All|*.*", Application.StartupPath, true, "");
        }

        public static string readFile(FileStream fs)
        {
            string v_ret = "";
            try
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    v_ret = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show("An error occured:\n" + ex.Message); }
            return v_ret;
        }

        public static string readFile(string filename, FileMode filemode, FileAccess fileaccess)
        {
            string v_ret = "";
            try
            {
                using (FileStream fs = new FileStream(filename, filemode, fileaccess))
                {
                    v_ret = readFile(fs);
                    fs.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show("An error occured:\n" + ex.Message); }
            return v_ret;
        }
        public static string readFile(string filename, FileMode filemode)
        {
            return readFile(filename, filemode, FileAccess.ReadWrite);
        }
        public static string readFile(string filename, FileAccess fileaccess)
        {
            return readFile(filename, FileMode.Open, fileaccess);
        }
        public static string readFile(string filename)
        {
            return readFile(filename, FileMode.Open, FileAccess.ReadWrite);
        }

        public static void writeFile(FileStream fs, string value)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(value);
                    sw.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show("An error occured:\n" + ex.Message); }
        }
        public static void writeFile(string filename, string value, FileMode filemode, FileAccess fileaccess)
        {
            try
            {
                using (FileStream fs = new FileStream(filename, filemode, fileaccess))
                {
                    writeFile(fs,value);
                    fs.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show("An error occured:\n" + ex.Message); }
        }
        public static void writeFile(string filename, string value, FileMode filemode)
        {
            writeFile(filename, value, filemode, FileAccess.ReadWrite);
        }
        public static void writeFile(string filename, string value, FileAccess fileaccess)
        {
            writeFile(filename, value, FileMode.OpenOrCreate, fileaccess);
        }
        public static void writeFile(string filename, string value)
        {
            writeFile(filename, value, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        public static void saveListView(string filename, ListView.ListViewItemCollection listviewitemcollection, string delimiter)
        {
            string strColumns = "";
            string strWrite = "";
            bool bColumnsAdded = false;
            foreach (ListViewItem lvi in listviewitemcollection)
            {
                foreach (ColumnHeader colh in lvi.ListView.Columns)
                {
                    if (!bColumnsAdded)
                    {
                        strColumns += strColumns != "" ? delimiter + strColumns : strColumns;
                    }
                    strWrite += strWrite != "" ? delimiter + strWrite : strWrite;
                }
                strWrite += "\r\n";
                bColumnsAdded = true;
            }
            strColumns += "\r\n";
            writeFile(filename, strColumns + strWrite);
        }
        public static void saveListView(string filename, ListView listview, string delimiter)
        {
            saveListView(filename, listview.Items, delimiter);
        }

        public static string chooseFolder(FolderBrowserDialog fbd)
        {
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                return fbd.SelectedPath;
            }
            
            return "";
        }
        public static string chooseFolder(Environment.SpecialFolder RootFolder, string SelectedPath, bool ShowNewFolderButton, string Description)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.RootFolder = RootFolder;
                fbd.SelectedPath = SelectedPath;
                fbd.ShowNewFolderButton = ShowNewFolderButton;
                fbd.Description = Description;
                return chooseFolder(fbd);
            }
        }
        public static string chooseFolder(string SelectedPath, bool ShowNewFolderButton, string Description)
        {
            return chooseFolder(Environment.SpecialFolder.Desktop, SelectedPath, ShowNewFolderButton, Description);
        }
        public static string chooseFolder(string SelectedPath, bool ShowNewFolderButton)
        {
            return chooseFolder(Environment.SpecialFolder.Desktop, SelectedPath, ShowNewFolderButton, "");
        }
        public static string chooseFolder(string SelectedPath)
        {
            return chooseFolder(Environment.SpecialFolder.Desktop, SelectedPath, false, "");
        }
        public static string chooseFolder()
        {
            return chooseFolder(Environment.SpecialFolder.Desktop, "", false, "");
        }

        public static void fileMove(string filepath, string dirpath)
        {
            if (filepath != "" && dirpath != "")
            {
                if (fileEx(filepath) && dirEx(dirpath))
                {
                    FileInfo fi = new FileInfo(filepath);
                    DirectoryInfo di = new DirectoryInfo(dirpath);
                    fileMove(fi, di);
                }
            }
        }
        public static void fileMove(FileInfo fi, DirectoryInfo di)
        {
            if (fi.FullName != "" && di.FullName != "")
            {
                if (fileEx(fi.FullName) && dirEx(di.FullName))
                {
                    File.Move(fi.FullName, di.FullName);
                }
            }
        }

        public static void dirMove(string sdirpath, string ddirpath)
        {
            if (sdirpath != "" && ddirpath != "")
            {
                if (dirEx(sdirpath) && dirEx(ddirpath))
                {
                    DirectoryInfo sdi = new DirectoryInfo(sdirpath);
                    DirectoryInfo ddi = new DirectoryInfo(ddirpath);
                    dirMove(sdi, ddi);
                }
            }
        }
        public static void dirMove(DirectoryInfo sdi, DirectoryInfo ddi)
        {
            if (sdi.FullName != "" && ddi.FullName != "")
            {
                if (dirEx(sdi.FullName) && dirEx(ddi.FullName))
                {
                    Directory.Move(sdi.FullName, ddi.FullName);
                }
            }
        }

        public static bool fileEx(FileInfo fileinfo)
        {
            return fileinfo.Exists;
        }
        public static bool fileEx(string filepath)
        {
            return filepath != "" && fileEx(new FileInfo(filepath));
        }

        public static bool dirEx(DirectoryInfo directoryinfo)
        {
            return directoryinfo.Exists;
        }
        public static bool dirEx(string directorypath)
        {
            return directorypath != "" && dirEx(new DirectoryInfo(directorypath));
        }

    }

}
