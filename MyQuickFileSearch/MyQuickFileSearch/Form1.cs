using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyQuickFileSearch
{
    public partial class Form1 : Form
    {
        MyList list = new MyList();
        private string SaveTxt = Application.CommonAppDataPath + "\\MyList.txt";
        public Form1()
        {
            InitializeComponent();
        }

        void list_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            textBox1_KeyUp(sender, null);
        }

        private void fileSystemWatcher1_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            if (checkBox1.Checked)
                ThreadPool.QueueUserWorkItem(FileCreated, e);
        }

        private void FileCreated(object state)
        {
            FileSystemEventArgs e = state as FileSystemEventArgs;
            this.Invoke((MethodInvoker)delegate
            {
                list.Add(e.FullPath);
            });
        }

        private void fileSystemWatcher1_Deleted(object sender, System.IO.FileSystemEventArgs e)
        {
            if(checkBox1.Checked)
                ThreadPool.QueueUserWorkItem(Deleted, e);
        }

        private void Deleted(object state)
        {
            FileSystemEventArgs e = state as FileSystemEventArgs;
            var f = list.FirstOrDefault(x => e.FullPath.Equals(x, StringComparison.InvariantCultureIgnoreCase));
            if (f != null)
                this.Invoke((MethodInvoker)delegate
                {
                    list.Remove(f);
                });
                
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(SaveTxt))
                ThreadPool.QueueUserWorkItem(LoadSavedTxt);
            else
                ThreadPool.QueueUserWorkItem(FindNew);

        }

        private void LoadSavedTxt(object state)
        {
            this.Invoke((MethodInvoker)delegate
            {
                progressBar1.Enabled = true;
            });
            var gfsgfs = File.ReadAllLines(SaveTxt);
                
            this.Invoke((MethodInvoker)delegate {
                this.list.AddRange(gfsgfs);
                progressBar1.Enabled = false;
            });
            FindNew(state);
        }

        private void UpdateList(object state)
        {
            this.Invoke((MethodInvoker)delegate
            {
                progressBar1.Enabled = true;
            });
            List<string> deleted = new List<string>();
            foreach (string fi in list)
            {
                try
                {
                    if (!new FileInfo(fi).Exists)
                        deleted.Add(fi);
                }
                catch
                { deleted.Add(fi); }
            }
            foreach (string fi in deleted)
                list.Remove(fi);

             var ll = list.Distinct();
             list = new MyList();
             list.AddRange(ll);

            this.Invoke((MethodInvoker)delegate
            {
                progressBar1.Enabled = false;
            });

            //list.PropertyChanged += list_PropertyChanged;
        }

        private void FindNew(object state)
        {
            this.Invoke((MethodInvoker)delegate
            {
                progressBar1.Enabled = true;
            });
            List<string> list = FindAll("c:\\");

            List<string> files = new List<string>();
            try
            {
                foreach (string d in list)
                {
                    try
                    {
                        files.AddRange(Directory.EnumerateFiles(d, "*.*", SearchOption.TopDirectoryOnly)) ;
                    }
                    catch { }
                }
            }
            catch { }
            this.Invoke((MethodInvoker)delegate
            {
                this.list.AddRange(files);
                progressBar1.Enabled = false;
                SaveToTxt(SaveTxt, this.list,0);
            });
            UpdateList(state);
        }

        private void SaveToTxt(string file, MyList myList,int icnt)
        {
            progressBar1.Enabled = true;
            try
            {
                File.WriteAllLines(file, myList);
            }
            catch(Exception e)
            {
                if (icnt < 10)
                {
                    Thread.Sleep(1000);
                    SaveToTxt(file, myList, icnt+1);
                }
            } 
            progressBar1.Enabled = false;
        }

        private List<string> FindAll(string dir)
        {
            Thread.Sleep(5);
            List<string> list = new List<string>();
            try
            {
                foreach (string s in Directory.EnumerateDirectories(dir))
                {
                    list.Add(s);
                    try
                    {
                        list.AddRange(FindAll(s));
                    }
                    catch { }
                }
            }
            catch { }
            return list;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveToTxt(SaveTxt, this.list,0);
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox1.Text.Length > 2)
            {
                var ala = getFilteredList(textBox1.Text); 
                dataGridView1.DataSource = ala;
                dataGridView1.Refresh();
            }
        }

        private IEnumerable<SmallFileInfo> getFilteredList(string p)
        {
            return list.Where(x => x.Contains(p)).Select(x => new SmallFileInfo(new FileInfo(x))).Take(2000).ToList();
        }

        private void fileSystemWatcher1_Renamed(object sender, RenamedEventArgs e)
        {
            if (checkBox1.Checked)
                ThreadPool.QueueUserWorkItem(Renamed, e);
        }

        private void Renamed(object state)
        {
            RenamedEventArgs e = state as RenamedEventArgs;
            this.Invoke((MethodInvoker)delegate
            {
                var ll = list.ToList();
                var f = ll.FirstOrDefault(x => e.OldFullPath.Equals(x));
                if (f != null)
                {
                    f = e.FullPath;
                }
            });
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try {
                label1.Text = string.Format(label1.Tag + "", list.Count);
            }
            catch { }
        }
    }
    public class MyList : List<string>, INotifyPropertyChanged
    {
        public event EventHandler FileAdded;
        public event EventHandler FileRemoved;
        public event PropertyChangedEventHandler PropertyChanged;
        public MyList()
        { }
        public void Add(string fullpath)
        {
            if (!this.Contains(fullpath))
            {
                base.Add(fullpath);
            }
        }
        public bool Remove(string fi)
        {
            bool bremove = base.Remove(fi);
            if (bremove && FileRemoved != null)
            {
                FileRemoved.Invoke(this, EventArgs.Empty);
            }
            InvokePropertyChanged("Files");
            return bremove;
        }
        private void InvokePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal object ToList<T1>()
        {
            return this.ToList<T1>();
        }
    }
    public class SmallFileInfo
    {
        private FileInfo _fi;
        public bool Exists { get { return _fi != null && _fi.Exists ? true : false; } set { } }
        public string Name { get { return _fi != null && Exists ? _fi.Name : ""; } set { } }
        public string FullName { get { return _fi != null && Exists ? _fi.FullName : ""; } set { } }
        public long Size { get { return _fi != null && Exists ? _fi.Length : 0; } set { } }
        public DateTime CreationTime { get { return _fi != null && Exists ? _fi.CreationTime : DateTime.MinValue; } set { } }
        public DateTime LastAccessTime { get { return _fi != null && Exists ? _fi.LastAccessTime : DateTime.MinValue; } set { } }
        public DateTime LastWriteTime { get { return _fi != null && Exists ? _fi.LastWriteTime : DateTime.MinValue; } set { } }
        public string Extension { get { return _fi != null && Exists ? _fi.Extension : ""; } set { } }


        public SmallFileInfo()
        { 
            
        }
        public SmallFileInfo(FileInfo fi)
        {
            _fi = fi;
        }
    }
    public class FullNameEquCom : EqualityComparer<string>
    {
        public override bool Equals(string x, string y)
        {
            return x.Equals(y);
        }
        public override int GetHashCode(string x)
        {
            return x.GetHashCode();
        }
    }
}
