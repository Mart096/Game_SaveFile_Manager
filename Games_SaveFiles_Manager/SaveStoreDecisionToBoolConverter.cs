using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Games_SaveFiles_Manager
{
    class SaveStoreDecisionToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            /*string temp_val = (string)value;
            if (temp_val.Equals(""))
            {
                return true;
            }
            else
            {
                return false;
            }*/

            int temp_val = (int)value;
            string temp_param = (string)parameter;

            if (temp_val == 1 && temp_param.Equals("appdir"))
            {
                return true;
            }
            else if (temp_val == 2 && temp_param.Equals("appdir"))
            {
                return false;
            }
            else if (temp_val == 1 && temp_param.Equals("local"))
            {
                return false;
            }
            else if (temp_val == 2 && temp_param.Equals("local"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool temp_val = (bool)value;
            string temp_param = (string)parameter;

            if (temp_val == true && temp_param.Equals("appdir"))
            {
                return 1;
            }
            else if (temp_val == false && temp_param.Equals("appdir"))
            {
                return 2;
            }
            else if (temp_val == false && temp_param.Equals("local"))
            {
                return 2;
                //1?
            }
            else if (temp_val == true && temp_param.Equals("local"))
            {
                return 2;
            }
            else
            {
                return 2;
            }
        }
    }
}
