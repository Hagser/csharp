using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace MyKillNonResponders
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            List<Proc> processes = new List<Proc>();
            foreach (Process p in Process.GetProcesses())
            {
                processes.Add(new Proc(p));
            }
            dataGridView1.DataSource = processes;
        }

        Dictionary<int, DateTime> nonresponders = new Dictionary<int, DateTime>();

        private void timer1_Tick(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(getprocesses, null);
        }
        bool bgetting = false;
        private void getprocesses(object state)
        {
            if (bgetting)
                return;
            bgetting = true;
            List<Proc> processes = dataGridView1.DataSource as List<Proc>;
            foreach (Process p in Process.GetProcesses())
            {
                if (!p.Responding)
                {
                    if (!nonresponders.ContainsKey(p.Id))
                        nonresponders.Add(p.Id, System.DateTime.Now);
                }
                else
                {
                    if (nonresponders.ContainsKey(p.Id))
                        nonresponders.Remove(p.Id);
                }
                var proc = processes.FirstOrDefault(x => x.MainWindowHandle == p.MainWindowHandle && x.Id == p.Id && x.ProcessName == p.ProcessName);
                if (proc != null)
                {
                    proc.Reload(p);
                }
                else
                {
                    this.Invoke((MethodInvoker)delegate {
                        processes.Add(new Proc(p));
                    });                    
                }
            }

            foreach (int pid in nonresponders.Where(x => processes.Any(y => y.Id == x.Key) && (new TimeSpan(System.DateTime.Now.Ticks - x.Value.Ticks)).TotalMinutes >= 1).Select(x => x.Key))
            {
                if (DialogResult.Yes == MessageBox.Show("Kill it: " + processes.FirstOrDefault(x => x.Id == pid).ProcessName, "Kill", MessageBoxButtons.YesNo))
                {
                    Process.GetProcessById(pid).Kill();
                }
                else
                {
                    nonresponders.Remove(pid);
                }
            }

            this.Invoke((MethodInvoker)delegate
            {
                dataGridView1.DataSource = processes;
                //dataGridView1.RefreshEdit();
                dataGridView1.Update();
            });
            bgetting = false;
        }
    }
    public class Proc
    {
        private Process process;
        public int Id { get { return process.Id; } set { } }
        public int Priority { get { return process.BasePriority; } set { } }
        //public ProcessPriorityClass PriorityClass { get { return process.PriorityClass; } set { } }
        public string ProcessName { get { return process.ProcessName; } set { } }
        public bool Responding { get { return process.Responding; } set { } }
        public IntPtr MainWindowHandle { get { return process.MainWindowHandle; } set { } }
        public long WorkingSet { get { return process.WorkingSet64; } set { } }
        public long VirtualMemorySize { get { return process.VirtualMemorySize64; } set { } }
        public Proc(Process process)
        {
            this.process = process;            
        }
        public void Reload(Process process)
        {
            this.process = process;
        }
    }
}
