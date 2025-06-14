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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WpfApplication4
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string cs = ConfigurationManager.ConnectionStrings["WpfApplication4.Properties.Settings.TESTConnectionString"].ConnectionString;
        public MainWindow()
        {
            InitializeComponent();
            ChargerClients();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            crud fenetre = new crud();
            fenetre.Show();
            this.Close();
        }
        private void ChargerClients()
        {
            using(SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * from client", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                table1.ItemsSource = dt.DefaultView;
            }
        }
    }
}
