using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Screens;
using MorpheusInTheUnderworld.Classes;


namespace MorpheusInTheUnderworld.Screens
{
    /// <summary>
    /// This abstract class is used among all the MenuScreen.
    /// </summary>
    public abstract class MenuScreen : Screen
    {
        private readonly IServiceProvider _serviceProvider;
        private SpriteBatch _spriteBatch;

        protected MenuScreen(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            MenuItems = new List<MenuItem>();
        }

        public List<MenuItem> MenuItems { get; set; }
        protected BitmapFont Font { get; private set; }
        /// <summary>
        /// Providing a ContentManager for each MenuScreen
        /// </summary>
        protected ContentManager Content { get; private set; }
        /// <summary>
        /// Providing a GraphicsDevice for each MenuScreen
        /// </summary>
        protected GraphicsDevice GraphicsDevice { get; set; }

        protected void AddMenuItem(string text, Action action)
        {
            var menuItem = new MenuItem(Font, text)
            {
                Position = new Vector2(400, 200 + 32 * MenuItems.Count),
                Action = action
            };

            MenuItems.Add(menuItem);
        }

        public override void Initialize()
        {
            base.Initialize();

            Content = new ContentManager(_serviceProvider, "Content");
        }

        public override void Dispose()
        {
            base.Dispose();

            _spriteBatch.Dispose();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            var graphicsDeviceService = (IGraphicsDeviceService)_serviceProvider.GetService(typeof(IGraphicsDeviceService));
            GraphicsDevice = graphicsDeviceService.GraphicsDevice;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Font = Content.Load<BitmapFont>("Fonts/fixedsys");
        }

        public override void UnloadContent()
        {
            Content.Unload();
            Content.Dispose();

            base.UnloadContent();
        }

        private MouseState _previousState;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var mouseState = Mouse.GetState();
            var isPressed = mouseState.LeftButton == ButtonState.Released && _previousState.LeftButton == ButtonState.Pressed;

            foreach (var menuItem in MenuItems)
            {
                var isHovered = menuItem.BoundingRectangle.Contains(new Point2(mouseState.X, mouseState.Y));
                
                menuItem.Color = isHovered ? Color.Yellow : Color.White;

                if (isHovered && isPressed)
                {
                    if (menuItem.Action != null)
                        menuItem.Action.Invoke();
                    break;
                }
            }

            _previousState = mouseState;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _spriteBatch.Begin();

            foreach (var menuItem in MenuItems)
                menuItem.Draw(_spriteBatch);

            _spriteBatch.End();
        }
    }
}