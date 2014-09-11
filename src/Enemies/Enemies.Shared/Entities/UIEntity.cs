using Jv.Games.Xna.XForms;
using Microsoft.Xna.Framework;

namespace Enemies.Entities
{
    class UIEntity : IEntity
    {
        public readonly RendererGameComponent Component;

        Vector2 _position;
        Xamarin.Forms.Size? _size;

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                UpdateArrange();
            }
        }

        public Xamarin.Forms.Size? Size
        {
            get { return _size; }
            set
            {
                _size = value;
                UpdateArrange();
            }
        }

        public UIEntity(RendererGameComponent controlRenderer)
        {
            Component = controlRenderer;
            UpdateArrange();
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Component.Update(gameTime);
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.GameTime gameTime)
        {
            Component.Draw(gameTime);
        }

        private void UpdateArrange()
        {
            var measuredSize = Component.Page.GetSizeRequest(Size != null ? Size.Value.Width : -1, Size != null ? Size.Value.Height : -1);
            Component.Area = new Xamarin.Forms.Rectangle(Position.X, Position.Y, measuredSize.Request.Width, measuredSize.Request.Height);
        }
    }

    class UIEntity<TModel> : UIEntity
        where TModel : Xamarin.Forms.VisualElement
    {
        public UIEntity(RendererGameComponent controlRenderer, TModel model)
            : base(controlRenderer)
        {
            Model = model;
        }

        public TModel Model { get; private set; }
    }

    static class UIEntityExtensions
    {
        public static UIEntity<TModel> AsEntity<TModel>(this TModel view)
            where TModel : Xamarin.Forms.VisualElement
        {
            var v = view as Xamarin.Forms.View;
            if (v != null)
                return new UIEntity<TModel>(new Xamarin.Forms.ContentPage { Content = v }.AsGameComponent(), view);
            var p = view as Xamarin.Forms.Page;
            if (p != null)
                return new UIEntity<TModel>(p.AsGameComponent(), view);
            throw new System.ArgumentException("Not supported view type", "view");
        }
    }
}
