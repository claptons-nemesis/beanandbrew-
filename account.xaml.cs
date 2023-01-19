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

namespace sqlandcsharpstuff
{
    /// <summary>
    /// Interaction logic for account.xaml
    /// </summary>
    public partial class account : Window
    {
        private int id;
        public account(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bookingconfirmation bookingconfirmation = new bookingconfirmation("leeds", id);
            bookingconfirmation.Show();
        }

        private void harrogatebutton_Click(object sender, RoutedEventArgs e)
        {
            bookingconfirmation bookingconfirmation = new bookingconfirmation("harrogate", id);
            bookingconfirmation.Show();

        }

        private void knaresboroughbutton_Click(object sender, RoutedEventArgs e)
        {
            bookingconfirmation bookingconfirmation = new bookingconfirmation("knaresborough", id);
            bookingconfirmation.Show();

        }
    }
}
