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

        //    private void Load_Games_List() //this method should be availabale in the main window
        //    {
        //        //games_listbox.SelectedItem = null; 
        //        games_listbox.Items.Clear();
        //        try
        //        {
        //            //Try to load file in xml extension, with list of games
        //            if(File.Exists(AppDomain.CurrentDomain.BaseDirectory + "games_list.xml"))
        //            {
        //                XDocument xdoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "games_list.xml");
        //                //FileStream fs = new FileStream(xdoc.BaseUri, FileMode.Open, FileAccess.Read);

        //                //reminder: games_list.xml construction: Games_list->Games->Game->Id|Name|Save_file_location|Store_profile_saves_in_app_location|
        //                var query = from item in xdoc.Element("Games_list").Element("Games").Elements("Game")
        //                                                           select item;

        //                foreach (var item in query) //adding games to list
        //                {
        //                    Game gi_ob = new Game();
        //                    gi_ob.Game_name = item.Element("Name").Value;/*(from temp_it in item.Descendants()
        //                                      select temp_it.Element("Name")).First().ToString();*/

        //                    gi_ob.Save_file_location = item.Element("Save_file_location").Value;/*(from temp_it in item.Descendants()
        //                                               select temp_it.Element("Save_file_location")).First().ToString();*/

        //                    string temp_profile_specific_file_storage_method;
        //                    temp_profile_specific_file_storage_method = item.Element("Store_profile_saves_in_app_location").Value; /*(from temp_it in item.Descendants()
        //                                               select temp_it.Element("Store_profile_saves_in_app_location")).First().ToString();*/

        //                    gi_ob.Profile_specific_save_file_storage_method = Convert.ToInt32(temp_profile_specific_file_storage_method);

        //                    games_listbox.Items.Add(gi_ob);
        //                }
        //            }
        //            else
        //            {
        //                //create new "games_list.xml" file
        //                XDeclaration decl = new XDeclaration("1.0", "UTF-8", "no");
        //                XElement root = new XElement("Games_list",
        //                    new XElement("Games"));
        //                XDocument xdoc = new XDocument(decl, root);
        //                xdoc.Save(AppDomain.CurrentDomain.BaseDirectory + "games_list.xml");
        //            }
        //        }
        //        catch(FileNotFoundException ex)
        //        {

        //        }
        //        catch (FileLoadException ex)
        //        {

        //        }
        //        catch (Exception)
        //        {

        //        }
        //    }

        //    private void Add_New_Game()
        //    {
        //        try
        //        {
        //            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "games_list.xml"))
        //            {
        //                string path_to_config = AppDomain.CurrentDomain.BaseDirectory + "games_list.xml";
        //                XDocument xdoc = XDocument.Load(path_to_config/*AppDomain.CurrentDomain.BaseDirectory + "games_list.xml"*/, LoadOptions.SetBaseUri);

        //                int query = (from item in xdoc.Element("Games_list").Element("Games").Elements("Game")
        //                            where item.Element("Name").Value == new_game_name_textbox.Text
        //                            select item).Count(); //get number of added games

        //                if(query == 0) //there are no games with such name. Duplicate games prevention measure.
        //                using (FileStream fs = new FileStream(path_to_config, FileMode.Open, FileAccess.ReadWrite))
        //                {
        //                    xdoc.Element("Games_list").Element("Games").Add(new XElement("Game",
        //                        new XElement("Id", " "), new XElement("Name", new_game_name_textbox.Text), new XElement("Save_file_location", ""), new XElement("Store_profile_saves_in_app_location", "1")));
        //                    xdoc.Save(fs);
        //                    fs.Close();
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Game with this name has been already added!", "Attention!", MessageBoxButton.OK, MessageBoxImage.Information);
        //                }
        //            }
        //            else
        //            {
        //                //Warning
        //            }
        //        }
        //        catch (FileNotFoundException ex)
        //        {

        //        }
        //        catch (FileLoadException ex)
        //        {

        //        }
        //        catch(Exception ex)
        //        {

        //        }
        //    }

        //    private void Edit_button_Click(object sender, RoutedEventArgs e)
        //    {
        //        Toggle_Edit_Mode(true); //edit mode activated!
        //    }

        //    private void Toggle_Edit_Mode(bool is_edit_mode)
        //    {
        //        if (is_edit_mode == true)
        //        {
        //            //blocking controls that user should not be able to be interact with during edit mode
        //            games_listbox.IsEnabled = false;
        //            add_new_game_button.IsEnabled = false;
        //            new_game_name_textbox.IsReadOnly = true;

        //            //showing controls unique to edit mode
        //            game_name_textbox.IsReadOnly = false;
        //            save_file_directory_path_textbox.IsReadOnly = false;
        //            edit_button.IsEnabled = false;
        //            edit_button.Visibility = Visibility.Hidden;
        //            cancel_edit_button.IsEnabled = true;
        //            save_changes_button.IsEnabled = true;
        //            cancel_edit_button.Visibility = Visibility.Visible;
        //            save_changes_button.Visibility = Visibility.Visible;
        //            save_store_decision_radarbox1.IsEnabled = true;
        //            save_store_decision_radarbox2.IsEnabled = true;
        //        }
        //        else
        //        {

        //            games_listbox.IsEnabled = true;
        //            add_new_game_button.IsEnabled = true;
        //            new_game_name_textbox.IsReadOnly = false;

        //            game_name_textbox.IsReadOnly = true;
        //            save_file_directory_path_textbox.IsReadOnly = true;
        //            edit_button.IsEnabled = true;
        //            edit_button.Visibility = Visibility.Visible;
        //            cancel_edit_button.IsEnabled = false;
        //            save_changes_button.IsEnabled = false;
        //            cancel_edit_button.Visibility = Visibility.Hidden;
        //            save_changes_button.Visibility = Visibility.Hidden;
        //            save_store_decision_radarbox1.IsEnabled = false;
        //            save_store_decision_radarbox2.IsEnabled = false;
        //        }
        //    }

        //    private void Save_changes_button_Click(object sender, RoutedEventArgs e)
        //    {
        //        //Make changes to game selected from list
        //        try
        //        {
        //            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "games_list.xml"))
        //            {
        //                XDocument xdoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "games_list.xml", LoadOptions.SetBaseUri);

        //                using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "games_list.xml"/*xdoc.BaseUri*/, FileMode.Open, FileAccess.ReadWrite))
        //                {
        //                    Game temp_game_item = (Game)games_listbox.SelectedItem;
        //                    //xdoc.Element("Games_list").Element("Games").Add(new XElement("Game",
        //                    //new XElement("Id", " "), new XElement("Name", new_game_name_textbox.Text), new XElement("Save_file_location", ""), new XElement("Store_profile_saves_in_app_location", "1")));
        //                    var query = (from item in xdoc.Element("Games_list").Element("Games").Elements("Game")
        //                                where (string) item.Element("Name").Value == temp_game_item.Game_name //create field containing temporary game name or use Game fields' values
        //                                            select item).First();
        //                    query.Element("Name").Value = game_name_textbox.Text;
        //                    query.Element("Save_file_location").Value = save_file_directory_path_textbox.Text;
        //                    if (save_store_decision_radarbox1.IsChecked == true)
        //                    {
        //                        query.Element("Store_profile_saves_in_app_location").Value = "1";
        //                    }
        //                    else
        //                    {
        //                        query.Element("Store_profile_saves_in_app_location").Value = "2";
        //                    }

        //                    xdoc.Save(fs);
        //                    fs.Close();
        //                }
        //            }
        //            else
        //            {
        //                //Warning
        //            }
        //        }
        //        catch (FileNotFoundException ex)
        //        {

        //        }
        //        catch (FileLoadException ex)
        //        {

        //        }
        //        catch (Exception)
        //        {

        //        }

        //        //End edit mode
        //        Toggle_Edit_Mode(false);
        //        Load_Selected_Games_Data((Game) games_listbox.SelectedItem);
        //    }

        //    private void Cancel_edit_button_Click(object sender, RoutedEventArgs e)
        //    {
        //        //End edit mode and rollback changes
        //        Toggle_Edit_Mode(false);
        //    }

        //    private void Games_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //    {
        //        /*Game temp_obj = (Game)games_listbox.SelectedItem;

        //        game_name_textbox.Text = temp_obj.game_name;
        //        save_file_directory_path_textbox.Text = temp_obj.save_file_location;

        //        if (temp_obj.profile_specific_save_file_storage_method == 1)
        //        {
        //            save_store_decision_radarbox1.IsChecked = true;
        //        }
        //        else if (temp_obj.profile_specific_save_file_storage_method == 2)
        //        {
        //            save_store_decision_radarbox2.IsChecked = true;
        //        }
        //        else
        //        {
        //            save_store_decision_radarbox1.IsChecked = true;
        //        }*/
        //        if(games_listbox.Items.Count>0)
        //        Load_Selected_Games_Data((Game) games_listbox.SelectedItem);
        //    }

        //    private void Load_Selected_Games_Data(Game temp_obj)
        //    {
        //        //Game temp_obj = (Game)games_listbox.SelectedItem;
        //        try
        //        {
        //            game_name_textbox.Text = temp_obj.Game_name;
        //            save_file_directory_path_textbox.Text = temp_obj.Save_file_location;

        //            if (temp_obj.Profile_specific_save_file_storage_method == 1)
        //            {
        //                save_store_decision_radarbox1.IsChecked = true;
        //            }
        //            else if (temp_obj.Profile_specific_save_file_storage_method == 2)
        //            {
        //                save_store_decision_radarbox2.IsChecked = true;
        //            }
        //            else
        //            {
        //                save_store_decision_radarbox1.IsChecked = true;
        //            }
        //        }
        //        catch(Exception ex)
        //        {
        //            MessageBox.Show("Failed to load selected game's data! Error occured.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }

        //    private void Add_new_game_button_Click(object sender, RoutedEventArgs e)
        //    {

        //        Add_New_Game();
        //        new_game_name_textbox.Text = "";
        //        Load_Games_List();
        //    }
    }

    /*class Game
    {
        public string game_name { get; set; }
        public string save_file_location { get; set; }
        public int profile_specific_save_file_storage_method { get; set; }
    }*/
}
