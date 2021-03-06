﻿using System.Windows;
using MahApps.Metro.Controls;

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
            //Load_Games_List();
        }

        /*private void Load_Games_List() //this method should be availabale in the main window
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

                        games_listbox.Items.Add(gi_ob);
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
        }*/

        private void Menu_about_button_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about_window = new AboutWindow();
            about_window.ShowDialog();
        }

        private void Menu_quit_button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Manage_Games_List_Click(object sender, RoutedEventArgs e)
        {
            GamesListManager games_list_manager = new GamesListManager();
            games_list_manager.ShowDialog();
            //System.Diagnostics.Process.Start(Application.ResourceAssembly.Location); //not very good solution. Need to find another way to force refreshing the data in listboxes
            //Application.Current.Shutdown();
        }

        private void Manage_Profiles_List_Click(object sender, RoutedEventArgs e)
        {
            ProfileManagerWindow profile_manager = new ProfileManagerWindow();
            profile_manager.ShowDialog();
            //System.Diagnostics.Process.Start(Application.ResourceAssembly.Location); //not very good solution. Need to find another way to force refreshing the data in listboxes
            //Application.Current.Shutdown();
        }
    }

}
