using System;

[assembly: Jv.Games.Xna.XForms.ExportRenderer(typeof(Enemies.Controls.ImageButton), typeof(Enemies.Controls.ImageButtonRenderer))]
namespace Enemies.Controls
{
    using Jv.Games.Xna.XForms;
    using Jv.Games.Xna.XForms.Renderers;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class ImageButtonRenderer : LabelRenderer
    {
        Texture2D _currentImage;
        Texture2D _imageNormal;
        Texture2D _imageOver;

        public new ImageButton Model { get { return (ImageButton)base.Model; } }

        public ImageButtonRenderer()
        {
            PropertyTracker.AddHandler(ImageButton.ImageNormalProperty, HandleImageNormal);
            PropertyTracker.AddHandler(ImageButton.ImageOverProperty, HandleImageOver);
        }

        void HandleImageNormal(Xamarin.Forms.BindableProperty prop)
        {
            _imageNormal = Forms.Game.Content.Load<Texture2D>(Model.ImageNormal);
            UseImage(_currentImage ?? _imageNormal ?? _imageOver);
        }

        void HandleImageOver(Xamarin.Forms.BindableProperty prop)
        {
            _imageOver = Forms.Game.Content.Load<Texture2D>(Model.ImageOver);
            UseImage(_currentImage ?? _imageNormal ?? _imageOver);
        }

        public override Xamarin.Forms.SizeRequest Measure(Xamarin.Forms.Size availableSize)
        {
            var lblSize = base.Measure(availableSize);
            if (_currentImage != null)
                return new Xamarin.Forms.SizeRequest(new Xamarin.Forms.Size(_currentImage.Width, _currentImage.Height), default(Xamarin.Forms.Size));
            return lblSize;
        }

        protected override void Arrange()
        {
            base.Arrange();
        }

        public override void Update(GameTime gameTime)
        {
            try
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
            }
            catch (InvalidOperationException invalidOpEx)
            {
                System.Console.WriteLine(invalidOpEx.Message);
            }

            base.Update(gameTime);
        }

        protected override void LocalDraw(GameTime gameTime)
        {
            if (_currentImage == null)
                return;

            var drawArea = new Rectangle(0, 0, (int)Model.Bounds.Width, (int)Model.Bounds.Height);
            SpriteBatch.Draw(_currentImage, drawArea, Color.White);
            base.LocalDraw(gameTime);
        }

        void UseImage(Texture2D image)
        {
            if (_currentImage == image)
                return;
            _currentImage = image;
            if (_currentImage == null)
                return;
            InvalidateMeasure();
        }

        Xamarin.Forms.Rectangle GetArea()
        {
            var pos = new Xamarin.Forms.Point();
            var current = (Jv.Games.Xna.XForms.IRenderer)this;
            while (current != null)
            {
                var visual = current as VisualElementRenderer;
                if (visual != null)
                {
                    pos = new Xamarin.Forms.Point(
                        pos.X + visual.Model.Bounds.X + visual.Model.TranslationX,
                        pos.Y + visual.Model.Bounds.Y + visual.Model.TranslationY);
                }

                current = current.Parent;
            }
            return new Xamarin.Forms.Rectangle(pos, Model.Bounds.Size);
        }
    }
}

