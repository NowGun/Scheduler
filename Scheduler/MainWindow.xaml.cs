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
        private void InitializeUi()
        {
            Loaded += (sender, args) =>
            {
                WPFUI.Appearance.Watcher.Watch(this, WPFUI.Appearance.BackgroundType.Mica, true, true);
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
    }
}
