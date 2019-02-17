using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MorpheusInTheUnderworld.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorpheusInTheUnderworld.Screens
{
    public class CongratulationsScreen : MenuScreen
    {

        UserInterface previousInterface;
        UserInterface currentInterface;

        Label mainLabel;
        public CongratulationsScreen(Game game)
            :base(game)
        {

        }
        public override void LoadContent()
        {
            base.LoadContent();
            Viewport viewport = GraphicsDevice.Viewport;

            previousInterface = UserInterface.Active;
            currentInterface = new UserInterface();
            UserInterface.Active = currentInterface;


            Panel mainPanel = new Panel(new Vector2(viewport.Width, viewport.Height), PanelSkin.None);
            Panel labelPanel = new Panel(new Vector2(0.5f, 0.5f), PanelSkin.None);
            mainLabel = new Label("Congratulations, You have finished the game thanks for playing! oh and if you are looking for a happy ending well, you just repelled the curse of the princess ;)", Anchor.Auto, new Vector2(0,-1));
            mainLabel.FillColor = Color.White;

            labelPanel.AddChild(mainLabel);
            mainPanel.AddChild(labelPanel);
            MusicPlayer.Stop();

            UserInterface.Active.AddEntity(mainPanel);
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
            UserInterface.Active = previousInterface;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (KeyboardExtended.GetState().WasKeyJustDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                ScreenManager.LoadScreen(new MainMenuScreen(Game), new FadeTransition(GraphicsDevice, Color.Black, 1.3f));
        }
        public override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
            spriteBatch.Begin();

            spriteBatch.End();
        }

    }
}
