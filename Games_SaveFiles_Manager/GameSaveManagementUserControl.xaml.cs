using Games_SaveFiles_Manager.ViewModels;
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
    /// Logika interakcji dla klasy GameSaveManagementUserControl.xaml
    /// </summary>
    public partial class GameSaveManagementUserControl : UserControl
    {

        private MainWindowViewModel viewModel;

        public GameSaveManagementUserControl()
        {
            viewModel = new MainWindowViewModel();
            this.DataContext = viewModel;
            InitializeComponent();
        }
    }
}
