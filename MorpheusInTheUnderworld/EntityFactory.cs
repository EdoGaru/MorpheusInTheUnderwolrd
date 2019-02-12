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
using RogueSharp;
using RogueSharp.MapCreation;
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
            entity.Attach(new Body { Position = position, Size = new Vector2(32, 32), BodyType = BodyType.Dynamic });
            entity.Attach(new Focusable { IsFocused = true });
            entity.Attach(new Player());

            return entity;
        }

        public Entity CreateTile32(Vector2 position)
        {
            var tileTexture = _contentManager.Load<Texture2D>("Graphics/tile_32x32");
            Vector2 size = new Vector2(32, 32);

            var entity = _world.CreateEntity();

            entity.Attach(new Tile() { Color = Color.Black });
            entity.Attach(new Sprite(tileTexture));
            entity.Attach(new Transform2(position, 0, Vector2.One));
            entity.Attach(new Body { Position = position, Size = size, BodyType = BodyType.Static });

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
