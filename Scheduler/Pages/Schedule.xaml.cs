using Scheduler.Classes;
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

        }
    }

    public class CaseWork
    {
        public string? titleCase { get; set; }
        public string? isBookMark { get; set; }
    }
}
