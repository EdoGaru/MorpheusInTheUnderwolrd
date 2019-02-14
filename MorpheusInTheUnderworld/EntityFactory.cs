using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MorpheusInTheUnderworld.Collisions;
using World = MonoGame.Extended.Entities.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Animations;
using MonoGame.Extended;
using MorpheusInTheUnderworld.Classes.Components;
using MonoGame.Extended.Sprites;
using MorpheusInTheUnderworld.Classes;
using System.IO;

namespace MorpheusInTheUnderworld
{
    class EntityFactory
    {
        private readonly World _world;
        private readonly ContentManager _contentManager;

        public EntityFactory(World world, ContentManager contentManager)
        {
            _world = world;
            _contentManager = contentManager;
        }

        public Entity CreatePlayer(Vector2 position)
        {
            var dudeTexture = _contentManager.Load<Texture2D>("Graphics/hero");
            var dudeAtlas = TextureAtlas.Create("dudeAtlas", dudeTexture, 16, 16);
            var entity = _world.CreateEntity();

            var animationFactory = new SpriteSheetAnimationFactory(dudeAtlas);
            animationFactory.Add("idle", new SpriteSheetAnimationData(new[] { 0, 1, 2, 1 }));
            animationFactory.Add("walk", new SpriteSheetAnimationData(new[] { 6, 7, 8, 9, 10, 11 }, frameDuration: 0.1f));
            animationFactory.Add("combat", new SpriteSheetAnimationData(new[] { 17 }, frameDuration: 0.3f, isLooping: false));
            entity.Attach(new AnimatedSprite(animationFactory, "idle"));
            entity.Attach(new Transform2(position, 0, Vector2.One*4));
            entity.Attach(new Body { Position = position, Size = new Vector2(96, 96), BodyType = BodyType.Dynamic });
            entity.Attach(new Focusable { IsFocused = true });
            entity.Attach(new Player());

            return entity;
        }

        public Entity CreateEnemy(Vector2 position)
        {
            var baddieTexture = _contentManager.Load<Texture2D>("Graphics/baddie");
            var baddieAtlas = TextureAtlas.Create("baddieAtlas", baddieTexture, 16, 16);
            var entity = _world.CreateEntity();

            var animationFactory = new SpriteSheetAnimationFactory(baddieAtlas);
            animationFactory.Add("idle", new SpriteSheetAnimationData(new[] { 5,4,3,4 }));
            animationFactory.Add("walk", new SpriteSheetAnimationData(new[] { 11,10,9,8,7,6 }, frameDuration: 0.1f));
            animationFactory.Add("combat", new SpriteSheetAnimationData(new[] { 29 }, frameDuration: 0.3f, isLooping: false));
            entity.Attach(new AnimatedSprite(animationFactory, "idle"));
            entity.Attach(new Transform2(position, 0, Vector2.One*4));
            entity.Attach(new Body { Position = position, Size = new Vector2(64, 64), BodyType = BodyType.Dynamic });
            entity.Attach(new Focusable { IsFocused = true });
            entity.Attach(new Enemy());

            return entity;
        }

        //160,121 normal tile
        public Entity CreatePath(Vector2 position)
        {
            Texture2D tile_cave_platform = _contentManager.Load<Texture2D>("Graphics/tile_cave_platform");
            var cavePathRegion = new TextureRegion2D(tile_cave_platform, new Rectangle(160, 121, 32, 32));
            var size = new Vector2(32, 32);

            var entity = _world.CreateEntity();

            entity.Attach(new Tile() {  });
            entity.Attach(new Sprite(cavePathRegion));
            entity.Attach(new Transform2(position, 0, Vector2.One*2));
            entity.Attach(new Body { Position = position, Size = size, BodyType = BodyType.Static });

            return entity;
        }

