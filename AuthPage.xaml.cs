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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginTB.Text;

            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Введите логин");
                return;
            }

            var user = App.Database.users.FirstOrDefault(u => u.login == login);
            if (user == null)
            {
                MessageBox.Show("Пользователь не найден");
                return;
            }

            App.CurrentUser = user;
            App.Cart.Clear();
            App.MainFrame.Navigate(new ProductsPage());
            if (App.Current.MainWindow is MainWindow window)
                window.CurrentUserInfoTB.Text = $"{user.firstname} {user.lastname} {user.patronymic}\n{user.roles.name}";
            MessageBox.Show($"Добро пожаловать, {user.firstname} {user.lastname}!");

        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            App.Cart.Clear();
            App.MainFrame.Navigate(new ProductsPage());
            if (App.Current.MainWindow is MainWindow window)
                window.CurrentUserInfoTB.Text = "Неавторизованный пользователь";
            MessageBox.Show("Вы продолжаете без авторизации.");
        }
    }
}
