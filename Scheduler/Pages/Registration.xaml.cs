using Microsoft.EntityFrameworkCore;
using Scheduler.DataBaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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

        private int? secretCode;
        Classes.SettingProgram s = new();

        private async void ButtonReg_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).ProgressRingLoad.Visibility = Visibility.Visible;
            ButtonReg.IsEnabled = false;

            if (!String.IsNullOrWhiteSpace(TextBoxName.Text) && !String.IsNullOrWhiteSpace(TextBoxSurname.Text))
            {
                if (PasswordBoxPass.Text == PasswordBoxRPass.Text && !String.IsNullOrWhiteSpace(PasswordBoxPass.Text))
                {
                    if (!String.IsNullOrWhiteSpace(TextBoxMail.Text))
                    {
                        if (Regex.IsMatch(TextBoxMail.Text, @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)")) 
                        {
                            using schedulerContext db = new();

                            var userExists = await db.Users.Where(u => u.UsersEmail == TextBoxMail.Text.Trim()).ToListAsync();

                            if (userExists.Count == 0)
                            {
                                secretCode = await SendMail(TextBoxMail.Text.Trim());
                                RootDialogApplyMail.Show();
                            }
                            else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Почта занята");
                        }
                        else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Почта введена неверно");
                    }
                    else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Введите почту");
                }
                else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Пароли не совпадают");
            }
            else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Введите Имя и Фамилию");

            (Application.Current.MainWindow as MainWindow).ProgressRingLoad.Visibility = Visibility.Hidden;
            ButtonReg.IsEnabled = true;
        }
        private void RootDialogApplyMail_ButtonRightClick(object sender, RoutedEventArgs e)
        {
            RootDialogApplyMail.Hide();
        }
        private async void RootDialogApplyMail_ButtonLeftClick(object sender, RoutedEventArgs e)
        {
            if (TextBoxCode1.Text == Convert.ToString(secretCode))
            {
                using schedulerContext db = new();

                User u = new()
                {
                    UsersName = TextBoxName.Text,
                    UsersSurname = TextBoxSurname.Text,
                    UsersPassword = s.Hash(PasswordBoxPass.Text.Trim()),
                    UsersEmail = TextBoxMail.Text.Trim(),
                    UsersPhone = TextBoxPhone.Text.Trim(),
                };

                await db.Users.AddAsync(u);
                await db.SaveChangesAsync();

                (Application.Current.MainWindow as MainWindow)?.Notify("Уведомление", "Регистрация успешна");
                (Application.Current.MainWindow as MainWindow)?.RootNavigation.Navigate("Entry");
                RootDialogApplyMail.Hide();
            }
            else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Введен неверный код");
        }
        private async Task<int> SendMail(string mail)
        {
            Random random = new();
            int secretCode = random.Next(100000, 999999);

            MailAddress from = new("mail@techno-review.ru", "Подтверждение почты");
            MailAddress to = new(mail);
            MailMessage m = new(from, to);
            m.Subject = Title;
            m.Body = $"Код для подтверждения почты:\n {secretCode}  ";
            SmtpClient smtp = new("connect.smtp.bz", 25);
            smtp.Credentials = new NetworkCredential("zhirowdaniil@gmail.com", "CB1W3lAeBwQ6");
            smtp.EnableSsl = true;

            try
            {
                await smtp.SendMailAsync(m);
            }
            catch (SmtpException)
            {
                (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Почтовый сервис недоступен");
            }

            return secretCode;
        }
        private async void Button_Click(object sender, RoutedEventArgs e) => secretCode = await SendMail(TextBoxMail.Text);
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxName.Clear();
            TextBoxSurname.Clear();
            TextBoxPhone.Clear();
            TextBoxMail.Clear();
            PasswordBoxPass.Clear();
            PasswordBoxRPass.Clear();
        }
    }
}
