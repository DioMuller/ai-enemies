namespace Enemies.Controls
{
    using System;
    using Xamarin.Forms;

    public enum ButtonState
    {
        Normal,
        Over,
        Pressed,
        Pressing
    }

    public class ImageButton : Label
    {
        public static BindableProperty StateProperty = BindableProperty.Create<ImageButton, ButtonState>(p => p.State, defaultValue: ButtonState.Normal);
        public static BindableProperty ImageProperty = BindableProperty.Create<ImageButton, string>(p => p.Image, defaultValue: null);
        public static BindableProperty ContinuousClickProperty = BindableProperty.Create<ImageButton, bool>(p => p.ContinuousClick, defaultValue: false);

        public ButtonState State
        {
            get { return (ButtonState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public bool ContinuousClick
        {
            get { return (bool)GetValue(ContinuousClickProperty); }
            set { SetValue(ContinuousClickProperty, value); }
        }

        public ImageButton()
        {
            XAlign = TextAlignment.Center;
            YAlign = TextAlignment.Center;
            BindingContext = this;
        }

        public event EventHandler OnClick;

        public void FireClicked()
        {
            if (OnClick != null)
                OnClick(this, EventArgs.Empty);
        }
    }
}

