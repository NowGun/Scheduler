using Microsoft.EntityFrameworkCore;
using Scheduler.DataBaseClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Scheduler.Pages
{
    /// <summary>
    /// Логика взаимодействия для Groups.xaml
    /// </summary>
    public partial class Groups : Page
    {
        public Groups()
        {
            InitializeComponent();
        }

        private bool isOpenUser = false;
        private bool isOpenMenu = false;

        private async void CheckBoxUser_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as System.Windows.Controls.CheckBox;
            string[] name = cb.Content.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            using schedulerContext db = new();

            var gr = await db.Groups.Where(g => g.GroupsName == ((ListGroups)ListBoxGroups.SelectedItem).GroupName).FirstOrDefaultAsync();
            var u = await db.Users.Where(u => u.UsersName == name[1] && u.UsersSurname == name[0]).FirstOrDefaultAsync();

            if ((bool)cb.IsChecked)
            { 
                UserHasGroup hs = new()
                {
                    GroupsIdgroups = gr.Idgroups,
                    UsersIdusers = u.Idusers
                };

                if (hs != null)
                {
                    await db.UsersHasGroups.AddAsync(hs);
                    await db.SaveChangesAsync();
                }
            }
            else
            {
                UserHasGroup? hs = await db.UsersHasGroups.Where(h => h.UsersIdusers == u.Idusers && h.GroupsIdgroups == gr.Idgroups).FirstOrDefaultAsync();

                if (hs != null)
                {
                    db.UsersHasGroups.Remove(hs);
                    await db.SaveChangesAsync();
                }
            }
        }
        private void SearchBoxUsers_PreviewKeyUp(object sender, KeyEventArgs e) => FillUsers();
        private async void FillUsers()
        {
            ObservableCollection<ListUsers> co1 = new();

            using schedulerContext db = new();

            if (!String.IsNullOrWhiteSpace(SearchBoxUsers.Text))
            {
                var u = await db.Users
                    .OrderBy(u => u.UsersSurname)
                    .Where(u => u.Idusers != Properties.Settings.Default.IdUser 
                    && (EF.Functions.Like(u.UsersName, $"%{SearchBoxUsers.Text}%") 
                && EF.Functions.Like(u.UsersSurname, $"%{SearchBoxUsers.Text}%")))
                    .ToListAsync();

                foreach (var user in u)
                {
                    var hs = await db.UsersHasGroups.Where(h => h.UsersIdusers == user.Idusers && h.GroupsIdgroupsNavigation.GroupsName == ((ListGroups)ListBoxGroups.SelectedItem).GroupName).FirstOrDefaultAsync();

                    if (hs != null)
                    {
                        co1.Add(new ListUsers
                        {
                            nameUser = $"{user.UsersSurname} {user.UsersName}",
                            checkUser = "True"
                        });
                    }
                    else
                    {
                        co1.Add(new ListUsers
                        {
                            nameUser = $"{user.UsersSurname} {user.UsersName}",
                            checkUser = "False"
                        });
                    }
                }
            }
            else
            {
                var u = await db.Users.OrderBy(u => u.UsersSurname).Where(u => u.Idusers != Properties.Settings.Default.IdUser).ToListAsync();

                foreach (var user in u)
                {
                    var hs = await db.UsersHasGroups.Where(h => h.UsersIdusers == user.Idusers && h.GroupsIdgroupsNavigation.GroupsName == ((ListGroups)ListBoxGroups.SelectedItem).GroupName).FirstOrDefaultAsync();

                    if (hs != null)
                    {
                        co1.Add(new ListUsers
                        {
                            nameUser = $"{user.UsersSurname} {user.UsersName}",
                            checkUser = "True"
                        });
                    }
                    else
                    {
                        co1.Add(new ListUsers
                        {
                            nameUser = $"{user.UsersSurname} {user.UsersName}",
                            checkUser = "False"
                        });
                    }
                }
            }
            ListBoxUsers.ItemsSource = co1;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FillListGroup();
            ButtonOpenInfoGroup.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            GridCases.Visibility = Visibility.Collapsed;
            GridInfoCase.Visibility = Visibility.Collapsed;
            GridInfoGroup.Visibility = Visibility.Collapsed;
            GridGroupUser.Visibility = Visibility.Collapsed;
        }
        private void ButtonOpenInfoGroup_Click(object sender, RoutedEventArgs e)
        {
            switch (isOpenUser)
            {
                case false:
                    GridGroupUser.Visibility = Visibility.Visible;
                    GridInfoGroup.Visibility = Visibility.Collapsed;
                    FillUsers();
                    isOpenUser = true;
                    isOpenMenu = false;
                    break;
                case true:
                    GridGroupUser.Visibility = Visibility.Collapsed;
                    isOpenUser = false;
                    break;
            }
        }
        private void Page_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ListBoxGroups.SelectedIndex = -1;
            }
        }
        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            switch (isOpenMenu)
            {
                case false:
                    GridInfoGroup.Visibility = Visibility.Visible;
                    GridGroupUser.Visibility= Visibility.Collapsed;
                    FillUsers();
                    isOpenMenu = true;
                    isOpenUser = false;
                    break;
                case true:
                    GridInfoGroup.Visibility = Visibility.Collapsed;
                    isOpenMenu = false;
                    break;
            }
        }
        private async void ButtonAddGroup_Click(object sender, RoutedEventArgs e)
        {
            using schedulerContext db = new();

            var isGroup = await db.Groups.Where(g => g.GroupsName == TextBoxNameGroup.Text).ToListAsync();

            if (isGroup.Count == 0)
            {
                Group group = new()
                {
                    GroupsName = TextBoxNameGroup.Text,
                    UsersCreate = (uint)Properties.Settings.Default.IdUser
                };

                await db.Groups.AddAsync(group);
                await db.SaveChangesAsync();
                TextBoxNameGroup.Clear();
                SearchBoxGroups.Clear();
                FillListGroup();
            }
            else (Application.Current.MainWindow as MainWindow)?.Notify("Ошибка", "Такое название уже существует");
        }
        private async void FillListGroup()
        {
            ObservableCollection<ListGroups> co1 = new();

            using schedulerContext db = new();

            var hs = await db.UsersHasGroups.Include(h => h.GroupsIdgroupsNavigation).Where(h => h.UsersIdusers == Properties.Settings.Default.IdUser).ToListAsync();


            if (String.IsNullOrWhiteSpace(SearchBoxGroups.Text))
            {
                if (hs.Count != 0)
                {
                    foreach (var h in hs)
                    {
                        var g = await db.Groups
                        .OrderBy(g => g.GroupsName)
                        .Where(g => g.UsersCreate == Properties.Settings.Default.IdUser || g.Idgroups == h.GroupsIdgroups)
                        .ToListAsync();

                        foreach (var group in g)
                        {
                            var casesOpen = await db.Casegroups.Where(c => c.GroupsIdgroups == group.Idgroups).ToListAsync();

                            co1.Add(new ListGroups
                            {
                                GroupName = group.GroupsName,
                                GroupCases = casesOpen.Count.ToString()
                            });
                        }
                    }
                }
                else
                {
                    var g = await db.Groups
                        .OrderBy(g => g.GroupsName)
                        .Where(g => g.UsersCreate == Properties.Settings.Default.IdUser)
                        .ToListAsync();

                    foreach (var group in g)
                    {
                        var casesOpen = await db.Casegroups.Where(c => c.GroupsIdgroups == group.Idgroups).ToListAsync();

                        co1.Add(new ListGroups
                        {
                            GroupName = group.GroupsName,
                            GroupCases = casesOpen.Count.ToString()
                        });
                    }
                }
                
            }
            else
            {
                if (hs.Count != 0)
                {
                    foreach (var h in hs)
                    {
                        var g = await db.Groups
                     .OrderBy(g => g.GroupsName)
                     .Where(g => EF.Functions.Like(g.GroupsName, $"%{SearchBoxGroups.Text}%")
                     && g.UsersCreate == Properties.Settings.Default.IdUser || g.Idgroups == h.GroupsIdgroups)
                     .ToListAsync();

                        foreach (var group in g)
                        {
                            var casesOpen = await db.Casegroups.Where(c => c.GroupsIdgroups == group.Idgroups).ToListAsync();

                            co1.Add(new ListGroups
                            {
                                GroupName = group.GroupsName,
                                GroupCases = casesOpen.Count.ToString()
                            });
                        }
                    }
                }
                else
                {
                    var g = await db.Groups
                   .OrderBy(g => g.GroupsName)
                   .Where(g => EF.Functions.Like(g.GroupsName, $"%{SearchBoxGroups.Text}%")
                   && g.UsersCreate == Properties.Settings.Default.IdUser)
                   .ToListAsync();

                    foreach (var group in g)
                    {
                        var casesOpen = await db.Casegroups.Where(c => c.GroupsIdgroups == group.Idgroups).ToListAsync();

                        co1.Add(new ListGroups
                        {
                            GroupName = group.GroupsName,
                            GroupCases = casesOpen.Count.ToString()
                        });
                    }
                }
            }


            ListBoxGroups.ItemsSource = co1;
        }
        private void SearchBoxGroups_PreviewKeyUp(object sender, KeyEventArgs e) => FillListGroup();
        private void SearchBoxGroups_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) => ListBoxGroups.SelectedIndex = -1;
        private void ListBoxGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxGroups.SelectedIndex != -1)
            {
                ButtonOpenMenu.Visibility = Visibility.Visible;
                ButtonOpenInfoGroup.Visibility = Visibility.Visible;
                GridCases.Visibility = Visibility.Visible;
                GridInfoGroup.Visibility = Visibility.Collapsed;
                GridGroupUser.Visibility = Visibility.Collapsed;
                FillListCases();
                FillComboBoxCategory();
                LabelNameGroup.Content = ((ListGroups)ListBoxGroups.SelectedItem).GroupName;
            }
            else
            {
                ButtonOpenMenu.Visibility = Visibility.Collapsed;
                ButtonOpenInfoGroup.Visibility = Visibility.Collapsed;
                GridCases.Visibility = Visibility.Collapsed;
            }
        }
        private async void TextBoxNameCase_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                using schedulerContext db = new();
                
                Group? group = await db.Groups.Where(g => g.GroupsName == ((ListGroups)ListBoxGroups.SelectedItem).GroupName).FirstOrDefaultAsync();

                Casegroup casegroup = new()
                {
                    GroupsIdgroups = group.Idgroups,
                    Title = TextBoxNameCase.Text,
                    CategoryIdcategory = 1,
                    Done = 0,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                };

                await db.Casegroups.AddAsync(casegroup);
                await db.SaveChangesAsync();

                TextBoxNameCase.Clear();
                TextBoxNameCase.Focus();
                FillListCases();
            }
        }
        private async void FillListCases()
        {
            ObservableCollection<ListCases> co1 = new();

            using schedulerContext db = new();

            var d = await db.Casegroups.Where(d => d.Done == 1).ToListAsync();

            if (d.Count == 0)
            {
                ExpanderCasesDone.Visibility = Visibility.Collapsed;
            }
            else ExpanderCasesDone.Visibility = Visibility.Visible;

            var c = await db.Casegroups.Where(c => c.GroupsIdgroupsNavigation.GroupsName == ((ListGroups)ListBoxGroups.SelectedItem).GroupName && c.Done == 0).ToListAsync();

            if (c != null)
            {
                foreach (var cases in c)
                {
                    co1.Add(new ListCases
                    {
                        checkContent = cases.Title
                    });
                }
                ListBoxCases.ItemsSource = co1;
            }
        }
        private async void FillListCasesDone()
        {
            ObservableCollection<ListCasesDone> co1 = new();

            using schedulerContext db = new();

            var c = await db.Casegroups.Where(c => c.GroupsIdgroupsNavigation.GroupsName == ((ListGroups)ListBoxGroups.SelectedItem).GroupName && c.Done == 1).ToListAsync();

            if (c != null)
            {
                foreach (var cases in c)
                {
                    co1.Add(new ListCasesDone
                    {
                        checkContent = cases.Title,
                        isCheck = "True"
                    });
                }
                ListBoxDoneCases.ItemsSource = co1;
            }
        }
        private async void FillComboBoxCategory()
        {
            ComboBoxCategory.Items.Clear();

            using schedulerContext db = new();
            await db.Categories.ForEachAsync(g =>
            {
                ComboBoxCategory.Items.Add(g.CategoryName);
            });
        }
        private void ExpanderCasesDone_Expanded(object sender, RoutedEventArgs e) => FillListCasesDone();
        private async void CheckBoxCases_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;

            using schedulerContext db = new();

            if ((bool)cb.IsChecked)
            {
                Casegroup? cases = await db.Casegroups.Where(c => c.Title == cb.Content).FirstOrDefaultAsync();

                if (cases != null)
                {
                    cases.Done = 1;
                    await db.SaveChangesAsync();

                    FillListCasesDone();
                    FillListCases();
                }
            }
        }
        private async void CheckBoxCasesDone_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;

            using schedulerContext db = new();

            if (!(bool)cb.IsChecked)
            {
                Casegroup? cases = await db.Casegroups.Where(c => c.Title == cb.Content).FirstOrDefaultAsync();

                if (cases != null)
                {
                    cases.Done = 0;
                    await db.SaveChangesAsync();

                    FillListCasesDone();
                    FillListCases();
                }
            }
        }
        private async void ButtonDeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxGroups.SelectedIndex != -1)
            {
                using schedulerContext db = new();

                Group? gr = await db.Groups.FirstOrDefaultAsync(g => g.GroupsName == ((ListGroups)ListBoxGroups.SelectedItem).GroupName);

                if (gr != null)
                {
                    db.Remove(gr);
                    await db.SaveChangesAsync();

                    FillListGroup();
                }
            }
            ButtonOpenInfoGroup.Visibility = Visibility.Collapsed;
        }
        private async void ComboBoxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using schedulerContext db = new();

            Casegroup? cg = await db.Casegroups.Where(c => c.Title == ((ListCases)ListBoxCases.SelectedItem).checkContent).FirstOrDefaultAsync();

            if (cg != null)
            {
                cg.CategoryIdcategory = (uint)ComboBoxCategory.SelectedIndex + 1;
                await db.SaveChangesAsync();
            }
        }
        private async void ButtonDeleteCase_Click(object sender, RoutedEventArgs e)
        {
            using schedulerContext db = new();

            if (ListBoxCases.SelectedIndex != -1)
            {
                Casegroup? cg = await db.Casegroups.FirstOrDefaultAsync(c => c.Title == ((ListCases)ListBoxCases.SelectedItem).checkContent);

                if (cg != null)
                {
                    db.Casegroups.Remove(cg);
                    await db.SaveChangesAsync();
                }
            }
            else if (ListBoxDoneCases.SelectedIndex != -1)
            {
                Casegroup? cg = await db.Casegroups.FirstOrDefaultAsync(c => c.Title == ((ListCasesDone)ListBoxDoneCases.SelectedItem).checkContent);

                if (cg != null)
                {
                    db.Casegroups.Remove(cg);
                    await db.SaveChangesAsync();
                }
            }

            FillListCasesDone();
            FillListCases();
            GridInfoCase.Visibility = Visibility.Collapsed;
        }
        private async void TextBoxCaseDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            using schedulerContext db = new();

            Casegroup? cg = await db.Casegroups.Where(c => c.Title == ((ListCases)ListBoxCases.SelectedItem).checkContent).FirstOrDefaultAsync();

            if (cg != null)
            {
                cg.Description = TextBoxCaseDescription.Text;

                await db.SaveChangesAsync();
            }
        }
        private void ListBoxCases_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxCases.SelectedIndex != -1)
            {
                GridInfoCase.Visibility = Visibility.Visible;
                LoadInfoCase();
            }
        }
        private void ButtonCloseInfoGroup_Click(object sender, RoutedEventArgs e)
        {
            GridInfoCase.Visibility = Visibility.Collapsed;
        }
        private async void LoadInfoCase()
        {
            CheckBoxNameCase.Content = ((ListCases)ListBoxCases.SelectedItem).checkContent;

            using schedulerContext db = new();
            var cases = await db.Casegroups.Where(c => c.Title == ((ListCases)ListBoxCases.SelectedItem).checkContent).Include(c => c.GroupsIdgroupsNavigation.UsersCreateNavigation).FirstOrDefaultAsync();

            if (cases != null)
            {
                CheckBoxNameCase.IsChecked = cases.Done == 1 ? true : false;
                TextBoxCaseDescription.Text = cases.Description;
                ComboBoxCategory.SelectedIndex = (int)cases.CategoryIdcategory - 1;
                LabelCaseInfo.Content = $"Создано {cases.Date}, {cases.GroupsIdgroupsNavigation.UsersCreateNavigation.UsersSurname} {cases.GroupsIdgroupsNavigation.UsersCreateNavigation.UsersName.First()}.";
            }
        }
        private async void CheckBoxNameCase_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            using schedulerContext db = new();

            if ((bool)cb.IsChecked)
            {
                Casegroup? cases = await db.Casegroups.Where(c => c.Title == cb.Content).FirstOrDefaultAsync();

                if (cases != null)
                {
                    cases.Done = 1;
                    await db.SaveChangesAsync();
                }
                else
                {
                    cases.Done = 0;
                    await db.SaveChangesAsync();
                }

                FillListCasesDone();
                FillListCases();
            }
        }
        private async void TextBoxCaseDescription_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                using schedulerContext db = new();

                Casegroup? cg = await db.Casegroups.Where(c => c.Title == ((ListCases)ListBoxCases.SelectedItem).checkContent).FirstOrDefaultAsync();

                if (cg != null)
                {
                    cg.Description = TextBoxCaseDescription.Text;

                    await db.SaveChangesAsync();
                }
            }
        }
    }

    public class ListUsers
    {
        public string? checkUser { get; set; }
        public string? nameUser { get; set; }
    }
    public class ListCases
    {
        public string? checkContent { get; set; }
    }
    public class ListCasesDone
    {
        public string? isCheck { get; set; }
        public string? checkContent { get; set; }
    }
    public class ListGroups
    {
        public string? GroupName { get; set; }
        public string? GroupCases { get; set; }
    }
}
