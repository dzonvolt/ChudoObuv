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

namespace ChudoObuv
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public OrdersPage()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public List<orders> Orders
        {
            get
            {
                return App.Database.orders.ToList();
            }
        }


        private void ViewOrderDetailsBTN_Click(object sender, RoutedEventArgs e)
        {
            App.MainFrame.Navigate(new OrderDetailsPage((sender as Button).DataContext as orders));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = null;
            this.DataContext = this;
        }
    }
}
