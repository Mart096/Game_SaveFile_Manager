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
using System.Windows.Shapes;
using Games_SaveFiles_Manager.ViewModels;

namespace Games_SaveFiles_Manager
{
    /// <summary>
    /// Logika interakcji dla klasy ProfileManagerWindow.xaml
    /// </summary>
    public partial class ProfileManagerWindow : Window
    {
        ProfileManagerViewModel viewModel;

        public ProfileManagerWindow()
        {
            InitializeComponent();
            viewModel = new ProfileManagerViewModel();
            this.DataContext = viewModel;
        }
    }
}
