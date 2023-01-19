using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
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
    /// Interaction logic for staffbookingwindow.xaml
    /// </summary>
    public partial class staffbookingwindow : Window
    {
        public string connStr = "server=ND-COMPSCI;" +
        "user=S2102240_L_Reiss;" +
        "database=s2102240;" +
        "port=3306;" +
        "password=S2102240_L_Reiss;";


        public staffbookingwindow()
        {
            InitializeComponent();
        }

        private void submitbutton_Click(object sender, RoutedEventArgs e)
        {
            DateTime startDate = bookingdates.SelectedDates.First(); //getting start and end range for dates
            DateTime endDate = bookingdates.SelectedDates.Last();
            string location = locationdropdown.Text;
            MySqlCommand cmd;

            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();

                string query2 = String.Format("SELECT name, seats, date, time, description FROM cafebookings WHERE (location = '{0}') " +
                    "AND (date BETWEEN '{1}' AND '{2}');", location, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
                cmd = new MySqlCommand(query2, conn); 
                //IT WORKED OMG basically parameters hate women did formatting instead. why? idk dont ask it works i can sleep peacefully now

                cmd.ExecuteNonQuery();


                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                cafebookingsgrid.ItemsSource = dt.DefaultView;  //filling data grid with results instead of adding them to a reader
                MessageBox.Show("finished");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
