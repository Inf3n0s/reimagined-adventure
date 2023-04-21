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
using System.Globalization;


namespace _2022_Poluation
{
    public partial class Autentificare : Form
    {
        public Autentificare()
        {
            InitializeComponent();
            sterge();
            init();
        }

        public static string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Poluare.mdf;Integrated Security=True;Connect Timeout=30";
        private void sterge()
        {
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand delCmd = new SqlCommand("Delete from Harti", con);
            delCmd.ExecuteNonQuery();
            delCmd.Dispose();
            delCmd = new SqlCommand("Delete from Harti", con);
            delCmd.ExecuteNonQuery();
            delCmd.Dispose();
            delCmd = new SqlCommand("Delete from Harti", con);
            delCmd.ExecuteNonQuery();
            delCmd.Dispose();
            con.Close();

        }
        private void init()
        {
            string NumeUtilizator = "oti2022";
            string Parola = "oti1234";
            string EmailUtilizator = "oti2022@oti.com";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Utilizatori WHERE NumeUtilizator = @NumeUtilizator", con);
            checkCmd.Parameters.AddWithValue("@NumeUtilizator", NumeUtilizator);
            int existingCount = (int)checkCmd.ExecuteScalar();
            if (existingCount == 0)
            {
                SqlCommand cmd1 = new SqlCommand("INSERT into Utilizatori (NumeUtilizator,Parola,EmailUtilizator) values (@NumeUtilizator,@Parola,@EmailUtilizator)", con);
                cmd1.Parameters.AddWithValue("NumeUtilizator", NumeUtilizator);
                cmd1.Parameters.AddWithValue("Parola", Parola);
                cmd1.Parameters.AddWithValue("EmailUtilizator", EmailUtilizator);
                cmd1.ExecuteNonQuery();
            }


            StreamReader sr = new StreamReader(Application.StartupPath + @"/OJTI_2022_C#_Resurse/harti.txt");
            string sir;
            char[] split = { '#' };
            while((sir = sr.ReadLine()) != null)
            {
                string[] siruri = sir.Split(split);
                SqlCommand cmd = new SqlCommand("Insert into Harti (NumeHarta,FisierHarta) values (@NumeHarta,@FisierHarta)", con);
                cmd.Parameters.AddWithValue("NumeHarta", siruri[0]);
                cmd.Parameters.AddWithValue("FisierHarta", siruri[1]);
                cmd.ExecuteNonQuery();
            }
            sr.Close();
            StreamReader srM = new StreamReader(Application.StartupPath + @"/OJTI_2022_C#_Resurse/masurari.txt"); 
            string sirM;
            while ((sirM = srM.ReadLine()) != null)
            {
                string[] siruriM = sirM.Split(split);
                int idHarta;

                SqlCommand cmd = new SqlCommand("Select IdHarta from Harti Where NumeHarta=@NumeHarta", con);
                cmd.Parameters.AddWithValue("NumeHarta", siruriM[0]);
                idHarta = (int)cmd.ExecuteScalar();
                string format = "dd/MM/yyyy HH:mm";
                DateTime d = DateTime.ParseExact(siruriM[4], format, CultureInfo.InvariantCulture);
                cmd = new SqlCommand("Insert into Masurare (IdHarta,PozitieX,PozitieY,ValoareMasurare,DataMasurare) values (@IdHarta,@PozitieX,@PozitieY,@ValoareMasurare,@DataMasurare)", con);
                cmd.Parameters.AddWithValue("IdHarta", idHarta);
                cmd.Parameters.AddWithValue("PozitieX", siruriM[1]);
                cmd.Parameters.AddWithValue("PozitieY", siruriM[2]);
                cmd.Parameters.AddWithValue("ValoareMasurare", siruriM[3]);
                cmd.Parameters.AddWithValue("DataMasurare", d);
                cmd.ExecuteNonQuery();
            }

        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Inregistrare inregistrare = new Inregistrare();
            inregistrare.ShowDialog();
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Vizualizare vizualizare = new Vizualizare();
            vizualizare.ShowDialog();
        }
    }
}
