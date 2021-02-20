using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace ProjectTracker.Converters
{
    public class TabWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double width = (double)values[3] - ((double)values[0] + (double)values[1] + (double)values[2]);
            double tabsWidth = (double)values[4] * (int)values[5];

            if (width > tabsWidth)
            {
                return tabsWidth;
            } else
            {
                return width;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
