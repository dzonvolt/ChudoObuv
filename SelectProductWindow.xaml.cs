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

namespace ChudoObuv
{
    /// <summary>
    /// Логика взаимодействия для SelectProductWindow.xaml
    /// </summary>
    public partial class SelectProductWindow : Window
    {
        public SelectProductWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public List<products> Products
        {
            get
            {
                return App.Database.products.ToList();
            }
        }

        private void ProductsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelBTN_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
