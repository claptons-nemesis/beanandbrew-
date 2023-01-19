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
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace sqlandcsharpstuff
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string connStr = "server=ND-COMPSCI;" +
            "user=S2102240_L_Reiss;" +
            "database=s2102240;" +
            "port=3306;" +
            "password=S2102240_L_Reiss;";
        public MainWindow()
        {
            InitializeComponent();

        }

        private void loginbtn_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                conn.Open();
                string query = "SELECT * FROM userinfo";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                List<int> userids = new List<int>();
                List<string> usernames = new List<string>();
                List<string> passwords = new List<string>();

                while (rdr.Read())
                {
                    userids.Add(rdr.GetInt32(0));
                    usernames.Add(rdr.GetString(1)); //adding different values at different index points to corresponding lists
                    passwords.Add(rdr.GetString(2));
                }

                string myUsername = usertxt.Text;
                string myPassword = passtxt.Text;

                for (int i = 0; i < usernames.Count; i++)
                {
                    if (myUsername == usernames[i] && myPassword == passwords[i])
                    {
                        this.Visibility = Visibility.Hidden;
                        MessageBox.Show("user found!");
                        //loginwindow newLogin = new loginwindow(userids[i]);
                        //newLogin.Show();
                        preorder preorder = new preorder(userids[i]);
                        preorder.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void registerbtn_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(connStr);

            conn.Open();
            string query = "SELECT * FROM userinfo";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            List<int> userids = new List<int>();
            List<string> usernames = new List<string>();
            List<string> passwords = new List<string>();

            while (rdr.Read())
            {
                userids.Add(rdr.GetInt32(0));
                usernames.Add(rdr.GetString(1)); //adding different values at different index points to corresponding lists
                passwords.Add(rdr.GetString(2));
            }

            rdr.Close();
            int nextId = (userids.Last() + 1);
            string myUsername = usertxt.Text;
            string myPassword = passtxt.Text;
            bool foundUser = false;
            MessageBox.Show(nextId.ToString());

            for (int i = 0; i < usernames.Count; i++)
            {
                if (myUsername == usernames[i])
                {
                    foundUser = true;
                    break;
                }
            }
            if (foundUser)
            {
                MessageBox.Show("sorry, user already exists");
            }
            else
            {
                bool lengthCheck = false, symbolCheck = false, numberCheck = false;
                if (myPassword.Length >= 8)
                {
                    lengthCheck = true;
                }
                if (myPassword.Contains('!') || myPassword.Contains('£') || myPassword.Contains('#') || myPassword.Contains('?') || myPassword.Contains('@') || myPassword.Contains('$') || myPassword.Contains('%') || myPassword.Contains('&'))
                {
                    symbolCheck = true;
                }
                if (myPassword.Contains('1') || myPassword.Contains('2') || myPassword.Contains('3') || myPassword.Contains('4') || myPassword.Contains('5') || myPassword.Contains('6') || myPassword.Contains('7') || myPassword.Contains('8') || myPassword.Contains('9') || myPassword.Contains('0'))
                {
                    numberCheck = true;
                }
                if(lengthCheck == true && symbolCheck == true && numberCheck == true)
                {
                    query = "INSERT INTO userinfo VALUES(@nextId,@myUsername,@myPassword)";
                    cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nextId", nextId);
                    cmd.Parameters.AddWithValue("@myUsername", myUsername);
                    cmd.Parameters.AddWithValue("@myPassword", myPassword);

                    rdr = cmd.ExecuteReader();
                    MessageBox.Show("user added :)");
                }
                else
                {
                    MessageBox.Show("wrong, literally wrong");
                }
            }
        }

        private void validationPracticeStuff()
        {
            string thingy = @"S[0-9]{7}";
            Regex newThing = new Regex(thingy);

        }
    }
}
