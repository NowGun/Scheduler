using Microsoft.EntityFrameworkCore;
using Scheduler.Classes;
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
    /// Логика взаимодействия для Account.xaml
    /// </summary>
    public partial class Account : Page
    {
        public Account()
        {
            InitializeComponent();
        }
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).isEntry = false;
            (Application.Current.MainWindow as MainWindow).NavigationItemEntry.Visibility = Visibility.Visible;
            (Application.Current.MainWindow as MainWindow).NavigationItemShedule.Visibility = Visibility.Collapsed;
            (Application.Current.MainWindow as MainWindow).NavigationItemBookMark.Visibility = Visibility.Collapsed;
            (Application.Current.MainWindow as MainWindow).NavigationItemLC.Visibility = Visibility.Collapsed;
            (Application.Current.MainWindow as MainWindow).NavigationItemGroups.Visibility = Visibility.Collapsed;
            (Application.Current.MainWindow as MainWindow)?.RootNavigation.Navigate("Entry");
        }
        private async void LoadInfo()
        {
            using schedulerContext db = new();

            User? u = await db.Users.FirstOrDefaultAsync(u => u.Idusers == Properties.Settings.Default.IdUser);

            if (u != null)
            {
                LabelName.Content = $"{u.UsersSurname} {u.UsersName}";
                TextBoxName.Text = u.UsersName;
                TextBoxSurname.Text = u.UsersSurname;
                LabelMail.Content = $"Текущая почта: {u.UsersEmail}";
                LabelPhone.Content = $"Текущий номер телефона: {u.UsersPhone}";
            }
        }
        private async void ButtonUpdateName_Click(object sender, RoutedEventArgs e)
        {
            using schedulerContext db = new();

            User? nsn = await db.Users.FirstOrDefaultAsync(n => n.Idusers == Properties.Settings.Default.IdUser);

            if (nsn != null)
            {
                nsn.UsersName = TextBoxName.Text;
                nsn.UsersSurname = TextBoxSurname.Text;

                await db.SaveChangesAsync();
                (Application.Current.MainWindow as MainWindow)?.Notify("Уведомление", "Данные обновлены");
            }
            else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Произошла ошибка");
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadInfo();
        }
        private async void ButtonUpdatePass_Click(object sender, RoutedEventArgs e)
        {
            using schedulerContext db = new();
            User? nsn = await db.Users.FirstOrDefaultAsync(n => n.Idusers == Properties.Settings.Default.IdUser);

            if (nsn != null)
            {
                nsn.UsersPassword = SettingProgram.Hash(PasswordBoxNewPass.Text);

                await db.SaveChangesAsync();
                (Application.Current.MainWindow as MainWindow)?.Notify("Уведомление", "Данные обновлены");
            }
            else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Произошла ошибка");
        }
        private async void ButtonUpdateMail_Click(object sender, RoutedEventArgs e)
        {
            using schedulerContext db = new();
            User? nsn = await db.Users.FirstOrDefaultAsync(n => n.Idusers == Properties.Settings.Default.IdUser);

            if (nsn != null)
            {
                nsn.UsersEmail = TextBoxMail.Text;

                await db.SaveChangesAsync();
                (Application.Current.MainWindow as MainWindow)?.Notify("Уведомление", "Данные обновлены");
            }
            else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Произошла ошибка");
        }
        private async void TextBoxUpdatePhone_Click(object sender, RoutedEventArgs e)
        {
            using schedulerContext db = new();
            User? nsn = await db.Users.FirstOrDefaultAsync(n => n.Idusers == Properties.Settings.Default.IdUser);

            if (nsn != null)
            {
                nsn.UsersPhone = TextBoxPhone.Text;

                await db.SaveChangesAsync();
                (Application.Current.MainWindow as MainWindow)?.Notify("Уведомление", "Данные обновлены");
            }
            else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Произошла ошибка");
        }
    }
}
