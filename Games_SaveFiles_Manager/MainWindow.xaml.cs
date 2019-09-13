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

namespace Games_SaveFiles_Manager
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Menu_about_button_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about_window = new AboutWindow();
            about_window.ShowDialog();
        }

        private void Menu_quit_button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Apply_profile_button_Copy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Apply_profile_button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
