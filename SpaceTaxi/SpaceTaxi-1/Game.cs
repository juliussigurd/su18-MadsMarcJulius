using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Timers;
using SpaceTaxi_1.States;

namespace SpaceTaxi_1{
    public class Game : IGameEventProcessor<object>{
        //Fields
        private readonly Window _win;
        private readonly GameTimer _gameTimer;
        private readonly StateMachine _stateMachine;
        public static int KeepTrackOfUpdates;
        
        
        private readonly string[] _filePath = Directory.GetFiles("Levels");

        
        private static List<string[]> _levelInfo;
        private static List<EntityContainer> _levels;
        private static Dictionary<char, string> _legendsDictionary;
        private static List<EntityContainer> _levelObstacles;
        private static List<EntityContainer> _levelPlatforms;
        private static List<float> _playerXcoordinates;
        private static List<float> _playerYcoordinates;

        private static List<Dictionary<char,List<Entity>>> _levelDiffrentPlatforms;
        
        public Game(){

            // window
            _win = new Window("Space Taxi Game v0.1", 500, AspectRatio.R1X1);
             _stateMachine = new StateMachine();

            // event bus
            SpaceBus.GetBus().InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent,      // key press / key release
                GameEventType.WindowEvent,     // messages to the window, e.g. CloseWindow()
                GameEventType.PlayerEvent,      // commands issued to the player object, e.g. move, destroy, receive health, etc.
                GameEventType.GameStateEvent,
                GameEventType.TimedEvent
            });
            _win.RegisterEventBus(SpaceBus.GetBus());
            SpaceBus.GetBus().Subscribe(GameEventType.WindowEvent, this);
            SpaceBus.GetBus().Subscribe(GameEventType.GameStateEvent, _stateMachine);
            SpaceBus.GetBus().Subscribe(GameEventType.InputEvent, _stateMachine);

            // game timer, events
            _gameTimer = new GameTimer(60, 60); // 60 UPS, no FPS limit

            _levelInfo = new List<string[]>();
            _levels = new List<EntityContainer>();
            //_levelObstacles = new List<CollisionChecker>();
            _legendsDictionary = new Dictionary<char, string>();
            _levelDiffrentPlatforms = new List<Dictionary<char, List<Entity>>>();
            _levelObstacles = new List<EntityContainer>();
            _levelPlatforms = new List<EntityContainer>();
            _playerXcoordinates = new List<float>();
            _playerYcoordinates = new List<float>();
            
            for (int i = 0; i < _filePath.Length; i++){
                CreateMap(_filePath,i);  
            }     
        }
        
        /// <summary>
        /// Method creates a dictionary which holds all the legends from the file of the level
        /// </summary>
        /// <param name="_filePath">File of the level</param>
        /// <param name="filePathNum">File number of the level</param>
        /// <returns></returns>
        private Dictionary<char, string> CreateLegendDictionary(string[] _filePath, int filePathNum)
        {
            _levelInfo.Add(Level.ReadFile((_filePath[filePathNum])));
            _legendsDictionary = new Dictionary<char, string>();
            Level.SetLegendsDictionaryToNew();
            Level.ReadLegends(_levelInfo[filePathNum]);
            return Level.GetLegendsDictionary();
        }

        /// <summary>
        /// Method creates the map with the platform, obstacles and the player position
        /// with use of other methods such as CreateLegendDictionary
        /// </summary>
        /// <param name="_filePath">File of the level</param>
        /// <param name="filePathNum">File number of the level</param>
        private void CreateMap(string[] _filePath, int filePathNum)
        {
            _legendsDictionary = CreateLegendDictionary(_filePath, filePathNum);
            Level.ReadPlatforms(_levelInfo[filePathNum][25]);
            Console.WriteLine(Level.GetDiffrenPlatforms().Count);
            Level.AddAllEntitiesToContainer(_levelInfo[filePathNum], _legendsDictionary);
            _levels.Add(Level.GetLevelEntities());
            _levelDiffrentPlatforms.Add(Level.GetDiffrenPlatforms());
            _levelObstacles.Add(Level.GetLevelObstacles());
            _levelPlatforms.Add(Level.GetLevelPlatforms());
            
            
            _playerXcoordinates.Add(Level.GetPlayerPosX());
            _playerYcoordinates.Add(Level.GetPlayerPosY());

        }
        
        /// <summary>
        /// Method that gets the different platforms in the levels. 
        /// </summary>
        /// <returns>levelDifferentPlatforms</returns>
        public static List<Dictionary<char, List<Entity>>> GetLevelDiffrentPlatforms()
        {
            return _levelDiffrentPlatforms;
        }

        /// <summary>
        /// Get level info
        /// </summary>
        /// <returns>levelInfo</returns>
        public static List<string[]> GetLevelInfo()
        {
            return _levelInfo;
        }
        
        /// <summary>
        /// Get level obstacles
        /// </summary>
        /// <returns>levelObstacles</returns>
        public static List<EntityContainer> GetLevelObstacles()
        {
            return _levelObstacles;
        }
        
        /// <summary>
        /// get level platforms 
        /// </summary>
        /// <returns>levelPlatforms</returns>
        public static List<EntityContainer> GetLevelPlatforms()
        {
            return _levelPlatforms;
        }

        /// <summary>
        /// get levels
        /// </summary>
        /// <returns>levels</returns>
        public static List<EntityContainer> GetLevels()
        {
            return _levels;
        }

        /// <summary>
        /// Get player x coordinates
        /// </summary>
        /// <returns>playerXcoordinates</returns>
        public static List<float> GetPlayerXCoordinates()
        {
            return _playerXcoordinates;
        }
        
        /// <summary>
        /// Get player y coordinates
        /// </summary>
        /// <returns>playerYcoordinates</returns>
        public static List<float> GetPlayerYCoordinates()
        {
            return _playerYcoordinates;
        }

       /// <summary>
       /// Runs all the features and methods that are being used in the game.
       /// Takes the active states features and methods and render those. 
       /// </summary>
        public void GameLoop(){
            while (_win.IsRunning()){
                _gameTimer.MeasureTime();
                while (_gameTimer.ShouldUpdate()){
                    _win.PollEvents();
                    SpaceBus.GetBus().ProcessEvents();
                    _stateMachine.ActiveState.UpdateGameLogic();  
                }

                if (_gameTimer.ShouldRender())
                {
                    _win.Clear();
                    _stateMachine.ActiveState.RenderState();
                    _win.SwapBuffers();
                }

                if (_gameTimer.ShouldReset()){
                    // 1 second has passed - display last captured ups and fps from the timer
                    _win.Title = "Space Taxi | UPS: " + _gameTimer.CapturedUpdates + ", FPS: " +
                                _gameTimer.CapturedFrames;
                    KeepTrackOfUpdates = _gameTimer.CapturedUpdates;
                }
            }
        }
        
        /// <summary>
        /// Method that processes window events. 
        /// </summary>
        /// <param name="eventType">which kind of event, such as window event</param>
        /// <param name="gameEvent">The different state events</param>
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message){
                    case "CLOSE_WINDOW":
                        _win.CloseWindow();
                        break;
                default: 
                    break;
                }
            }
        }
    }
}
