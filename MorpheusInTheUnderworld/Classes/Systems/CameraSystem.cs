using Microsoft.Xna.Framework;
using MonoGame.Extended;
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
    class CameraSystem : EntityProcessingSystem
    {
        private ComponentMapper<Focusable> focusableMapper;
        private ComponentMapper<Transform2> transformMapper;

        private OrthographicCamera orthographicCamera;

        public CameraSystem(OrthographicCamera orthographicCamera)
            :base(Aspect.All(typeof(Focusable), typeof(Transform2)))
        {
            this.orthographicCamera = orthographicCamera;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            focusableMapper = mapperService.GetMapper<Focusable>();
            transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            foreach (var entity in ActiveEntities)
            {
                var focus = focusableMapper.Get(entity);
                var transform = transformMapper.Get(entity);

                //There can only be one object focused
                if (focus.IsFocused)
                    orthographicCamera.LookAt(transform.Position);
            }
        }
    }
}
