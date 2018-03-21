using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_3.GalagaEntities
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