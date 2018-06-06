using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using DIKUArcade.Timers;
using SpaceTaxi_1.Collision;
using SpaceTaxi_1.Entities;
using SpaceTaxi_1.Entities.Passenger;
using SpaceTaxi_1.Entities.Player;
using Image = DIKUArcade.Graphics.Image;

namespace SpaceTaxi_1.States {
    
    public class GameRunning : IGameState {
        
        //Fields
        private readonly Player _player;
        private Passenger _passenger;
        private readonly Entity _backGroundImage;
        private readonly List<float> _playerXcoordinates;
        private readonly List<float> _playerYcoordinates;
        private Dictionary<char, string> _legendsDictionary;
        private static int _levelCounter;
        private readonly List<EntityContainer> _levels;
        private readonly List<EntityContainer> _levelobstacles;
        private readonly List<EntityContainer> _levelplatforms;
        private readonly string[] _filePath = Directory.GetFiles("Levels");
        
        GameTimer _gameTimer = new GameTimer(60, 60);
        private static GameRunning _instance;
        private readonly List<CollisionChecker> _levelCollisionCheckers;
        private readonly List<TimeHandler> _timerHandlers;
        private readonly List<List<Passenger>> _levelPassengers;
        private List<Dictionary<int, string>> _passengerListInfo;
        private readonly List<Dictionary<char, List<Entity>>> _levelDiffrentPlatforms;
        private bool _deathBool = true;
        private TimedEvent _timeBeforeGameOver;
        private static Text _cashLabel;
        private static int _cash;
        
        private System.Timers.Timer _deathtimer;
      
        private List<string[]> _levelInfo;


        /// <summary>
        ///GetInstance looks if there is any control instance. If it's not the case it returns
        /// new GameControls 
        /// </summary>
        /// <returns>Either the game instance or a new if its null</returns>       
        public static GameRunning GetInstance() {
            return GameRunning._instance ?? (GameRunning._instance = new GameRunning());
        }
        
        /// <summary>
        /// Resets the Game instance of GameRunning
        /// </summary>
        public static void ResetGameRunning() {
            GameRunning._instance = null;
        }

