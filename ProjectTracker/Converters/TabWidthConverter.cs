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
            double width = (double)values[2] - ((double)values[0] + (double)values[1]);

            double tabsWidth; 

            if (!(values[4] is int))
            {
                tabsWidth = 0;
            } else
            {
                tabsWidth = (double)values[3] * (int)values[4];
            }
            
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
