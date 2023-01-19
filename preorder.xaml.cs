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
    /// Interaction logic for preorder.xaml
    /// </summary>
    public partial class preorder : Window
    {
        public string connStr = "server=ND-COMPSCI;" +
        "user=S2102240_L_Reiss;" +
        "database=s2102240;" +
        "port=3306;" +
        "password=S2102240_L_Reiss;";

        private int id;
        List<string> products = new List<string>();
        List<int> quantities = new List<int>();
        List<string>allProducts = new List<string>();

        public preorder(int id)
        {
            InitializeComponent();
            this.id = id;

            allProducts.Add("cofi"); //getting definitive list of all products on site
            allProducts.Add("latte");
            allProducts.Add("chai");
            allProducts.Add("flat white");
            allProducts.Add("americano");
            allProducts.Add("mocha");

            MySqlConnection conn;
            foreach (var item in allProducts)
            {
                conn = new MySqlConnection(connStr);
                conn.Open();

                Label label = selectLabel(item);
                ComboBox dropdown = selectDropDown(item);
                Button button = selectButton(item);

                string query = "SELECT stock FROM cafeproducts WHERE name = @item"; //finding the stock value for each item
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@item", item);
                MySqlDataReader rdr = cmd.ExecuteReader();

                List<int> stock = new List<int>();

                while (rdr.Read())
                {
                    stock.Add(rdr.GetInt32(0));
                }
                rdr.Close();

                if (stock[0] == 0) //if there's no stock, don't let the user order any
                {
                    label.Foreground = Brushes.Red;
                    label.Content = "Sold out";
                    dropdown.Items.RemoveAt(2);
                    dropdown.Items.RemoveAt(1);
                    dropdown.Items.RemoveAt(0);

                    button.IsEnabled = false;
                }
                else if (stock[0] <= 3) //if there's less stock, warn the user and don't let them order over the amount that is available
                {
                    label.Foreground = Brushes.Gold;
                    label.Content = "Low on stock";
                    if (stock[0] == 2)
                    {
                        dropdown.Items.RemoveAt(2);
                    }
                    else if (stock[0] == 1)
                    {
                        dropdown.Items.RemoveAt(2);
                        dropdown.Items.RemoveAt(1);
                    }
                }
                else //if there's more than 3 available, show the item is in stock with no restrictions
                {
                    label.Foreground = Brushes.Green;
                    label.Content = "In stock";
                }
            }
        }

        private void bookingsbutton_Click(object sender, RoutedEventArgs e)
        {
            account account = new account(id);
            account.Show();
        }

        private void item_button_click(object sender, RoutedEventArgs e)
        {
            Button thisItem = sender as Button;
            receipttext.Text = receipttext.Text + thisItem.Content + "\n"; //adding items to visual reciept
            products.Add(thisItem.Content.ToString()); //storing product name to be added to orderdetails

            if (thisItem == lattebutton)
            {
                quantities.Add(int.Parse(lattequantity.Text)); //storing product quantity to match up with product name by indexes
                receipttext.Text = receipttext.Text + lattequantity.Text + "x" + "\n";
            }
            else if (thisItem == chaibutton)
            {
                quantities.Add(int.Parse(chaiquantity.Text));
                receipttext.Text = receipttext.Text + chaiquantity.Text + "x" + "\n";

            }
            else if (thisItem == americanobutton)
            {
                quantities.Add(int.Parse(americanoquantity.Text));
                receipttext.Text = receipttext.Text + americanoquantity.Text + "x" + "\n";
            }
            else if (thisItem == mochabutton)
            {
                quantities.Add(int.Parse(mochaquantity.Text));
                receipttext.Text = receipttext.Text + mochaquantity.Text + "x" + "\n";
            }
            else if (thisItem == flatwhitebutton)
            {
                quantities.Add(int.Parse(flatwhitequantity.Text));
                receipttext.Text = receipttext.Text + flatwhitequantity.Text + "x" + "\n";
            }
            else if (thisItem == cofibutton)
            {
                quantities.Add(int.Parse(cofiquantity.Text));
                receipttext.Text = receipttext.Text + cofiquantity.Text + "x" + "\n";
            }

        }

        private void submit_order(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = DateTime.Now; //getting current date for order record
            int productId = -1;
            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();
                string query = "SELECT * FROM orders";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                List<int> orderids = new List<int>();

                while (rdr.Read())
                {
                    orderids.Add(rdr.GetInt32(0));
                }
                rdr.Close();
                int nextId = (orderids.Last() + 1);
                int orderid = nextId; //getting new order id
                        
                conn = new MySqlConnection(connStr);
                conn.Open();

                query = "INSERT INTO orders VALUES (@idorders, @userid, @date);"; //adding new order to link order details together
                cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idorders", nextId);
                cmd.Parameters.AddWithValue("@userid", id);
                cmd.Parameters.AddWithValue("@date", dateTime);
                rdr = cmd.ExecuteReader();
                rdr.Close();

                for (int i = 0; i < products.Count; i++) //making a new order detail entry for each item bought
                {
                    conn = new MySqlConnection(connStr);
                    conn.Open();
                    query = "SELECT * FROM orderdetails";
                    cmd = new MySqlCommand(query, conn);
                    rdr = cmd.ExecuteReader();

                    List<int> orderdetails = new List<int>();

                    while (rdr.Read())
                    {
                        orderdetails.Add(rdr.GetInt32(0));
                    }
                    rdr.Close();
                    nextId = (orderdetails.Last() + 1);
                    int orderDetailId = nextId;

                    conn = new MySqlConnection(connStr);
                    conn.Open();
                    query = "SELECT * FROM cafeproducts;";
                    cmd = new MySqlCommand (query, conn);
                    cmd.Parameters.AddWithValue("@productname", products[i]);
                    rdr = cmd.ExecuteReader();

                    List<int> productids = new List<int>();
                    List<string> productnames = new List<string>(); //finding product id via product name to add to orderdetails
                    List<int> productStock = new List<int>();
                    int stock = -1;

                    while (rdr.Read())
                    {
                        productids.Add(rdr.GetInt32(0));
                        productnames.Add(rdr.GetString(1));
                        productStock.Add(rdr.GetInt32(3));
                    }
                    rdr.Close();

                    for (int j = 0; j < productnames.Count; j++)
                    {
                        if (products[i] == productnames[j])
                        {
                            productId = productids[j];
                            stock = productStock[j];
                            stock -= 1;
                            break;
                        }
                    }

                    conn = new MySqlConnection(connStr);
                    conn.Open();
                    query = "INSERT INTO orderdetails VALUES (@idorderdetail, @orderid, @productid, @quantity);";
                    cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idorderdetail", orderDetailId);
                    cmd.Parameters.AddWithValue("@orderid", orderid);
                    cmd.Parameters.AddWithValue("@productid", productId); //get productid from list first
                    cmd.Parameters.AddWithValue("@quantity", quantities[i]);
                    rdr = cmd.ExecuteReader();
                    rdr.Close();

                    conn = new MySqlConnection(connStr);
                    conn.Open();
                    query = "UPDATE cafeproducts SET stock = @stock WHERE idcafeproducts = @id;";
                    cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@stock", stock);
                    cmd.Parameters.AddWithValue("@id", productId);
                    rdr = cmd.ExecuteReader();
                    rdr.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            MessageBox.Show("order placed!");
        }

        private Label selectLabel(string item) //selecting the right warning label to tell the user about current stock for an item
        {
            if (item == "cofi")
            {
                return cofilabel;
            }
            else if (item == "latte")
            {
                return lattelabel;
            }
            else if (item == "mocha")
            {
                return mochalabel;
            }
            else if (item == "chai")
            {
                return chailabel;
            }
            else if (item == "americano")
            {
                return americanolabel;
            }
            else
            {
                return flatwhitelabel;
            }
        }
        private ComboBox selectDropDown(string item) //setting correct drop down box for items to set the right amount of quantity available
        {
            if (item == "cofi")
            {
                return cofiquantity;
            }
            else if (item == "latte")
            {
                return lattequantity;
            }
            else if (item == "mocha")
            {
                return mochaquantity;
            }
            else if (item == "chai")
            {
                return chaiquantity;
            }
            else if (item == "americano")
            {
                return americanoquantity;
            }
            else
            {
                return flatwhitequantity;
            }
        }

        private Button selectButton(string item) //setting the right button when formatting stock
        {
            if (item == "cofi")
            {
                return cofibutton;
            }
            else if (item == "latte")
            {
                return lattebutton;
            }
            else if (item == "mocha")
            {
                return mochabutton;
            }
            else if (item == "chai")
            {
                return chaibutton;
            }
            else if (item == "americano")
            {
                return americanobutton;
            }
            else
            {
                return flatwhitebutton;
            }
        }
    }
}
