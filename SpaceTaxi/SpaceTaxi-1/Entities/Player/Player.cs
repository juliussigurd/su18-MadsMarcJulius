using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Math;

namespace SpaceTaxi_1.Entities.Player
{
    public class Player : IGameEventProcessor<object>
    {
        //Fields
        //private readonly DynamicShape shape = new DynamicShape(new Vec2F(), new Vec2F());

        private Entity _player { get; set; }
        
        private int _leftValue = 1;
        private int _rightValue = 0;
        private int _upValue = 0;
        private int _totalValue;
        private Vec2F _gravity;
        private readonly Vec2F _boostForce;
        public bool Alive = true;
        private readonly DynamicShape _shape;
        
        
        /// <summary>
        /// Values and features for player
        /// </summary>
        public Player()
        {
            _shape = new DynamicShape(new Vec2F(), new Vec2F());
            _player = new Entity(_shape, PlayerImage.ImageDecider(_totalValue));
            _gravity = new Vec2F(0.0f, -0.3f);
            _boostForce =  new Vec2F(0.0f, 0.0f);
        }

        /// <summary>
        /// Creates gravity which makes the player fall downwards.
        /// Does this with a vector with boostForce. And calls PlayerMove.
        /// </summary>
        public void Physics() {
            if (Alive) {
                var netForce = _boostForce + _gravity;
                _shape.Direction +=
                    netForce * (Game.KeepTrackOfUpdates / 300000.0f);
                PlayerMove();
            }
        }

        /// <summary>
        /// Moves the player. 
        /// </summary>
        public void PlayerMove()
        {
            if (_shape.Direction.Y == 0.0f){
                _shape.Direction.X = 0.0f;
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
            _shape.Position.X = x;
            _shape.Position.Y = y;         
        }
        
        /// <summary>
        /// Makes it possible to change the physics if need so it is possible for the player
        /// to stand still without falling. 
        /// </summary>
        public void Changephysics()
        {
            _gravity = new Vec2F(0.0f, 0.0f);
            _shape.Direction = new Vec2F(0.0f,0.0f);
        }
        
        /// <summary>
        /// Gets the shap of the player 
        /// </summary>
        /// <returns>shape</returns>
        public DynamicShape GetsShape()
        {
            return _shape;
        }
      
        
        /// <summary>
        /// sets the extent of the player 
        /// </summary>
        /// <param name="width">players extend x value</param>
        /// <param name="height">players extend y value</param>
        public void SetExtent(float width, float height)
        {
            _shape.Extent.X = width;
            _shape.Extent.Y = height;
        }
        
        /// <summary>
        /// Render the player 
        /// </summary>
        public void RenderPlayer()
        {
            if (Alive) {
                _totalValue = _rightValue + _leftValue + _upValue;
                _player.Image = PlayerImage.ImageDecider(_totalValue);
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
                         _boostForce.Y += 1;
                         _gravity = new Vec2F(0.0f,-0.3f);
                         _upValue = 10;
                         break;
                     
                    case "BOOSTER_TO_LEFT":
                        if (_shape.Direction.Y == 0.0f) {
                            
                            _boostForce.X = 0;
                        } else {
                            _boostForce.X -= 1;
                        }
                        _rightValue = 0;
                         _leftValue = 2;
                         break;
                    
                    case "BOOSTER_TO_RIGHT":            
                        if (_shape.Direction.Y == 0.0f)
                            _boostForce.X = 0;
                        else
                            _boostForce.X += 1;
                        _leftValue = 0;
                        _rightValue = -2;
                        break;
                    
                    case "STOP_ACCELERATE_LEFT":
                        _boostForce.X = 0;
                        _rightValue = 0;
                        _leftValue = 1;
                        break;

                    case "STOP_ACCELERATE_RIGHT":
                        _boostForce.X = 0;
                        _rightValue = -1;
                        _leftValue = 0;
                        break;

                    case "STOP_ACCELERATE_UP":
                        _boostForce.Y = 0;
                        _upValue = 0;
                        break;
                    
                    default:
                        break;
                }
            }
        }
    }
}
