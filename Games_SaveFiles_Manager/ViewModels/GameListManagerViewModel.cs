using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Games_SaveFiles_Manager.Models;

namespace Games_SaveFiles_Manager.ViewModels
{
    class GameListManagerViewModel:INotifyPropertyChanged
    {
        #region Fields
        private ObservableCollection<Game> games = new ObservableCollection<Game>();
        private Game selectedGame;
        private Game tempSelectedGameData;
        private string newGameName = "";
        private bool editMode = false;
        private string applicationDataFilePath;
        //private bool saveFileMode1 = true; //this implementation is wrong. RadioButton property IsCheked does not update.
        //private bool saveFileMode2 = false;
        //private ObservableCollection<string> saveFileMode_list = new ObservableCollection<string>();


        private ICommand _activateEditModeCommand;
        private ICommand _deactivateEditModeCommand;
        private ICommand _saveChangesToGameDataCommand;
        private ICommand _addNewGameCommand;
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

        public string NewGameName
        {
            get
            {
                return newGameName;
            }
            set
            {
                if (newGameName != value)
                {
                    newGameName = value;
                    OnPropertyChange("NewGameName");
                }
            }
        }

        public bool EditMode
        {
            get
            {
                return editMode;
            }
            set
            {
                if (editMode != value)
                {
                    editMode = value;

                    OnPropertyChange("EditMode");
                }
            }
        }

        //public bool SaveFileMode1
        //{
        //    get

        //    {
        //        return saveFileMode1;
        //    }
        //    set
        //    {
        //        if (saveFileMode1 != value)
        //        {
        //            saveFileMode1 = value;
        //            OnPropertyChange("SaveFileMode1");
        //        }
        //    }
        //}
            
        //public bool SaveFileMode2
        //{
        //    get

        //    {
        //        return saveFileMode2;
        //    }
        //    set
        //    {
        //        if (saveFileMode2 != value)
        //        {
        //            saveFileMode2 = value;
        //            OnPropertyChange("SaveFileMode2");
        //        }
        //    }
        //}

        /*public ObservableCollection<string> SaveFileMode_list
        {
            get
            {
                return saveFileMode_list;
            }
            set
            {
                if (saveFileMode_list != value)
                {
                    saveFileMode_list = value;
                    OnPropertyChange("SaveFileMode_List");
                }
            }
        }*/

        public ICommand ActivateEditModeCommand
        {
            get
            {
                if (_activateEditModeCommand == null)
                {
                    _activateEditModeCommand = new CommandHandler(
                        param => Activate_Edit_Mode(),
                        param => CanExecute(true)

                        );
                }
                return _activateEditModeCommand;
            }
        }

        public ICommand DeactivateEditModeCommand
        {
            get
            {
                if (_deactivateEditModeCommand == null)
                {
                    _deactivateEditModeCommand = new CommandHandler(
                        param => Deactivate_Edit_Mode()

                        );
                }
                return _deactivateEditModeCommand;
            }
        }

        public ICommand AddNewGameCommand
        {
            get
            {
                if(_addNewGameCommand == null)
                {
                    _addNewGameCommand = new CommandHandler(
                        param => Add_New_Game());
                }
                return _addNewGameCommand;
            }
        }
        
        public ICommand SaveChangesToGameDataCommand
        {
            get
            {
                if (_saveChangesToGameDataCommand == null)
                {
                    _saveChangesToGameDataCommand = new CommandHandler(
                        param => Save_changes_to_selected_game()
                        );
                }
                return _saveChangesToGameDataCommand;
            }
        }

        
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructors

        internal GameListManagerViewModel()
        {
            //Load_Games_List();
            LoadApplicationData(AppDomain.CurrentDomain.BaseDirectory + "game_save_file_manager_config.xml");
        }
        #endregion

        #region Methods

        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool CanExecute(object parameter)
        {
            return parameter == null ? false : true;
        }

        private void Load_Games_List() //this method should be availabale in the main window
        {
            //games_listbox.SelectedItem = null; 
            Games.Clear(); //games_listbox.Items.Clear();
            try
            {
                //Try to load file in xml extension, with list of games
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "games_list.xml"))
                {
                    XDocument xdoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "games_list.xml");
                    //FileStream fs = new FileStream(xdoc.BaseUri, FileMode.Open, FileAccess.Read);

