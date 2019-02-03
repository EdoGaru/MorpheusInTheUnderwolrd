using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonoGame.Extended.Screens;

namespace MorpheusInTheUnderworld.Screens
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            AddMenuItem("New Game!", Show<GameplayScreen>);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
