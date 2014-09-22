using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Enemies.Controls
{
    public class ButtonTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch((ImageButtonState)value)
            {
                case ImageButtonState.Pressed:
                case ImageButtonState.Pressing: return Xamarin.Forms.Color.White;
                default: return Xamarin.Forms.Color.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
