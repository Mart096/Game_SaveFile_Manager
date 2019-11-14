using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games_SaveFiles_Manager.Models
{
    public class Game
    {
        #region Fields
        private string game_name;
        private string save_file_location;
        private int profile_specific_save_file_storage_method;
        #endregion

        #region Fields
        public string Game_name { get => game_name; set => game_name = value; }
        public string Save_file_location { get => save_file_location; set => save_file_location = value; }
        public int Profile_specific_save_file_storage_method { get => profile_specific_save_file_storage_method; set => profile_specific_save_file_storage_method = value; }
        #endregion

        #region Constructors
        public Game()
        {
            Game_name = "Undefined";
            Save_file_location = "";
            profile_specific_save_file_storage_method = -1;
        }

        public Game(string new_game_name, string new_game_save_file_location, int new_game_save_file_storage_method)
        {
            Game_name = new_game_name;
            Save_file_location = new_game_save_file_location;
            Profile_specific_save_file_storage_method = new_game_save_file_storage_method;
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
