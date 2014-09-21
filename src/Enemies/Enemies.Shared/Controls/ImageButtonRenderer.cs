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
        Texture2D _image;

        public new ImageButton Model { get { return (ImageButton)base.Model; } }

        public ImageButtonRenderer()
        {
            PropertyTracker.AddHandler(ImageButton.ImageProperty, HandleImage);
        }

        void HandleImage(Xamarin.Forms.BindableProperty prop)
        {
            _image = Model.Image == null ? null : Forms.Game.Content.Load<Texture2D>(Model.Image);
            InvalidateMeasure();
        }

        public override Xamarin.Forms.SizeRequest Measure(Xamarin.Forms.Size availableSize)
        {
            var lblSize = base.Measure(availableSize);
            if (_image != null)
                return new Xamarin.Forms.SizeRequest(new Xamarin.Forms.Size(_image.Width, _image.Height), default(Xamarin.Forms.Size));
            return lblSize;
        }

        public override void Update(GameTime gameTime)
        {
            try
            {
                var mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
                var region = GetArea();
                if (region.Contains(new Xamarin.Forms.Point(mouse.X, mouse.Y)))
                {
                    if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (Model.State != ButtonState.Pressed &&
                            Model.State != ButtonState.Pressing)
                        {
                            Model.FireClicked();
                            Model.State = ButtonState.Pressed;
                        }
                        else
                        {
                            Model.State = ButtonState.Pressing;
                            if (Model.ContinuousClick)
                                Model.FireClicked();
                        }
                    }
                    else
                    {
                        Model.State = ButtonState.Over;
                    }
                }
                else
                {
                    Model.State = ButtonState.Normal;
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
            if (_image == null)
                return;

            var drawArea = new Rectangle(0, 0, (int)Model.Bounds.Width, (int)Model.Bounds.Height);
            SpriteBatch.Draw(_image, drawArea, new Color(Color.White, (float)Model.Opacity));
            base.LocalDraw(gameTime);
        }

        Xamarin.Forms.Rectangle GetArea()
        {
            var pos = new Xamarin.Forms.Point();
            var current = (Xamarin.Forms.VisualElement)Model;
            while (current != null)
            {
                pos = new Xamarin.Forms.Point(
                    pos.X + current.Bounds.X + current.TranslationX,
                    pos.Y + current.Bounds.Y + current.TranslationY);

                current = current.ParentView;
            }
            return new Xamarin.Forms.Rectangle(pos, Model.Bounds.Size);
        }

        protected override float GetAlpha()
        {
            return 1;
        }
    }
}

