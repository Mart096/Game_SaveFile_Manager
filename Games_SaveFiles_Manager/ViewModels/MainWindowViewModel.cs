using Games_SaveFiles_Manager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        private ICommand _applyProfileCommand;
        private ICommand _verifySaveFilePathCommand;
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

        public ICommand ApplyProfileCommand
        {
            get
            {
                if(_applyProfileCommand == null)
                {
                    _applyProfileCommand = new CommandHandler(
                        param => ApplyProfile());
                }
                return _applyProfileCommand;
            }
        }

        public ICommand VerifySaveFilePathCommand
        {
            get
            {
                if( _verifySaveFilePathCommand == null)
                {
                    _verifySaveFilePathCommand = new CommandHandler(
                        param=> VerifySaveFilePath());
                }
                return _verifySaveFilePathCommand;
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            //Load_Games_List();
            //Load_Profiles_List();
            Load_Application_Data();
        }

        #endregion

        #region Methods

        //this method has been retired from use
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

                        gi_ob.Profile_used = item.Element("Profile_used").Value;

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
            catch (System.Xml.XmlException)
            {
                //create new "games_list.xml" file
                XDeclaration decl = new XDeclaration("1.0", "UTF-8", "no");
                XElement root = new XElement("Games_list",
                    new XElement("Games"));
                XDocument xdoc = new XDocument(decl, root);
                xdoc.Save(AppDomain.CurrentDomain.BaseDirectory + "games_list.xml");
            }
         }

        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //this method has been retired from use
        private void Load_Profiles_List()
        {
            try
            {
                Profiles.Clear();
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "profiles_list.xml"))
                {
                    string path_to_config = AppDomain.CurrentDomain.BaseDirectory + "profiles_list.xml";
                    XDocument xdoc = XDocument.Load(path_to_config, LoadOptions.SetBaseUri);

                    var query = (from items in xdoc.Element("Profiles_list").Element("Profiles").Elements("Profile")
                                 select items);

                    foreach (var item in query)
                    {
                        string pattern = "dd.MM.yyyy HH:mm:ss";
                        DateTime temp_creation_date;

                        if (DateTime.TryParseExact(item.Element("Creation_date").Value, pattern, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out temp_creation_date))
                        {
                            //successful
                            Profile loaded_profile = new Profile();
                            loaded_profile.Profile_name = item.Element("Name").Value;
                            loaded_profile.Creation_time = temp_creation_date;

                            Profiles.Add(loaded_profile);
                        }
                        else
                        {
                            //failure

                        }
                    }

                }
                else //file containing list of profiles was not found and will be created instead
                {
                    XDeclaration declaration = new XDeclaration("1.0", "UTF-8", "no");
                    XElement root = new XElement("Profiles_list", new XElement("Profiles"));
                    XDocument xdoc = new XDocument(declaration, root);
                    xdoc.Save(AppDomain.CurrentDomain.BaseDirectory + "profiles_list.xml");
                }
            }
            catch
            {

            }
        }

        private void Load_Application_Data()
        {
            try
            {
                Profiles.Clear();
                Games.Clear();
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "game_save_file_manager_config.xml"))
                {
                    string path_to_config = AppDomain.CurrentDomain.BaseDirectory + "game_save_file_manager_config.xml";
                    XDocument xdoc = XDocument.Load(path_to_config, LoadOptions.SetBaseUri);

                    var query_profiles = (from items in xdoc.Element("Game_save_file_manager").Element("Profiles").Elements("Profile")
                                 select items);

                    var query_games = (from items in xdoc.Element("Game_save_file_manager").Element("Games").Elements("Game")
                                       select items);

                    //Put loaded profiles on the list
                    foreach (var item in query_profiles)
                    {
                        string pattern = "dd.MM.yyyy HH:mm:ss";
                        DateTime temp_creation_date;

                        if (DateTime.TryParseExact(item.Element("Creation_date").Value, pattern, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out temp_creation_date))
                        {
                            //successful
                            Profile loaded_profile = new Profile();
                            loaded_profile.Profile_name = item.Element("Name").Value;
                            loaded_profile.Creation_time = temp_creation_date;

                            Profiles.Add(loaded_profile);
                        }
                        else
                        {
                            //failure

                        }
                    }

                    foreach (var item in query_games) //adding games to list
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

                        gi_ob.Profile_used = item.Element("Profile_used").Value;

                        Games.Add(gi_ob);
                    }

                }
                else //file containing application data was not found and will be created instead
                {
                    XDeclaration declaration = new XDeclaration("1.0", "UTF-8", "no");
                    //XElement root = new XElement("Profiles_list", new XElement("Profiles"));
                    XElement root = new XElement("Game_save_file_manager", new XElement("Profiles", new XElement("Profile", 
                        new XElement("Name", "default"), new XElement("Creation_date", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")))), 
                        new XElement("Games"));
                    XDocument xdoc = new XDocument(declaration, root);
                    xdoc.Save(AppDomain.CurrentDomain.BaseDirectory + "game_save_file_manager_config.xml");

                    Load_Application_Data();
                }
            }
            catch
            {

            }
        }

        public void ApplyProfile()
        {
            MessageBox.Show("Approved");

            //1. Copy current save file to currently selected profile
            //2. Change current profile to profile selected from the list
            //3. Import save game to game's save file directory from profile's directory

            string temp_path = SelectedGame.Save_file_location;
            string previous_profile_name = SelectedGame.Profile_used;
            int method = SelectedGame.Profile_specific_save_file_storage_method;

            try
            {
                if (!(SelectedProfile.Profile_name.Equals(previous_profile_name)))
                {
                    XDocument xdoc = new XDocument();

                    if (Directory.Exists(temp_path))
                    {
                        if (method == 1) //1st method
                        {
                            var files_in_directory = Directory.GetFiles(temp_path);
                            string[] files_in_selected_profile_directory = Directory.GetFiles(temp_path + "\\..\\" + SelectedProfile.Profile_name);

                            //copy save game files of current profile to profile directory
                            foreach (string file in files_in_directory)
                            {
                                File.Copy(file, temp_path + "\\..\\" + previous_profile_name + "\\" + Path.GetFileName(file), true);
                            }

                            //copy save game files of another profile to game's save file directory
                            foreach (string file in files_in_selected_profile_directory)
                            {
                                File.Copy(file, temp_path + "\\" + Path.GetFileName(file), true);
                            }

                            //make changes in app's config file
                            using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "game_save_file_manager_config.xml", FileMode.Open, FileAccess.Read))
                            {
                                xdoc = XDocument.Load(fs);
                            }

                            XElement edited_game = (from games in xdoc.Element("Game_save_file_manager").Element("Games").Elements("Game")
                                                    where (string)games.Element("Name").Value == SelectedGame.Game_name
                                                    select games).First();

                            edited_game.Element("Profile_used").Value = SelectedProfile.Profile_name;

                            xdoc.Save(AppDomain.CurrentDomain.BaseDirectory + "game_save_file_manager_config.xml");

                        }
                        else //2nd method
                        {

                        }
                    }
                    else
                    {
                        //it doesn't exist
                        MessageBox.Show("Save file directory doesn\'t exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("This profile is already activated!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!" + ex.Message, "Error");
            }
            finally
            {
                
                Load_Application_Data();
            }
    }

        public void VerifySaveFilePath()
        {
            string temp_path = SelectedGame.Save_file_location;
            int method = SelectedGame.Profile_specific_save_file_storage_method;

            try
            {
                XDocument xdoc = new XDocument();

                //check if given path exists and doesn't collide with other games
                if (Directory.Exists(temp_path))
                {
                    using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory+"game_save_file_manager_config.xml", FileMode.Open, FileAccess.Read))
                    {
                        xdoc = XDocument.Load(fs);
                    }

                    var profiles_query = (from items in xdoc.Element("Game_save_file_manager").Element("Profiles").Elements("Profile") select items);

                    foreach (var item in profiles_query)
                    {
                        string new_directory_path = temp_path + "\\..\\" + item.Element("Name").Value;
                        if (!(Directory.Exists(new_directory_path) == true))
                        {
                            //create directory for specified profile
                            Directory.CreateDirectory(new_directory_path);
                            if (item.Element("Name").Value.Equals("default"))
                            {
                                //copy current save files to default's profile directory
                            }
                        }
                    }
                }
                else
                {
                    //it doesn't exist
                    MessageBox.Show("Save file directory doesn\'t exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!" + ex.Message, "Error");
            }
        }
        #endregion
    }
}
