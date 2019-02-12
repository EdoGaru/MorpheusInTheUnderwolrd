using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MorpheusInTheUnderworld.Classes.Components;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorpheusInTheUnderworld.Classes.Systems
{
    class MapRenderSystem : EntityDrawSystem
    {
        private readonly SpriteBatch spriteBatch;
        private readonly ContentManager contentManager;

        private ComponentMapper<Map> mapMapper;
        private ComponentMapper<Transform2> transformMapper;

        private Texture2D wallTexture;
        private Texture2D pathTexture;

        public MapRenderSystem(SpriteBatch spriteBatch, ContentManager contentManager)
            : base(Aspect.All(typeof(Map), typeof(Transform2)))
        {
            this.spriteBatch = spriteBatch;
            this.contentManager = contentManager;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            mapMapper = mapperService.GetMapper<Map>();
            transformMapper = mapperService.GetMapper<Transform2>();

            wallTexture = contentManager.Load<Texture2D>("Graphics/tile_16x16");
            pathTexture = contentManager.Load<Texture2D>("Graphics/tile_16x16");
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (var entity in ActiveEntities)
            {
                 
                var transform = transformMapper.Get(entity);
                var map = mapMapper.Get(entity);

                int sizeOfSprites = 16;

                foreach (Cell cell in map.GetAllCells())
                {

                    if (cell.IsWalkable)
                    {
                        var position = new Vector2(transform.Position.X+(cell.X * sizeOfSprites), transform.Position.Y + (cell.Y * sizeOfSprites));
                        spriteBatch.Draw(pathTexture, position, Color.White);
                    }
                    else
                    {
                        var position = new Vector2(transform.Position.X+(cell.X * sizeOfSprites), transform.Position.Y+(cell.Y * sizeOfSprites));
                        spriteBatch.Draw(wallTexture, position, Color.Black);
                    }
                }
            }

            spriteBatch.End();
        }
    }
}
