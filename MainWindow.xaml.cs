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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            App.window = this;
            App.MainFrame = MainFrame;
            MainFrame.Navigate(new AuthPage());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            BackButton.Visibility = MainFrame.CanGoBack ? Visibility.Visible : Visibility.Hidden;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.GoBack();
        }
    }
}
