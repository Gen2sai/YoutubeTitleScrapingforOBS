using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Automation;
using WindowsFormsApplication1.img;
using HelperProject;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        loopingform form = new loopingform();
        object obj1;
        object obj2;
        public Form1()
        {
            InitializeComponent();

            MessageBox.Show("Make sure chrome is opened and is not minimized (keep it behind a window or something but just do not minimize it). If you have it minimized, just unminimize google chrome and refresh the application.");

            button2.Visible = false;
            button2.Enabled = false;
            backgroundWorker1.WorkerSupportsCancellation = true;

            string title = GetActiveTabUrl(listBox1);

        }

        public static string GetActiveTabUrl(ListBox listBox1)
        {
            Process[] procsChrome = Process.GetProcessesByName("chrome");
            List<string> tabs = new List<string>();

            if (procsChrome.Length <= 0)
                tabs.Add("Google Chrome not detected please start or restart google chrome.");

            foreach (Process proc in procsChrome)
            {
                // the chrome process must have a window 
                if (proc.MainWindowHandle == IntPtr.Zero)
                    continue;

                AutomationElement root = AutomationElement.FromHandle(proc.MainWindowHandle);
                Condition condNewTab = new PropertyCondition(AutomationElement.NameProperty, "New Tab");
                AutomationElement elmNewTab = root.FindFirst(TreeScope.Descendants, condNewTab);
                // get the tabstrip by getting the parent of the 'new tab' button 
                TreeWalker treewalker = TreeWalker.ControlViewWalker;
                AutomationElement elmTabStrip = treewalker.GetParent(elmNewTab);
                // loop through all the tabs and get the names which is the page title 
                Condition condTabItem = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem);
                foreach (AutomationElement tabitem in elmTabStrip.FindAll(TreeScope.Children, condTabItem))
                {
                    Console.WriteLine(tabitem.Current.Name);
                    tabs.Add(tabitem.Current.Name);
                }
            }

            listBox1.DataSource = tabs;

            return null;
        }

        public static string loopTab(string tabName, string pathName)
        {

            Process[] procsChrome = Process.GetProcessesByName("chrome");

            if (procsChrome.Length <= 0)
                return null;

            //trim received tabName

            if (tabName.Contains("Audio playing"))
            {
                tabName = tabName.Replace("Audio playing", "").Trim();
            }

            foreach (Process proc in procsChrome)
            {
                WriterHelper writer = new WriterHelper();
                // the chrome process must have a window 
                if (proc.MainWindowHandle == IntPtr.Zero)
                    continue;

                AutomationElement root = AutomationElement.FromHandle(proc.MainWindowHandle);
                Condition condNewTab = new PropertyCondition(AutomationElement.NameProperty, "New Tab");
                AutomationElement elmNewTab = root.FindFirst(TreeScope.Descendants, condNewTab);
                // get the tabstrip by getting the parent of the 'new tab' button 
                TreeWalker treewalker = TreeWalker.ControlViewWalker;
                AutomationElement elmTabStrip = treewalker.GetParent(elmNewTab);
                // loop through all the tabs and get the names which is the page title 
                Condition condTabItem = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem);
                foreach (AutomationElement tabitem in elmTabStrip.FindAll(TreeScope.Children, condTabItem))
                {
                    if (tabitem.Current.Name.Contains(tabName))
                    {
                        while (true)
                        {
                            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
                            List<string> tempList = new List<string>();
                            string temp = tabitem.Current.Name;
                            if (temp.Contains("Audio playing"))
                            {
                                temp = temp.Replace("Audio playing", "").Trim();
                            }
                            tempList.Add(temp);
                            //Console.WriteLine(tabitem.Current.Name);
                            writer.WriterText(pathName, @"\youtubetitle.txt", tempList);
                        }
                    }
                    else
                        continue;
                }
            }

            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1 || (string)listBox1.SelectedItem == "Google Chrome not detected please start or restart google chrome.")
                MessageBox.Show("Please select an Item first or restart chrome if nothing is detected.");
            else
            {
                DisableForm();
                form.StartPosition = FormStartPosition.Manual;
                form.Location = new Point(this.Location.X + (this.Width - form.Width) / 2, this.Location.Y + (this.Height - form.Height) / 2);
                obj2 = (string)listBox1.SelectedValue;
                object[] param = new object[] { obj1, obj2 };
                backgroundWorker1.RunWorkerAsync(param);
                form.Show();

            }
        }

        private void DisableForm()
        {
            listBox1.Enabled = false;
            button1.Enabled = false;
            button3.Enabled = false;
            button2.Visible = true;
            button2.Enabled = true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            bool x = true;
            while (x)
            {
                //dowork
                //loopTab((string)e.Argument);

                //if (backgroundWorker1.CancellationPending)
                //{
                //    x = false;
                //    e.Cancel = true;
                //    return;
                //}

                while (!backgroundWorker1.CancellationPending)
                {
                    object[] param = e.Argument as object[];

                    loopTab((string)param[1], (string)param[0]);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Visible = false;
            backgroundWorker1.CancelAsync();
            button2.Enabled = false;
            form.Close();
            listBox1.Enabled = true;
            button1.Enabled = true;
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string title = GetActiveTabUrl(listBox1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath; // prints path
            }
            obj1 = textBox1.Text;
        }
    }
}