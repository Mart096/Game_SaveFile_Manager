using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games_SaveFiles_Manager.Models
{
    public class Profile
    {
        #region Fields
        private string profile_name;
        private DateTime creation_time; //will be used for identification of profiles
        #endregion

        #region Properties
        public string Profile_name { get => profile_name; set => profile_name = value; }
        public DateTime Creation_time { get => creation_time; set => creation_time = value; }
        #endregion

        #region Constructors
        public Profile()
        {
            Profile_name = "Unspecified";
            Creation_time = DateTime.Now;
        }

        public Profile(string new_name)
        {
            Profile_name = new_name;
            Creation_time = DateTime.Now;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return Profile_name.ToString();
        }
        #endregion
    }
}
