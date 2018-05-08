using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1
{
    public class Player : IGameEventProcessor<object>
    {
        public Entity Entity { get; }
        private readonly DynamicShape shape;
        private readonly Image taxiBoosterOffImageLeft;
        private readonly Image taxiBoosterOffImageRight;
        private Orientation taxiOrientation;
        

        public Player()
        {
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            taxiBoosterOffImageLeft = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
            taxiBoosterOffImageRight = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png"));

            Entity = new Entity(shape, taxiBoosterOffImageLeft);
            
        }

        public void PlayerMove()
        {
            shape.Move();
        }

        public double PlayerStopX() {
            return shape.Direction.X = 0.0f;
        }
        
        public double PlayerStopY() {
            return shape.Direction.Y = 0.0f;
        }

        public void SetPosition(float x, float y)
        {
            shape.Position.X = x;
            shape.Position.Y = y;
        }

        public DynamicShape GetsShape()
        {
            return shape;
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
            Entity.Image = taxiOrientation == Orientation.Left
                ? taxiBoosterOffImageLeft
                : taxiBoosterOffImageRight;
            Entity.RenderEntity();
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent)
        {
            if (eventType == GameEventType.PlayerEvent)
            {
                switch (gameEvent.Message)
                {
                    case "BOOSTER_UPWARDS":
                         shape.Direction.Y += 0.0045f;
                         break;
                     
                    case "BOOSTER_TO_LEFT":
                        shape.Direction.X -= 0.0045f;
                        taxiOrientation = Orientation.Left;
                        break;
                    
                    case "BOOSTER_TO_RIGHT":
                        shape.Direction.X += 0.0045f;
                        taxiOrientation = Orientation.Right;
                        break;
                    
                    case "STOP_ACCELERATE_X":
                        PlayerStopX();
                        break;
                        
                    case "STOP_ACCELERATE_Y":
                        PlayerStopY();
                        break;  
                        
                    default:
                        break;
                }
            }
        }
    }
}
