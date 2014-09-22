using System;
using Xamarin.Forms;

namespace Enemies.Controls
{
    public class ClickableGrid : Grid
    {
        public event EventHandler OnClick;

        public void FireClick()
        {
            if (OnClick != null)
                OnClick(this, EventArgs.Empty);
        }
    }
}
