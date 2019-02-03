using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using Microsoft.Xna.Framework.Content;

namespace MorpheusInTheUnderworld.Screens
{
    /// <summary>
    /// This abstract class is intended to be used by Game Screens
    /// Also this class does many things for the user
    /// </summary>
    public abstract class GameScreen : Screen
    {
        private readonly IServiceProvider _serviceProvider;
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Each GameScreen is indivually having their own ContentManager
        /// Is user responsibility to Unload its content and Dispose.
        /// </summary>
        public ContentManager Content;

        public GraphicsDevice GraphicsDevice;

        protected GameScreen(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override void Initialize()
        {
            base.Initialize();
            var graphicsDeviceService = (IGraphicsDeviceService)_serviceProvider.GetService(typeof(IGraphicsDeviceService));
            GraphicsDevice = graphicsDeviceService.GraphicsDevice;
            Content = new ContentManager(_serviceProvider, "Content");
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
            Content.Unload();
            Content.Dispose();
        }
        public override void Dispose()
        {
            base.Dispose();
            spriteBatch.Dispose();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
