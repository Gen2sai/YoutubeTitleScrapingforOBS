using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1.img
{
    public partial class loopingform : Form
    {
        public int i = 0;
        public loopingform()
        {
            InitializeComponent();
            ControlBox = false;
            ShowIcon = false;

            Timer t = new Timer();
            t.Tick += new EventHandler(loadingText);
            t.Interval = 1000;
            t.Start();
        }

        private void loadingText(object sender, EventArgs e)
        {
            if (i == 0)
            {
                label1.Text = "Looping .";
                i++;
            }
            else if (i == 1)
            {
                label1.Text = "Looping ..";
                i++;
            }
            else
            {
                label1.Text = "Looping ...";
                i = 0;
            }
        }
    }
}
