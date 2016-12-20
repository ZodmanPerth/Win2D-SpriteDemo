using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace SpriteDemo
{
    public class ValueConverterSelectedItemToFormulaBackground : IValueConverter
    {
        SolidColorBrush _transparent;
        SolidColorBrush _colour;

        public ValueConverterSelectedItemToFormulaBackground()
        {
            _transparent = new SolidColorBrush(Colors.Transparent);
            _colour = Application.Current.Resources["SystemControlHighlightListAccentLowBrush"] as SolidColorBrush;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Equals((value as FrameworkElement)?.Tag, parameter)
                ? _colour
                : _transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
