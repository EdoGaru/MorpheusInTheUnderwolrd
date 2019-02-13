using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Gui;
using MorpheusInTheUnderworld.Classes;

namespace MorpheusInTheUnderworld.Screens
{
    public class OptionMenuScreen : MenuScreen
    {
        GameTime _gametime;
        UserInterface optionInterface;
        UserInterface previousInterface;
        public OptionMenuScreen(Game game)
            : base(game)
        {

        }

        public override void LoadContent()
        {
            base.LoadContent();

            previousInterface = UserInterface.Active;

            // Our interface
            optionInterface = new UserInterface();
            // Set our interface as the Active
            UserInterface.Active = optionInterface;

            // Sizes
            Vector2 mainPanelSize = new Vector2(Viewport.Width, Viewport.Height);
            Vector2 optionsPanelSize = new Vector2(Viewport.Width / 1.2f, Viewport.Height / 1.2f);

            // Buttons
            Button loadButton, saveButton, confirmButton;

            // Sliders
            Slider fxSlider, musicSlider;

            //Labels
            Label fxPercentage, musicPercentage;

            // Main Panels
            Panel mainPanel = new Panel(mainPanelSize, PanelSkin.None, Anchor.Auto);
            // our options panel
            Panel optionsPanel = new Panel(optionsPanelSize, PanelSkin.Default, Anchor.Center);

            optionsPanel.AddChild(new Header("Options", Anchor.TopCenter));
            optionsPanel.AddChild(new HorizontalLine());
            // a width of 0 means take parent full width size
            Panel masterVolPanel = new Panel(new Vector2(0, 50), PanelSkin.None, Anchor.Auto);
            // Declare our master volume controls
            masterVolPanel.AddChild(new Label("Master Volume", Anchor.AutoInline, size: new Vector2(0.4f, -1)));
            Slider masterVolSlider = new Slider(0, 100, new Vector2(250, -1), SliderSkin.Default, Anchor.AutoInline) { Value = GameSettings.MasterVolume };
            Label masterPercentage = new Label(GameSettings.MasterVolume.ToString() + "%", Anchor.AutoInline, new Vector2(0.2f, -1)) { SpaceBefore = new Vector2(30, 0) };

            // Event to grab our slider value and set it to our GameSettings.
            masterVolSlider.OnValueChange = (Entity ent) =>
                        {
                            GameSettings.MasterVolume = masterVolSlider.Value;
                            masterPercentage.Text = GameSettings.MasterVolume.ToString() + "%";
                        };

            masterVolPanel.AddChild(masterVolSlider);
            masterVolPanel.AddChild(masterPercentage);
            optionsPanel.AddChild(masterVolPanel);

            // music vol panel
            Panel musicVolPanel = new Panel(new Vector2(0, 50), PanelSkin.None, Anchor.Auto);
            musicVolPanel.AddChild(new Label("Music Volume", Anchor.AutoInline, size: new Vector2(0.4f, -1)));
            musicVolPanel.AddChild(musicSlider = new Slider(0, 100, new Vector2(250, -1), SliderSkin.Default, Anchor.AutoInline) { Value = GameSettings.MusicVolume });
            musicVolPanel.AddChild(musicPercentage = new Label(GameSettings.MusicVolume.ToString() + "%", Anchor.AutoInline, new Vector2(0.2f, -1)) { SpaceBefore = new Vector2(30, 0) });

            musicSlider.OnValueChange = (Entity ent) =>
            {
                GameSettings.MusicVolume = musicSlider.Value;
                musicPercentage.Text = GameSettings.MusicVolume.ToString() + "%";
            };


            optionsPanel.AddChild(musicVolPanel);

            // master effect panel
            Panel masterEffectPanel = new Panel(new Vector2(0, 50), PanelSkin.None, Anchor.Auto);
            masterEffectPanel.AddChild(new Label("Effects Volume", Anchor.AutoInline, size: new Vector2(0.4f, -1)));
            masterEffectPanel.AddChild(fxSlider = new Slider(0, 100, new Vector2(250, -1), SliderSkin.Default, Anchor.AutoInline) { Value = GameSettings.EffectsVolume });
            masterEffectPanel.AddChild(fxPercentage = new Label(GameSettings.EffectsVolume.ToString() + "%", Anchor.AutoInline, new Vector2(0.2f, -1)) { SpaceBefore = new Vector2(30, 0) });

            fxSlider.OnValueChange = (Entity ent) =>
                        {
                            GameSettings.EffectsVolume = fxSlider.Value;
                            fxPercentage.Text = GameSettings.EffectsVolume.ToString() + "%";
                        };


            optionsPanel.AddChild(masterEffectPanel);
           
            //load & save config panel
            Panel configPanel = new Panel(new Vector2(0, 50), PanelSkin.None, Anchor.Auto);
            configPanel.AddChild(new Header("Configuration", Anchor.TopCenter));
            configPanel.AddChild(new HorizontalLine());
            configPanel.AddChild(loadButton = new Button("Load config", size: new Vector2(0.5f, -1)));
            configPanel.AddChild(saveButton = new Button("Save config", ButtonSkin.Default, Anchor.AutoInline, size: new Vector2(0.5f, -1)));

            optionsPanel.AddChild(configPanel);
            mainPanel.AddChild(optionsPanel);
            UserInterface.Active.AddEntity(mainPanel);

            //load/save button events
            loadButton.OnClick = (Entity ent) =>
            {
                GameSettings.Read();
                musicSlider.Value = GameSettings.MusicVolume;
                fxSlider.Value = GameSettings.EffectsVolume;
                masterVolSlider.Value = GameSettings.MasterVolume;
                
            };

            saveButton.OnClick = (Entity ent) =>
            {
                ConfirmOverwrite();

            };

            void ConfirmOverwrite() // broken out into separate function because can't nest Entities
            {
                configPanel.RemoveChild(saveButton);
                configPanel.AddChild(confirmButton = new Button("Click to overwrite", ButtonSkin.Default, Anchor.AutoInline, size: new Vector2(0.5f, -1)));
                confirmButton.OnClick = (Entity ent) =>
                {
                    GameSettings.Write();
                    configPanel.RemoveChild(confirmButton);
                    configPanel.AddChild(saveButton);


                };
            }

       
                


        }

        

        public override void UnloadContent()
        {
            base.UnloadContent();
            UserInterface.Active = previousInterface;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

    }
}
