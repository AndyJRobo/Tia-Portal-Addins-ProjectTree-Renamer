using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectTreeRenamer
{
    public partial class RenameForm : Form
    {
        public RenameForm()
        {
            InitializeComponent();
        }

        private void TimerShowForm_Tick(object sender, EventArgs e)
        {
            TimerShowForm.Stop();
            TopMost = true;
            WindowState = FormWindowState.Normal;
            BringToFront();
            //TopLevel = true;
            Focus();
            TopMost = false;
        }

        private void RenameForm_Shown(object sender, EventArgs e)
        {
            TimerShowForm.Start();
        }
    }
}
