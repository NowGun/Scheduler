using Microsoft.EntityFrameworkCore;
using Scheduler.Classes;
using Scheduler.DataBaseClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Scheduler.Pages
{
    /// <summary>
    /// Логика взаимодействия для Schedule.xaml
    /// </summary>
    public partial class Schedule : Page
    {
        public Schedule()
        {
            InitializeComponent();
            LoadSchedule();
        }

        private bool isOpen = false;
        private List<int> idCaseWork = new();
        private void LoadSchedule()
        {
            DatePickerDate.SelectedDate = DateTime.Now.Date;
        }
        private void ButtonDateNext_Click(object sender, RoutedEventArgs e)
        {
            DatePickerDate.SelectedDate = DatePickerDate.SelectedDate.Value.AddDays(1);
        }
        private void ButtonDatePrev_Click(object sender, RoutedEventArgs e)
        {
            DatePickerDate.SelectedDate = DatePickerDate.SelectedDate.Value.AddDays(-1);
        }

        private void ButtonOpenInfo_Click(object sender, RoutedEventArgs e)
        {
            switch (isOpen)
            {
                case false:
                    var anim = (Storyboard)FindResource("AnimOpenInfo");
                    anim.Begin();
                    isOpen = true;
                    break;
                case true:
                    var anim2 = (Storyboard)FindResource("AnimCloseInfo");
                    anim2.Begin();
                    isOpen = false;
                    break;
            }
        }
        private async void LoadListBoxCaseWork()
        {
            try
            {
                idCaseWork.Clear();
                ObservableCollection<CaseWork> co1 = new();

                using schedulerContext db = new();

                var c2 = await db.Cases.Where(c => c.UsersIdusers == Properties.Settings.Default.IdUser && c.CaseDone == 0).ToListAsync();

                foreach (var c in c2)
                {
                    if (c.CaseBookmark == 0)
                    {
                        co1.Add(new CaseWork
                        {
                            titleCase = c.CaseTitle,
                        });
                    }
                    else
                    {
                        co1.Add(new CaseWork
                        {
                            titleCase = c.CaseTitle,
                        });
                    }
                    idCaseWork.Add((int)c.Idcase);
                }

                ListBoxCaseWork.ItemsSource = co1;
            }
            catch (Exception ex)
            {
                (Application.Current.MainWindow as MainWindow)?.MessageBoxNotify("Ошибка", ex.Message);
            }
        }
        private void ExpanderWork_Expanded(object sender, RoutedEventArgs e)
        {
            LoadListBoxCaseWork();
        }

        private async void ButtonAddCase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(TextBoxCase.Text)) 
                {
                    using schedulerContext db = new();

                    Case c = new()
                    {
                        UsersIdusers = (uint)Properties.Settings.Default.IdUser,
                        CaseTitle = TextBoxCase.Text,
                        CaseBookmark = 0,
                        CaseDone = 0,
                        CaseDate = DateOnly.FromDateTime(DateTime.Now),
                    };

                    await db.Cases.AddAsync(c);
                    await db.SaveChangesAsync();

                    TextBoxCase.Clear();
                    TextBoxCase.Focus();

                    LoadListBoxCaseWork();
                }
            }
            catch (Exception ex)
            {
                (Application.Current.MainWindow as MainWindow)?.MessageBoxNotify("Ошибка", ex.Message);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadListBoxCaseWork();
        }

        private async void ListBoxCaseWork_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxCaseWork.SelectedItem != null)
            {
                var anim = (Storyboard)FindResource("AnimOpenInfo");
                anim.Begin();
                isOpen = true;

                using schedulerContext db = new();

                var caseCheck = await db.Cases.Where(c => c.Idcase == idCaseWork[ListBoxCaseWork.SelectedIndex]).FirstOrDefaultAsync();

                if (caseCheck != null)
                {
                    TextBoxCaseTitle.Text = caseCheck.CaseTitle;
                    TextBoxCaseDescription.Text = caseCheck.CaseDescription;
                    DatePickerCaseDate.Text = caseCheck.CaseDate.ToString();
                    TextBoxCaseTimeStart.Text = caseCheck.CaseTimestart.ToString();
                    TextBoxCaseTimeEnd.Text = caseCheck.CaseTimeend.ToString();

                    switch (caseCheck.CaseBookmark)
                    {
                        case 0:
                            ButtonBookMark.Appearance = WPFUI.Common.Appearance.Transparent;
                            break;
                        case 1:
                            ButtonBookMark.Appearance = WPFUI.Common.Appearance.Caution;
                            break;
                    }
                }
            }
        }

        private async void ButtonBookMark_Click(object sender, RoutedEventArgs e)
        {
            using schedulerContext db = new();

            Case? caseInfo = await db.Cases.FirstOrDefaultAsync(c => c.Idcase == idCaseWork[ListBoxCaseWork.SelectedIndex]);

            if (caseInfo != null)
            {
                if (ButtonBookMark.Appearance == WPFUI.Common.Appearance.Caution)
                {
                    ButtonBookMark.Appearance = WPFUI.Common.Appearance.Transparent;
                    caseInfo.CaseBookmark = 0;
                }
                else
                {
                    ButtonBookMark.Appearance = WPFUI.Common.Appearance.Caution;
                    caseInfo.CaseBookmark = 1;
                }

                await db.SaveChangesAsync();
            }
        }

        private async void ButtonCaseSave_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(TextBoxCaseTitle.Text))
            {
                if (ListBoxCaseWork.SelectedItem != null)
                {
                    using schedulerContext db = new();

                    Case? c = await db.Cases.FirstOrDefaultAsync(c => c.Idcase == idCaseWork[ListBoxCaseWork.SelectedIndex]);

                    if (c != null)
                    {
                        c.CaseTitle = TextBoxCaseTitle.Text;
                        c.CaseDescription = TextBoxCaseDescription.Text;
                        c.CaseDate = DateOnly.FromDateTime(DatePickerCaseDate.DisplayDate);

                        if (!String.IsNullOrWhiteSpace(TextBoxCaseTimeStart.Text))
                        {
                            c.CaseTimestart = TimeOnly.FromDateTime(DateTime.Parse(TextBoxCaseTimeStart.Text));
                        }

                        if (!String.IsNullOrWhiteSpace(TextBoxCaseTimeEnd.Text))
                        {
                            c.CaseTimeend = TimeOnly.FromDateTime(DateTime.Parse(TextBoxCaseTimeEnd.Text));
                        }
                        await db.SaveChangesAsync();

                        var anim = (Storyboard)FindResource("AnimCloseInfo");
                        anim.Begin();
                        isOpen = false;

                        LoadListBoxCaseWork();
                    }
                }
            }
            else (Application.Current.MainWindow as MainWindow).Notify("Уведомление", "Заполните заголовок");
        }

        private async void ButtonCaseDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxCaseWork.Items.Count == 0) (Application.Current.MainWindow as MainWindow).Notify("Сообщение", "Произошла ошибка");
            else if (ListBoxCaseWork.SelectedItem != null)
            {
                using schedulerContext db = new();

                Case? c = await db.Cases.FirstOrDefaultAsync(c => c.Idcase == idCaseWork[ListBoxCaseWork.SelectedIndex]);

                if (c != null)
                {
                    db.Cases.Remove(c);
                    await db.SaveChangesAsync();

                    LoadListBoxCaseWork();

                    var anim2 = (Storyboard)FindResource("AnimCloseInfo");
                    anim2.Begin();
                    isOpen = false;
                }
            }
        }
    }

    public class CaseWork
    {
        public string? titleCase { get; set; }
    }
}
