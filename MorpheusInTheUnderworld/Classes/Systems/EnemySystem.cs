using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;
using MorpheusInTheUnderworld.Classes.Components;
using MorpheusInTheUnderworld.Collisions;
using MorpheusInTheUnderworld.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorpheusInTheUnderworld.Classes.Systems
{
    // TODO: ADD PLAYER PHYSICS
    class EnemySystem : EntityProcessingSystem
    {
        private readonly OrthographicCamera orthographicCamera;

        private ComponentMapper<Enemy> enemyMapper;
        private ComponentMapper<AnimatedSprite> spriteMapper;
        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<Body> bodyMapper;



        // This System only filter types of Body, Player, Transform2 and AnimatedSprite
        public EnemySystem()
            : base(Aspect.All(typeof(Body), typeof(Transform2), typeof(AnimatedSprite),typeof(Enemy)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            enemyMapper = mapperService.GetMapper<Enemy>();
            spriteMapper = mapperService.GetMapper<AnimatedSprite>();
            transformMapper = mapperService.GetMapper<Transform2>();
            bodyMapper = mapperService.GetMapper<Body>();

        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var enemy = enemyMapper.Get(entityId);
            var sprite = spriteMapper.Get(entityId);
            var transform = transformMapper.Get(entityId);
            var body = bodyMapper.Get(entityId);
            var keyboardState = KeyboardExtended.GetState();

            /*
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                body.Velocity.X += 150;
                player.Facing = Facing.Right;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                body.Velocity.X -= 150;
                player.Facing = Facing.Left;
            }

            if (!player.IsAttacking)
            {
                if (body.Velocity.X > 0 || body.Velocity.X < 0)
                    player.State = State.Walking;

                //if (body.Velocity.Y < 0)
                //    player.State = State.Jumping;

                //if (body.Velocity.Y > 0)
                //    player.State = State.Falling;

                if (body.Velocity.EqualsWithTolerence(Vector2.Zero, 5))
                    player.State = State.Idle;
            }

            switch (player.State)
            {
                case State.Walking:
                    sprite.Play("walk");
                    sprite.Effect = player.Facing == Facing.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    break;
                case State.Idle:
                    sprite.Play("idle");
                    break;
                case State.Combat:
                    //   ScreenManager.LoadScreen(new CombatScreen(Game), new FadeTransition(GraphicsDevice, Color.Black, 0.5f));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        //    body.Velocity.X *= 0.7f;
        */
      
        }
    }
}
