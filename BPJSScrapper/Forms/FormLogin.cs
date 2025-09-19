using BPJSScrapper.Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Windows.Forms;

namespace BPJSScrapper.Forms
{
    public partial class FormLogin : Form
    {

        DatabaseHelper dbHelper;

        public static ArrayList LoggedInUser { get; private set; }

        public FormLogin()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

       

        private void btn_login_Click(object sender, EventArgs e)
        {
            if (dbHelper.TestConnection())
            {
                string username = txt_username.Text.Trim();
                string password = txt_password.Text.Trim();
                string hwid = dbHelper.getHWID();
                if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Username dan Password tidak boleh kosong");
                    return;
                }
                try
                {
                    using (MySqlConnection conn = dbHelper.GetConnection())
                    {
                        conn.Open();

                        string query = "SELECT * FROM users WHERE username=@username AND password=@password LIMIT 1";
                        using(MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", password);
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // login success
                                    LoggedInUser = new ArrayList();
                                    LoggedInUser.Add(reader["users_id"].ToString());
                                    LoggedInUser.Add(reader["hwid"].ToString());
                                    LoggedInUser.Add(reader["username"].ToString());
                                    LoggedInUser.Add(reader["role"].ToString());

                                    string dbHWID = reader["hwid"] == DBNull.Value ? "" : reader["hwid"].ToString();
                                    reader.Close();

                                    if(string.IsNullOrEmpty(dbHWID))
                                    {
                                        // update hwid
                                        string updateQuery = "UPDATE users SET hwid=@hwid WHERE users_id=@id";
                                        using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                                        {
                                            updateCmd.Parameters.AddWithValue("@hwid", hwid);
                                            updateCmd.Parameters.AddWithValue("@id", LoggedInUser[0].ToString());
                                            updateCmd.ExecuteNonQuery();
                                        }
                                        MessageBox.Show("Login Berhasil! HWID telah disimpan.", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        Form1 form = new Form1();
                                        form.Show();
                                        this.Hide();
                                    } else if(dbHWID == hwid)
                                        {
                                            MessageBox.Show("Login Berhasil!", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            Form1 form = new Form1();
                                            form.Show();
                                            this.Hide();
                                        
                                    }
                                    else
                                    {
                                            MessageBox.Show("Login ditolak! Device Tidak Terdaftar", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                }
                                else
                                {
                                    MessageBox.Show("Username atau Password salah","Login Failed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message,"Database Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                
            }
            else
            {
                MessageBox.Show("Koneksi Gagal Karena : " + dbHelper.lastError+"\n Silahkan cek koneksi database anda !");
            }
        }

        private void btn_test_connections_Click(object sender, EventArgs e)
        {
            if(dbHelper.TestConnection())
            {
                MessageBox.Show("Koneksi Berhasil");
            }
            else
            {
                MessageBox.Show("Koneksi Gagal: " + dbHelper.lastError);
            }
        }
    }
}
