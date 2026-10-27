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
    /// Логика взаимодействия для OrderDetailsPage.xaml
    /// </summary>
    public partial class OrderDetailsPage : Page
    {
        public orders order;

        public orders Order { get { return order; } }

        public OrderDetailsPage(orders order)
        {
            InitializeComponent();
            this.DataContext = this;
            this.order = order;
        }

        public void Reload()
        {
            this.DataContext = null;
            this.DataContext = this;
        }

        public bool EditEnabled
        {
            get
            {
                return App.CurrentUser.role_name == "Администратор";
            }
        }

        private void DleteBTN_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот заказ?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No) return;
            try
            {
                App.Database.orders.Remove(order);
                App.Database.SaveChanges();
                App.MainFrame.GoBack();
                MessageBox.Show("Заказ успешно удален.", "Удаление заказа", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении заказа: {ex.Message}");
            }
        }

        private void SaveBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.Database.SaveChanges();
            } catch (Exception ex) {
                MessageBox.Show($"Произошла ошибка при сохранении заказа: {ex}");
                return;
            }

            App.MainFrame.GoBack();
            MessageBox.Show("Заказ успешно сохранён");
        }

        private void RemoveFromOrderBTN_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот товар из заказа?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No) return;
            order_items item = ItemsListView.SelectedItem as order_items;
            this.order.order_items.Remove(item);
            this.Reload();
            
        }

        private void AddPositionBTN_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectProductWindow();
            if (dialog.ShowDialog() == true)
            {
                var product = dialog.ProductsListView.SelectedItem as products;
                if (product == null) return;
                order_items item = this.order.order_items.FirstOrDefault(i => i.positions.product_name == product.name);
                if ((item != null && item.count >= product.count) || product.positions.Count == 0) {
                    MessageBox.Show($"Недостаточно товара {product.name} на складе!");
                    return;
                }
                if (item != null) item.count++;
                else this.order.order_items.Add(new order_items { orders=this.order, positions = product.positions.First(), count = 1 });
                this.Reload();
            }
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

        private void SelectSizeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var item = comboBox.DataContext as order_items;
            if (item == null) return;
            var position = comboBox.SelectedItem as positions;
            this.order.order_items.Remove(item);
            this.order.order_items.Add(new order_items { orders=this.order, positions=position, count=item.count});
            this.Reload();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ChudoObuvEntities.RecreateContext();
        }
    }
}