        //125,182
        public Entity CreateUnderground(Vector2 position)
        {
            Texture2D tile_cave_platform = _contentManager.Load<Texture2D>("Graphics/tile_cave_platform");
            var cavePathRegion = new TextureRegion2D(tile_cave_platform, new Rectangle(125, 182, 32, 32));
            var size = new Vector2(32, 32);

            var entity = _world.CreateEntity();

            entity.Attach(new Tile() {  });
            entity.Attach(new Sprite(cavePathRegion));
            entity.Attach(new Transform2(position, 0, Vector2.One*2));
            entity.Attach(new Body { Position = position, Size = size, BodyType = BodyType.Static });

            return entity;
        }

        //64, 249
        public List<Entity> CreateWall(Vector2 position, int Height)
        {
            Texture2D tile_cave_platform = _contentManager.Load<Texture2D>("Graphics/tile_cave_platform");
            var cavePathRegion = new TextureRegion2D(tile_cave_platform, new Rectangle(64, 249, 32, 32));
            var size = new Vector2(32, 32);
            List<Entity> wallList = new List<Entity>();

            for (int i = 0; i < Height; i++)
            {
                var entity = _world.CreateEntity();

                entity.Attach(new Tile() { });
                entity.Attach(new Sprite(cavePathRegion));
                entity.Attach(new Transform2(position, 90, Vector2.One * 2));
                entity.Attach(new Body { Position = new Vector2(position.X, position.Y - (i*32)), Size = size, BodyType = BodyType.Static });

                wallList.Add(entity);
            }
            return wallList;

        }

        public List<Entity> CreateWall(Vector2 position, int Height, TextureRegion2D texture)
        {
            var size = new Vector2(32, 32);
            List<Entity> wallList = new List<Entity>();

            for (int i = 0; i < Height; i++)
            {
                var entity = _world.CreateEntity();

                entity.Attach(new Tile() { });
                entity.Attach(new Sprite(texture));
                entity.Attach(new Transform2(position, 90, Vector2.One * 2));
                entity.Attach(new Body { Position = new Vector2(position.X, position.Y - (i*32)), Size = size, BodyType = BodyType.Static });

                wallList.Add(entity);
            }
            return wallList;

        }

        public Entity CreateRocksBackground(Vector2 position, float scale)
        {
            TextureRegion2D background_rocks_region = new TextureRegion2D(_contentManager.Load<Texture2D>("Graphics/tile_cave_bg_rock"), new Rectangle(0, 0, 448, 224));

            var entity = _world.CreateEntity();

            entity.Attach(new Sprite(background_rocks_region));
            entity.Attach(new Transform2(position, 0, Vector2.One*scale));

            return entity;
        }
        // THIS IS FOR A FUTURE VERSION 
        //public Entity CreateMap(Vector2 position, string from)
        //{
        //     var expectedMap = File.ReadAllText(from);

        //    IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>(expectedMap);   
        //    var map = Map.Create(mapCreationStrategy);

        //    var entity = _world.CreateEntity();
        //    entity.Attach(new Transform2(position, 0, Vector2.One));
        //    entity.Attach(map);

        //    return entity;
        //}

        //// This player works together with the map, and is linked to our
        //// side view player
        //public Entity CreateDotPlayer(Entity map)
        //{
        //    var entity = _world.CreateEntity();
        //    var circle = _contentManager.Load<Texture2D>("Graphics/dotplayer");
        //    var Map = map.Get<Map>();
        //    Vector2 size = new Vector2(16, 16);
        //    var mapTransform = map.Get<Transform2>().Position;
        //    var position = new Vector2(mapTransform.X + (Map.GetCell(1,0).X * size.X) + size.X/2, mapTransform.Y + (Map.GetCell(1,0).Y * size.Y) + size.Y/2);

        //    entity.Attach(new DotPlayer());
        //    entity.Attach(new Transform2(position, 0, Vector2.One));
        //    entity.Attach(new Body() { Position = position, Size = size, BodyType = BodyType.Static });
        //    entity.Attach(new Sprite(circle));
        //    return entity;
        //}
    }
}
