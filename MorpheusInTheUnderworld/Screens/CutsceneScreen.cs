using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorpheusInTheUnderworld.Screens
{
    public class CutsceneScreen : GameScreen
    {
        SpriteBatch spriteBatch;
        public ContentManager cutsceneContentManager;
        public Texture2D mainScene;

        UserInterface previousInterface;
        UserInterface cutsceneInterface;

        string[] dialogs;
        int inDialog=0;
        int charactersToShow = 0;
        float charactersElapsed = 0f;
        Viewport viewport;
        Label dialogLabel;
        Image dialogImage;

        Texture2D wizardFrame;
        Texture2D heroFrame;

        Texture2D keyboardTexture;
        Texture2D zKey;
        Image zKeyImage;
        public CutsceneScreen(Game game)
            :base(game)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cutsceneContentManager = new ContentManager(game.Services, "Content");
        }
        public override void LoadContent()
        {
            base.LoadContent();
            viewport = GraphicsDevice.Viewport;
            mainScene = cutsceneContentManager.Load<Texture2D>("Graphics/MorpheusInTheUnderworld_MainScene");

            heroFrame = cutsceneContentManager.Load<Texture2D>("Graphics/hero_frame");
            wizardFrame = cutsceneContentManager.Load<Texture2D>("Graphics/wizard_frame");
            keyboardTexture = cutsceneContentManager.Load<Texture2D>("Graphics/vk");
            zKey = cutsceneContentManager.Load<Texture2D>("Graphics/z_key");
            dialogs = new string[] { "wizard: nothing we can do....",
                                     "hero: so will she.....",
                                     "wizard: yes..." };

            cutsceneInterface = new UserInterface();
            previousInterface = UserInterface.Active;
            UserInterface.Active = cutsceneInterface;


            Panel dialogPanel = new Panel(new Vector2(viewport.Width, viewport.Height /4), PanelSkin.Default, Anchor.BottomCenter);
            dialogPanel.AddChild(dialogImage = new Image(wizardFrame, new Vector2(128, 128), ImageDrawMode.Stretch, Anchor.Auto));
            dialogPanel.AddChild(dialogLabel = new Label("",Anchor.AutoInline, new Vector2(viewport.Width/2, 20), new Vector2(10,0f)));
            dialogPanel.AddChild(zKeyImage = new Image(zKey, new Vector2(32, 32), ImageDrawMode.Stretch, Anchor.BottomRight) { Visible = false });
            cutsceneInterface.AddEntity(dialogPanel);
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
            cutsceneContentManager.Unload();
            UserInterface.Active = previousInterface;
        }
        
        bool showZKey = false;
        public override void Update(GameTime gameTime)
        {
            dialogLabel.Text = String.Empty;
            if (inDialog < dialogs.Length)
            {
                var dialogSplit = dialogs[inDialog].Split(':');
                var character = dialogSplit[0];
                var currentDialog = dialogSplit[1];
                switch (character)
                {
                    case "hero":
                        dialogImage.Texture = heroFrame;
                        break;
                    case "wizard":
                        dialogImage.Texture = wizardFrame;
                        break;
                }
                charactersElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (charactersElapsed > 0.1f)
                {
                    showZKey = false;
                    if (charactersToShow > currentDialog.Length)
                    {
                        showZKey = true;
                        if (KeyboardExtended.GetState().WasKeyJustDown(Keys.Z))
                        { inDialog++; charactersToShow = 0; dialogLabel.Text = String.Empty; }
                    }
                    else
                        charactersToShow++;
                    charactersElapsed = 0f;
                }

                for (int i = 0; i < charactersToShow-1; i++)
                {
                    dialogLabel.Text += currentDialog[i];
                }
            }
            else
            {
                ScreenManager.LoadScreen(new GameplayScreen(Game), new FadeTransition(GraphicsDevice, Color.Black));
            }

            zKeyImage.Visible = showZKey;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(mainScene, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
