using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1
{
    /// <summary>
    /// 
    /// </summary>
    public class Player : IGameEventProcessor<object>
    {
        //Fields
        //private readonly DynamicShape shape = new DynamicShape(new Vec2F(), new Vec2F());
        
        public Entity _player { get; private set; }
        
        private int leftValue = 1;
        private int rightValue = 0;
        private int upValue = 0;
        private int totalValue;
        private Vec2F gravity;
        private Vec2F boostForce;
        public bool alive = true;
        private readonly DynamicShape shape;
        private readonly Image taxiBoosterOffImageLeft;
        private readonly Image taxiBoosterOffImageRight;

        
        /// <summary>
        /// 
        /// </summary>
        public Player()
        {
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            _player = new Entity(shape, PlayerImage.ImageDecider(totalValue));
            gravity = new Vec2F(0.0f, -0.3f);
            boostForce =  new Vec2F(0.0f, 0.0f);
        }

        public void Physics() {
            if (alive) {
                var netForce = boostForce + gravity;
                shape.Direction +=
                    netForce * (Game.keepTrackOfUpdates / 300000.0f);
                _player.Shape.Move();
            }
        }

        public void PlayerMove()
        {
            _player.Shape.Move();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPosition(float x, float y)
        {
            shape.Position.X = x;
            shape.Position.Y = y;         
        }
        
        public void Changephysics()
        {
            gravity = new Vec2F(0.0f, 0.0f);
            shape.Direction = new Vec2F(0.0f,0.0f);
        }

        public void ChangeGravity(float newGravity)
        {
            gravity = new Vec2F(0.0f, newGravity);
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DynamicShape GetsShape()
        {
            return shape;
        }
      
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetExtent(float width, float height)
        {
            shape.Extent.X = width;
            shape.Extent.Y = height;
        }

        
        /// <summary>
        /// 
        /// </summary>
        public void RenderPlayer()
        {
            if (alive) {
                totalValue = rightValue + leftValue + upValue;
                _player.Image = PlayerImage.ImageDecider(totalValue);
                _player.RenderEntity(); 
            }
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="gameEvent"></param>
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent)
        {
            if (eventType == GameEventType.PlayerEvent)
            {
                switch (gameEvent.Message)
                {
                     case "BOOSTER_UPWARDS":
                         boostForce.Y += 1;
                         gravity = new Vec2F(0.0f,-0.3f);
                         upValue = 10;
                         break;
                     
                    case "BOOSTER_TO_LEFT":
                        if (shape.Direction.Y == 0.0f) {
                            
                            boostForce.X = 0;
                        } else {
                            boostForce.X -= 1;
                        }
                        rightValue = 0;
                         leftValue = 2;
                         break;
                    
                    case "BOOSTER_TO_RIGHT":            
                        if (shape.Direction.Y == 0.0f) {
                            
                            boostForce.X = 0;
                        } else {
                            boostForce.X += 1;
                        }
                        leftValue = 0;
                        rightValue = -2;
                        break;
                    
                    case "STOP_ACCELERATE_LEFT":
                        boostForce.X = 0;
                        rightValue = 0;
                        leftValue = 1;
                        break;

                    case "STOP_ACCELERATE_RIGHT":
                        boostForce.X = 0;
                        rightValue = -1;
                        leftValue = 0;
                        break;

                    case "STOP_ACCELERATE_UP":
                        boostForce.Y = 0;
                        upValue = 0;
                        break;
                    
                    default:
                        break;
                }
            }
        }
    }
}
