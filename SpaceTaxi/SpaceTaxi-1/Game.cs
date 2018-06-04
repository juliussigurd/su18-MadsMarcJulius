using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;

namespace SpaceTaxi_1{
    public class Game : IGameEventProcessor<object>{
        //Fields
        private Window _win;
        private GameTimer _gameTimer;
        private Player _player;
        private StateMachine stateMachine;
        public static int keepTrackOfUpdates;
        
        
        private string[] filePath = Directory.GetFiles("Levels");

        
        private static List<string[]> levelInfo;
        private static List<EntityContainer> levels;
        //private static List<CollisionChecker> _levelObstacles;
        private static Dictionary<char, string> legendsDictionary;
        private static List<EntityContainer> levelObstacles;
        private static List<EntityContainer> levelPlatforms;
        private static List<float> playerXcoordinates;
        private static List<float> playerYcoordinates;

        private static List<Dictionary<char,List<Entity>>> levelDiffrentPlatforms;
        
        //TODO: Er XML nødvendigt i game?
        public Game(){

            // window
            _win = new Window("Space Taxi Game v0.1", 500, AspectRatio.R1X1);
             stateMachine = new StateMachine();

            // event bus
            SpaceBus.GetBus().InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent,      // key press / key release
                GameEventType.WindowEvent,     // messages to the window, e.g. CloseWindow()
                GameEventType.PlayerEvent,      // commands issued to the player object, e.g. move, destroy, receive health, etc.
                GameEventType.GameStateEvent
            });
            _win.RegisterEventBus(SpaceBus.GetBus());
            SpaceBus.GetBus().Subscribe(GameEventType.WindowEvent, this);
            SpaceBus.GetBus().Subscribe(GameEventType.GameStateEvent, stateMachine);
            SpaceBus.GetBus().Subscribe(GameEventType.InputEvent, stateMachine);

            // game timer, events
            _gameTimer = new GameTimer(60, 60); // 60 UPS, no FPS limit

            levelInfo = new List<string[]>();
            levels = new List<EntityContainer>();
            //_levelObstacles = new List<CollisionChecker>();
            legendsDictionary = new Dictionary<char, string>();
            levelDiffrentPlatforms = new List<Dictionary<char, List<Entity>>>();
            levelObstacles = new List<EntityContainer>();
            levelPlatforms = new List<EntityContainer>();
            playerXcoordinates = new List<float>();
            playerYcoordinates = new List<float>();
            
            for (int i = 0; i < filePath.Length; i++){
                CreateMap(filePath,i);  
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
            levelInfo.Add(Level.ReadFile((_filePath[filePathNum])));
            legendsDictionary = new Dictionary<char, string>();
            Level.SetLegendsDictionaryToNew();
            Level.ReadLegends(levelInfo[filePathNum]);
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
            legendsDictionary = CreateLegendDictionary(_filePath, filePathNum);
            Level.ReadPlatforms(levelInfo[filePathNum][25]);
            Console.WriteLine(Level.GetDiffrenPlatforms().Count);
            Level.AddAllEntitiesToContainer(levelInfo[filePathNum], legendsDictionary);
            levels.Add(Level.GetLevelEntities());
            levelDiffrentPlatforms.Add(Level.GetDiffrenPlatforms());
            levelObstacles.Add(Level.GetLevelObstacles());
            levelPlatforms.Add(Level.GetLevelPlatforms());
            
            
            playerXcoordinates.Add(Level.GetPlayerPosX());
            playerYcoordinates.Add(Level.GetPlayerPosY());

        }
        
        /// <summary>
        /// Method that gets the different platforms in the levels. 
        /// </summary>
        /// <returns>levelDifferentPlatforms</returns>
        public static List<Dictionary<char, List<Entity>>> GetLevelDiffrentPlatforms()
        {
            return levelDiffrentPlatforms;
        }

        /// <summary>
        /// Get level info
        /// </summary>
        /// <returns>levelInfo</returns>
        public static List<string[]> GetLevelInfo()
        {
            return levelInfo;
        }
        
        /// <summary>
        /// Get level obstacles
        /// </summary>
        /// <returns>levelObstacles</returns>
        public static List<EntityContainer> GetLevelObstacles()
        {
            return levelObstacles;
        }
        
        /// <summary>
        /// get level platforms 
        /// </summary>
        /// <returns>levelPlatforms</returns>
        public static List<EntityContainer> GetLevelPlatforms()
        {
            return levelPlatforms;
        }

        /// <summary>
        /// get levels
        /// </summary>
        /// <returns>levels</returns>
        public static List<EntityContainer> GetLevels()
        {
            return levels;
        }

        /// <summary>
        /// Get player x coordinates
        /// </summary>
        /// <returns>playerXcoordinates</returns>
        public static List<float> GetPlayerXCoordinates()
        {
            return playerXcoordinates;
        }
        
        /// <summary>
        /// Get player y coordinates
        /// </summary>
        /// <returns>playerYcoordinates</returns>
        public static List<float> GetPlayerYCoordinates()
        {
            return playerYcoordinates;
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
                    stateMachine.ActiveState.UpdateGameLogic();  
                }

                if (_gameTimer.ShouldRender())
                {
                    _win.Clear();
                    stateMachine.ActiveState.RenderState();
                    _win.SwapBuffers();
                }

                if (_gameTimer.ShouldReset()){
                    // 1 second has passed - display last captured ups and fps from the timer
                    _win.Title = "Space Taxi | UPS: " + _gameTimer.CapturedUpdates + ", FPS: " +
                                _gameTimer.CapturedFrames;
                    Game.keepTrackOfUpdates = _gameTimer.CapturedUpdates;
                }
            }
        }
        
        /// <summary>
        /// Method that processes window events. 
        /// </summary>
        /// <param name="eventType">which kind of event, such as input event</param>
        /// TODO: Tjek lige gameEvent om det er rigtigt.
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
