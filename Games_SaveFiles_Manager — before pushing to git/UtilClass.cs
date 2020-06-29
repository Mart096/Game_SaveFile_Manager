using System;

namespace Games_SaveFiles_Manager
{
    static internal class UtilClass
    {
        #region methods
        /// <summary>
        /// Returns path to configuration file.
        /// </summary>
        /// <returns></returns>
        static internal string GetConfigFilePath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "game_save_file_manager_config.xml";
        }

        /// <summary>
        /// Returns subdirectory containing directories of profiles INSIDE game save files' directory, or directory of a profile if the value of 'profile_name' parameter has been specified.
        /// </summary>
        /// <param name="save_file_path">Path to directory containing game save files.</param>
        /// <param name="game_name">Name of the game</param>
        /// <returns></returns>
        static internal string GetManagerProfilesDirectoryOfAGame(string save_file_path, string game_name)
        {
            return save_file_path + "\\..\\" + game_name + "_managerprofiles\\";
        }

        /// <summary>
        /// Returns subdirectory containing directories of profiles INSIDE game save files' directory, or the path of directory of a profile if the value of 'profile_name' parameter has been specified.
        /// </summary>
        /// <param name="save_file_path"></param>
        /// <param name="game_name"></param>
        /// <param name="profile_name"></param>
        /// <returns></returns>
        static internal string GetManagerProfilesDirectoryOfAGame(string save_file_path, string game_name, string profile_name)
        {
            return save_file_path + "\\..\\" + game_name + "_managerprofiles\\"+profile_name+"\\";
        }

        /// <summary>
        /// Returns path to directory with profiles of a specific game, stored inside game save files manager directory
        /// </summary>
        /// <param name="game_name"></param>
        /// <returns></returns>
        static internal string GetManagerProfilesDirectoryOfAGameInManagerDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "\\games\\";
        }

        /// <summary>
        /// Returns path to directory with profiles of a specific game, stored inside game save files manager directory
        /// </summary>
        /// <param name="game_name"></param>
        /// <returns></returns>
        static internal string GetManagerProfilesDirectoryOfAGameInManagerDirectory(string game_name)
        {
            return AppDomain.CurrentDomain.BaseDirectory + "\\games\\" + game_name + "\\";
        }

        /// <summary>
        /// Returns path to directory of a profile for a specific game, stored inside game save files manager directory
        /// </summary>
        /// <param name="game_name"></param>
        /// <param name="profile_name"></param>
        /// <returns></returns>
        static internal string GetManagerProfilesDirectoryOfAGameInManagerDirectory(string game_name, string profile_name)
        {
            return AppDomain.CurrentDomain.BaseDirectory + "\\games\\" + game_name + "\\" + profile_name + "\\";
        }
        #endregion


    }
}
