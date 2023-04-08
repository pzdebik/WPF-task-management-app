using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Linq;
using ClassLibrary.Model;

namespace ToDoWpfApp
{
    public class BoolToBrushConverter : IValueConverter
    {
        public Brush FalseColor { get; set; } = Brushes.Black; // kolor dla zadania niezrealizowanego
        public Brush TrueColor { get; set; } = Brushes.Gray; // kolor dla zadania zrealizowanego
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = (bool)value;
            return !b ? FalseColor : TrueColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TaskConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string name = (string)values[0];
            DateTime? date = (DateTime?)values[1];
            Priority priority = (Priority)(int)values[2];
            Guid id = Guid.NewGuid();


            if (!string.IsNullOrEmpty(name) && date.HasValue)
            {
                return new ViewModel.TaskViewModel(id, name, date.Value, priority, false);
            }
            else return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
