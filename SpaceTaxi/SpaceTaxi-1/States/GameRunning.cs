using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using DIKUArcade.Timers;

namespace SpaceTaxi_1.States {
    
    /// <summary>
    /// 
    /// </summary>
    public class GameRunning : IGameEventProcessor<object>, IGameState {
        
        //Fields
        private Player player;
        private Entity backGroundImage;
        private List<float> playerXcoordinates;
        private List<float> playerYcoordinates;
        private Dictionary<char, string> legendsDictionary;
        public int LevelCounter = GameLevels.Levelcount;
        private List<EntityContainer> levels;
        private List<EntityContainer> levelobstacles;
        private List<EntityContainer> levelplatforms;
        private string[] filePath = Directory.GetFiles("Levels");
        private string[] levelInfo;
        GameTimer gameTimer = new GameTimer(60, 60);
        private static GameRunning instance;
        private List<CollisionChecker> _levelObstacles;
        private CollisionChecker _collisionChecker;

        
        /// <summary>
        /// 
        /// </summary>
        public static void ResetGameRunning() {
            GameRunning.instance = null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public GameRunning() {
            // new player
            // game entities
            player = new Player();
            player.SetExtent(0.1f, 0.1f);
            // defines the different events

            // game assets
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
            
            SpaceBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            SpaceBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
            
            // _level = new Level();
            _levelObstacles = new List<CollisionChecker>();
            levels = new List<EntityContainer>();
            levelobstacles = new List<EntityContainer>();
            levelplatforms = new List<EntityContainer>();
            playerXcoordinates = new List<float>();
            playerYcoordinates = new List<float>();

            for (int i = 0; i < filePath.Length; i++){
                CreateMap(filePath,i);  
            }
            
            // creating obstacles
            for (int i = 0; i < levels.Count; i++)
            {
                CreateLevelCollision(filePath,i);
            }
            
            player.SetPosition(playerXcoordinates[LevelCounter], playerYcoordinates[LevelCounter]);
            
        }
        
        /// <summary>
        /// This method takes two arguments and render the map by using the methods from the level class. 
        /// </summary>
        /// <param name="filePath"> Directory of levels </param>
        /// <param name="filePathNum"> Number of the level in the level container </param>
      
        private Dictionary<char, string> CreateLegendDictionary(string[] _filePath, int filePathNum)
        {
            levelInfo = Level.ReadFile((_filePath[filePathNum]));
            legendsDictionary = new Dictionary<char, string>();
            Level.SetDictionaryToNew();
            Level.ReadLegends(levelInfo);
            return Level.GetLegendsDictionary();
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_filePath"></param>
        /// <param name="filePathNum"></param>
        private void CreateMap(string[] _filePath, int filePathNum)
        {
            legendsDictionary = CreateLegendDictionary(_filePath, filePathNum);
            Level.ReadPlatforms(levelInfo[25]);
            Level.AddAllEntitiesToContainer(levelInfo, legendsDictionary);
            levels.Add(Level.GetLevelEntities());
            levelobstacles.Add(Level.GetLevelObstacles());
            levelplatforms.Add(Level.GetLevelPlatforms());
            
            playerXcoordinates.Add(Level.GetPlayerPosX());
            playerYcoordinates.Add(Level.GetPlayerPosY());
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_filePath"></param>
        /// <param name="levelNum"></param>
        private void CreateLevelCollision(string[] _filePath, int levelNum)
        {
            legendsDictionary = CreateLegendDictionary(_filePath, levelNum);
            _levelObstacles.Add(new CollisionChecker(levelobstacles[levelNum], levelplatforms[levelNum],
                player));
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        public void GameLoop() {
        
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        public void UpdateGameLogic() {
            RenderState();
            player.Physics();
            _levelObstacles[LevelCounter].CheckCollsion();
            if (player.GetsShape().Position.Y >= 1.0f){
                LevelCounter++;
                player.SetPosition(playerXcoordinates[LevelCounter], playerYcoordinates[LevelCounter]);
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        public void RenderState() {
            backGroundImage.RenderEntity();
            player.RenderPlayer();  
            levels[LevelCounter].RenderEntities();
        }


        /// <summary>
        /// 
        /// </summary>
        public void InitializeGameState() {
            // Left empty on purpose
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="keyAction"></param>
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
                
            case "KEY_O":
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.GameStateEvent, this, "GAME_OVER", "", "")); 
                break;
                
            default: 
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
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="gameEvent"></param>
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.InputEvent) {
                // if event input is called, process here
                switch (gameEvent.Message) { }
            }
        }
    }
}


            
    