                    //reminder: games_list.xml construction: Games_list->Games->Game->Id|Name|Save_file_location|Store_profile_saves_in_app_location|
                    var query = from item in xdoc.Element("Game_save_file_manager").Element("Games").Elements("Game")
                                select item;

                    foreach (var item in query) //adding games to list
                    {
                        Game gi_ob = new Game();
                        gi_ob.Game_name = item.Element("Name").Value;/*(from temp_it in item.Descendants()
                                          select temp_it.Element("Name")).First().ToString();*/

                        gi_ob.Save_file_location = item.Element("Save_file_location").Value;/*(from temp_it in item.Descendants()
                                                   select temp_it.Element("Save_file_location")).First().ToString();*/

                        string temp_profile_specific_file_storage_method;
                        temp_profile_specific_file_storage_method = item.Element("Store_profile_saves_in_app_location").Value; /*(from temp_it in item.Descendants()
                                                   select temp_it.Element("Store_profile_saves_in_app_location")).First().ToString();*/

                        gi_ob.Profile_specific_save_file_storage_method = Convert.ToInt32(temp_profile_specific_file_storage_method);

                        //games_listbox.Items.Add(gi_ob);
                        Games.Add(gi_ob);
                    }
                }
                else
                {
                    FileNotFoundMethod(applicationDataFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load games' list. " + ex.Message);
            }
        }

