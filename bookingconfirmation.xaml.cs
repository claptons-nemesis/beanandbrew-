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
using System.Globalization;
using System.Text.RegularExpressions;

namespace sqlandcsharpstuff
{
    /// <summary>
    /// Interaction logic for bookingconfirmation.xaml
    /// </summary>
    public partial class bookingconfirmation : Window
    {
        public string connStr = "server=ND-COMPSCI;" +
        "user=S2102240_L_Reiss;" +
        "database=s2102240;" +
        "port=3306;" +
        "password=S2102240_L_Reiss;";
        private int id;
        private string location;
        public bookingconfirmation(string location, int id)
        {
            InitializeComponent();
            this.id = id;
            this.location = location;
            dateselect.BlackoutDates.AddDatesInPast();
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            string firstName, lastName, description, time, seats, stringDate, email;
            firstName = firstname.Text;
            lastName = lastname.Text;
            stringDate = dateselect.SelectedDate.ToString();
            time = timedropdown.Text;
            seats = seatsdropdown.Text;
            description = descriptiontextbox.Text;
            email = emailtextbox.Text;
            string name = firstName + lastName;

            if (Regex.IsMatch(email, @"[0-9A-Za-z.]+@[A-Za-z]+(.com|.co.uk|.ac.uk)") && //using regex to validate email
                firstName != "" && lastName != "" && stringDate != "" && time != "" && seats != "") //presence checks
            {
                DateTime date = dateselect.SelectedDate.Value; //defining date as datetime only once there is defo data there otherwise there's an error
                MySqlConnection conn = new MySqlConnection(connStr);

                conn.Open();
                string query = "SELECT * FROM cafebookings";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                List<int> bookingsids = new List<int>(); //get new booking id

                while (rdr.Read())
                {
                    bookingsids.Add(rdr.GetInt32(0));
                }
                rdr.Close();
                int nextId = (bookingsids.Last() + 1);

                try
                {
                    conn = new MySqlConnection(connStr);
                    conn.Open();
                    query = "INSERT INTO cafebookings VALUES(@nextId, @name, @location, @seats, @date, @time, @description, @email);"; //adding booking
                    cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nextId", nextId);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@location", location);
                    cmd.Parameters.AddWithValue("@seats", seats);
                    cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@time", time);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@email", email);
                    rdr = cmd.ExecuteReader();
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                MessageBox.Show("Table booked!");
                if (emailtextbox.Background == Brushes.Red) //validation error catching to change colour of text boxes
                {
                    emailtextbox.Background = Brushes.White;
                }
                if (firstname.Background == Brushes.Red)
                {
                    firstname.Background = Brushes.White;
                }
                if (lastname.Background == Brushes.Red)
                {
                    lastname.Background = Brushes.White;
                }
                if (dateselect.Background == Brushes.Red)
                {
                    dateselect.Background = Brushes.White;
                }
                if (seatsdropdown.Background == Brushes.Red)
                {
                    seatsdropdown.Background = Brushes.White;
                }
                if(timedropdown.Background == Brushes.Red)
                {
                    timedropdown.Background = Brushes.White;
                }
            }
            else
            {
                if (email == "")
                {
                    emailtextbox.Background = Brushes.Red;
                    MessageBox.Show("Email not valid :(");
                }
                if (firstName == "")
                {
                    firstname.Background = Brushes.Red;
                    MessageBox.Show("First Name not valid :(");
                }
                if (lastName == "")
                {
                    lastname.Background = Brushes.Red;
                    MessageBox.Show("Last Name not valid :(");
                }
                if (seats == "")
                {
                    seatsdropdown.Background = Brushes.Red;
                    MessageBox.Show("Seats Number not valid :(");
                }
                if (time == "")
                {
                    timedropdown.Background = Brushes.Red;
                    MessageBox.Show("Time not valid :(");
                }

            }

        }

        private void staffbutton_Click(object sender, RoutedEventArgs e)
        {
            staffbookingwindow staffbookingwindow = new staffbookingwindow();
            staffbookingwindow.Show();
        }

        private void bookingsbutton_Click(object sender, RoutedEventArgs e)
        {
            account account = new account(id);
            account.Show();
        }

        private void preorderbutton_Click(object sender, RoutedEventArgs e)
        {
            preorder preorder = new preorder(id);
            preorder.Show();
        }
    }
}