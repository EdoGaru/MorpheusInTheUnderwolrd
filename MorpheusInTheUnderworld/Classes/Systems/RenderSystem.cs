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

    // This rendersystem only works with a camera
    public class RenderSystem : EntityDrawSystem
    {
        private readonly SpriteBatch spriteBatch;
        private readonly OrthographicCamera camera;
        private ComponentMapper<AnimatedSprite> animatedSpriteMapper;
        private ComponentMapper<Sprite> spriteMapper;
        private ComponentMapper<Transform2> transforMapper;

        public RenderSystem(SpriteBatch spriteBatch, OrthographicCamera camera)
         : base(Aspect.All(typeof(Transform2)).One(typeof(AnimatedSprite), typeof(Sprite)).Exclude(typeof(Tile)))
        {
            this.spriteBatch = spriteBatch;
            this.camera = camera;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transforMapper = mapperService.GetMapper<Transform2>();
            animatedSpriteMapper = mapperService.GetMapper<AnimatedSprite>();
            spriteMapper = mapperService.GetMapper<Sprite>();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());

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
