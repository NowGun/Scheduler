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
            
        }

        private bool isOpen = false;
        private List<int> idCaseWork = new();
        private List<int> idCaseDone = new();

        private void LoadSchedule()
        {
            DatePickerDate.SelectedDate = DateTime.Now.Date;
        }
        private void ButtonDateNext_Click(object sender, RoutedEventArgs e)
        {
            DatePickerDate.SelectedDate = DatePickerDate.SelectedDate.Value.AddDays(1);
            LoadListBoxCaseWork();
            LoadListBoxCaseDone();
        }
        private void ButtonDatePrev_Click(object sender, RoutedEventArgs e)
        {
            DatePickerDate.SelectedDate = DatePickerDate.SelectedDate.Value.AddDays(-1);
            LoadListBoxCaseWork();
            LoadListBoxCaseDone();
        }
        private void ButtonOpenInfo_Click(object sender, RoutedEventArgs e)
        {
            switch (isOpen)
            {
                case false:
                    var anim = (Storyboard)FindResource("AnimOpenInfo");
                    anim.Begin();
                    TextBoxCaseDescription.Clear();
                    TextBoxCaseTitle.Clear();
                    TextBoxCaseTimeEnd.Clear();
                    TextBoxCaseTimeStart.Clear();
                    ButtonDone.Appearance = WPFUI.Common.Appearance.Secondary;
                    ButtonBookMark.Appearance = WPFUI.Common.Appearance.Transparent;
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

                var c2 = await db.Cases.Where(c => c.UsersIdusers == Properties.Settings.Default.IdUser && c.CaseDone == 0 && c.CaseDate == DateOnly.FromDateTime((DateTime)DatePickerDate.SelectedDate)).ToListAsync();

                foreach (var c in c2)
                {
                    co1.Add(new CaseWork
                    {
                        titleCase = c.CaseTitle,
                    });

                    idCaseWork.Add((int)c.Idcase);
                }

                ListBoxCaseWork.ItemsSource = co1;
            }
            catch (Exception ex)
            {
                (Application.Current.MainWindow as MainWindow)?.MessageBoxNotify("Ошибка", ex.Message);
            }
        }
        private async void LoadListBoxCaseDone()
        {
            try
            {
                idCaseDone.Clear();
                ObservableCollection<CaseDone> co2 = new();

                using schedulerContext db = new();

                var c2 = await db.Cases.Where(c => c.UsersIdusers == Properties.Settings.Default.IdUser && c.CaseDone == 1 && c.CaseDate == DateOnly.FromDateTime((DateTime)DatePickerDate.SelectedDate)).ToListAsync();

                foreach (var c in c2)
                {
                    co2.Add(new CaseDone
                    {
                        titleCaseDone = c.CaseTitle,
                    });

                    idCaseDone.Add((int)c.Idcase);
                }
                ListBoxCaseDone.ItemsSource = co2;
            }
            catch (Exception ex)
            {
                (Application.Current.MainWindow as MainWindow)?.MessageBoxNotify("Ошибка", ex.Message);
            }
        }
        private void ExpanderWork_Expanded(object sender, RoutedEventArgs e)
        {
            //LoadListBoxCaseWork();
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
                        CaseDate = DateOnly.FromDateTime((DateTime)DatePickerDate.SelectedDate),
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
            LoadSchedule();
            LoadListBoxCaseWork();
            LoadListBoxCaseDone();
        }
        private async void ListBoxCaseWork_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxCaseWork.SelectedItem != null)
            {
                ListBoxCaseDone.SelectedIndex = -1;

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

                    switch (caseCheck.CaseDone)
                    {
                        case 0:
                            ButtonDone.Appearance = WPFUI.Common.Appearance.Transparent;
                            break;
                        case 1:
                            ButtonDone.Appearance = WPFUI.Common.Appearance.Success;
                            break;
                    }
                }
            }
        }
        private async void ButtonBookMark_Click(object sender, RoutedEventArgs e)
        {
            using schedulerContext db = new();

            if (ListBoxCaseDone.SelectedIndex == -1)
            {
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
            else
            {
                Case? caseInfo = await db.Cases.FirstOrDefaultAsync(c => c.Idcase == idCaseDone[ListBoxCaseDone.SelectedIndex]);

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
            
        }
        private async void ButtonCaseSave_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(TextBoxCaseTitle.Text))
            {
                if (ListBoxCaseWork.SelectedItem != null || ListBoxCaseDone.SelectedItem != null)
                {
                    using schedulerContext db = new();

                    if (ListBoxCaseDone.SelectedIndex == -1)
                    {
                        Case? c = await db.Cases.FirstOrDefaultAsync(c => c.Idcase == idCaseWork[ListBoxCaseWork.SelectedIndex]);

                        if (c != null)
                        {
                            c.CaseTitle = TextBoxCaseTitle.Text;
                            c.CaseDescription = TextBoxCaseDescription.Text;
                            c.CaseDate = DateOnly.FromDateTime((DateTime)DatePickerCaseDate.SelectedDate);
                            try
                            {
                                if (!String.IsNullOrWhiteSpace(TextBoxCaseTimeStart.Text))
                                {
                                    c.CaseTimestart = TimeOnly.FromDateTime(DateTime.Parse(TextBoxCaseTimeStart.Text));
                                }

                                if (!String.IsNullOrWhiteSpace(TextBoxCaseTimeEnd.Text))
                                {
                                    c.CaseTimeend = TimeOnly.FromDateTime(DateTime.Parse(TextBoxCaseTimeEnd.Text));
                                }
                            }
                            catch (Exception ex)
                            {
                                (Application.Current.MainWindow as MainWindow).Notify("Ошибка", "Время введено неверно");
                            }
                            
                            await db.SaveChangesAsync();

                            LoadListBoxCaseWork();
                            LoadListBoxCaseDone();
                        }
                    }
                    else
                    {
                        Case? c = await db.Cases.FirstOrDefaultAsync(c => c.Idcase == idCaseDone[ListBoxCaseDone.SelectedIndex]);

                        if (c != null)
                        {
                            c.CaseTitle = TextBoxCaseTitle.Text;
                            c.CaseDescription = TextBoxCaseDescription.Text;
                            c.CaseDate = DateOnly.FromDateTime((DateTime)DatePickerCaseDate.SelectedDate);

                            if (!String.IsNullOrWhiteSpace(TextBoxCaseTimeStart.Text))
                            {
                                c.CaseTimestart = TimeOnly.FromDateTime(DateTime.Parse(TextBoxCaseTimeStart.Text));
                            }

                            if (!String.IsNullOrWhiteSpace(TextBoxCaseTimeEnd.Text))
                            {
                                c.CaseTimeend = TimeOnly.FromDateTime(DateTime.Parse(TextBoxCaseTimeEnd.Text));
                            }
                            await db.SaveChangesAsync();

                            LoadListBoxCaseWork();
                            LoadListBoxCaseDone();
                        }
                    }

                    var anim = (Storyboard)FindResource("AnimCloseInfo");
                    anim.Begin();
                    isOpen = false;
                }
            }
            else (Application.Current.MainWindow as MainWindow).Notify("Уведомление", "Заполните заголовок");
        }
        private async void ButtonCaseDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxCaseWork.Items.Count == 0) (Application.Current.MainWindow as MainWindow).Notify("Сообщение", "Произошла ошибка");
            else if (ListBoxCaseWork.SelectedItem != null ||  ListBoxCaseDone.SelectedItem != null)
            {
                using schedulerContext db = new();

                if (ListBoxCaseDone.SelectedIndex == -1)
                {
                    Case? c = await db.Cases.FirstOrDefaultAsync(c => c.Idcase == idCaseWork[ListBoxCaseWork.SelectedIndex]);

                    if (c != null)
                    {
                        db.Cases.Remove(c);
                        await db.SaveChangesAsync();  
                    }
                }
                else
                {
                    Case? c = await db.Cases.FirstOrDefaultAsync(c => c.Idcase == idCaseDone[ListBoxCaseDone.SelectedIndex]);

                    if (c != null)
                    {
                        db.Cases.Remove(c);
                        await db.SaveChangesAsync();
                    }
                }

                LoadListBoxCaseWork();
                LoadListBoxCaseDone();

                var anim2 = (Storyboard)FindResource("AnimCloseInfo");
                anim2.Begin();
                isOpen = false;
            }
        }
        private void DatePickerDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            LoadListBoxCaseWork();
            LoadListBoxCaseDone();
        }
        private async void ButtonDone_Click(object sender, RoutedEventArgs e)
        {
            using schedulerContext db = new();

            if (ListBoxCaseDone.SelectedIndex == -1)
            {
                Case? caseInfo = await db.Cases.FirstOrDefaultAsync(c => c.Idcase == idCaseWork[ListBoxCaseWork.SelectedIndex]);
                if (caseInfo != null)
                {
                    if (caseInfo.CaseDone == 0)
                    {
                        ButtonDone.Appearance = WPFUI.Common.Appearance.Success;
                        caseInfo.CaseDone = 1;
                    }
                    else
                    {
                        ButtonDone.Appearance = WPFUI.Common.Appearance.Secondary;
                        caseInfo.CaseDone = 0;
                    }

                    await db.SaveChangesAsync();

                    LoadListBoxCaseWork();
                    LoadListBoxCaseDone();
                }
            }
            else
            {
                Case? caseInfo = await db.Cases.FirstOrDefaultAsync(c => c.Idcase == idCaseDone[ListBoxCaseDone.SelectedIndex]);
                if (caseInfo != null)
                {
                    if (caseInfo.CaseDone == 0)
                    {
                        ButtonDone.Appearance = WPFUI.Common.Appearance.Success;
                        caseInfo.CaseDone = 1;
                    }
                    else
                    {
                        ButtonDone.Appearance = WPFUI.Common.Appearance.Secondary;
                        caseInfo.CaseDone = 0;
                    }

                    await db.SaveChangesAsync();

                    LoadListBoxCaseWork();
                    LoadListBoxCaseDone();
                }
            }

            var anim = (Storyboard)FindResource("AnimCloseInfo");
            anim.Begin();
        }
        private async void ListBoxCaseDone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxCaseDone.SelectedItem != null)
            {
                ListBoxCaseWork.SelectedIndex = -1;

                var anim = (Storyboard)FindResource("AnimOpenInfo");
                anim.Begin();
                isOpen = true;

                using schedulerContext db = new();

                var caseCheck = await db.Cases.Where(c => c.Idcase == idCaseDone[ListBoxCaseDone.SelectedIndex]).FirstOrDefaultAsync();

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
                    switch (caseCheck.CaseDone)
                    {
                        case 0:
                            ButtonDone.Appearance = WPFUI.Common.Appearance.Transparent;
                            break;
                        case 1:
                            ButtonDone.Appearance = WPFUI.Common.Appearance.Success;
                            break;
                    }

                }
            }
        }
    }

    public class CaseWork
    {
        public string? titleCase { get; set; }
    }

    public class CaseDone
    {
        public string? titleCaseDone { get; set; }
    }
}
