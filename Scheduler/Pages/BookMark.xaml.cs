using Microsoft.EntityFrameworkCore;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Scheduler.Pages
{
    /// <summary>
    /// Логика взаимодействия для BookMark.xaml
    /// </summary>
    public partial class BookMark : Page
    {
        public BookMark()
        {
            InitializeComponent();
        }
        private async void LoadListBox()
        {
            ObservableCollection<ListMark> co = new();

            using schedulerContext db = new();

            var list = await db.Cases.Where(l => l.CaseBookmark == 1).ToListAsync();

            foreach (var item in list)
            {
                co.Add(new ListMark 
                {
                    header = item.CaseTitle,
                    info = item.CaseDescription,
                    date = item.CaseDate.ToString(),
                    time = $"{item.CaseTimestart} - {item.CaseTimeend}"
                });
            }
            ListBoxCaseMark.ItemsSource = co;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadListBox();
        }
    }
    partial class ListMark
    {
        public string? time { get; set; }
        public string? info { get; set; }
        public string? header { get; set; }
        public string? date { get; set; }
    }
}