        private GameRunning() {
            
            _player = new Player();
            _player.SetExtent(0.05f, 0.05f);
            
            GameRunning._levelCounter = GameLevels.Levelcount;
            
            _backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "GameSpaceBaground.png")));
            
            _cash = 0; 
            _cashLabel = new Text($"Cash: {_cash}", new Vec2F(0.8f,0.004f),new Vec2F(0.2f,0.2f));
            _cashLabel.SetColor(Color.DarkRed);
            PassengerCollision.SetPassengerPickupsToNew();
            SpaceBus.GetBus().Subscribe(GameEventType.PlayerEvent, _player);
            
            _levelCollisionCheckers = new List<CollisionChecker>();
            _timerHandlers = new List<TimeHandler>();
            _levels = Game.GetLevels();
            _levelobstacles = Game.GetLevelObstacles();
            _levelplatforms = Game.GetLevelPlatforms();
            _levelPassengers = new List<List<Passenger>>();
            _levelDiffrentPlatforms = Game.GetLevelDiffrentPlatforms();
               
            _playerXcoordinates = Game.GetPlayerXCoordinates();
            _playerYcoordinates = Game.GetPlayerYCoordinates();

            _levelInfo = Game.GetLevelInfo();
            
            _passengerListInfo = new List<Dictionary<int, string>>();
            
            _deathtimer = new System.Timers.Timer(interval: 1000);

            for (int i = 0; i < _filePath.Length; i++)
            {
                _levelPassengers.Add(new List<Passenger>());
            }
            
            for (int cp = 0; cp < _filePath.Length; cp++){
                CreatePassenger(cp);  
            }
            
            for (int clc = 0; clc < _levels.Count; clc++)
            {
                CreateLevelCollision(clc);
            }

            for (int cth = 0; cth < _levels.Count; cth++)
            {
                CreateTimerHandlers(cth);
            }
            
            _player.SetPosition(_playerXcoordinates[_levelCounter], _playerYcoordinates[_levelCounter]);
            
        }

        
        /// <summary>
        /// This method takes two arguments and render the map by using the methods from the level class. 
        /// </summary>
        /// <param name="levelNum"> Number of the level in the level container </param>
        private void CheckGameOver(int levelNum)
        {

            if (_levelCollisionCheckers[levelNum].GetGameOverChecker()) {
                ObstacleCollision.CreateExplosion(_player);
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.GameStateEvent, this, "GAME_OVER", "", ""));               
            }
        }


        /// <summary>
        /// Creates the passenger and all its features for the game.
        /// </summary>
        /// <param name="filePathNum">The number on the file we are reading</param>
        private void CreatePassenger(int filePathNum)
        {
            Level.UpdatePassengerInfo(_levelInfo[filePathNum]);
            
            _passengerListInfo = Level.GetPassengerInfo();

            foreach (var passengerinfo in _passengerListInfo)
            {
                _passenger = new Passenger(passengerinfo[1], int.Parse(passengerinfo[2]), char.Parse(passengerinfo[3]),
                    passengerinfo[4], int.Parse(passengerinfo[5]), int.Parse(passengerinfo[6]),
                    _levelDiffrentPlatforms, filePathNum);
                _passenger.SetExtent(0.02f, 0.05f);

                _passenger.SetPosition(
                    _levelDiffrentPlatforms[filePathNum][_passenger.GetPlatFormSpawn()][4].Shape.Position.X,
                    _levelDiffrentPlatforms[filePathNum][_passenger.GetPlatFormSpawn()][4].Shape.Position.Y + 0.035f);

                _levelPassengers[filePathNum].Add(_passenger);
            }
        }

        /// <summary>
        /// Creates the collision of entities in the level.
        /// </summary>
        /// <param name="levelNum">The number of the current level </param>
        private void CreateLevelCollision( int levelNum)
        {
             _levelCollisionCheckers.Add(new CollisionChecker(_levelobstacles[levelNum], _levelplatforms[levelNum],
             _player,_levelPassengers[levelNum], _levelDiffrentPlatforms));
        }

        private void CreateTimerHandlers(int levelNum)
        {
            _timerHandlers.Add(new TimeHandler(_levelPassengers[levelNum]));
        }

        private void AddCash()
        {
            foreach (Passenger passenger in PassengerCollision.GetPassengerPickups())
            {
                if (passenger.GetreleasedWithinTimer())
                {
                    _cash += passenger.GetPoints();
                    _cashLabel = new Text($"Cash: {_cash}", new Vec2F(0.8f,0.004f),new Vec2F(0.2f,0.2f));
                    _cashLabel.SetColor(Color.DarkRed);
                    passenger.SetReleasedWithinTimer(false);
                } else if (passenger.PickedUp && passenger.GetReleasePlatformChar() == '^' &&
                           _player.GetsShape().Position.Y >= 0.98f && _levelCounter == _levelInfo.Count - 1 && 
                           passenger.GetChecker())
                {
                    _cash += passenger.GetPoints();
                    _cashLabel = new Text($"Cash: {_cash}", new Vec2F(0.8f,0.004f),new Vec2F(0.2f,0.2f));
                    _cashLabel.SetColor(Color.DarkRed);
                    passenger.SetChecker(false);
                }
            }
            
        }

        
        /// <summary>
        ///GetInstance looks if there is any control instance. If it's not the case it returns
        /// new GameControls 
        /// </summary>
        /// <returns>Either the game instance or a new if its null</returns>
        public static void ResetGameInstance()
        {
            GameRunning._instance = new GameRunning();
        }
      
        /// <summary>
        /// Left empty because use of IGameState. 
        /// </summary>
        public void GameLoop() {
        // Left empty on purpose
        }

        /// <summary>
        /// Method which returns LevelCounter
        /// </summary>
        /// <returns>LevelCounter</returns>
        public static int GetLevelCounter()
        {
            return _levelCounter;
        }

        public static int GetCash()
        {
            return _cash;
        }


        /// <summary>
        /// Method that updates all logic features, which could be gravity.
        /// </summary>
        public void UpdateGameLogic()
        {
            if (_player.GetsShape().Position.Y >= 0.98f && _levelCounter == _levelInfo.Count - 1)
            {
                
                AddCash();
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.GameStateEvent, this, "GAME_VICTORY", "", ""));
            }
            CheckGameOver(_levelCounter);
            RenderState();
            _player.Physics();
            AddCash();
            foreach (Passenger passenger in _levelPassengers[_levelCounter])
            {
                passenger.PassengerMove();
            }

            _levelCollisionCheckers[_levelCounter].CheckCollsion();
            _timerHandlers[_levelCounter].SpawnPassengerTimer();
            _timerHandlers[_levelCounter].SetPickUpTimer();
            CheckGameOver(_levelCounter);
            if (_player.GetsShape().Position.Y >= 1.0f)
            {
                _levelCounter++;
                _player.SetPosition(_playerXcoordinates[_levelCounter], _playerYcoordinates[_levelCounter]);
            } 
        }

        
        /// <summary>
        /// Render the different entities and features.
        /// </summary>
        public void RenderState() {
            _backGroundImage.RenderEntity();
            _cashLabel.RenderText();
            foreach (Passenger passenger in _levelPassengers[_levelCounter])
            {
                passenger.RenderPassenger();
            }
            foreach (Passenger passenger in PassengerCollision.GetPassengerPickups())
            {
                passenger.RenderPassenger();
            }
            _player.RenderPlayer();  
            _levels[_levelCounter].RenderEntities();
            ObstacleCollision.GetExplosion().RenderAnimations();
        }
        
        /// <summary>
        /// Left empty because use of IGameState./ 
        /// </summary>
        public void InitializeGameState() {
            // Left empty on purpose
        }

        
        /// <summary>
        /// Handles the key events. For key up, key down and enter.
        /// </summary>
        /// <param name="keyValue">The given key pressed</param>
        /// <param name="keyAction">Registers if a certain button is pressed or released</param>
        public void HandleKeyEvent(string keyValue, string keyAction) {
            if (keyAction == "KEY_PRESS") { }

            switch (keyValue) {
            case "KEY_UP":
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "BOOSTER_UPWARDS", "", ""));
                break;
            case "KEY_LEFT":
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "BOOSTER_TO_LEFT", "", ""));
                break;
            case "KEY_RIGHT":
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "BOOSTER_TO_RIGHT", "", ""));
                break;
            case "KEY_ESCAPE":
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.GameStateEvent, this, "GAME_PAUSED", "", "")); 
                break;
            }

            if (keyAction == "KEY_RELEASE") {
                switch (keyValue) {
                case "KEY_LEFT":
                    SpaceBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "STOP_ACCELERATE_LEFT", "", ""));
                    break;
                case "KEY_RIGHT":
                    SpaceBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "STOP_ACCELERATE_RIGHT", "", ""));
                    break;
                case "KEY_UP":
                    SpaceBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "STOP_ACCELERATE_UP", "", ""));
                    break;
                    
                default: 
                    break;
                }
                
            }
        }
      
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
           // Left empty on purpose.         
        }
    }
}


            
    