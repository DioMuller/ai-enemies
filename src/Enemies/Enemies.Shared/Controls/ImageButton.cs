namespace Enemies.Controls
{
    using System;
    using Xamarin.Forms;

    public class ImageButton : Label
    {
        public static BindableProperty ImageNormalProperty = BindableProperty.Create<ImageButton, string>(p => p.ImageNormal, defaultValue: null);
        public string ImageNormal
        {
            get { return (string)GetValue(ImageNormalProperty); }
            set { SetValue(ImageNormalProperty, value); }
        }

        public static BindableProperty ImageOverProperty = BindableProperty.Create<ImageButton, string>(p => p.ImageOver, defaultValue: null);
        public string ImageOver
        {
            get { return (string)GetValue(ImageOverProperty); }
            set { SetValue(ImageOverProperty, value); }
        }

        public event EventHandler OnClick;

        public void FireClicked()
        {
            if (OnClick != null)
                OnClick(this, EventArgs.Empty);
        }
    }
}

