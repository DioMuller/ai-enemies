using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Enemies.Controls
{
    class GUI : StackLayout
    {
        public ImageButton TestButton;

        public GUI()
        {
            TestButton = new ImageButton {
                Text = "Teste",
                HorizontalOptions = Xamarin.Forms.LayoutOptions.CenterAndExpand,
                VerticalOptions = Xamarin.Forms.LayoutOptions.CenterAndExpand,
                ImageNormal = "GUI/cursor_closed", ImageOver = "GUI/cursor_pointer",
                YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Center
            };
            TestButton.OnClick += delegate
            {
                TestButton.RotationY = (TestButton.RotationY + 2) % 360;
            };
            Children.Add(TestButton);
        }
    }
}
