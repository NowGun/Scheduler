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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
        }

        private async void ButtonReg_Click(object sender, RoutedEventArgs e)
        {
            Classes.SettingProgram s = new();
            (Application.Current.MainWindow as MainWindow).ProgressRingLoad.Visibility = Visibility.Visible;

            if (!String.IsNullOrWhiteSpace(TextBoxLogin.Text))
            {
                if (PasswordBoxPass.Text == PasswordBoxRPass.Text && !String.IsNullOrWhiteSpace(PasswordBoxPass.Text))
                {
                    if (!String.IsNullOrWhiteSpace(TextBoxMail.Text))
                    {
                        schedulerContext db = new();

                        var userExists = await db.Users.Where(u => u.UsersLogin == TextBoxLogin.Text.Trim()).ToListAsync();

                        if (userExists.Count == 0)
                        {
                            User u = new()
                            {
                                UsersLogin = TextBoxLogin.Text.Trim(),
                                UsersPassword = s.Hash(PasswordBoxPass.Text.Trim()),
                                UsersEmail = TextBoxMail.Text.Trim(),
                                UsersPhone = TextBoxPhone.Text.Trim(),
                            };

                            await db.Users.AddAsync(u);
                            await db.SaveChangesAsync();

                            (Application.Current.MainWindow as MainWindow)?.Notify("Уведомление", "Регистрация успешна");
                            (Application.Current.MainWindow as MainWindow)?.RootNavigation.Navigate("вход");
                        }
                        else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Логин занят");
                    }
                    else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Введите почту");
                }
                else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Пароли не совпадают");
            }
            else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Введите логин");

            (Application.Current.MainWindow as MainWindow).ProgressRingLoad.Visibility = Visibility.Hidden;
        }
    }
}
