using Microsoft.EntityFrameworkCore;
using Scheduler.DataBaseClass;
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
        private async void ButtonEntry_Click(object sender, RoutedEventArgs e)
        {
            Classes.SettingProgram s = new();
            (Application.Current.MainWindow as MainWindow).ProgressRingLoad.Visibility = Visibility.Visible;
            ButtonEntry.IsEnabled = false;
            ButtonReg.IsEnabled = false;

            if (!String.IsNullOrWhiteSpace(TextBoxLogin.Text) && !String.IsNullOrWhiteSpace(PasswordBoxPass.Text))
            {
                using schedulerContext db = new();

                if (await db.Database.CanConnectAsync())
                {
                    User? user = await db.Users.FirstOrDefaultAsync(u => u.UsersLogin == TextBoxLogin.Text.Trim() && u.UsersPassword == s.Hash(PasswordBoxPass.Text.Trim()));

                    if (user != null)
                    {
                        (Application.Current.MainWindow as MainWindow).NavigationItemEntry.Visibility = Visibility.Collapsed;
                        (Application.Current.MainWindow as MainWindow).NavigationItemShedule.Visibility = Visibility.Visible;
                        (Application.Current.MainWindow as MainWindow)?.RootNavigation.Navigate("планы");
                    }
                    else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Логин или пароль не совпадают");
                }
                else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "База данных недоступна");
            }
            else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Заполните поля");

            (Application.Current.MainWindow as MainWindow).ProgressRingLoad.Visibility = Visibility.Hidden;
            ButtonEntry.IsEnabled = true;
            ButtonReg.IsEnabled = true;
        }
    }
}
