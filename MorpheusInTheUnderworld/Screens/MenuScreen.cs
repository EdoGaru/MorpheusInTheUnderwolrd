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
    public abstract class MenuScreen : GameScreen
    {
        // MenuScreen provides child classes a spritebatch.
        public SpriteBatch spriteBatch;

        public List<MenuItem> MenuItems { get; set; }

        // MenuScreen also provides child classes a Font.
        protected BitmapFont Font { get; private set; }

        // Our ContentManager for MenuScreens;
        protected ContentManager mainMenuContent { get; set; }

        public  Viewport Viewport { get; set; }

        private Texture2D flatNightBg;

        protected MenuScreen(Game game) : base(game)
        {
            MenuItems = new List<MenuItem>();   

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Dispose()
        {
            base.Dispose();

            spriteBatch.Dispose();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            mainMenuContent = new ContentManager(Game.Services, "Content");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Viewport = GraphicsDevice.Viewport;

            Font = mainMenuContent.Load<BitmapFont>("Fonts/fixedsys");
            flatNightBg = mainMenuContent.Load<Texture2D>("Graphics/Flat Night 4 BG");
        }
        
        /// <summary>
        /// This method no need to be overrided in child classes
        /// </summary>
        public override void UnloadContent()
        {
            mainMenuContent.Unload();
            mainMenuContent.Dispose();

            base.UnloadContent();
        }
        
        private MouseState _previousState;

        public override void Update(GameTime gameTime)
        {
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

            spriteBatch.Begin();

            //spriteBatch.Draw(flatNightBg, new Rectangle(0, 0, Viewport.Width, Viewport.Height), Color.White);
            foreach (var menuItem in MenuItems)
                menuItem.Draw(spriteBatch);

            spriteBatch.End();
        }

        protected void AddMenuItem(string text, Action action)
        {
            var menuItem = new MenuItem(Font, text)
            {
                Position = new Vector2(400, 200 + 64 * MenuItems.Count),
                Action = action
            };

            MenuItems.Add(menuItem);

        }

    }
}