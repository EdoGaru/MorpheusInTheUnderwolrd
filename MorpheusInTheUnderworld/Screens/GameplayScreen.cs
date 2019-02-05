
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
namespace MorpheusInTheUnderworld.Screens
{
    /// <summary>
    /// This is the main Gameplay Screen
    /// </summary>
    class GameplayScreen : GameScreen
    {
        public GameplayScreen(Game game) : base(game) 
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        public override void LoadContent()
        {
            base.LoadContent();
            
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if(KeyboardExtended.GetState().WasKeyJustDown(Keys.Escape))
                ScreenManager.LoadScreen(new MainMenuScreen(Game));
        }
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }
    }
}
