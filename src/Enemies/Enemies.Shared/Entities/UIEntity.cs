using Jv.Games.Xna.XForms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Enemies.Entities
{
    class UIEntity : IEntity
    {
        public readonly IControlRenderer ControlRenderer;
        public Xamarin.Forms.Point Position;
        public Xamarin.Forms.Size? Size;

        public UIEntity(IControlRenderer controlRenderer)
        {
            ControlRenderer = controlRenderer;
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ControlRenderer.Update(gameTime);
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.GameTime gameTime)
        {
            var anySize = new Xamarin.Forms.Size(double.PositiveInfinity, double.PositiveInfinity);

            ControlRenderer.Measure(Size ?? anySize);
            if (ControlRenderer.MeasuredSize.Width > 0 && ControlRenderer.MeasuredSize.Height > 0)
            {
                ControlRenderer.Arrange(new Xamarin.Forms.Rectangle(
                    Position,
                    Size ?? ControlRenderer.MeasuredSize));

                ControlRenderer.Draw(spriteBatch, gameTime);
            }
        }
    }

    class UIEntity<TModel> : UIEntity
        where TModel : Xamarin.Forms.View
    {
        public UIEntity(IControlRenderer controlRenderer)
            : base(controlRenderer)
        {
        }

        public TModel Model { get { return (TModel)ControlRenderer.Model; } }
    }

    static class UIEntityExtensions
    {
        public static UIEntity<TModel> AsEntity<TModel>(this TModel view)
            where TModel : Xamarin.Forms.View
        {
            return new UIEntity<TModel>(RendererFactory.Create(view));
        }
    }
}
