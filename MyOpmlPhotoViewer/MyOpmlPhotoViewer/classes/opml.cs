using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace MyOpmlPhotoViewer
{
    public static class OCExt
    {
        [DllImport(@"C:\Program Files\Canon\EOS Utility\Ucs32.dll")]
        public extern static int GetActiveWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr MessageBox(int hWnd, String text,
                                           String caption, uint type);

        public static ObservableCollection<T> AddRange<T>(this ObservableCollection<T> c, ObservableCollection<T> list)
        {

            foreach (T t in list)
            {
                c.Add(t);
            }
            return c;
        }
    }
    public class opml
    {
        public opml()
        {
            children = new ObservableCollection<outline>();
        }
        public ObservableCollection<outline> children { get; set; }
    }
    public class outline
    {
        public outline()
        {
            children = new ObservableCollection<outline>();
        }
        public ObservableCollection<outline> children { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public string url { get; set; }
    }
}
