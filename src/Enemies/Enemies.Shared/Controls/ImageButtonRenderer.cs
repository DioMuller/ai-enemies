[assembly: Jv.Games.Xna.XForms.ExportRenderer(typeof(Enemies.Controls.ImageButton), typeof(Enemies.Controls.ImageButtonRenderer))]
namespace Enemies.Controls
{
    using Jv.Games.Xna.XForms.Renderers;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class ImageButtonRenderer : LabelRenderer
    {
        ContentManager _content;
        Texture2D _currentImage;
        Texture2D _imageNormal;
        Texture2D _imageOver;

        public new ImageButton Model { get { return (ImageButton)base.Model; } }

        public override void Initialize(Game game)
        {
            base.Initialize(game);
            _content = game.Content;
            HandleProperty(ImageButton.ImageNormalProperty, HandleImageNormal);
            HandleProperty(ImageButton.ImageOverProperty, HandleImageOver);
        }

        protected virtual bool HandleImageNormal(Xamarin.Forms.BindableProperty prop)
        {
            _imageNormal = _content.Load<Texture2D>(Model.ImageNormal);
            return true;
        }

        protected virtual bool HandleImageOver(Xamarin.Forms.BindableProperty prop)
        {
            _imageOver = _content.Load<Texture2D>(Model.ImageOver);
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            var mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
            var region = GetArea();
            if (region.Contains(new Xamarin.Forms.Point(mouse.X, mouse.Y)))
            {
                UseImage(_imageOver);

                if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    Model.FireClicked();
            }
            else
            {
                UseImage(_imageNormal);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (_currentImage == null)
                return;

            var drawArea = new Rectangle(0, 0, (int)RenderArea.Width, (int)RenderArea.Height);
            spriteBatch.Draw(_currentImage, drawArea, Color.White);
            base.Draw(spriteBatch, gameTime);
        }

        void UseImage(Texture2D image)
        {
            if (_currentImage == image)
                return;
            _currentImage = image;
            if (_currentImage == null)
                return;
            Model.WidthRequest = _currentImage.Width;
            Model.HeightRequest = _currentImage.Height;
            InvalidateMeasure();
        }

        Xamarin.Forms.Rectangle GetArea()
        {
            var pos = new Xamarin.Forms.Point();
            var current = (Jv.Games.Xna.XForms.IControlRenderer)this;
            while (current != null)
            {
                pos = new Xamarin.Forms.Point(
                    pos.X + current.RenderArea.Location.X,
                    pos.Y + current.RenderArea.Location.Y);

                var visual = current as VisualElementRenderer;
                if (visual != null)
                {
                    pos = new Xamarin.Forms.Point(
                        pos.X + visual.Model.TranslationX,
                        pos.Y + visual.Model.TranslationY);
                }

                current = current.Parent;
            }
            return new Xamarin.Forms.Rectangle(pos, RenderArea.Size);
        }
    }
}

