using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Enemies.Controls
{
    public class ButtonImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch((ImageButtonState)value)
            {
                case ImageButtonState.Normal: return "GUI/button_normal.9";
                case ImageButtonState.Over: return "GUI/button_over.9";
                case ImageButtonState.Pressed:
                case ImageButtonState.Pressing: return "GUI/button_pressed.9";
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
