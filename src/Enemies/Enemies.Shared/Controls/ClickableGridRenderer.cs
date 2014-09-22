[assembly: Jv.Games.Xna.XForms.ExportRenderer(typeof(Enemies.Controls.ClickableGrid), typeof(Enemies.Controls.ClickableGridRenderer))]
namespace Enemies.Controls
{
    using Jv.Games.Xna.XForms.Renderers;
    using Microsoft.Xna.Framework.Input;
    using Xamarin.Forms;

    class ClickableGridRenderer : VisualElementRenderer<ClickableGrid>, IClickableRenderer
    {
        bool _clicked;

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var area = this.GetArea();

            var mouse = Mouse.GetState();
            if(area.Contains(new Point(mouse.X, mouse.Y)))
            {
                if(mouse.LeftButton == ButtonState.Pressed)
                {
                    if(!_clicked)
                        Model.FireClick();
                    _clicked = true;
                }
                else _clicked = false;
            }

            base.Update(gameTime);
        }
    }
}
