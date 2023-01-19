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
using MySql.Data.MySqlClient;

namespace sqlandcsharpstuff
{
    /// <summary>
    /// Interaction logic for loginwindow.xaml
    /// </summary>
    public partial class loginwindow : Window
    {
        public string connStr = "server=ND-COMPSCI;" +
        "user=S2102240_L_Reiss;" +
        "database=s2102240;" +
        "port=3306;" +
        "password=S2102240_L_Reiss;";

        private int id;
        public loginwindow(int id)
        {
            this.id = id;
            InitializeComponent();
        }
        private void resetbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string newPassword = resetpassword.Text;
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();
                string query = "UPDATE userinfo SET password TO '@newpassword' WHERE userid = @id;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@newpassword", newPassword);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                MessageBox.Show("passsword changed :)");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
