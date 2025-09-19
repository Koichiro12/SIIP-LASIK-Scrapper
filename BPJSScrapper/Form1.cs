using BPJSScrapper.Forms;
using System;
using System.Collections;
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
            if (FormLogin.LoggedInUser[FormLogin.LoggedInUser.Count - 1].ToString() != "Admin")
            {
                btn_register_user.Visible = false;
            }
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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();

        }

        private void btn_dpt_Click(object sender, EventArgs e)
        {
            FormDPT dpt = new FormDPT();
            dpt.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void btn_register_user_Click(object sender, EventArgs e)
        {
            if (FormLogin.LoggedInUser[FormLogin.LoggedInUser.Count - 1].ToString() == "Admin")
            {
                FormRegisterUser f = new FormRegisterUser();
                f.ShowDialog();
            }
            else
            {
                MessageBox.Show("Hanya admin yang bisa mengakses fitur ini","Akses Ditolak",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
    }
}
