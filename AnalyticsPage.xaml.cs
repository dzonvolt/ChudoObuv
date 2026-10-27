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
    /// Логика взаимодействия для AnalyticsPage.xaml
    /// </summary>
    public partial class AnalyticsPage : Page
    {

        DateTime fromDate;
        DateTime toDate;


        public DateTime FromDate
        {
            get => fromDate;
            set { fromDate = value; this.Reload(); }
        }

        public DateTime ToDate
        {
            get => toDate;
            set { toDate = value; this.Reload(); }
        }


        public AnalyticsPage()
        {
            InitializeComponent();
            this.DataContext = this;
            this.fromDate = App.Database.orders.OrderBy(o => o.date).Take(1).ToList().FirstOrDefault()?.date ?? DateTime.Today;
            this.toDate = App.Database.orders.OrderByDescending(o => o.date).Take(1).ToList().FirstOrDefault()?.date ?? DateTime.Today;
        }

        public void Reload()
        {
            this.DataContext = null;
            this.DataContext = this;
        }

        public List<orders> Orders
        {
            get
            {
                return App.Database.orders.Select(o => o).Where(o => (o.date >= fromDate && o.date <= toDate)).ToList();
            }
        }


        public int TotalSales
        {
            get
            {
                return Orders.Sum(o => o.TotalPrice);
            }
        }

        public List<products> TopProducts {
            get
            {
                return Orders.SelectMany(o => o.order_items).Select(oi => oi.positions.products).Distinct().OrderByDescending(p => p.SalesCount).Take(5).ToList();
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //this.Reload();
        }


        private void ExportBTN_Click(object sender, RoutedEventArgs e)
        {
            ExportBTN.Visibility = Visibility.Collapsed;
            FromDatePanel.Visibility = Visibility.Collapsed;
            ToDatePanel.Visibility = Visibility.Collapsed;
            FromDateTextPanel.Visibility = Visibility.Visible;
            ToDateTextPanel.Visibility = Visibility.Visible;
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(this, "Отчёт по аналитике");
                MessageBox.Show("Отчёт подготовлен к печати!");
            }
            ExportBTN.Visibility = Visibility.Visible;
            FromDatePanel.Visibility = Visibility.Visible;
            ToDatePanel.Visibility = Visibility.Visible;
            FromDateTextPanel.Visibility = Visibility.Collapsed;
            ToDateTextPanel.Visibility = Visibility.Collapsed;
        }
    }
}
