using Games_SaveFiles_Manager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace Games_SaveFiles_Manager.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Fields
        private ObservableCollection<Game> games = new ObservableCollection<Game>();
        private ObservableCollection<Profile> profiles = new ObservableCollection<Profile>();
        private Game selectedGame;
        private Profile selectedProfile;
        private ICommand _getCommand;
        #endregion

        #region Properties
        public ObservableCollection<Game> Games
        {
            get
            {
                return games;
            }
            set
            {
                if (games != value)
                {
                    games = value;
                    OnPropertyChange("Games");
                }
            }
        }

        public ObservableCollection<Profile> Profiles
        {
            get
            {
                return profiles;
            }
            set
            {
                if (profiles != value)
                {
                    profiles = value;
                    OnPropertyChange("Profiles");
                }
            }
        }

        public Game SelectedGame
        {
            get
            {
                return selectedGame;
            }
            set
            {
                if (selectedGame != value)
                {
                    selectedGame = value;
                    OnPropertyChange("SelectedGame");
                }
            }
        }

        public Profile SelectedProfile
        {
            get
            {
                return selectedProfile;
            }
            set
            {
                if (selectedProfile != value)
                {
                    selectedProfile = value;
                    OnPropertyChange("SelectedProfile");
                }
            }
        }
        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            Load_Games_List();
            Load_Profiles_List();
        }

        #endregion

        #region Methods

        private void Load_Games_List() //this method should be availabale in the main window
        {
            try
             {
                 //Try to load file in xml extension, with list of games
                 if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "games_list.xml"))
                 {
                     XDocument xdoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "games_list.xml");
                     //FileStream fs = new FileStream(xdoc.BaseUri, FileMode.Open, FileAccess.Read);

                     //reminder: games_list.xml construction: Games_list->Games->Game->Id|Name|Save_file_location|Store_profile_saves_in_app_location|
                     var query = from item in xdoc.Element("Games_list").Element("Games").Elements("Game")
                                 select item;

                     foreach (var item in query) //adding games to list
                     {
                         Game gi_ob = new Game();
                         gi_ob.Game_name = item.Element("Name").Value;
                         //(from temp_it in item.Descendants()
                         //                  select temp_it.Element("Name")).First().ToString();

                         gi_ob.Save_file_location = item.Element("Save_file_location").Value;
                                             //(from temp_it in item.Descendants()
                                             //       select temp_it.Element("Save_file_location")).First().ToString();

                         string temp_profile_specific_file_storage_method;
                         temp_profile_specific_file_storage_method = item.Element("Store_profile_saves_in_app_location").Value; 
                                                    // (from temp_it in item.Descendants()
                                                    //select temp_it.Element("Store_profile_saves_in_app_location")).First().ToString();

                         gi_ob.Profile_specific_save_file_storage_method = Convert.ToInt32(temp_profile_specific_file_storage_method);

                         Games.Add(gi_ob);
                     }
                 }
                 else
                 {
                     //create new "games_list.xml" file
                     XDeclaration decl = new XDeclaration("1.0", "UTF-8", "no");
                     XElement root = new XElement("Games_list",
                         new XElement("Games"));
                     XDocument xdoc = new XDocument(decl, root);
                     xdoc.Save(AppDomain.CurrentDomain.BaseDirectory + "games_list.xml");
                 }
             }
             catch (FileNotFoundException ex)
             {

             }
             catch (FileLoadException ex)
             {

             }
         }

            protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Load_Profiles_List()
        {
            //throw new NotImplementedException();
        }
        #endregion
    }
}
