using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Input;
using MorpheusInTheUnderworld.Classes.Components;
using MorpheusInTheUnderworld.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorpheusInTheUnderworld.Classes.Systems
{
    // TODO: ADD PLAYER PHYSICS
    class PlayerSystem : EntityProcessingSystem
    {
        private readonly OrthographicCamera orthographicCamera;

        private ComponentMapper<Player> playerMapper;
        private ComponentMapper<AnimatedSprite> spriteMapper;
        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<Body> bodyMapper;

        

        // This System only filter types of Body, Player, Transform2 and AnimatedSprite
        public PlayerSystem(OrthographicCamera orthographicCamera) 
            : base(Aspect.All(typeof(Body), typeof(Transform2), typeof(AnimatedSprite)).One(typeof(Player), typeof(DotPlayer)))
        {
            this.orthographicCamera = orthographicCamera;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            playerMapper = mapperService.GetMapper<Player>();
            spriteMapper = mapperService.GetMapper<AnimatedSprite>();
            transformMapper = mapperService.GetMapper<Transform2>();
            bodyMapper = mapperService.GetMapper<Body>();

        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var player = playerMapper.Get(entityId);
            var sprite = spriteMapper.Get(entityId);
            var transform = transformMapper.Get(entityId);
            var body = bodyMapper.Get(entityId);
            var keyboardState = KeyboardExtended.GetState();


            if (player.CanJump)
            {
                if (keyboardState.WasKeyJustDown(Keys.Up))
                    body.Velocity.Y -= 550 + Math.Abs(body.Velocity.X) * 0.4f;

                //if (keyboardState.WasKeyJustUp(Keys.Z))
                //{
                //    body.Velocity.Y -= 550 + Math.Abs(body.Velocity.X) * 0.4f;
                //    player.State = player.State == State.Idle ? State.Punching : State.Kicking;
                //}
            }

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
                default:
                    throw new ArgumentOutOfRangeException();
            }

            body.Velocity.X *= 0.7f;

            //orthographicCamera.LookAt(transform.Position);

            // TODO: Can we remove this?
            //transform.Position = body.Position;
        }
    }
}
