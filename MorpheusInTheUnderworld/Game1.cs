using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Screens;
using MorpheusInTheUnderworld.Screens;
using MorpheusInTheUnderworld.Classes;
using System.IO;
using MonoGame.Extended.Screens.Transitions;

namespace MorpheusInTheUnderworld
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // A FramePerSecondCounter used only in Debug Mode.
        FramesPerSecondCounter fps;

        MusicPlayer musicPlayer;
        BitmapFont bitmapFont;

        private readonly ScreenManager screenManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024; // set screen dimensions 4:3 as per Bark's sketch
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            musicPlayer = new MusicPlayer();
            musicPlayer.Initialize();

            string currentDir = Directory.GetCurrentDirectory();
            musicPlayer.AddSong(currentDir + "\\Kickin.mp3");
            musicPlayer.LoadSong(0, true);
            musicPlayer.Play();

            // User needs to register all the Screens that have been created!
            screenManager = Components.Add<ScreenManager>();

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            fps = new MonoGame.Extended.FramesPerSecondCounter();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bitmapFont = Content.Load<BitmapFont>("Fonts/fixedsys");
            screenManager.LoadScreen(new MainMenuScreen(this), new FadeTransition(GraphicsDevice, Color.Black, 0.5f));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            
            fps.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Viewport viewport = GraphicsDevice.Viewport;

            // Only draw if we are Debugging
            #if DEBUG
            fps.Draw(gameTime);
            string fpsText = "FPS: " + fps.FramesPerSecond;
            spriteBatch.Begin();
            spriteBatch.DrawString(bitmapFont, fpsText, new Vector2(viewport.Width - (bitmapFont.MeasureString(fpsText).Width), 0), Color.White);
            spriteBatch.End();
            #endif

            base.Draw(gameTime);
        }
    }
}
