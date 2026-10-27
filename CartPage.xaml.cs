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
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    /// 


    public partial class CartPage : Page
    {
        public CartPage()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void Reload()
        {
            this.DataContext = null;
            this.DataContext = this;
        }

        public Visibility UserSelectVisibility
        {
            get
            {
                var role = App.CurrentUser.role_name;
                return (role == "Менеджер" || role == "Администратор") ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public List<users> Users
        {
            get
            {
                return App.Database.users.ToList();
            }
        }


        public int TotalPrice {
            get { 
                return App.Cart.Sum(i => i.positions.products.DiscountedPrice * i.count);
            }
        }

        public bool CheckoutButtonEnabled
        {
            get
            {
                return App.Cart.Count > 0;
            }
        }

        public List<order_items> Cart {
            get { return App.Cart; }
        }


        private void RemoveFromCartBTN_Click(object sender, RoutedEventArgs e)
        {
            order_items item = ItemsListView.SelectedItem as order_items;
            App.Cart.Remove(item);
            this.Reload();
        }



        private void CheckoutBTN_Click(object sender, RoutedEventArgs e)
        {
            users user = UserCB.SelectedItem != null ? UserCB.SelectedItem as users : App.CurrentUser;
            if (user == null) return;
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите оформить заказ на имя: {user.FullName}?", "Подтверждение", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;

            orders order = new orders
            {
                user_login = user.login,
                date = DateTime.Now,
                order_items = App.Cart.ToList()
            };

            try
            {
                App.Database.orders.Add(order);
                foreach (var item in App.Cart)
                    item.positions.products.count -= item.count;
                App.Database.SaveChanges();

                App.Cart.Clear();
                App.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при оформлении заказа: {ex.Message}");
                return;
            }

            MessageBox.Show("Оформление заказа успешно!");
        }

        private void MinusItemsBTN_Click(object sender, RoutedEventArgs e)
        {
            order_items item = (sender as Button).DataContext as order_items;
            if (item.count <= 1) return;
            item.count--;
            this.Reload();
        }

        private void PlusItemsBTN_Click(object sender, RoutedEventArgs e)
        {
            order_items item = (sender as Button).DataContext as order_items;
            if (item.count >= item.positions.products.count) return;
            item.count++;
            this.Reload();
        }

    }
}
