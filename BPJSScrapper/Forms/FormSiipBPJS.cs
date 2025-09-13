using BPJSScrapper.Constant;
using BPJSScrapper.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPJSScrapper.Forms
{
    public partial class FormSiipBPJS : Form
    {

        SeleniumHelper seleniumHelper;

        public FormSiipBPJS()
        {
            InitializeComponent();
            seleniumHelper = new SeleniumHelper(LinksVal.bpjs_url);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            seleniumHelper.Start();
        }

        private void FormSiipBPJS_Load(object sender, EventArgs e)
        {

        }

        private void FormSiipBPJS_FormClosed(object sender, FormClosedEventArgs e)
        {
            seleniumHelper.close();
        }
    }
}
