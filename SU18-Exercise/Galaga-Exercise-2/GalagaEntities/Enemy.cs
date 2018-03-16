using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.EventBus;
using System.Collections.Generic;
using DIKUArcade.Physics;
using DIKUArcade.Timers;

namespace Galaga_Exercise_2.GalagaEntities
{
   
    public class Enemy : DIKUArcade.Entities.Entity
    {
        public Vec2F position;
        
        public Enemy (StationaryShape shape, IBaseImage image) : base ((DynamicShape)shape, image)
        {
            position = shape.Position.Copy();
        }

        public Vec2F GetPosition
        {
            get { return position; }
        }
    }
}