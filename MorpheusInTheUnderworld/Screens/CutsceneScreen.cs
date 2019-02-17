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

    public enum Scene
    {
        Start,
        Ending
    }
    public class CutsceneScreen : GameScreen
    {

        Scene scene;
        SpriteBatch spriteBatch;
        public ContentManager cutsceneContentManager;
        public Texture2D sceneToDisplay;

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

        public CutsceneScreen(Game game, Scene scene)
            :base(game)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cutsceneContentManager = new ContentManager(game.Services, "Content");
            this.scene = scene;
        }
        public override void LoadContent()
        {
            base.LoadContent();
            viewport = GraphicsDevice.Viewport;

            heroFrame = cutsceneContentManager.Load<Texture2D>("Graphics/hero_frame");
            wizardFrame = cutsceneContentManager.Load<Texture2D>("Graphics/wizard_frame");
            keyboardTexture = cutsceneContentManager.Load<Texture2D>("Graphics/vk");
            zKey = cutsceneContentManager.Load<Texture2D>("Graphics/z_key");
            string[] startDialogs = new string[] { "wizard: I've done everything I can for her.",
                                     "hero: Is there no hope at all?",
                                     "wizard: We can only pray. Unless...",
					"hero: Yes?",
					"wizard: Forget it. It's too dangerous.",
					"hero: You mean... the necromancer.",
					"wizard: He could lift the curse.",
					"hero: I'm not afraid of his foul creatures.",
					"wizard: You are but a humble minstrel.",
					"wizard: Those creatures were made to dance.",
					"hero: For the princess, I would do anything.",
					"wizard: God go with you, my son." };

            string[] endDialogs = new string[] 
                {
                "wizard: So you've arrived this far....",
                "hero: Hey...",
                "wizard: Sorry fella, but you won't get past here.",
                "hero: What do you mean?",
                "wizard: It turns out that i was the one who did that to her.",
                "hero: What?!",
                "wizard: And now it'll be your turn to perish!",
                "hero: This place is going to be your grave..."};


            if (scene == Scene.Start)
            {
                dialogs = startDialogs;
                sceneToDisplay = cutsceneContentManager.Load<Texture2D>("Graphics/MorpheusInTheUnderworld_MainScene");
            }
            else
            {
                dialogs = endDialogs;
                sceneToDisplay = cutsceneContentManager.Load<Texture2D>("Graphics/ending_scene");
            }
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
                if (charactersElapsed > 0.05f)
                {
                    showZKey = false;
                    if (charactersToShow > currentDialog.Length)
                    {
                        showZKey = true;
                        if (KeyboardExtended.GetState().WasKeyJustDown(Keys.Z))
                        { inDialog++; charactersToShow = 0; dialogLabel.Text = String.Empty; }
                    }
                    else
                    {
                        charactersElapsed = 0f;
                        charactersToShow++;
                    }

                }

                for (int i = 0; i < charactersToShow-1; i++)
                {
                    dialogLabel.Text += currentDialog[i];
                }
            }
            else
            {
                if (scene == Scene.Start)
                    ScreenManager.LoadScreen(new GameplayScreen(Game,false), new FadeTransition(GraphicsDevice, Color.Black));
                else
                    ScreenManager.LoadScreen(new GameplayScreen(Game, true), new FadeTransition(GraphicsDevice, Color.Black));
            }
            if (KeyboardExtended.GetState().WasKeyJustDown(Keys.Escape))
            {
                if (scene == Scene.Start)
                    ScreenManager.LoadScreen(new GameplayScreen(Game,false), new FadeTransition(GraphicsDevice, Color.Black));
                else
                    ScreenManager.LoadScreen(new GameplayScreen(Game, true), new FadeTransition(GraphicsDevice, Color.Black));

            }
            zKeyImage.Visible = showZKey;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(sceneToDisplay, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
