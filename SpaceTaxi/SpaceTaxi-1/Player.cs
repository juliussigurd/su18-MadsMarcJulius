using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1
{
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
       
        
        /// <summary>
        /// Values and features for player
        /// </summary>
        public Player()
        {
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            _player = new Entity(shape, PlayerImage.ImageDecider(totalValue));
            gravity = new Vec2F(0.0f, -0.3f);
            boostForce =  new Vec2F(0.0f, 0.0f);
        }

        /// <summary>
        /// Creates gravity which makes the player fall downwards.
        /// Does this with a vector with boostForce. And calls PlayerMove
        /// </summary>
        public void Physics() {
            if (alive) {
                var netForce = boostForce + gravity;
                shape.Direction +=
                    netForce * (Game.keepTrackOfUpdates / 300000.0f);
                PlayerMove();
            }
        }

        /// <summary>
        /// Moves the player 
        /// </summary>
        public void PlayerMove()
        {
            if (shape.Direction.Y == 0.0f)
            {
                shape.Direction.X = 0.0f;
            }
            _player.Shape.Move();
        }
        
        /// <summary>
        /// sets the position of the player 
        /// </summary>
        /// <param name="x">players x value</param>
        /// <param name="y">players y value</param>
        public void SetPosition(float x, float y)
        {
            shape.Position.X = x;
            shape.Position.Y = y;         
        }
        
        /// <summary>
        /// Makes it possible to change the physics if need so it is possible for the player
        /// to stand still without falling. 
        /// </summary>
        public void Changephysics()
        {
            gravity = new Vec2F(0.0f, 0.0f);
            shape.Direction = new Vec2F(0.0f,0.0f);
        }
        
        /// <summary>
        /// Gets the shap of the player 
        /// </summary>
        /// <returns>shape</returns>
        public DynamicShape GetsShape()
        {
            return shape;
        }
      
        
        /// <summary>
        /// sets the extent of the player 
        /// </summary>
        /// <param name="width">players extend x value</param>
        /// <param name="height">players extend y value</param>
        public void SetExtent(float width, float height)
        {
            shape.Extent.X = width;
            shape.Extent.Y = height;
        }
        
        /// <summary>
        /// Render the player 
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
        /// Proccesses the events of the type player event.
        /// In which it takes player inputs
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
