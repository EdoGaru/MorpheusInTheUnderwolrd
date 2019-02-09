using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using MorpheusInTheUnderworld.Classes.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorpheusInTheUnderworld.Classes.Systems
{
    class HUDRenderSystem : EntityDrawSystem
    {
        private readonly SpriteBatch spriteBatch;
        private ComponentMapper<AnimatedSprite> animatedSpriteMapper;
        private ComponentMapper<Sprite> spriteMapper;
        private ComponentMapper<Transform2> transforMapper;

        public HUDRenderSystem(SpriteBatch spriteBatch)
         : base(Aspect.All(typeof(Transform2)).One(typeof(AnimatedSprite), typeof(Sprite)).Exclude(typeof(Tile)))
        {
            this.spriteBatch = spriteBatch;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transforMapper = mapperService.GetMapper<Transform2>();
            animatedSpriteMapper = mapperService.GetMapper<AnimatedSprite>();
            spriteMapper = mapperService.GetMapper<Sprite>();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (var entity in ActiveEntities)
            {
                var sprite = animatedSpriteMapper.Has(entity)
                    ? animatedSpriteMapper.Get(entity)
                    : spriteMapper.Get(entity);
                var transform = transforMapper.Get(entity);

                if(sprite is AnimatedSprite animatedSprite)
                    animatedSprite.Update(gameTime.GetElapsedSeconds());

                spriteBatch.Draw(sprite, transform);

            }

            spriteBatch.End();
        }
    }
}
