using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Games_SaveFiles_Manager
{
    class IntToSaveMethodConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int tempvalue = (int)value;
            string outputval = "Undefined";

            switch(tempvalue)
            {
                case 1:
                    outputval = "Original save file path";
                    break;
                case 2:
                    outputval = "Manager's directory";
                    break;
                default:
                    outputval = "UNDEFINED";
                    break;
            }

            return outputval;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string inputval = (string)value;
            //int outputval = 0;

            if (inputval.Equals("Original save file path"))
                return 1;
            else if (inputval.Equals("Manager's directory"))
                return 2;
            else
                return 0;

        }
    }
}
