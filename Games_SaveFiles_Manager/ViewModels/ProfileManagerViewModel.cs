using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Games_SaveFiles_Manager.Models;
using Games_SaveFiles_Manager.MediatorComms;

namespace Games_SaveFiles_Manager.ViewModels
{
    class ProfileManagerViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<Profile> profiles = new ObservableCollection<Profile>();
        private Profile selectedProfile;
        //private string selectedProfileName;
        private string newProfileName;
        private bool editMode = false;
        private string applicationDataFilePath;

        private ICommand _addNewProfileCommand; 
        private ICommand _editProfileCommand; 
        private ICommand _saveChangesCommand; 
        private ICommand _cancelChangesCommand; 
        private ICommand _deleteProfileCommand; 
        #endregion

        #region Properties
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

                    //notify subscribing viewmodels over mediator to update their values
                    //Mediator.Instance.NotifyColleagues(Mediator.ViewModelMessages.ProfileListUpdated, profiles);
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

        //public string SelectedProfileName
        //{
        //    get
        //    {
        //        return selectedProfile.Profile_name;
        //    }
        //    set
        //    {
        //        if (selectedProfile.Profile_name != value)
        //        {
        //            selectedProfile.Profile_name = value;
        //            OnPropertyChange("SelectedProfileName");
        //        }
        //    }
        //}

        public string NewProfileName
        {
            get
            {
                return newProfileName;
            }
            set
            {
                if (newProfileName != value)
                {
                    newProfileName = value;
                    OnPropertyChange("NewProfileName");
                }
            }
        }

        public ICommand AddNewProfileCommand
        {
            get
            {
                if (_addNewProfileCommand == null)
                {
                    _addNewProfileCommand = new CommandHandler(
                        param => AddNewProfile());
                }
                return _addNewProfileCommand;
            }
        }

        public ICommand EditProfileCommand
        {
            get
            {
                if(_editProfileCommand == null)
                {
                    _editProfileCommand = new CommandHandler(
                        param => EditModeChange(true));
                }
                return _editProfileCommand;
            }
        }

        public ICommand DeleteProfileCommand
        {
            get
            {
                if(_deleteProfileCommand == null)
                {
                    _deleteProfileCommand = new CommandHandler(
                        param => DeleteSelectedProfile());
                }
                return _deleteProfileCommand;
            }
        }

        public ICommand SaveChangesCommand
        {
            get
            {
                if (_saveChangesCommand == null)
                {
                    _saveChangesCommand = new CommandHandler(
                        param => Save_Changes_To_Profile());
                }
                return _saveChangesCommand;
            }
        }

        public ICommand CancelChangesCommand
        {
            get
            {
                if (_cancelChangesCommand == null)
                {
                    _cancelChangesCommand = new CommandHandler(
                        param => EditModeChange(false));
                }
                return _cancelChangesCommand;
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
        #endregion

        #region Events
        //public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructors
        public ProfileManagerViewModel()
        {
            //LoadProfilesList();
            LoadApplicationData(UtilClass.GetConfigFilePath());
        }
        #endregion

        #region Methods
        //public void OnPropertyChange(string propertyName)
        //{
        //    PropertyChanged ?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        //}

        /// <summary>
        /// Method formerly used for loading profiles. It no longer has any use (and should NOT be used at any time, it's only kept for archival purposes)
        /// </summary>
        [Obsolete("Obsolete method. Use \"LoadApplicationData\" method instead.")]
        private void LoadProfilesList()
        {
            try
            {
                Profiles.Clear();
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "profiles_list.xml"))
                {
                    string path_to_config = AppDomain.CurrentDomain.BaseDirectory + "profiles_list.xml";
                    XDocument xdoc = XDocument.Load(path_to_config, LoadOptions.SetBaseUri);

                    var query = (from items in xdoc.Element("Game_save_file_manager").Element("Profiles").Elements("Profile")
                                 select items);

                    foreach (var item in query)
                    {
                        string pattern = "dd.MM.yyyy HH:mm:ss";
                        DateTime temp_creation_date;

                        if (DateTime.TryParseExact(item.Element("Creation_date").Value, pattern, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out temp_creation_date))
                        {
                            //successful
                        }
                        else
                        {
                            //failure
                        }


                        Profile loaded_profile = new Profile();
                        loaded_profile.Profile_name = item.Element("Name").Value;
                        loaded_profile.Creation_time = temp_creation_date;

                        Profiles.Add(loaded_profile);
                    }

                }
                else //file containing list of profiles was not found and will be created instead
                {
                    XDeclaration declaration = new XDeclaration("1.0", "UTF-8", "no");
                    XElement root = new XElement("Game_save_file_manager", new XElement("Profiles"));
                    XDocument xdoc = new XDocument(declaration, root);
                    xdoc.Save(AppDomain.CurrentDomain.BaseDirectory + "Game_save_file_manager.xml");
                }
            }
            catch
            {

            }
        }

