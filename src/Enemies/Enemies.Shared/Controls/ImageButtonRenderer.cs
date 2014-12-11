[assembly: Jv.Games.Xna.XForms.ExportRenderer(typeof(Enemies.Controls.ImageButton), typeof(Enemies.Controls.ImageButtonRenderer))]
namespace Enemies.Controls
{
    using Jv.Games.Xna.XForms;
    using Jv.Games.Xna.XForms.Renderers;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;

    public class ImageButtonRenderer : LabelRenderer, IClickableRenderer
    {
        Texture2D _image;
        NinePatch _image9;
        bool? _mouseState;

        public new ImageButton Model { get { return (ImageButton)base.Model; } }

        public ImageButtonRenderer()
        {
            PropertyTracker.AddHandler(ImageButton.ImageProperty, HandleImage);
        }

        void HandleImage(Xamarin.Forms.BindableProperty prop)
        {
            if (Model.Image == null)
            {
                _image = null;
                _image9 = null;
            }
            else
            {
                _image = Forms.Game.Content.Load<Texture2D>(Model.Image);
                if (Model.Image.EndsWith(".9"))
                    _image9 = new NinePatch(_image);
            }

            InvalidateMeasure();
        }

        public override Xamarin.Forms.SizeRequest Measure(Xamarin.Forms.Size availableSize)
        {
            var lblSize = base.Measure(availableSize);
            if (_image9 != null)
                return new Xamarin.Forms.SizeRequest(new Xamarin.Forms.Size(_image9.Width, _image9.Height), new Xamarin.Forms.Size(_image9.Stretch.Horizontal.Margin, _image9.Stretch.Vertical.Margin));

            if (_image != null)
                return new Xamarin.Forms.SizeRequest(new Xamarin.Forms.Size(_image.Width, _image.Height), default(Xamarin.Forms.Size));

            return lblSize;
        }

        public override void Update(GameTime gameTime)
        {
            try
            {
                var mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
                var region = this.GetArea();
                if (region.Contains(new Xamarin.Forms.Point(mouse.X, mouse.Y)))
                {
                    if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _mouseState == false)
                    {
                        if (Model.State != ImageButtonState.Pressed &&
                            Model.State != ImageButtonState.Pressing)
                        {
                            Model.FireClicked();
                            Model.State = ImageButtonState.Pressed;
                        }
                        else
                        {
                            Model.State = ImageButtonState.Pressing;
                            if (Model.ContinuousClick)
                                Model.FireClicked();
                        }
                    }
                    else
                    {
                        Model.State = ImageButtonState.Over;
                    }
                }
                else
                {
                    Model.State = ImageButtonState.Normal;
                }

                _mouseState = mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
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
            if(_image9 != null)
                _image9.Draw(SpriteBatch, drawArea, new Color(Color.White, Model.ImageOpacity));
            else if (_image != null)
                SpriteBatch.Draw(_image, drawArea, new Color(Color.White, Model.ImageOpacity));
            base.LocalDraw(gameTime);
        }

        public override void Disappeared()
        {
            _mouseState = null;
        }
    }
}

