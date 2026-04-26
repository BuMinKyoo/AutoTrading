using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoGetMoney.Converter
{
    public class IsEnabledStopConverter : IValueConverter
    {
        // ViewModel -> View
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 입력된 값이 bool 형태인지 확인하고, 그에 따라  값을 반환합니다.
            if (value is bool boolValue)
            {
                return boolValue ? true : false;
            }

            return 0;
        }

        // View -> ViewModel
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
