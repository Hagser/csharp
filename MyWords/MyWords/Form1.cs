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

namespace MyWords
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(MergeLists, null);
        }
        private void MergeLists(object state)
        {
            string sf1 = @"C:\Users\jh\Desktop\ord.niklas.txt";
            string sf2 = @"C:\Users\jh\Desktop\ss100.txt";
            string sf5 = @"C:\Users\jh\Desktop\merged.txt";
            var f1all = System.IO.File.ReadAllLines(sf1,System.Text.Encoding.Default);
            var f2all = System.IO.File.ReadAllLines(sf2, System.Text.Encoding.Default);
            var l1 = new List<string>(f1all);
            var l2 = new List<string>(f2all);
            var l3 = new List<string>();
            var l4 = new List<string>(l1);
            l4.AddRange(l2);

            var l5 = l4.Distinct<string>();

            this.Invoke((MethodInvoker)delegate {
                label1.Text = l1.Count + "";
                label2.Text = l2.Count + "";
                label4.Text = l4.Count + "";
                label5.Text = l5.Count() + "";
            });
            if (System.IO.File.Exists(sf5))
                System.IO.File.Delete(sf5);

            System.IO.File.WriteAllLines(sf5, l5.OrderBy(x => x).ToArray(), System.Text.Encoding.Default);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(SplitNames, null);
        }
        private void SplitNames(object state)
        {
            string sf1 = @"C:\Users\jh\Desktop\merged.txt";
            var f1all = System.IO.File.ReadAllLines(sf1, System.Text.Encoding.Default);

            var l1 = new List<string>(f1all);
            this.Invoke((MethodInvoker)delegate
            {
                label1.Text = l1.Count + "";
            });

            string sfw = @"C:\Users\jh\Desktop\words.txt";
            string sfn = @"C:\Users\jh\Desktop\names.txt";

            var lw = new List<string>(l1.Where(x => !IsUpper(x.Substring(0, 1))));
            var ln = new List<string>(l1.Where(x => IsUpper(x.Substring(0, 1))));

            this.Invoke((MethodInvoker)delegate
            {
                label1.Text = l1.Count + "";
                label2.Text = lw.Count + "";
                label3.Text = ln.Count + "";
            });
            System.IO.File.WriteAllLines(sfw, lw.ToArray(), System.Text.Encoding.Default);
            System.IO.File.WriteAllLines(sfn, ln.ToArray(), System.Text.Encoding.Default);

        }

        private bool IsUpper(string p)
        {
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("[A-ZÅÄÖ]");
            bool bret =  rx.IsMatch(p);
            return bret;
        }
        DateTime dtStart;
        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text.Equals("Start"))
            {
                dtStart = DateTime.Now;
                
                progressBar1.Style = ProgressBarStyle.Marquee;
                listView1.Items.Clear();
                button3.Text = "Stop";

                ThreadPool.QueueUserWorkItem(getAnagrams, textBox1.Text);
            }
            else
            {
                button3.Text = "Start";
                bRunning3 = false;
            }

        }
        bool bRunning3 = false;
        private void getAnagrams(object state)
        {
            bRunning3 = true;
            string word = state+"";
            Permute p = new Permute();
            p.setper(word);
            for(int i=word.Length;i>0;i--)
            {
                if (!bRunning3)
                    break;
                var tl = p.Permutations.Where(x=>x.Length==i).ToList();
                foreach (var w in tl)
                {
                    if (!bRunning3)
                        break;

                    if (diff(dtOld, DateTime.Now).TotalSeconds > 5)
                    {

                        this.Invoke((MethodInvoker)delegate
                        {
                            label6.Text = p.Permutations.Count + "";
                            dtOld = DateTime.Now;
                            label8.Text = diff(dtStart, DateTime.Now).TotalSeconds + "";
                        });                    
                    }

                    p.setper(w.Substring(0, i - 1));
                }
            }


            List<string> strings = new List<string>();
            foreach (string str in p.Permutations.Distinct<string>().ToArray())
                if (checkBox1.Checked || lw.BinarySearch(str) > 0)
                    strings.Add(str);

            foreach (string str in strings.OrderBy(x => x).OrderByDescending(x => x.Length))
                this.Invoke((MethodInvoker)delegate
                {
                    listView1.Items.Add(str);
                });

            this.Invoke((MethodInvoker)delegate
            {
                label6.Text = listView1.Items.Count+"";
                progressBar1.Style = ProgressBarStyle.Blocks;
                button3.Text = "Start";
                label8.Text = diff(dtStart, DateTime.Now).TotalSeconds+"";
            });
            bRunning3 = false;
        }
        private TimeSpan diff(DateTime dtO, DateTime dtN)
        {
            return new TimeSpan(dtN.Ticks - dtO.Ticks);
        }
        DateTime dtOld = DateTime.Now;
        
        List<string> lw = new List<string>(System.IO.File.ReadAllLines(@"C:\Users\jh\Desktop\words.txt", System.Text.Encoding.Default));
        Dictionary<string, string> dw = Read();
        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show(dw.OrderByDescending(x => x.Value.Split(',').Length).FirstOrDefault().Key);
        }

        string Show(string w)
        {

            string v;
            if (dw.TryGetValue(Alphabetize(w), out v))
            {
                return v;
            }
            return "";
        }
        static Dictionary<string, string> Read()
        {
            var d = new Dictionary<string, string>();
            // Read each line
            using (StreamReader r = new StreamReader(@"C:\Users\jh\Desktop\words.txt",System.Text.Encoding.Default))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    // Alphabetize the line for the key
                    // Then add to the value string
                    string a = Alphabetize(line);
                    string v;
                    if (d.TryGetValue(a, out v))
                    {
                        d[a] = v + "," + line;
                    }
                    else
                    {
                        d.Add(a, line);
                    }
                }
            }
            return d;
        }

        static string Alphabetize(string s)
        {
            // Convert to char array, then sort and return
            char[] a = s.ToCharArray();
            Array.Sort(a);
            return new string(a);
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            listView1.Items.Clear();
            string words = Show(textBox1.Text);
            if(!string.IsNullOrEmpty(words))
            {
                string[] wordsarr = words.Split(',');
                foreach(string str in wordsarr)
                    listView1.Items.Add(str);

            }
            if (textBox1.Text.Length >= 7)
            {
                checkBox1.Checked = false;
                checkBox1.Enabled = false;
            }
            else
            {
                checkBox1.Enabled = true;
            }
        }

    }
    public class Permute
    {
        private void swap(ref char a, ref char b)
        {
            if (a == b)
                return;
            a ^= b;
            b ^= a;
            a ^= b;
        }
        public void setper(string s)
        {
            setper(s.ToCharArray());
        }
        public void setper(char[] list)
        {
            int x = list.Length - 1;
            go(list, 0, x);
        }

        private void go(char[] list, int k, int m)
        {
            int i;
            if (k == m)
            {
                Permutations.Add(new string(list));
            }
            else
                for (i = k; i <= m; i++)
                {
                    swap(ref list[k], ref list[i]);
                    go(list, k + 1, m);
                    swap(ref list[k], ref list[i]);
                }
        }
        public HashSet<string> Permutations = new HashSet<string>();
    }

}
