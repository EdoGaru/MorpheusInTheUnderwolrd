using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        private ComponentMapper<Player> playerMapper;
        private ComponentMapper<Enemy> enemyMapper;
        private ComponentMapper<AnimatedSprite> animatedSpriteMapper;
        private ComponentMapper<Sprite> spriteMapper;
        private ComponentMapper<Transform2> transforMapper;
        ContentManager content;
        Effect damageEffect;
        public RenderSystem(SpriteBatch spriteBatch, OrthographicCamera camera, ContentManager content)
         : base(Aspect.All(typeof(Transform2)).One(typeof(AnimatedSprite), typeof(Sprite)).Exclude(typeof(Tile), typeof(DotPlayer)))
        {
            this.spriteBatch = spriteBatch;
            this.camera = camera;
            this.content = content;
            damageEffect = content.Load<Effect>("Effects/damage_effect");
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            playerMapper = mapperService.GetMapper<Player>();
            enemyMapper = mapperService.GetMapper<Enemy>();
            transforMapper = mapperService.GetMapper<Transform2>();
            animatedSpriteMapper = mapperService.GetMapper<AnimatedSprite>();
            spriteMapper = mapperService.GetMapper<Sprite>();
        }

        public override void Draw(GameTime gameTime)
        {

            foreach (var entity in ActiveEntities)
            {
                var sprite = animatedSpriteMapper.Has(entity)
                                   ? animatedSpriteMapper.Get(entity)
                    : spriteMapper.Get(entity);
                var transform = transforMapper.Get(entity);

                if(sprite is AnimatedSprite animatedSprite)
                    animatedSprite.Update(gameTime.GetElapsedSeconds());

                spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());
               
                spriteBatch.Draw(sprite, transform);
                
                spriteBatch.End();

                var player = playerMapper.Get(entity);
                if (player != null)
                {
                    if (player.ImmuneTimer > 1f)
                    {
                        spriteBatch.Begin(effect: damageEffect, transformMatrix: camera.GetViewMatrix());
                        spriteBatch.Draw(sprite, transform);
                        spriteBatch.End();
                    }
                }

                var enemy = enemyMapper.Get(entity);
                if (enemy != null)
                {
                    if (enemy.ImmuneTimer > 1f)
                    {
                        spriteBatch.Begin(effect: damageEffect, transformMatrix: camera.GetViewMatrix());
                        spriteBatch.Draw(sprite, transform);
                        spriteBatch.End();
                    }
                }

            }

        }
    }
}
