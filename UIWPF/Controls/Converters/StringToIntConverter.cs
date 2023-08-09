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
            var strVal = value.ToString();

            var isInt = int.TryParse(strVal, out int result);

            if (isInt)
            {
                return value;
            }

            return null;
        }
    }
}
