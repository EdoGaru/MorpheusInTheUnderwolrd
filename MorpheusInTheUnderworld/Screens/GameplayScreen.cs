
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
using MorpheusInTheUnderworld.Collisions;
using World = MonoGame.Extended.Entities.World;
using MorpheusInTheUnderworld.Classes.Components;
using MonoGame.Extended.Screens.Transitions;

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
        Entity player;
        List<Entity> enemies;

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

            MusicPlayer.LoadSong(0, true);
            //DotPlayerSystem dotPlayerSystem = new DotPlayerSystem();
            world = new WorldBuilder()
                     .AddSystem(new WorldSystem())
                     .AddSystem(new PlayerSystem(orthographicCamera))
                     .AddSystem(new CameraSystem(orthographicCamera))
                     .AddSystem(new EnemySystem())
                     //.AddSystem(dotPlayerSystem)
                     //.AddSystem(new MapRenderSystem(spriteBatch, gameplayScreenContent))
                     //.AddSystem(new DotPlayerRenderSystem(spriteBatch))
                     .AddSystem(new BackgroundSystem(spriteBatch, gameplayScreenContent))
                     .AddSystem(new RenderSystem(spriteBatch, orthographicCamera, gameplayScreenContent))
                     .AddSystem(new TilesRenderSystem(spriteBatch, orthographicCamera))
                     .AddSystem(new HUDRenderSystem(spriteBatch, gameplayScreenContent))
                     .Build();
            world.DrawOrder = 0;
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
            player = entityFactory.CreatePlayer(new Vector2(64,(viewport.Height/2)-32));
            enemies = new List<Entity>();
            var enemyCount = 10;
            for (int i = 0; i < enemyCount; i++)
            {
                enemies.Add(entityFactory.CreateEnemy(new Vector2(256 + (i * 512), (viewport.Height / 2) - 64)));
            }

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
        public void CheckForCombat()
        {
            foreach (Entity enemy in enemies)
            {
                var playerRef = player.Get<Player>();
                var enemyRef = enemy.Get<Enemy>();
                var playerBody = player.Get<Body>();
                var enemyBody = enemy.Get<Body>();

                if (enemy.Has<Body>())
                {
                    if (CollisionTester.DistanceToAttack(playerBody.BoundingBox, enemyBody.BoundingBox))
                    {
                        enemyRef.OnCombat = true;
                        // If player attacks
                        if (playerRef.State == State.Combat)
                        {
                            var enemyHP = enemy.Get<Health>().LifePoints -= 1;
                            if (enemyHP < 1)
                            {
                                enemy.Destroy();
                                return;
                            }
                        }

                        if (playerRef.ImmuneTimer < 1f)
                        {
                            if (playerRef.State != State.Guard)
                            {
                                if (enemyRef.State == State.Combat)
                                {
                                    var playerHP = player.Get<Health>().LifePoints -= 1;
                                    playerRef.ImmuneTimer = 3.5f;
                                    if (playerHP < 1)
                                    {
                                        MusicPlayer.Stop();
                                        ScreenManager.LoadScreen(new GameOverScreen(Game), new FadeTransition(GraphicsDevice, Color.Black, 1.5f));
                                    }
                                }
                            }

                        }
                    }
                    else
                        enemyRef.OnCombat = false;
                }
            }

        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if(KeyboardExtended.GetState().WasKeyJustDown(Keys.Escape))
                ScreenManager.LoadScreen(new MainMenuScreen(Game));

            CheckForCombat();
        }
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            spriteBatch.End();


        }
    }
}
