
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
using Microsoft.Xna.Framework.Graphics;
using MorpheusInTheUnderworld.Classes.Systems;
using MonoGame.Extended.ViewportAdapters;
using Microsoft.Xna.Framework.Content;
using MorpheusInTheUnderworld.Classes;
using System.IO;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;

namespace MorpheusInTheUnderworld.Screens
{
    /// <summary>
    /// This is the main Gameplay Screen
    /// </summary>
    class GameplayScreen : GameScreen
    {
        private World world;
        private SpriteBatch spriteBatch;
        private OrthographicCamera orthographicCamera;
        private Viewport viewport;
        private EntityFactory entityFactory;

        ContentManager gameplayScreenContent;

        Texture2D minimapTile;
        Texture2D blackTexture;
        Texture2D background;
        Sprite background_rocks;

        public GameplayScreen(Game game) : base(game) 
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            viewport = GraphicsDevice.Viewport; 
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var viewportAdapter = new BoxingViewportAdapter(Game.Window, GraphicsDevice, viewport.Width, viewport.Height);
            orthographicCamera = new OrthographicCamera(viewportAdapter);
            gameplayScreenContent = new ContentManager(Game.Services, "Content");

            //DotPlayerSystem dotPlayerSystem = new DotPlayerSystem();
            world = new WorldBuilder()
                     .AddSystem(new WorldSystem())
                     .AddSystem(new CameraSystem(orthographicCamera))
                     .AddSystem(new PlayerSystem(orthographicCamera))
                     .AddSystem(new EnemySystem())
                     //.AddSystem(dotPlayerSystem)
                     //.AddSystem(new MapRenderSystem(spriteBatch, gameplayScreenContent))
                     //.AddSystem(new DotPlayerRenderSystem(spriteBatch))
                     .AddSystem(new RenderSystem(spriteBatch, orthographicCamera))
                     .AddSystem(new TilesRenderSystem(spriteBatch, orthographicCamera))
                     .Build();

            Game.Components.Add(world);

            entityFactory = new EntityFactory(world, gameplayScreenContent);

            var scaleRocks = 4;
            for (int i = 0; i < 30; i++)
            {
                entityFactory.CreateRocksBackground(new Vector2(i*(448*scaleRocks), 0), scaleRocks);
            }

            entityFactory.CreateWall(new Vector2(0,viewport.Height/2), 50);
            for (int i = 0; i < 200; i++)
            {
                entityFactory.CreatePath(new Vector2(i * 32, viewport.Height / 2));
                for(int j = 2; j < 10; j++)
                {
                    entityFactory.CreateUnderground(new Vector2(i * 32, (j * 32) + (viewport.Height / 2)));
                }

            }
            
            TextureRegion2D shadow_wall = new TextureRegion2D(gameplayScreenContent.Load<Texture2D>("Graphics/tile_cave_platform"), new Rectangle(125, 182, 32, 32));
            for (int i = 2; i < 30; i++)
            {
                entityFactory.CreateWall(new Vector2(i * -32, viewport.Height), 50, shadow_wall);
            }

            //Entity map = entityFactory.CreateMap(new Vector2(viewport.Width - (viewport.Width / 4)-200, viewport.Height - 300), "Content/Map/map_1.txt");
            //entityFactory.CreateDotPlayer(map);


            entityFactory.CreatePlayer(new Vector2(64,(viewport.Height/2)-32));

            //448,224
            
        }
        public override void LoadContent()
        {
            base.LoadContent();
            minimapTile = gameplayScreenContent.Load<Texture2D>("Graphics/minimap_tile");
            blackTexture = gameplayScreenContent.Load<Texture2D>("Graphics/tile_16x16");
            background = gameplayScreenContent.Load<Texture2D>("Graphics/back");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            Game.Components.Remove(world);
            gameplayScreenContent.Unload();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if(KeyboardExtended.GetState().WasKeyJustDown(Keys.Escape))
                ScreenManager.LoadScreen(new MainMenuScreen(Game));
        }
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            spriteBatch.Draw(background, new Microsoft.Xna.Framework.Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);
            spriteBatch.End();


        }
    }
}
