using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Esf;

namespace WinFormGyumolcsok
{
    public partial class Form1 : Form
    {
        MySqlConnection conn;
        MySqlCommand cmd;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.UserID = "root";
            builder.Password = "";
            builder.Database = "gyumolcsok";
            conn = new MySqlConnection(builder.ConnectionString);
            try
            {
                conn.Open();
                cmd = conn.CreateCommand();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "A program leáll!");
                Environment.Exit(0);
            }
            finally
            {
                conn.Close();
            }

            Gyumolcsok_Update();

        }
        private void Gyumolcsok_Update()
        {
            listBoxGyumolcsok.Items.Clear();
            cmd.CommandText = "SELECT `id`,`nev`,`egysegar`,`mennyiseg` FROM `gyumolcsok` WHERE 1";
            conn.Open();
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    Gyumolcs uj = new Gyumolcs(dr.GetInt32("id"), dr.GetString("nev"), dr.GetInt32("egysegar"), dr.GetInt32("mennyiseg"));
                    listBoxGyumolcsok.Items.Add(uj);
                }

                
            }
            conn.Close();


        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
           cmd.Parameters.Clear();
            cmd.CommandText = "INSERT INTO `gyumolcsok` (`id`, `nev`, `egysegar`, `mennyiseg`) VALUES (NULL, @nev, @egysegar, @mennyiseg);";
            cmd.Parameters.AddWithValue("@nev",textBoxNev.Text);
            cmd.Parameters.AddWithValue("@egysegar", textBoxEgysegar.Text);
            cmd.Parameters.AddWithValue("@mennyiseg",textBoxMennyiseg.Text);
            conn.Open();
            if (cmd.ExecuteNonQuery()==1) 
            {
                MessageBox.Show("Sikeres rogzites!");
            }
            if (conn.State== ConnectionState.Open)
            {
            conn.Close();
            }
            Gyumolcsok_Update();



        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (listBoxGyumolcsok.SelectedIndex < 0) 
            {
                MessageBox.Show("Nincs kijelolve gyumolcs!");
            }

            cmd.CommandText= "UPDATE `gyumolcsok` SET `id`=@id,`nev`=@nev,`egysegar`=@egysegar,`mennyiseg`=@mennyiseg WHERE id=@id";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@id",textBoxId.Text);
            cmd.Parameters.AddWithValue("@nev", textBoxNev.Text);
            cmd.Parameters.AddWithValue("@egysegar", textBoxEgysegar.Text);
            cmd.Parameters.AddWithValue("@mennyiseg",textBoxMennyiseg.Text);
            conn.Open();
            if (cmd.ExecuteNonQuery()==1)
            {
                MessageBox.Show("Sikeres modositas!");
                conn.Close();
                //textBoxId.Text = "";
                //textBoxNev.Text = "";
                //textBoxEgysegar.Text = "";
                //textBoxMennyiseg.Text = "";
            }
            conn.Close();
        }

        private void listBoxGyumolcsok_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxGyumolcsok.SelectedIndex<0)
            {
                return;
            }
            Gyumolcs kivalasztottgyumolcs = (Gyumolcs)listBoxGyumolcsok.SelectedItem;
            textBoxId.Text = kivalasztottgyumolcs.Id.ToString();
            textBoxNev.Text=kivalasztottgyumolcs.Nev.ToString();
            textBoxEgysegar.Text = kivalasztottgyumolcs.Egysegar.ToString();
            textBoxMennyiseg.Text = kivalasztottgyumolcs.Mennyiseg.ToString();

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listBoxGyumolcsok.SelectedIndex < 0)
            {
                return;
            }

            cmd.CommandText = "DELETE FROM `gyumolcsok` WHERE `id`=@id";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@id", textBoxId.Text)   ;
            conn.Open();
            if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("sikeres torles");
            }

            conn.Close();
            Gyumolcsok_Update();
        }
    }
}
