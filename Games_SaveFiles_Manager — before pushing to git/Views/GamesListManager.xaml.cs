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
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Games_SaveFiles_Manager.Models;
using Games_SaveFiles_Manager.ViewModels;

namespace Games_SaveFiles_Manager
{
    /// <summary>
    /// Logika interakcji dla klasy GamesListManager.xaml
    /// </summary>

    public partial class GamesListManager : Window
    {

        private GameListManagerViewModel viewModel;

        public GamesListManager()
        {
            viewModel = new GameListManagerViewModel();
            this.DataContext = viewModel;
            InitializeComponent();
            /*Load_Games_List();
            new_game_name_textbox.IsReadOnly = false;*/
        }
    }
}
