using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorpheusInTheUnderworld.Screens
{
    public class GameOverScreen : MenuScreen
    {

        public GameOverScreen(Game game)
            :base(game)
        {

        }
        public override void LoadContent()
        {
            base.LoadContent();

            AddMenuItem("Play Again", () => { ScreenManager.LoadScreen(new GameplayScreen(Game)); });

            AddMenuItem("Exit", () => { ScreenManager.LoadScreen(new MainMenuScreen(Game)); });

        }
        public override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
            spriteBatch.Begin();
            spriteBatch.DrawString(Font, "Game Over", new Vector2(Viewport.Width / 2, 50), Color.White);
            spriteBatch.End();
        }

    }
}
