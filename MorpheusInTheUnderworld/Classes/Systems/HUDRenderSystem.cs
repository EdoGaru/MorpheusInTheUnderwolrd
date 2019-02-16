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
    class HUDRenderSystem : EntityDrawSystem
    {
        private readonly SpriteBatch spriteBatch;
        private readonly ContentManager contentManager;
        private Sprite redHeartSprite;
        private Sprite grayHeartSprite;
        private Texture2D background;
        private ComponentMapper<Health> healthMapper;

        public HUDRenderSystem(SpriteBatch spriteBatch, ContentManager contentManager)
         : base(Aspect.All(typeof(Health), typeof(Player)))
        {
            this.spriteBatch = spriteBatch;
            this.contentManager = contentManager;

            redHeartSprite = new Sprite(contentManager.Load<Texture2D>("Graphics/heart pixel art 32x32"));
            grayHeartSprite = new Sprite(contentManager.Load<Texture2D>("Graphics/gray heart pixel art 32x32"));
            background = contentManager.Load<Texture2D>("Graphics/back");
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            healthMapper = mapperService.GetMapper<Health>();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //spriteBatch.Draw(background, new Microsoft.Xna.Framework.Rectangle(0, 0, 1280, 720), Color.White);

            foreach (var entity in ActiveEntities)
            {
                var healthPoints = healthMapper.Has(entity) ? healthMapper.Get(entity) : null;
                if (healthPoints != null)
                {
                    var maxHealthPoints = 3;
                    for(int i = 0; i < maxHealthPoints; i++)
                    {
                        Vector2 position = new Vector2(32+(i* 32), 32f);
                        spriteBatch.Draw(grayHeartSprite, new Transform2(position,0f,Vector2.One));
                    }

                    for (int i = 0; i < healthPoints.LifePoints; i++)
                    {
                        Vector2 position = new Vector2(32+(i * 32), 32f);
                        spriteBatch.Draw(redHeartSprite, new Transform2(position,0f,Vector2.One));
                       
                    }

                    

                }

            }

            spriteBatch.End();
        }
    }
}