        private void LoadApplicationData(string path)
        {
            Games.Clear(); //games_listbox.Items.Clear();
            try
            {
                applicationDataFilePath = path;
                //Try to load file in xml extension, with list of games
                if (File.Exists(applicationDataFilePath))
                {
                    XDocument xdoc = XDocument.Load(applicationDataFilePath);
                    //FileStream fs = new FileStream(xdoc.BaseUri, FileMode.Open, FileAccess.Read);

                    //reminder: games_list.xml construction: Games_list->Games->Game->Id|Name|Save_file_location|Store_profile_saves_in_app_location|
                    var query = from item in xdoc.Element("Game_save_file_manager").Element("Games").Elements("Game")
                                select item;

                    foreach (var item in query) //adding games to list
                    {
                        Game gi_ob = new Game();
                        gi_ob.Game_name = item.Element("Name").Value;/*(from temp_it in item.Descendants()
                                          select temp_it.Element("Name")).First().ToString();*/

                        gi_ob.Save_file_location = item.Element("Save_file_location").Value;/*(from temp_it in item.Descendants()
                                                   select temp_it.Element("Save_file_location")).First().ToString();*/

                        string temp_profile_specific_file_storage_method;
                        temp_profile_specific_file_storage_method = item.Element("Store_profile_saves_in_app_location").Value; /*(from temp_it in item.Descendants()
                                                   select temp_it.Element("Store_profile_saves_in_app_location")).First().ToString();*/

                        gi_ob.Profile_specific_save_file_storage_method = Convert.ToInt32(temp_profile_specific_file_storage_method);

                        //games_listbox.Items.Add(gi_ob);
                        Games.Add(gi_ob);
                    }
                }
                else
                {
                    //create new "games_list.xml" file
                    FileNotFoundMethod(applicationDataFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load games' list. " + ex.Message);
            }
        }

        private void Add_New_Game()
        {
            try
            {
                if (File.Exists(applicationDataFilePath))
                {
                    //string path_to_config = AppDomain.CurrentDomain.BaseDirectory + "games_list.xml";
                    XDocument xdoc = XDocument.Load(applicationDataFilePath, LoadOptions.SetBaseUri);

                    int query = (from item in xdoc.Element("Game_save_file_manager").Element("Games").Elements("Game")
                                 where item.Element("Name").Value == NewGameName
                                 select item).Count(); //get number of added games

                    if (query == 0) //there are no games with such name. Duplicate games prevention measure.
                        using (FileStream fs = new FileStream(applicationDataFilePath, FileMode.Open, FileAccess.ReadWrite))
                        {
                            xdoc.Element("Game_save_file_manager").Element("Games").Add(new XElement("Game",
                                new XElement("Id", " "), new XElement("Name", NewGameName), new XElement("Save_file_location", ""), new XElement("Store_profile_saves_in_app_location", "1")));
                            xdoc.Save(fs);
                            fs.Close();
                        }
                    else
                    {
                        MessageBox.Show("Game with this name has been already added!", "Attention!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    //Warning
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //Load_Games_List();
                LoadApplicationData(applicationDataFilePath);
            }
        }

        private void Save_changes_to_selected_game()
        {
            //Make changes to game selected from list
            try
            {
                if (File.Exists(applicationDataFilePath))
                {
                    //double stream to one file might cause unpredictable behavior. Close filestream as soon as you load data.

                    XDocument xdoc = new XDocument();//XDocument.Load(applicationDataFilePath, LoadOptions.SetBaseUri);

                    using (FileStream fs = new FileStream(applicationDataFilePath/*xdoc.BaseUri*/, FileMode.Open, FileAccess.Read))
                    {
                        xdoc = XDocument.Load(fs);
                    }

                    Game temp_game_item = tempSelectedGameData;
                    //xdoc.Element("Games_list").Element("Games").Add(new XElement("Game",
                    //new XElement("Id", " "), new XElement("Name", new_game_name_textbox.Text), new XElement("Save_file_location", ""), new XElement("Store_profile_saves_in_app_location", "1")));
                    var query = (from item in xdoc.Element("Game_save_file_manager").Element("Games").Elements("Game")
                                 where (string)item.Element("Name").Value == temp_game_item.Game_name //create field containing temporary game name or use Game fields' values
                                 select item).First();
                    query.Element("Name").Value = SelectedGame.Game_name; //game_name_textbox.Text;
                    query.Element("Save_file_location").Value = SelectedGame.Save_file_location;//save_file_directory_path_textbox.Text;

                    //save_store_decision_radarbox1.IsChecked == true)
                    //if (SaveFileMode1==true)//SelectedGame.Profile_specific_save_file_storage_method == 1)
                    //{
                    //    query.Element("Store_profile_saves_in_app_location").Value = "1";
                    //}
                    //else
                    //{
                    //    query.Element("Store_profile_saves_in_app_location").Value = "2";
                    //}

                    query.Element("Store_profile_saves_in_app_location").Value = SelectedGame.Profile_specific_save_file_storage_method.ToString();

                    xdoc.Save(applicationDataFilePath);
                }
                else
                {
                    //Warning
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save games' list correctly. " + ex.Message);
            }

            //End edit mode
            Deactivate_Edit_Mode();
            //Load_Games_List();
            //Load_Selected_Games_Data(SelectedGame); 
        }

        private void Toggle_Edit_Mode(bool mode)
        {
            EditMode = mode;
        }

        private void Activate_Edit_Mode()
        {
            if (SelectedGame != null)
            {
                tempSelectedGameData = new Game(SelectedGame);
                EditMode = true;
            }
        }

        private void Deactivate_Edit_Mode()
        {
            EditMode = false;
            //Load_Games_List();
            LoadApplicationData(applicationDataFilePath);
        }

        private void FileNotFoundMethod(string path)
        {
            XDeclaration declaration = new XDeclaration("1.0", "UTF-8", "no");
            //XElement root = new XElement("Profiles_list", new XElement("Profiles"));
            XElement root = new XElement("Game_save_file_manager", new XElement("Profiles", new XElement("Profile",
                new XElement("Name", "default"), new XElement("Creation_date", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")))),
                new XElement("Games"));
            XDocument xdoc = new XDocument(declaration, root);
            xdoc.Save(path);
        }

        //private void Load_Selected_Games_Data(Game temp_obj)
        //{
        //    //Game temp_obj = (Game)games_listbox.SelectedItem;
        //    try
        //    {
        //        SelectedGame.Game_name = temp_obj.Game_name; //game_name_textbox.Text = temp_obj.Game_name;
        //        save_file_directory_path_textbox.Text = temp_obj.Save_file_location;

        //        if (temp_obj.Profile_specific_save_file_storage_method == 1)
        //        {
        //            save_store_decision_radarbox1.IsChecked = true;
        //        }
        //        else if (temp_obj.Profile_specific_save_file_storage_method == 2)
        //        {
        //            save_store_decision_radarbox2.IsChecked = true;
        //        }
        //        else
        //        {
        //            save_store_decision_radarbox1.IsChecked = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Failed to load selected game's data! Error occured.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
        #endregion
    }
}
