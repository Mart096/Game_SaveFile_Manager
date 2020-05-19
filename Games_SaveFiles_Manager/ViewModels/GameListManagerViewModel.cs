using Games_SaveFiles_Manager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace Games_SaveFiles_Manager.ViewModels
{
    class GameListManagerViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<Game> games = new ObservableCollection<Game>();
        private ObservableCollection<string> filesToBeManaged = new ObservableCollection<string>();
        private Game selectedGame;
        private Game tempSelectedGameData;
        private string newGameName = "";
        private bool editMode = false;
        private string applicationDataFilePath;
        private bool manageSelectedFilesOnlyFlag = false;
        private string selectedFileToManage;
        //private bool saveFileMode1 = true; //this implementation is wrong. RadioButton property IsCheked does not update.
        //private bool saveFileMode2 = false;
        //private ObservableCollection<string> saveFileMode_list = new ObservableCollection<string>();


        private ICommand _activateEditModeCommand;
        private ICommand _deactivateEditModeCommand;
        private ICommand _saveChangesToGameDataCommand;
        private ICommand _addNewGameCommand;
        private ICommand _addFileToListOfManagedFilesCommand;
        private ICommand _removeFileFromListOfManagedFilesCommand;
        
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

        public ObservableCollection<string> FilesToBeManaged
        {
            get
            {
                return filesToBeManaged;
            }
            set
            {
                if(filesToBeManaged != value)
                {
                    filesToBeManaged = value;
                    OnPropertyChange("FilesToBeManaged");
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
                    FilesToBeManaged.Clear();
                    selectedGame = value;
                    if (selectedGame.ManageSelectedFilesOnly == true)
                    {
                        foreach (string filename in selectedGame.FilesToManage)
                        {
                            FilesToBeManaged.Add(filename);
                        }
                    }
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

        public string SelectedFileToManage
        {
            get
            {
                return selectedFileToManage;
            }
            set
            {
                if(selectedFileToManage != value)
                {
                    selectedFileToManage = value;
                    OnPropertyChange("SelectedFileToManage");
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

        public ICommand AddFileToListOfManagedFilesCommand
        {
            get
            {
                if(_addFileToListOfManagedFilesCommand == null)
                {
                    _addFileToListOfManagedFilesCommand = new CommandHandler(
                        param => Add_file_to_list_of_managed_files());
                }
                return _addFileToListOfManagedFilesCommand;
            }
        }

        public ICommand RemoveFileFromListOfManagedFilesCommand
        {
            get
            {
                if (_removeFileFromListOfManagedFilesCommand == null)
                {
                    _removeFileFromListOfManagedFilesCommand = new CommandHandler(
                        param => Remove_file_from_list_of_managed_files());
                }
                return _removeFileFromListOfManagedFilesCommand;
            }
        }

        public bool ManageSelectedFilesOnlyFlag
        {
            get
            {
                return manageSelectedFilesOnlyFlag;
            }
            set
            {
                if(manageSelectedFilesOnlyFlag != value)
                {
                    manageSelectedFilesOnlyFlag = value;
                    //if(manageSelectedFilesOnlyFlag == true)
                    //{
                    //    //load files from directory
                    //}
                    OnPropertyChange("ManageSelectedFilesOnlyFlag");
                }
            }
        }

        #endregion

        #region Events
        //public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructors

        internal GameListManagerViewModel()
        {
            //Load_Games_List();
            LoadApplicationData(UtilClass.GetConfigFilePath());
        }
        #endregion

        #region Methods

        //private void OnPropertyChange(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        private bool CanExecute(object parameter)
        {
            return parameter == null ? false : true;
        }

        //private void Load_Games_List() //this method should be availabale in the main window
        //{
        //    //games_listbox.SelectedItem = null; 
        //    Games.Clear(); //games_listbox.Items.Clear();
        //    try
        //    {
        //        //Try to load file in xml extension, with list of games
        //        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "games_list.xml"))
        //        {
        //            XDocument xdoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "games_list.xml");
        //            //FileStream fs = new FileStream(xdoc.BaseUri, FileMode.Open, FileAccess.Read);

        //            //reminder: games_list.xml construction: Games_list->Games->Game->Id|Name|Save_file_location|Store_profile_saves_in_app_location|
        //            var query = from item in xdoc.Element("Game_save_file_manager").Element("Games").Elements("Game")
        //                        select item;

        //            foreach (var item in query) //adding games to list
        //            {
        //                Game gi_ob = new Game();
        //                gi_ob.Game_name = item.Element("Name").Value;/*(from temp_it in item.Descendants()
        //                                  select temp_it.Element("Name")).First().ToString();*/

        //                gi_ob.Save_file_location = item.Element("Save_file_location").Value;/*(from temp_it in item.Descendants()
        //                                           select temp_it.Element("Save_file_location")).First().ToString();*/

        //                string temp_profile_specific_file_storage_method;
        //                temp_profile_specific_file_storage_method = item.Element("Store_profile_saves_in_app_location").Value; /*(from temp_it in item.Descendants()
        //                                           select temp_it.Element("Store_profile_saves_in_app_location")).First().ToString();*/

        //                gi_ob.Profile_specific_save_file_storage_method = Convert.ToInt32(temp_profile_specific_file_storage_method);

        //                //games_listbox.Items.Add(gi_ob);
        //                Games.Add(gi_ob);
        //            }
        //        }
        //        else
        //        {
        //            FileNotFoundMethod(applicationDataFilePath);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Failed to load games' list. " + ex.Message);
        //    }
        //}

        private void LoadApplicationData(string path)
        {
            Games.Clear(); //games_listbox.Items.Clear();
            FilesToBeManaged.Clear();

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

                        bool node_exists = item.Elements("Manage_selected_files_only").Any();
                        if (node_exists == false)
                        {
                            gi_ob.ManageSelectedFilesOnly = false;
                        }
                        else
                        {
                            gi_ob.ManageSelectedFilesOnly = Convert.ToBoolean(item.Element("Manage_selected_files_only").Value);
                        }

                        if (item.Elements("Files_to_manage").Any())//.Elements("File").Any())
                        {
                            if(item.Element("Files_to_manage").Elements("File").Any())
                                foreach (string filename in item.Element("Files_to_manage").Elements("File"))
                                {
                                    gi_ob.FilesToManage.Add(filename);
                                }
                        }
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
                                 select item).Count(); //get number of added games with the name specified for a new one

                    if (query == 0) //there are no games with such name. Duplicate games prevention measure.
                        using (FileStream fs = new FileStream(applicationDataFilePath, FileMode.Open, FileAccess.ReadWrite))
                        {
                            xdoc.Element("Game_save_file_manager").Element("Games").Add(new XElement("Game",
                                new XElement("Id", " "), new XElement("Name", NewGameName), new XElement("Save_file_location", ""), 
                                new XElement("Store_profile_saves_in_app_location", "1"), new XElement("Manage_selected_files_only", "false"), 
                                new XElement("Files_to_manage")));
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
            catch (Exception)
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
                    bool prompt_save_file_migration = false;

                    using (FileStream fs = new FileStream(applicationDataFilePath/*xdoc.BaseUri*/, FileMode.Open, FileAccess.Read))
                    {
                        xdoc = XDocument.Load(fs);
                    }

                    Game temp_game_item = tempSelectedGameData;
                    XElement query = (from item in xdoc.Element("Game_save_file_manager").Element("Games").Elements("Game")
                                 where (string)item.Element("Name").Value == temp_game_item.Game_name //create field containing temporary game name or use Game fields' values
                                 select item).First();
                    //Same as above, but with lambda expression
                    //XElement elem1 = xdoc.Element("Game_save_file_manager").Element("Games").Elements("Game").Where(item => (string)item.Element("Name").Value == temp_game_item.Game_name).Select(item => item).First();

                    query.Element("Name").Value = SelectedGame.Game_name;

                    //update save file location element if the value was changed
                    if((query.Element("Save_file_location").Value != SelectedGame.Save_file_location) && Directory.Exists(SelectedGame.Save_file_location))
                    {
                        query.Element("Save_file_location").Value = SelectedGame.Save_file_location;
                        prompt_save_file_migration = true;
                    }
                    else if (!Directory.Exists(SelectedGame.Save_file_location))
                    {
                        throw new DirectoryNotFoundException();
                    }

                    
                    bool node_exists = query.Elements("Manage_selected_files_only").Any();
                    if (node_exists == false)
                    {
                        query.Add(new XElement("Manage_selected_files_only", SelectedGame.ManageSelectedFilesOnly.ToString()));
                    }
                    else
                        query.Element("Manage_selected_files_only").SetValue(SelectedGame.ManageSelectedFilesOnly.ToString());

                    //add files to list of managed files
                    if (SelectedGame.ManageSelectedFilesOnly == true && !temp_game_item.FilesToManage.Equals(FilesToBeManaged.ToList<string>()))
                    {
                        List<string> new_filenames_list = FilesToBeManaged.ToList<string>();
                        //var subquery = from item in query.Element("Files_to_manage").Elements("Files") select item;
                        if(query.Elements("Files_to_manage").Any())
                        {
                            query.Element("Files_to_manage").Elements("File").Remove();
                        }
                        else
                        {
                            query.Add(new XElement("Files_to_manage"));
                        }

                        foreach (string filename in new_filenames_list)
                        {
                            query.Element("Files_to_manage").Add(new XElement("File", filename));
                        }
                    }

                    //if save file storage method had been changed, game's element will be updated and prompt will be displayed to the user, asking if manager should move files to a new directory
                    if (!(query.Element("Store_profile_saves_in_app_location").Value == SelectedGame.Profile_specific_save_file_storage_method.ToString()))
                    {
                        query.Element("Store_profile_saves_in_app_location").Value = SelectedGame.Profile_specific_save_file_storage_method.ToString();
                        prompt_save_file_migration = true;
                    }

                    if (prompt_save_file_migration)
                    { 
                        //prompt
                        MessageBoxResult boxresult1 = MessageBox.Show("Game save file storage method or game save file directory has been changed. Would you like to migrate save files to a new directory?", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (boxresult1 == MessageBoxResult.Yes)
                            MigrateSaveFilesToNewDirectory(temp_game_item.Game_name, temp_game_item.Profile_specific_save_file_storage_method, SelectedGame.Profile_specific_save_file_storage_method, temp_game_item.Save_file_location, SelectedGame.Save_file_location);
                    }

                    xdoc.Save(applicationDataFilePath);
                    //End edit mode
                    Deactivate_Edit_Mode();
                }
                else
                {
                    //Warning
                }
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Specified save files' directory has not been found or is incorrect.", "Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save games' list correctly. " + ex.Message);
                //End edit mode
                Deactivate_Edit_Mode();
            }
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

        private void Add_file_to_list_of_managed_files()
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.InitialDirectory = SelectedGame.Save_file_location;

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    List<string> selected_filenames = dlg.FileNames.ToList<string>();

                    foreach (string selected_filename in selected_filenames)
                    {
                        FileInfo finfo = new FileInfo(selected_filename);

                        if(!FilesToBeManaged.Contains(selected_filename) && string.Equals(finfo.DirectoryName, SelectedGame.Save_file_location))
                            FilesToBeManaged.Add(finfo.Name);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to add file names into list.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                FilesToBeManaged.Clear();
            }
        }

        private void Remove_file_from_list_of_managed_files()
        {
            if (SelectedFileToManage != null)
            {
                FilesToBeManaged.Remove(SelectedFileToManage);
            }
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

        /// <summary>
        /// Method responsible for moving game save files to new location, depending on settings, such as currently selected storage method
        /// </summary>
        /// <param name="game_name">Name of modified game</param>
        /// <param name="previous_storage_method"></param>
        /// <param name="currently_selected_method"></param>
        /// <param name="save_file_path"></param>
        /// <param name="new_save_file_path"></param>
        private bool MigrateSaveFilesToNewDirectory(string game_name, int previous_storage_method, int currently_selected_method, string save_file_path, string new_save_file_path="")
        {
            try
            {
                XDocument xdoc = new XDocument();

                using (FileStream fs = new FileStream(UtilClass.GetConfigFilePath(), FileMode.Open, FileAccess.Read))
                    xdoc = XDocument.Load(fs);

                var profiles_query = (from items in xdoc.Element("Game_save_file_manager").Element("Profiles").Elements("Profile") select items); //will be used to move directories dedicated to the storage of profile specific game save files.
                //string game_name = (from item in xdoc.Element("Game_save_file_manager").Element("Games").Elements("Game")
                //                    where item.Element("Save_file_location").Value == save_file_path
                //                    select item.Element("Name").Value).First();

                string path_to_profiles_in_save_file_location = save_file_path + "\\..\\" + game_name + "_managerprofiles";
                string path_to_profiles_in_new_save_file_location = new_save_file_path + "\\..\\" + game_name + "_managerprofiles";

                string movedir1="";
                string movedir2="";

                //if both storage methods are equal and...
                if(previous_storage_method == currently_selected_method)
                {
                    if (currently_selected_method == 1) //...it's equal to 1 (store files locally), then move save files to a new location
                    {
                        if(!Directory.Exists(path_to_profiles_in_save_file_location))
                            Directory.CreateDirectory(path_to_profiles_in_save_file_location);
                        if (!Directory.Exists(path_to_profiles_in_new_save_file_location))
                            Directory.CreateDirectory(path_to_profiles_in_new_save_file_location);


                        movedir1 = path_to_profiles_in_save_file_location + "\\";// + profile.Element("Name").Value;
                        movedir2 = path_to_profiles_in_new_save_file_location + "\\";// + profile.Element("Name").Value;
                        //foreach (var profile in profiles_query)
                        //{
                        //    try
                        //    {
                        //        Directory.Move(path_to_profiles_in_save_file_location+"\\"+profile.Element("Name").Value, path_to_profiles_in_new_save_file_location+"\\" + profile.Element("Name").Value);
                        //    }
                        //    catch (Exception)
                        //    {
                        //        //failed to move directory
                        //    }
                        //}
                    }
                    else if (currently_selected_method == 2) //it's equal to 2 (store files in manager's directory), then move save files to manager's directory
                    {
                        return true;
                    }
                    else //...something went wrong, because there has been wrong value specified for storage method
                    {
                        throw new InvalidOperationException("Save file storage method has invalid value!");
                    }
                }
                else //if storage methods have diferrent value and...
                {
                    if (currently_selected_method == 1) //...current method is equal to 1 then move save files from manager's directory to a new directory
                    {
                        movedir1 = AppDomain.CurrentDomain.BaseDirectory + "\\games\\" + game_name;// + profile.Element("Name").Value;
                        movedir2 = path_to_profiles_in_save_file_location;
                    }
                    else if(currently_selected_method == 2) //current method is equal to 2 then move save file from old directory to manager's directory
                    {
                        movedir1 = path_to_profiles_in_save_file_location;
                        movedir2 = AppDomain.CurrentDomain.BaseDirectory + "\\games\\" + game_name;// + profile.Element("Name").Value;
                    }
                    else
                    {
                        //ERROR
                    }
                }

                //moving game save files to selected directory
                foreach (var profile in profiles_query)
                {
                    string temp1 = movedir1 + "\\" + profile.Element("Name").Value.ToString();
                    string temp2 = movedir2 + "\\" + profile.Element("Name").Value.ToString();

                    string[] files_in_selected_profile_directory = Directory.GetFiles(temp1);
                    Debug.Print("Moving directories (from - to):\n " + movedir1 + "\\" + profile.Element("Name").Value + "\n " + movedir2 + "\\" + profile.Element("Name").Value);

                    //copy save game files of current profile to profile directory
                    foreach (string file in files_in_selected_profile_directory)
                    {
                        File.Copy(file, temp2 + "\\" + Path.GetFileName(file), true);
                        Debug.Print("Copy file: \"" + Path.GetFileName(file) + "\" from " + file + " to " + temp2);
                        File.Delete(file);
                    }
                    //WARNING, this solution doesn't work for directories on different volumes. Possible solution assumes file copying to target directory and deleting them in old directory
                    //Directory.Move(movedir1 + "\\" + profile.Element("Name").Value, movedir2 + "\\" + profile.Element("Name").Value);
                }

                //depending on settings, migrate stored game save files from old directory to a new one, specified in the path
                //save files will be moved from manager's directory to specified path
                //if (currently_selected_method == 1 && Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "games\\" + game_name))
                //{
                //    if (!Directory.Exists(path_to_profiles_in_save_file_location))
                //        Directory.CreateDirectory(path_to_profiles_in_save_file_location);

                //    foreach (var profile in profiles_query)
                //    {
                //        try
                //        {
                //            Directory.Move(AppDomain.CurrentDomain.BaseDirectory + "games\\" + game_name + "\\" + profile.Element("Name").Value, path_to_profiles_in_save_file_location);
                //        }
                //        catch(Exception)
                //        {
                //            //failed to move directory
                //        }
                //    }
                //}
                //else if (currently_selected_method == 2 && Directory.Exists(save_file_path))
                //{
                //    if (!Directory.Exists(path_to_profiles_in_save_file_location))
                //    {
                //        throw new DirectoryNotFoundException("Directory with profiles' save files was not found.");
                //    }
                //    else if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "games\\" + game_name))
                //        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "games\\" + game_name);

                //    foreach (var profile in profiles_query)
                //    {
                //        try
                //        {
                //            Directory.Move(path_to_profiles_in_save_file_location + "\\" + profile.Element("Name").Value, AppDomain.CurrentDomain.BaseDirectory + "games\\" + game_name + "\\");
                //        }
                //        catch (Exception)
                //        {
                //            //failed to move directory
                //        }
                //    }
                //}
                //else
                //{
                //    throw new Exception("Failed to move files to current directory");
                //}
                return true;
            }
            catch(Exception ex)
            {
                Debug.Print(ex.Message);
                return false;
            }

        }

            #endregion
    }
}
