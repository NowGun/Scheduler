using Microsoft.EntityFrameworkCore;
using Scheduler.DataBaseClass;
using Scheduler.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
using WPFUI.Common;
using WPFUI.Controls.Interfaces;

namespace Scheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeUi();
        }

        public bool isEntry = false;
        private int? userId;
        private int? secretCode;
        private void InitializeUi()
        {
            Loaded += (sender, args) =>
            {
                Classes.SettingProgram s = new();
                s.ChangeTheme();
            };
        }
        private void RootNavigation_OnNavigated(INavigation sender, RoutedNavigationEventArgs e)
        {
            RootFrame.Margin = new Thickness(
                left: 0,
                top: e.CurrentPage.Tag as string == "Entry" ? -69 : 0,
                right: 0,
                bottom: 0);
        }
        public void Notify(string title, string message)
        {
            RootSnackbar.Title = title;
            RootSnackbar.Message = message;
            RootSnackbar.Show();
        }
        private void RootDialog_ButtonRightClick(object sender, RoutedEventArgs e)
        {
            RootDialog.Hide();
        }
        private async void RootDialog_ButtonLeftClick(object sender, RoutedEventArgs e)
        {
            using schedulerContext db = new();

            switch (RootDialog.ButtonLeftName) {
                case "Отправить код":

                    if (!String.IsNullOrWhiteSpace(TextBoxLogin.Text.Trim()) && !String.IsNullOrWhiteSpace(TextBoxMail.Text.Trim()))
                    {
                        var user = await db.Users.FirstOrDefaultAsync(u => u.UsersLogin == TextBoxLogin.Text.Trim() && u.UsersEmail == TextBoxMail.Text.Trim());

                        if (user != null)
                        {
                            userId = (int)user.Idusers;
                            secretCode = await SendMail(TextBoxMail.Text.Trim());
                        }
                        else Notify("Ошибка", "Аккаунт не найден");
                    }
                    else Notify("Ошибка", "Заполните все поля");
                    break;
                case "Проверить код":
                    if (TextBoxCode.Text == Convert.ToString(secretCode))
                    {
                        RootDialog.ButtonLeftName = "Сменить пароль";
                        GridCode.Visibility = Visibility.Collapsed;
                        GridNewPass.Visibility = Visibility.Visible;
                    }
                    else Notify("Ошибка", "Код неверный");
                    break;
                case "Сменить пароль":
                    Classes.SettingProgram s = new();

                    if (!String.IsNullOrWhiteSpace(PasswordBoxPass.Text.Trim()) && !String.IsNullOrWhiteSpace(PasswordBoxRPass.Text.Trim()))
                    {
                        if (PasswordBoxPass.Text == PasswordBoxRPass.Text)
                        {
                            var user = await db.Users.FirstOrDefaultAsync(u => u.Idusers == userId);

                            if (user != null)
                            {
                                user.UsersPassword = s.Hash(PasswordBoxPass.Text.Trim());
                                await db.SaveChangesAsync();

                                Notify("Сообщение", "Пароль успешно изменен");
                                RootDialog.Hide();
                            }
                        }
                        else Notify("Ошибка", "Пароли не совпадают");
                    }
                    else Notify("Ошибка", "Введите пароль");
                    break;
            }
        }
        private async Task<int> SendMail(string mail)
        {
            Random random = new();
            int secretCode = random.Next(100000, 999999);

            MailAddress from = new("mail@techno-review.ru", "Восстановление пароля");
            MailAddress to = new(mail);
            MailMessage m = new(from, to);
            m.Subject = Title;
            m.Body = $"Код для восстановления пароля в планировщике:\n {secretCode} ";
            SmtpClient smtp = new("connect.smtp.bz", 25);
            smtp.Credentials = new NetworkCredential("zhirowdaniil@gmail.com", "CB1W3lAeBwQ6");
            smtp.EnableSsl = true;

            try
            {
                await smtp.SendMailAsync(m);

                RootDialog.ButtonLeftName = "Проверить код";
                GridChangePasswordLogin.Visibility = Visibility.Collapsed;
                GridCode.Visibility = Visibility.Visible;
            }
            catch (SmtpException)
            {
                Notify("Ошибка", "Почтовый сервис недоступен");
            }

            return secretCode;
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            secretCode = await SendMail(TextBoxMail.Text);
        }
    }
}
