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

namespace Scheduler.Pages
{
    /// <summary>
    /// Логика взаимодействия для Entry.xaml
    /// </summary>
    public partial class Entry : Page
    {
        public Entry()
        {
            InitializeComponent();
        }

        private void ButtonReg_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow)?.RootNavigation.Navigate("регистрация");
        }
        private void ButtonEntry_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).NavigationItemEntry.Visibility = Visibility.Collapsed;
            (Application.Current.MainWindow as MainWindow).NavigationItemShedule.Visibility = Visibility.Visible;
            (Application.Current.MainWindow as MainWindow)?.RootNavigation.Navigate("планы");
        }
    }
}
