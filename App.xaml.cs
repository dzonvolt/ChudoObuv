using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChudoObuv
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ChudoObuvEntities Database { get { return ChudoObuvEntities.GetContext(); } }
        public static MainWindow window;
        public static Frame MainFrame;
        public static users CurrentUser;

        public static List<order_items> Cart = new List<order_items>();

    }
}
