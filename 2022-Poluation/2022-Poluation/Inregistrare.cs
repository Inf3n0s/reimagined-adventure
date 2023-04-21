using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace _2022_Poluation
{
    public partial class Inregistrare : Form
    {
        public Inregistrare()
        {
            InitializeComponent();
        }
        public static string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Poluare.mdf;Integrated Security=True;Connect Timeout=30";

        private static bool IsValid(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }
            return valid;
        }

        private void CancelRegisterBtn_Click(object sender, EventArgs e)
        {
            this.Hide();

            Autentificare autentificare = new Autentificare();
            autentificare.Show();
        }

        private void SaveRegisterBtn_Click(object sender, EventArgs e)
        {
           
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                string numeF = textBox1.Text;
                if (numeF.Length > 4)
                { SqlCommand cmd = new SqlCommand("Select COUNT(*) from Utilizatori where NumeUtilizator=@nume", con);
                    cmd.Parameters.AddWithValue("nume", numeF);
                    int gasit = (int)cmd.ExecuteScalar();
                    if (gasit > 0)
                        MessageBox.Show("Numele exista in baza de date");
                    else
                    {
                    if (pass.Text.Length < 6 || pass.Text != passC.Text)
                        {
                            MessageBox.Show("Parolele nu corespund sau parola este prea scurta minim 6 caractere");
                        }
                        else
                        {
                            if (IsValid(textBox4.Text) == false)
                            {
                                MessageBox.Show("Emailul nu e valid");
                            }
                            else
                            {
                            SqlCommand cmd1 = new SqlCommand("INSERT into Utilizatori (NumeUtilizator,Parola,EmailUtilizator) values (@NumeUtilizator,@Parola,@EmailUtilizator)", con);
                            cmd1.Parameters.AddWithValue("NumeUtilizator", numeF);
                            cmd1.Parameters.AddWithValue("Parola", pass.Text);
                            cmd1.Parameters.AddWithValue("EmailUtilizator", textBox4.Text);
                            cmd1.ExecuteNonQuery();

                            MessageBox.Show("Ati fost inregistrat/ă cu succes");
                            }
                        }
                    } 
                }
                else
                {
                    MessageBox.Show("Nume prea scurt");
                }
        }
    }
}
