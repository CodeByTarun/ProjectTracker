using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace ProjectTracker.Converters
{
    class ColorToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] bytes = BitConverter.GetBytes((int)value);
            return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            Color mediaColor = (Color)value;

            System.Drawing.Color color = System.Drawing.Color.FromArgb(mediaColor.A, mediaColor.R, mediaColor.G, mediaColor.B);

            return color.ToArgb();
        }
    }
}
