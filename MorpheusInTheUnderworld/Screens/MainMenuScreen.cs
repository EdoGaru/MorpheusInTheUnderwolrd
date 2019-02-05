
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonoGame.Extended.Screens;
using Microsoft.Xna.Framework;
using System;

namespace MorpheusInTheUnderworld.Screens
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen(Game game) : base(game)
        {

        }
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            ScreenManager.LoadScreen(new GameplayScreen(Game));
            AddMenuItem("New Game!", () => { ScreenManager.LoadScreen(new GameplayScreen(Game)); });
            
        }
        

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
