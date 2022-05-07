using ReactiveUI;
using Scheduler.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
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
    /// Логика взаимодействия для Schedule.xaml
    /// </summary>
    public partial class Schedule : Page
    {
        public static readonly DependencyProperty StartDateProperty =
             DependencyProperty.Register(nameof(StartDate), typeof(DateTime), typeof(Schedule),
                new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty SelectedDateProperty =
             DependencyProperty.Register(nameof(SelectedDate), typeof(DateTime), typeof(Schedule),
                new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty EndDateProperty =
             DependencyProperty.Register(nameof(EndDate), typeof(DateTime), typeof(Schedule),
                new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty UserTasksProperty =
             DependencyProperty.Register(nameof(UserTasks), typeof(ObservableCollection<UserTask>), typeof(Schedule),
                new PropertyMetadata(default));

        public Schedule()
        {
            InitializeComponent();

            PreviousDay = ReactiveCommand.Create(PreviousDayImpl);
            NextDay = ReactiveCommand.Create(NextDayImpl);
        }

        public ReactiveCommand<Unit, Unit> PreviousDay { get; }

        public ReactiveCommand<Unit, Unit> NextDay { get; }

        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public DateTime EndDate
        {
            get { return (DateTime)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

        public ObservableCollection<UserTask> UserTasks
        {
            get { return (ObservableCollection<UserTask>)GetValue(UserTasksProperty); }
            set { SetValue(UserTasksProperty, value); }
        }

        private void PreviousDayImpl()
        {
            StartDate = StartDate.Date.AddDays(-1);
            SelectedDate = StartDate;
            EndDate = StartDate.Date.AddDays(1).AddMilliseconds(-1);
        }

        private void NextDayImpl()
        {
            StartDate = StartDate.Date.AddDays(1);
            SelectedDate = StartDate;
            EndDate = StartDate.Date.AddDays(1).AddMilliseconds(-1);
        }
    }
}
