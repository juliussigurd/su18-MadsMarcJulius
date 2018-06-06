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
        private readonly Window win;
        private readonly GameTimer gameTimer;
        private readonly StateMachine stateMachine;
        public static int KeepTrackOfUpdates;
        
        private readonly string[] filePath = Directory.GetFiles("Levels");
        
        private static List<string[]> levelInfo;
        private static List<EntityContainer> levels;
        private static Dictionary<char, string> legendsDictionary;
        private static List<EntityContainer> levelObstacles;
        private static List<EntityContainer> levelPlatforms;
        private static List<float> playerXcoordinates;
        private static List<float> playerYcoordinates;

        private static List<Dictionary<char,List<Entity>>> levelDiffrentPlatforms;
        
        public Game(){

            // window
            win = new Window("Space Taxi Game v0.1", 500, AspectRatio.R1X1);
             stateMachine = new StateMachine();

            // event bus
            SpaceBus.GetBus().InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent,      // key press / key release
                GameEventType.WindowEvent,     // messages to the window, e.g. CloseWindow()
                GameEventType.PlayerEvent,      // commands issued to the player object, e.g. move, destroy, receive health, etc.
                GameEventType.GameStateEvent,
                GameEventType.TimedEvent
            });
            win.RegisterEventBus(SpaceBus.GetBus());
            SpaceBus.GetBus().Subscribe(GameEventType.WindowEvent, this);
            SpaceBus.GetBus().Subscribe(GameEventType.GameStateEvent, stateMachine);
            SpaceBus.GetBus().Subscribe(GameEventType.InputEvent, stateMachine);

            // game timer, events
            gameTimer = new GameTimer(60, 60); // 60 UPS, no FPS limit

            Game.levelInfo = new List<string[]>();
            Game.levels = new List<EntityContainer>();
            //_levelObstacles = new List<CollisionChecker>();
            Game.legendsDictionary = new Dictionary<char, string>();
            Game.levelDiffrentPlatforms = new List<Dictionary<char, List<Entity>>>();
            Game.levelObstacles = new List<EntityContainer>();
            Game.levelPlatforms = new List<EntityContainer>();
            Game.playerXcoordinates = new List<float>();
            Game.playerYcoordinates = new List<float>();
            
            for (var i = 0; i < filePath.Length; i++){
                CreateMap(filePath,i);  
            }     
        }
        
        /// <summary>
        /// Method creates a dictionary which holds all the legends from the file of the level
        /// </summary>
        /// <param name="_filePath">File of the level</param>
        /// <param name="filePathNum">File number of the level</param>
        /// <returns></returns>
        private Dictionary<char, string> CreateLegendDictionary(string[] _filePath, int filePathNum){
            Game.levelInfo.Add(Level.ReadFile(_filePath[filePathNum]));
            Game.legendsDictionary = new Dictionary<char, string>();
            Level.SetLegendsDictionaryToNew();
            Level.ReadLegends(Game.levelInfo[filePathNum]);
            return Level.GetLegendsDictionary();
        }

        /// <summary>
        /// Method creates the map with the platform, obstacles and the player position
        /// with use of other methods such as CreateLegendDictionary
        /// </summary>
        /// <param name="_filePath">File of the level</param>
        /// <param name="filePathNum">File number of the level</param>
        private void CreateMap(string[] _filePath, int filePathNum){
            Game.legendsDictionary = CreateLegendDictionary(_filePath, filePathNum);
            Level.ReadPlatforms(Game.levelInfo[filePathNum][25]);
            Level.AddAllEntitiesToContainer(Game.levelInfo[filePathNum], Game.legendsDictionary);
            Game.levels.Add(Level.GetLevelEntities());
            Game.levelDiffrentPlatforms.Add(Level.GetDiffrenPlatforms());
            Game.levelObstacles.Add(Level.GetLevelObstacles());
            Game.levelPlatforms.Add(Level.GetLevelPlatforms());
            
            
            Game.playerXcoordinates.Add(Level.GetPlayerPosX());
            Game.playerYcoordinates.Add(Level.GetPlayerPosY());

        }
        
        /// <summary>
        /// Method that gets the different platforms in the levels. 
        /// </summary>
        /// <returns>levelDifferentPlatforms</returns>
        public static List<Dictionary<char, List<Entity>>> GetLevelDiffrentPlatforms(){
            return Game.levelDiffrentPlatforms;
        }

        /// <summary>
        /// Get level info
        /// </summary>
        /// <returns>levelInfo</returns>
        public static List<string[]> GetLevelInfo(){
            return Game.levelInfo;
        }
        
        /// <summary>
        /// Get level obstacles
        /// </summary>
        /// <returns>levelObstacles</returns>
        public static List<EntityContainer> GetLevelObstacles(){
            return Game.levelObstacles;
        }
        
        /// <summary>
        /// get level platforms 
        /// </summary>
        /// <returns>levelPlatforms</returns>
        public static List<EntityContainer> GetLevelPlatforms(){
            return Game.levelPlatforms;
        }

        /// <summary>
        /// get levels
        /// </summary>
        /// <returns>levels</returns>
        public static List<EntityContainer> GetLevels(){
            return Game.levels;
        }

        /// <summary>
        /// Get player x coordinates
        /// </summary>
        /// <returns>playerXcoordinates</returns>
        public static List<float> GetPlayerXCoordinates(){
            return Game.playerXcoordinates;
        }
        
        /// <summary>
        /// Get player y coordinates
        /// </summary>
        /// <returns>playerYcoordinates</returns>
        public static List<float> GetPlayerYCoordinates(){
            return Game.playerYcoordinates;
        }

       /// <summary>
       /// Runs all the features and methods that are being used in the game.
       /// Takes the active states features and methods and render those. 
       /// </summary>
        public void GameLoop(){
            while (win.IsRunning()){
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()){
                    win.PollEvents();
                    SpaceBus.GetBus().ProcessEvents();
                    stateMachine.ActiveState.UpdateGameLogic();  
                }

                if (gameTimer.ShouldRender()){
                    win.Clear();
                    stateMachine.ActiveState.RenderState();
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()){
                    // 1 second has passed - display last captured ups and fps from the timer
                    win.Title = "Space Taxi | UPS: " + gameTimer.CapturedUpdates + ", FPS: " +
                                gameTimer.CapturedFrames;
                    Game.KeepTrackOfUpdates = gameTimer.CapturedUpdates;
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
                        win.CloseWindow();
                        break;
                default: 
                    break;
                }
            }
        }
    }
}
