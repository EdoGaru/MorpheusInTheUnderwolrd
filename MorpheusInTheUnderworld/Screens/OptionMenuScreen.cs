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
        UserInterface optionInterface;
        UserInterface previousInterface;
        public OptionMenuScreen(Game game)
            :base(game)
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
            Slider masterVolSlider = new Slider(0, 100, new Vector2(250, -1), SliderSkin.Default, Anchor.AutoInline) { Value = 100 };
            Label masterPercentage = new Label("100%", Anchor.AutoInline, new Vector2(0.2f, -1)) { SpaceBefore = new Vector2(30, 0) };

            // Event to grab our slider value and set it to our GameSettings.
            masterVolSlider.OnValueChange = (Entity ent) =>
                        {
                            GameSettings.MasterVolume = masterVolSlider.Value;
                            masterPercentage.Text = GameSettings.MasterVolume.ToString() + "%";
                        };

            masterVolPanel.AddChild(masterVolSlider);
            masterVolPanel.AddChild(masterPercentage);
            optionsPanel.AddChild(masterVolPanel);
            // master effect panel
            Panel masterEffectPanel = new Panel(new Vector2(0, 50), PanelSkin.None, Anchor.Auto);
            masterEffectPanel.AddChild(new Label("Effects Volume", Anchor.AutoInline, size: new Vector2(0.4f, -1)));
            masterEffectPanel.AddChild(new Slider(0, 100, new Vector2(250, -1), SliderSkin.Default, Anchor.AutoInline) { Value = 100 });
            masterEffectPanel.AddChild(new Label("100%", Anchor.AutoInline, new Vector2(0.2f, -1)) { SpaceBefore = new Vector2(30,0) });
            optionsPanel.AddChild(masterEffectPanel);
            //load & save config panel
            Panel configPanel = new Panel(new Vector2(0, 50), PanelSkin.None, Anchor.Auto);
            configPanel.AddChild(new Header("Configuration", Anchor.TopCenter));
            configPanel.AddChild(new HorizontalLine());
            configPanel.AddChild(new Button("Load config", size: new Vector2(0.4f, -1)));
            configPanel.AddChild(new Button("Save config", size: new Vector2(0.4f, -1)));

            optionsPanel.AddChild(configPanel);

            mainPanel.AddChild(optionsPanel);
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
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

    }
}
