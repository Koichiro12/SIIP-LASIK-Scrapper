using BPJSScrapper.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPJSScrapper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_bpjs_Click(object sender, EventArgs e)
        {
            FormSiipBPJS f = new FormSiipBPJS();
            f.ShowDialog();
        }

        private void btn_lasik_Click(object sender, EventArgs e)
        {
            FormLasik f = new FormLasik();
            f.ShowDialog();
        }
    }
}
