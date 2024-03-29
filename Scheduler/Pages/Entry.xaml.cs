﻿using Microsoft.EntityFrameworkCore;
using Scheduler.Classes;
using Scheduler.DataBaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для Entry.xaml
    /// </summary>
    public partial class Entry : Page
    {
        public Entry()
        {
            InitializeComponent();
            LoadData();
        }

        private string cond = @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)";

        private void LoadData()
        {
            CheckBoxSaveEntry.IsChecked = Properties.Settings.Default.SaveEntry;
            TextBoxLogin.Text = Properties.Settings.Default.Login;
            PasswordBoxPass.Text = Properties.Settings.Default.Password;
        }
        private void ButtonReg_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow)?.RootNavigation.Navigate("регистрация");
        }
        private async void ButtonEntry_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).ProgressRingLoad.Visibility = Visibility.Visible;
            ButtonEntry.IsEnabled = false;
            ButtonReg.IsEnabled = false;

            if (!String.IsNullOrWhiteSpace(TextBoxLogin.Text) && !String.IsNullOrWhiteSpace(PasswordBoxPass.Text))
            {
                if (Regex.IsMatch(TextBoxLogin.Text, cond))
                {
                    using schedulerContext db = new();

                    if (await db.Database.CanConnectAsync())
                    {
                        User? user = await db.Users.FirstOrDefaultAsync(u => u.UsersEmail == TextBoxLogin.Text.Trim() && u.UsersPassword == SettingProgram.Hash(PasswordBoxPass.Text.Trim()));

                        if (user != null)
                        {
                            if (CheckBoxSaveEntry.IsChecked == true)
                            {
                                Properties.Settings.Default.Login = TextBoxLogin.Text.Trim();
                                Properties.Settings.Default.Password = PasswordBoxPass.Text.Trim();
                            }
                            else
                            {
                                Properties.Settings.Default.Login = "";
                                Properties.Settings.Default.Password = "";
                            }

                            Properties.Settings.Default.IdUser = (int)user.Idusers;

                            Properties.Settings.Default.Save();

                            (Application.Current.MainWindow as MainWindow).isEntry = true;
                            (Application.Current.MainWindow as MainWindow).NavigationItemEntry.Visibility = Visibility.Collapsed;
                            (Application.Current.MainWindow as MainWindow).NavigationItemShedule.Visibility = Visibility.Visible;
                            (Application.Current.MainWindow as MainWindow).NavigationItemBookMark.Visibility = Visibility.Visible;
                            (Application.Current.MainWindow as MainWindow).NavigationItemLC.Visibility = Visibility.Visible;
                            (Application.Current.MainWindow as MainWindow).NavigationItemGroups.Visibility = Visibility.Visible;
                            (Application.Current.MainWindow as MainWindow)?.RootNavigation.Navigate("планы");
                        }
                        else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Логин или пароль не совпадают");
                    }
                    else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "База данных недоступна");
                }
                else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Почта введена неверно");
            }
            else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Заполните поля");

            (Application.Current.MainWindow as MainWindow).ProgressRingLoad.Visibility = Visibility.Hidden;
            ButtonEntry.IsEnabled = true;
            ButtonReg.IsEnabled = true;
        }
        private void CheckBoxSaveEntry_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.SaveEntry = CheckBoxSaveEntry.IsChecked ?? false;

            if (CheckBoxSaveEntry.IsChecked == true)
            {
                Properties.Settings.Default.Login = TextBoxLogin.Text.Trim();
                Properties.Settings.Default.Password = PasswordBoxPass.Text.Trim();
            }
            else
            {
                Properties.Settings.Default.Login = "";
                Properties.Settings.Default.Password = "";
            }

            Properties.Settings.Default.Save();
        }
        private void ButtonResetPass_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).RootDialog.Show();
        }
    }
}
