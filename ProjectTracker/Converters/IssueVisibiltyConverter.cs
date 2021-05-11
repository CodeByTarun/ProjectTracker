using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace ProjectTracker.Converters
{
    public class IssueVisibiltyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<Tag> Tags = (ObservableCollection<Tag>)values[3];

            Tag tag = (Tag)values[4];

            if (tag == null && values[2] == null)
            {
                return true;
            }

            else if(tag == null)
            {
                return ((values[0] as string).ToLower().Contains(values[2] as string) || (values[1] as string).ToLower().Contains(values[2] as string));
            }

            else if(values[2] == null)
            {
                return Tags.Any(t => t.Id == tag.Id);
            }
            else
            {
                return (Tags.Any(t => t.Id == tag.Id) && ((values[0] as string).ToLower().Contains(values[2] as string) || (values[1] as string).ToLower().Contains(values[2] as string)));
            }           
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
