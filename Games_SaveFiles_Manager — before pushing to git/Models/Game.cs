using System.Collections.Generic;

namespace Games_SaveFiles_Manager.Models
{
    public class Game
    {
        #region Fields
        private string _game_name;
        private string _save_file_location;
        private int _profile_specific_save_file_storage_method;
        private string _profile_used;
        private bool _manageSelectedFilesOnly;
        private List<string> _filesToManage = new List<string>();
        #endregion

        #region Fields
        public string Game_name { get => _game_name; set => _game_name = value; }
        public string Save_file_location { get => _save_file_location; set => _save_file_location = value; }
        public int Profile_specific_save_file_storage_method { get => _profile_specific_save_file_storage_method; set => _profile_specific_save_file_storage_method = value; }
        public string Profile_used { get => _profile_used; set => _profile_used = value; }
        public bool ManageSelectedFilesOnly { get { return _manageSelectedFilesOnly; } set => _manageSelectedFilesOnly = value; }
        public List<string> FilesToManage { get { return _filesToManage; } set => _filesToManage = value; } // = new List<string>();

        #endregion

        #region Constructors
        public Game() : this ("Undefined", "", -1) {}

        public Game(Game game)
        {
            if (game != null)
            {
                Game_name = game.Game_name;
                Save_file_location = game.Save_file_location;
                Profile_specific_save_file_storage_method = game.Profile_specific_save_file_storage_method;
                Profile_used = "default";
                ManageSelectedFilesOnly = false;
            }
        }

        public Game(string new_game_name, string new_game_save_file_location, int new_game_save_file_storage_method)
        {
            Game_name = new_game_name;
            Save_file_location = new_game_save_file_location;
            Profile_specific_save_file_storage_method = new_game_save_file_storage_method;
            Profile_used = "default";
            ManageSelectedFilesOnly = false;
        }
        #endregion

        #region Methods

        //public override string ToString()
        //{
        //    return Game_name.ToString();
        //}
        #endregion
    }
}
