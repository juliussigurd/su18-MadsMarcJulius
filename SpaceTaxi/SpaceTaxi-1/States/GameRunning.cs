using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Threading;
using System.Timers;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using DIKUArcade.Timers;

namespace SpaceTaxi_1.States {
    
    public class GameRunning : IGameEventProcessor<object>, IGameState {
        
        //Fields
        //TODO: Bedre navngiving!
        private Player player;
        private Passenger passenger;
        private Entity backGroundImage;
        private List<float> playerXcoordinates;
        private List<float> playerYcoordinates;
        private Dictionary<char, string> legendsDictionary;
        private static int LevelCounter = GameLevels.Levelcount;
        private List<EntityContainer> levels;
        private List<EntityContainer> levelobstacles;
        private List<EntityContainer> levelplatforms;
        private string[] filePath = Directory.GetFiles("Levels");
        
        GameTimer gameTimer = new GameTimer(60, 60);
        private static GameRunning instance;
        private List<CollisionChecker> _levelCollisionCheckers;
        private CollisionChecker _collisionChecker;
        private List<List<Passenger>> levelPassengers;
        private List<Dictionary<int, string>> passengerListInfo;
        private List<Dictionary<char, List<Entity>>> levelDiffrentPlatforms;
        
        private System.Timers.Timer deathtimer;
        
        
        private List<string[]> levelInfo;


        /// <summary>
        ///GetInstance looks if there is any control instance. If it's not the case it returns
        /// new GameControls 
        /// </summary>
        /// <returns>Either the game instance or a new if its null</returns>       
        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }
        
        /// <summary>
        /// Resets the Game instance of GameRunning
        /// </summary>
        public static void ResetGameRunning() {
            GameRunning.instance = null;
        }

        //TODO:
        /// <summary>
        /// 
        /// </summary>
        public GameRunning() {
            
            // new player
            // game entities
            player = new Player();
            player.SetExtent(0.05f, 0.05f);
            
            // defines the different events
            // game assets
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "GameSpaceBaground.png")));
            
            SpaceBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            SpaceBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
            
            _levelCollisionCheckers = new List<CollisionChecker>();
            levels = Game.GetLevels();
            levelobstacles = Game.GetLevelObstacles();
            levelplatforms = Game.GetLevelPlatforms();
            levelPassengers = new List<List<Passenger>>();
            levelDiffrentPlatforms = Game.GetLevelDiffrentPlatforms();
               
            playerXcoordinates = Game.GetPlayerXCoordinates();
            playerYcoordinates = Game.GetPlayerYCoordinates();

            levelInfo = Game.GetLevelInfo();
            
            passengerListInfo = new List<Dictionary<int, string>>();
            
            deathtimer = new System.Timers.Timer(interval: 1000);
            //deathtimer.Elapsed += TimerMethod;

            for (int i = 0; i < filePath.Length; i++)
            {
                levelPassengers.Add(new List<Passenger>());
            }
            
            for (int i = 0; i < filePath.Length; i++){
                CreatePassenger(i);  
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

        private void CheckGameOver(int levelNum)
        {
            if (_levelCollisionCheckers[levelNum].GetGameOverChecker()) {
                Obstacle.CreateExplosion(player);
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.GameStateEvent, this, "GAME_OVER", "", "")); 
                //TODO: Fjern unødig kode!
                    //deathtimer.Enabled = true;
                
            }
        }
        //TODO: er den nødvendig?
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void TimerMethod(object s, System.Timers.ElapsedEventArgs e) {
            deathtimer.Enabled = false;
        }
        
        /*private void TimerMethod(object s, System.Timers.ElapsedEventArgs e) {
            deathtimer.Enabled = false;
            SpaceBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent, this, "GAME_OVER", "", "")); 
            
        }*/


        /// <summary>
        /// Creates the passenger and all its features for the game.
        /// </summary>
        /// <param name="_filePath">File directory</param>
        /// <param name="filePathNum">The number on the file we are reading</param>
        //TODO: Lidt kommentarer!

        public void CreatePassenger(int filePathNum)
        {
            Level.UpdatePassengerInfo(levelInfo[filePathNum]);
            
            passengerListInfo = Level.GetPassengerInfo();

            foreach (var passengerinfo in passengerListInfo)
            {
                passenger = new Passenger(passengerinfo[1], int.Parse(passengerinfo[2]), char.Parse(passengerinfo[3]),
                    passengerinfo[4], int.Parse(passengerinfo[5]), int.Parse(passengerinfo[6]),
                    levelDiffrentPlatforms[filePathNum], filePathNum);
                
                passenger.SetExtent(0.02f, 0.05f);

                passenger.SetPosition(
                    levelDiffrentPlatforms[filePathNum][passenger.GetPlatFormSpawn()][4].Shape.Position.X,
                    levelDiffrentPlatforms[filePathNum][passenger.GetPlatFormSpawn()][4].Shape.Position.Y + 0.035f);

                levelPassengers[filePathNum].Add(passenger);
            }
        }

        /// <summary>
        /// Creates the collision of entities in the level.
        /// </summary>
        /// <param name="_filePath">File directory</param>
        /// <param name="levelNum"">The number of the current level</param>
        private void CreateLevelCollision(string[] _filePath, int levelNum)
        {
             //legendsDictionary = CreateLegendDictionary(_filePath, levelNum);
             _levelCollisionCheckers.Add(new CollisionChecker(levelobstacles[levelNum], levelplatforms[levelNum],
             player,levelPassengers[levelNum], levelDiffrentPlatforms));
        }

        
        /// <summary>
        ///GetInstance looks if there is any control instance. If it's not the case it returns
        /// new GameControls 
        /// </summary>
        /// <returns>Either the game instance or a new if its null</returns>
        public static void ResetGameInstance()
        {
            GameRunning.instance = new GameRunning();
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
            return LevelCounter;
        }
        
        /// <summary>
        /// Method that updates all logic features, which could be gravity.
        /// </summary>
        public void UpdateGameLogic(){
            CheckGameOver(LevelCounter);
            RenderState();
            player.Physics();
            foreach (Passenger passenger in levelPassengers[LevelCounter])
            {
                passenger.PassengerMove();
            }
            _levelCollisionCheckers[LevelCounter].CheckCollsion();
            CheckGameOver(LevelCounter);
            if (player.GetsShape().Position.Y >= 1.0f){
                LevelCounter++;
                player.SetPosition(playerXcoordinates[LevelCounter], playerYcoordinates[LevelCounter]);
               
            }
        }

        
        /// <summary>
        /// Render the different entities and features.
        /// </summary>
        public void RenderState() {
            backGroundImage.RenderEntity();
            foreach (Passenger passenger in levelPassengers[LevelCounter])
            {
                passenger.RenderPassenger();
            }
            player.RenderPlayer();  
            levels[LevelCounter].RenderEntities();
            Obstacle.getExplosion().RenderAnimations();
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
            case "KEY_E":
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.GameStateEvent, this, "GAME_VICTORY", "", "")); 
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
        /// Handles all input events from other classes. 
        /// </summary>
        /// <param name="eventType">which kind of event, such as input event</param>
        /// TODO: Tjek lige gameEvent om det er rigtigt.
        /// <param name="gameEvent">The different state events</param>
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.InputEvent) {
                // if event input is called, process here
                switch (gameEvent.Message) { }
            }
        }
    }
}


            
    