        private void LoadApplicationData(string path)
        {
            try
            {
                applicationDataFilePath = path;
                Profiles.Clear();
                if (File.Exists(applicationDataFilePath))
                {
                    //string path_to_config = applicationDataFilePath;
                    XDocument xdoc = XDocument.Load(applicationDataFilePath, LoadOptions.SetBaseUri);

                    var query = (from items in xdoc.Element("Game_save_file_manager").Element("Profiles").Elements("Profile")
                                 select items);

                    foreach (var item in query)
                    {
                        string pattern = "dd.MM.yyyy HH:mm:ss";
                        DateTime temp_creation_date;

                        if (DateTime.TryParseExact(item.Element("Creation_date").Value, pattern, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out temp_creation_date))
                        {
                            //successful
                        }
                        else
                        {
                            //failure
                        }


                        Profile loaded_profile = new Profile();
                        loaded_profile.Profile_name = item.Element("Name").Value;
                        loaded_profile.Creation_time = temp_creation_date;

                        Profiles.Add(loaded_profile);
                    }
                    //had to deviate from standard implementation, since collection is being updated, 
                    //not created & assigned to the field
                    Mediator.Instance.NotifyColleagues(Mediator.ViewModelMessages.ProfileListUpdated, profiles);

                }
                else //file containing list of profiles was not found and will be created instead
                {
                    FileNotFoundMethod(path);
                    LoadApplicationData(path);
                }
            }
            catch
            {
                MessageBox.Show("Loading application data failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddNewProfile()
        {
            try
            {
                if (File.Exists(applicationDataFilePath))
                {
                    //string path_to_config = AppDomain.CurrentDomain.BaseDirectory + "profiles_list.xml";
                    XDocument xdoc = XDocument.Load(applicationDataFilePath/*AppDomain.CurrentDomain.BaseDirectory + "games_list.xml"*/, LoadOptions.SetBaseUri);

                    int query = (from item in xdoc.Element("Game_save_file_manager").Element("Profiles").Elements("Profile")
                                 where item.Element("Name").Value == NewProfileName
                                 select item).Count(); //get number of added games

                    if (query == 0) //there are no profiles with such name. Duplicate profiles prevention measure.
                        using (FileStream fs = new FileStream(applicationDataFilePath, FileMode.Open, FileAccess.ReadWrite))
                        {
                            DateTime creation_time = DateTime.Now;//.ToString("yyyyMMddHHmmss"); ;
                            xdoc.Element("Game_save_file_manager").Element("Profiles").Add(new XElement("Profile",
                                new XElement("Name", NewProfileName), 
                                new XElement("Creation_date", creation_time.ToString("dd/MM/yyyy HH:mm:ss"))));
                            xdoc.Save(fs);
                            fs.Close();
                        }
                    else
                    {
                        MessageBox.Show("Profile with this name already exists!", "Attention!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    //Warning
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Exception!");
            }
            finally
            {
                //Clear the field with new profile's name
                NewProfileName = "";
                LoadApplicationData(applicationDataFilePath);
            }
        }

        private void EditModeChange(bool mode)
        {
            EditMode = mode;
            if (mode == false)
            {
                SelectedProfile = null;
            }
        }

        public void DeleteSelectedProfile()
        {
            try
            {
                if (File.Exists(applicationDataFilePath))
                {
                    XDocument xdoc = new XDocument();

                    using(FileStream fs = new FileStream(applicationDataFilePath, FileMode.Open, FileAccess.Read))
                    {
                        xdoc = XDocument.Load(fs);
                    }

                    if (SelectedProfile.Profile_name.Equals("default"))
                    {
                        MessageBox.Show("Cannot delete default profile!", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        xdoc.Element("Game_save_file_manager").Element("Profiles").Elements("Profile").Where(x => (string)x.Element("Creation_date") == SelectedProfile.Creation_time.ToString("dd/MM/yyyy HH:mm:ss") && (string)x.Element("Name") != "default").Remove();
                        xdoc.Save(applicationDataFilePath);
                    }
                }
                else
                {
                    //File doesn't exist
                    MessageBox.Show("File doesn\'t exist!");
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Profile removal failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadApplicationData(applicationDataFilePath);
            }
        }

        public void Save_Changes_To_Profile()
        {
            try
            {
                //string path_to_file = AppDomain.CurrentDomain.BaseDirectory + "profiles_list.xml";
                if (File.Exists(applicationDataFilePath))
                {
                    XDocument xdoc = new XDocument();

                    using (FileStream fs = new FileStream(applicationDataFilePath, FileMode.Open, FileAccess.Read))
                    {
                        xdoc= XDocument.Load(applicationDataFilePath, LoadOptions.SetBaseUri);
                    }

                    //string original_profile_name = (from item in xdoc.Element("Game_save_file_manager").Element("Profiles").Elements("Profile")
                    //                                where item.Element("Creation_date").Value == SelectedProfile.Creation_time.ToString("dd/MM/yyyy HH:mm:ss")
                    //                                select item).First().Value;

                    int query_check_names = (from item in xdoc.Element("Game_save_file_manager").Element("Profiles").Elements("Profile")
                                 where item.Element("Name").Value == SelectedProfile.Profile_name
                                 select item).Count(); //get number of profiles with the same name

                    if (query_check_names == 0) //there are no profiles with such name. Duplicate profiles prevention measure.
                    {
                        var query = (from item in xdoc.Element("Game_save_file_manager").Element("Profiles").Elements("Profile")
                                     where (string)item.Element("Creation_date").Value == SelectedProfile.Creation_time.ToString("dd/MM/yyyy HH:mm:ss")  //create field containing temporary game name or use Game fields' values
                                     select item).First();

                        var query_games_save_file_paths = from item in xdoc.Element("Game_save_file_manager").Element("Games").Elements("Game")
                                                          select item;
                        var query_games_with_this_profile_active = from item in xdoc.Element("Game_save_file_manager").Element("Games").Elements("Game")
                                                                   where item.Element("Profile_used").Value == query.Element("Name").Value
                                                                   select item;

                        string original_name = query.Element("Name").Value;
                        query.Element("Name").Value = SelectedProfile.Profile_name; //replace name of profile with a new one

                        foreach (var item in query_games_save_file_paths)
                        {
                            string directory_path="";

                            //change profile's directory name accordingly to used method and changes made to profile
                            if (Convert.ToInt32(item.Element("Store_profile_saves_in_app_location").Value) == 1)
                            {
                                directory_path = item.Element("Save_file_location").Value + "\\..\\" + item.Element("Name").Value + "_managerprofiles\\";
                            }
                            else
                            {   
                                directory_path = AppDomain.CurrentDomain.BaseDirectory + "\\games\\" + item.Element("Name").Value+"\\";
                            }
                            Debug.Write("Directory path: " + directory_path);

                            if (Directory.Exists(directory_path + original_name))
                            {
                                Directory.Move((directory_path + original_name), (directory_path + SelectedProfile.Profile_name));
                                //Directory.Delete(directory_path + original_name);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Profile with this name already exists!", "Attention!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    xdoc.Save(applicationDataFilePath);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error occured!" + ex.Message);
            }
            finally
            {
                EditModeChange(false);
                LoadApplicationData(applicationDataFilePath);   
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
        #endregion
    }
}
