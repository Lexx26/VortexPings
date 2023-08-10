using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UIWPF.Controls.Converters
{
    class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            var strVal = value.ToString();

            var isInt = int.TryParse(strVal, out int result);

            if(isInt)
            {
                return value;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var strVal = value.ToString();

            var isInt = int.TryParse(strVal, out int result);

            if (isInt)
            {
                return value;
            }

            

            return GetNumber(strVal);
        }

        private int? GetNumber(string str)
        {
            string resultStr = null;

            foreach (var charC in str)
            {
                if(int.TryParse(charC.ToString(),out int intVal))
                {
                    resultStr = resultStr + intVal.ToString();
                }
                else
                {
                    break;
                }
            }

            if (resultStr != null)
                return int.Parse(resultStr);

            return null;
        }
    }
}
