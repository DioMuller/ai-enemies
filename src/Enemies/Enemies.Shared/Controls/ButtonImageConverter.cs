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
            switch((ButtonState)value)
            {
                case ButtonState.Normal: return "GUI/button_normal";
                case ButtonState.Over: return "GUI/button_over";
                case ButtonState.Pressed:
                case ButtonState.Pressing: return "GUI/button_pressed";
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
