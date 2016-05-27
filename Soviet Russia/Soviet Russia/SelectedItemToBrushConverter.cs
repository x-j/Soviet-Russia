using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Soviet_Russia {
    class SelectedItemToBrushConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

            var isChecked = (bool) parameter;

            if (isChecked) {

                if (value == null) {
                    return Binding.DoNothing;
                } else {
                    SolidColorBrush b = (SolidColorBrush)new BrushConverter().ConvertFromString(value.ToString().Split(' ')[1]);
                    return b;
                }
            } else return Binding.DoNothing;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
