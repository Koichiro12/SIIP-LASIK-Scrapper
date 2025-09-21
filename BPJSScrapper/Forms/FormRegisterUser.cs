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
using BPJSScrapper.Constant;

namespace BPJSScrapper.Forms
{
    public partial class FormRegisterUser : Form
    {
        List<string> roleData;
        DatabaseHelper databaseHelper;
        FirebaseHelper firebaseHelper;
        public FormRegisterUser()
        {
            InitializeComponent();
            //Loading Data Role
            roleData = new List<string>() { "Admin", "User" };
            cb_role.DataSource = roleData;
            cb_role.DisplayMember = "Admin";
            cb_role.DropDownStyle = ComboBoxStyle.DropDownList;

            databaseHelper = new DatabaseHelper();
            firebaseHelper = new FirebaseHelper(LinksVal.firebaseDatabaseUrl, LinksVal.firebaseAuthToken != "" ? LinksVal.firebaseAuthToken : null);

        }


        private void RegisterMysql()
        {
            string username = txt_username.Text.Trim();
            string password = txt_password.Text.Trim();
            string role = cb_role.SelectedItem.ToString();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username dan Password tidak boleh kosong.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                using (var conn = databaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO users (username, password, role) VALUES (@username, @password, @role)";
                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@role", role);
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("User berhasil didaftarkan.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Gagal mendaftarkan user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RegisterFirebase()
        {

        }

        private void btn_register_Click(object sender, EventArgs e)
        {
            if (firebaseHelper.testConnections())
            {
                string username = txt_username.Text.Trim();
                string password = txt_password.Text.Trim();
                string role = cb_role.SelectedItem.ToString();
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Username dan Password tidak boleh kosong.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    Users user = new Users( username.Trim(), "", username, password, role);
                    var insert = firebaseHelper.addData<Users>("users",username, user);
                    
                        MessageBox.Show("User berhasil didaftarkan.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    

                }catch(Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Koneksi Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Koneksi Gagal, Silahkan cek koneksi internet anda !");
            }
        }
    }
}
