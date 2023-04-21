using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace _2022_Poluation
{
    public partial class Vizualizare : Form
    {
        public Vizualizare()
        {
            InitializeComponent();
        }

        public static string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Poluare.mdf;Integrated Security=True;Connect Timeout=30";


        private void Vizualizare_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = null;
            comboBox1.SelectedText = "Selecteaza o harta";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            //label4.Text = "Utilizatorul logat este :" +
            SqlCommand cmd = new SqlCommand("SELECT * from Harti", con);
            var read = cmd.ExecuteReader();
            while (read.Read())
                comboBox1.Items.Add(read[1]);
            comboBox2.SelectedItem = null;
            comboBox2.SelectedText = "Niciun filtru";
            comboBox2.Items.Add("Valoarea < 20");
            comboBox2.Items.Add("20 <= Valoarea <= 40");
            comboBox2.Items.Add("Valoarea > 40");
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                pictureBox1.BackgroundImage = Image.FromFile("OJTI_2022_C#_Resurse/Harti/default_harta.png");
            }
            else
            {
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT FisierHarta from Harti Where NumeHarta=@NumeHarta", con);
                cmd.Parameters.AddWithValue("NumeHarta", comboBox1.SelectedText);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string path = reader["FisierHarta"].ToString();
                    MessageBox.Show(path);
                    pictureBox1.BackgroundImage = Image.FromFile("OJTI_2022_C#_Resurse/Harti/" + path);
                }
                reader.Close();
                con.Close();

            }
        }
    }
}
