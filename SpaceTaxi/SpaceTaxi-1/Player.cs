 using System.Collections.Generic;
using System.IO;
 using System.Xml.Schema;
 using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1
{
    public class Player : IGameEventProcessor<object>
    {
        public Entity _player { get; private set; }
        private readonly DynamicShape shape = new DynamicShape(new Vec2F(), new Vec2F());
        private int leftValue = 1;
        private int rightValue = 0;
        private int upValue = 0;
        private int totalValue;
        
     

        public Player()
        {
            //shape = new DynamicShape(new Vec2F(), new Vec2F());
            _player = new Entity(shape, PlayerImage.ImageDecider(totalValue));
            
            
        }

        public void PlayerMove()
        {
            shape.Move();
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
            if(upValue != 10)
            shape.Direction.Y = -0.0014f;
        }

        public void SetExtent(float width, float height)
        {
            shape.Extent.X = width;
            shape.Extent.Y = height;
        }

        public void RenderPlayer()
        {
            //TODO: Next version needs animation. Skipped for clarity.
            totalValue = rightValue + leftValue + upValue;
            _player.Image = PlayerImage.ImageDecider(totalValue);
            _player.RenderEntity(); 
        }
        
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent)
        {
            if (eventType == GameEventType.PlayerEvent)
            {
                switch (gameEvent.Message)
                {
                     case "BOOSTER_UPWARDS":
                         upValue = 10;
                         shape.Direction.Y = 0.0045f;
                         break;
                     
                    case "BOOSTER_TO_LEFT":
                        rightValue = 0;
                        leftValue = 2;
                        shape.Direction.X = -0.0045f;
                        
                        break;
                    
                    case "BOOSTER_TO_RIGHT":
                        leftValue = 0;
                        rightValue = -2;
                        shape.Direction.X = 0.0045f;
                        
                        break;
                    
                    case "STOP_ACCELERATE_LEFT":
                        rightValue = 0;
                        leftValue = 1;
                        shape.Direction.X = 0.0f;
                        
                        break;

                    case "STOP_ACCELERATE_RIGHT":
                        rightValue = -1;
                        leftValue = 0;
                        shape.Direction.X = 0.0f;
                        
                        break;

                    case "STOP_ACCELERATE_UP":
                        upValue = 0;
                        shape.Direction.Y = 0.0f;
                        
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
