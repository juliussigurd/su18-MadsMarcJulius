using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    public class Passenger {
        private new List<EntityContainer> passengerList;
        public Entity _passenger { get; private set; }
        private DynamicShape shape;
        private IBaseImage imageWalk;
        
        public Passenger() {
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            imageWalk = new ImageStride(80, ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkRight")));
            _passenger = new Entity(shape, imageWalk);
        }
        
        public void SetPosition(float x, float y){
            shape.Position.X = x;
            shape.Position.Y = y;
            
        }
        
        public void SetExtent(float x, float y){
            shape.Extent.X = x;
            shape.Extent.Y = y;
            
        }
        
    }
}