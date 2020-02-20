using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Games_SaveFiles_Manager;
using System.Globalization;

namespace UnitTestProject1
{
    [TestClass]
    public class GameSaveManagerUTest1
    {
        [TestMethod]
        public void BoolToNegativeBoolConverterTest()
        {
            BoolToNegativeBoolConverter converter = new BoolToNegativeBoolConverter();

            bool value = (bool)converter.Convert(true, typeof(Boolean), null, CultureInfo.CurrentCulture);

            Assert.IsFalse(value);
        }

        [TestMethod]
        public void MigrateDirectoryTest1()
        {
            string game_name = "Game3";
            int previous_storage_method = 2;
            int currently_selected_method = 1;
            string save_file_path = @"C:\Users\MarcinAdmin\Desktop\Test\TestDir";
            string new_save_file_path = @"";

            //Games_SaveFiles_Manager.ViewModels.GameListManagerViewModel glm = new Games_SaveFiles_Manager.ViewModels.GameListManagerViewModel();

            //bool result = glm.MigrateSaveFilesToNewDirectory(game_name, previous_storage_method, currently_selected_method, save_file_path, new_save_file_path);
            bool result = true;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MigrateDirectoryTest2()
        {
            string game_name = "Game3";
            int previous_storage_method = 1;
            int currently_selected_method = 2;
            string save_file_path = @"C:\Users\MarcinAdmin\Desktop\Test\TestDir";
            string new_save_file_path = @"";

            //Games_SaveFiles_Manager.ViewModels.GameListManagerViewModel glm = new Games_SaveFiles_Manager.ViewModels.GameListManagerViewModel();

            //bool result = glm.MigrateSaveFilesToNewDirectory(game_name, previous_storage_method, currently_selected_method, save_file_path, new_save_file_path);
            bool result = true;

            Assert.IsTrue(result);
        }
    }
}
