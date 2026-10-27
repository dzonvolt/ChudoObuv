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
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {

        public Visibility ToolBarVisibility
        {
            get {
                return  App.CurrentUser != null ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility AddProductToCartVisibility
        {
            get
            {
                return App.CurrentUser != null ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility OrdersButtonVisibility
        {
            get
            {
                return App.CurrentUser?.role_name == "Менеджер" || App.CurrentUser?.role_name == "Администратор" ? Visibility.Visible : Visibility.Collapsed;
            }
        }


        public Visibility CartButtonVisibility
        {
            get
            {
                return (App.CurrentUser != null && App.Cart.Count > 0) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility AnalyticsButtonVisibility
        {
            get
            {
                return App.CurrentUser?.role_name == "Администратор" ? Visibility.Visible : Visibility.Collapsed;
            }
        }


        public ProductsPage()
        {
            InitializeComponent();
            this.DataContext = this;

            var categories = App.Database.categories.ToList();
            categories.Insert(0, new categories { name = "Все категории" });
            CategoryCB.ItemsSource = categories;
            CategoryCB.SelectedIndex = 0;
        }


        public void Reload()
        {
            this.DataContext = null;
            this.DataContext = this;
        }

        public List<products> Products {
            get
            {
                if (ProductsListView == null) return new List<products>();
                var products = App.Database.products.ToList();

                if (!string.IsNullOrEmpty(SearchTB.Text))
                    products = products.Where(p =>
                        p.name.ToLower().Contains(SearchTB.Text.ToLower()) ||
                        p.description.ToLower().Contains(SearchTB.Text.ToLower()) ||
                        p.construction.ToLower().Contains(SearchTB.Text.ToLower())
                    ).ToList();

                if (SortCB.SelectedIndex == 1)
                    products = products.OrderByDescending(p => p.DiscountedPrice).ToList();
                else if (SortCB.SelectedIndex == 2)
                    products = products.OrderBy(p => p.DiscountedPrice).ToList();


                if (CategoryCB.SelectedIndex > 0)
                    products = products.Where(p => p.categories.name == ((categories)CategoryCB.SelectedItem).name).ToList();

                return products;
            }
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Reload();
        }

        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Reload();
        }

        private void CategoryCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Reload();
        }

        private void AddToCartMenuItemBTN_Click(object sender, RoutedEventArgs e)
        {
            products product = ProductsListView.SelectedItem as products;
            order_items item = App.Cart.FirstOrDefault(i => i.positions.product_name == product.name);
            if ((item != null && item.count >= product.count) || product.positions.Count == 0) {
                MessageBox.Show($"Недостаточно товара {product.name} на складе!");
                return;
            }
            if (item != null) item.count++;
            else App.Cart.Add(new order_items { positions = product.positions.First(), count = 1 });
            this.Reload();
            MessageBox.Show($"Товар {product.name} добавлен в корзину!");
        }

        private void OpenCartBTN_Click(object sender, RoutedEventArgs e)
        {
            App.MainFrame.Navigate(new CartPage());
        }

        private void OpenOrdersBTN_Click(object sender, RoutedEventArgs e)
        {
            App.MainFrame.Navigate(new OrdersPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Reload();
        }

        private void AnalyticsBTN_Click(object sender, RoutedEventArgs e)
        {
            App.MainFrame.Navigate(new AnalyticsPage());
        }
    }
}
