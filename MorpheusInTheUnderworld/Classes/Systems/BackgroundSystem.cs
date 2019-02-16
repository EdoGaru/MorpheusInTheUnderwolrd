using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MorpheusInTheUnderworld.Classes.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorpheusInTheUnderworld.Classes.Systems
{
    class BackgroundSystem : DrawSystem
    {
        private readonly SpriteBatch spriteBatch;
        private readonly ContentManager contentManager;
        private Texture2D background;

        public BackgroundSystem(SpriteBatch spriteBatch, ContentManager contentManager)
         : base()
        {
            this.spriteBatch = spriteBatch;
            this.contentManager = contentManager;

            background = contentManager.Load<Texture2D>("Graphics/back");
        }

        public override void Initialize(World world)
        {
            base.Initialize(world);

        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            spriteBatch.Draw(background, new Microsoft.Xna.Framework.Rectangle(0, 0, 1280, 720), Color.White);

            spriteBatch.End();
        }
    }
}
