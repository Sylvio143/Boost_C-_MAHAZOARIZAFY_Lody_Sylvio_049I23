using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WpfApplication4
{
    /// <summary>
    /// Logique d'interaction pour crud.xaml
    /// </summary>
    public partial class crud : Window
    {
        public crud()
        {
            InitializeComponent();
            ChargerClients();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetre = new MainWindow();
            fenetre.Show();
            this.Close();
        }

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int id = int.Parse(btn.Tag.ToString());

            string cs = ConfigurationManager.ConnectionStrings["WpfApplication4.Properties.Settings.TESTConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM Client WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            ChargerClients();
        }

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            string nomClient = nom.Text.Trim();
            string prenomClient = prenom.Text.Trim();
            int ageClient;

            if (string.IsNullOrEmpty(nomClient) || string.IsNullOrEmpty(prenomClient) || !int.TryParse(age.Text, out ageClient))
            {
                MessageBox.Show("Veuillez remplir tous les champs correctement.");
                return;
            }

            string cs = ConfigurationManager.ConnectionStrings["WpfApplication4.Properties.Settings.TESTConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "INSERT INTO Client (Nom, Prenom, Age) VALUES (@Nom, @Prenom, @Age)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Nom", nomClient);
                cmd.Parameters.AddWithValue("@Prenom", prenomClient);
                cmd.Parameters.AddWithValue("@Age", ageClient);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            ChargerClients();
            nom.Clear();
            prenom.Clear();
            age.Clear();
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            if (table2.SelectedItem == null)
            {
                MessageBox.Show("Sélectionnez un client à modifier.");
                return;
            }

            DataRowView row = table2.SelectedItem as DataRowView;
            int id = Convert.ToInt32(row["Id"]);

            string nomClient = nom.Text.Trim();
            string prenomClient = prenom.Text.Trim();
            int ageClient;

            if (string.IsNullOrEmpty(nomClient) || string.IsNullOrEmpty(prenomClient) || !int.TryParse(age.Text, out ageClient))
            {
                MessageBox.Show("Veuillez remplir tous les champs correctement.");
                return;
            }

            string cs = ConfigurationManager.ConnectionStrings["WpfApplication4.Properties.Settings.TESTConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "UPDATE Client SET Nom = @Nom, Prenom = @Prenom, Age = @Age WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Nom", nomClient);
                cmd.Parameters.AddWithValue("@Prenom", prenomClient);
                cmd.Parameters.AddWithValue("@Age", ageClient);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            ChargerClients();
            nom.Clear();
            prenom.Clear();
            age.Clear();
        }

        private void ChargerClients()
        {
            string cs = ConfigurationManager.ConnectionStrings["WpfApplication4.Properties.Settings.TESTConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Client";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (table2 != null)
                {
                    table2.ItemsSource = dt.DefaultView;
                }
            }
        }

        private void table2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (table2.SelectedItem == null) return;

            DataRowView row = table2.SelectedItem as DataRowView;
            if (row != null)
            {
                nom.Text = row["Nom"].ToString();
                prenom.Text = row["Prenom"].ToString();
                age.Text = row["Age"].ToString();
            }
        }
    }
}
