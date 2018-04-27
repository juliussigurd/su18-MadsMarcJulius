using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1
{
    public class Player : IGameEventProcessor<object>
    {
        public Entity Entity { get; private set; }
        private readonly DynamicShape shape;
        private readonly Image _taxiBoosterOffImageLeft;
        private readonly Image _taxiBoosterOffImageRight;
        private Orientation _taxiOrientation;
        

        public Player()
        {
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            _taxiBoosterOffImageLeft = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
            _taxiBoosterOffImageRight = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png"));

            Entity = new Entity(shape, _taxiBoosterOffImageLeft);
            
        }

        public void SetPosition(float x, float y)
        {
            shape.Position.X = x;
            shape.Position.Y = y;
        }

        //selfmade gravity
        public void Gravity()
        {
            shape.Position.Y -= 0.00045f;
        }

        public void SetExtent(float width, float height)
        {
            shape.Extent.X = width;
            shape.Extent.Y = height;
        }

        public void RenderPlayer()
        {
            //TODO: Next version needs animation. Skipped for clarity.
            Entity.Image = _taxiOrientation == Orientation.Left
                ? _taxiBoosterOffImageLeft
                : _taxiBoosterOffImageRight;
            Entity.RenderEntity();
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent)
        {
            if (eventType == GameEventType.PlayerEvent)
            {
                switch (gameEvent.Message)
                {
                     case "BOOSTER_UPWARDS":
                         shape.Position.Y += 0.0045f;
                         break;
                     
                    case "BOOSTER_TO_LEFT":
                        shape.Position.X -= 0.0045f;
                        break;
                    
                    case "BOOSTER_TO_RIGHT":
                        shape.Position.X += 0.0045f;
                        break;
                    
                    case "STOP_ACCELERATE_LEFT":
                        shape.Position.X += 0.0f;
                        break;

                    case "STOP_ACCELERATE_RIGHT":
                        shape.Position.X += 0.0f;
                        break;

                    case "STOP_ACCELERATE_UP":
                        shape.Position.Y += 0.0f;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
