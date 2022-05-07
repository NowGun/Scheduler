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
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
        }
        
        private void LoadSetting()
        {
            ComboBoxTheme.SelectedIndex = Properties.Settings.Default.Theme;

            if (!(Application.Current.MainWindow as MainWindow).isEntry) ExpanderUser.Visibility = Visibility.Collapsed;
            else if ((Application.Current.MainWindow as MainWindow).isEntry)
            {
                ExpanderUser.Visibility = Visibility.Visible;
                LoadProfile();
            }
        }
        private async void LoadProfile()
        {
            using schedulerContext db = new();

            User? u = await db.Users.FirstOrDefaultAsync(u => u.Idusers == Properties.Settings.Default.IdUser);

            if (u != null)
            {
                LabelLogin.Content = $"Добро пожаловать, {u.UsersLogin}";
            }
        }
        private void ComboBoxTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.Theme = ComboBoxTheme.SelectedIndex;
            Properties.Settings.Default.Save();

            Classes.SettingProgram s = new();
            s.ChangeTheme();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSetting();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).isEntry = false;
            (Application.Current.MainWindow as MainWindow).NavigationItemEntry.Visibility = Visibility.Visible;
            (Application.Current.MainWindow as MainWindow).NavigationItemShedule.Visibility = Visibility.Collapsed;
            (Application.Current.MainWindow as MainWindow)?.RootNavigation.Navigate("Entry");
        }

        private void ButtonChangePassword_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).isEntry = false;
            (Application.Current.MainWindow as MainWindow).NavigationItemEntry.Visibility = Visibility.Visible;
            (Application.Current.MainWindow as MainWindow).NavigationItemShedule.Visibility = Visibility.Collapsed;
            (Application.Current.MainWindow as MainWindow)?.RootNavigation.Navigate("Entry");
            (Application.Current.MainWindow as MainWindow).RootDialog.Show();
        }
    }
}
