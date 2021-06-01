using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace ProjectTracker.Converters
{
    public class KanbanIsHitTestVisibleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null)
            {
                return true;
            }
            else if ((int?)values[0] == (int?)values[1])
            {
                return true;
            }
            else
            {
                return true;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
