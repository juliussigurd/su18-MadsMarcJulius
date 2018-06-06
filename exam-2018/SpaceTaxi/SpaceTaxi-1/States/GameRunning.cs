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
using SpaceTaxi_1.Entities.Passenger;
using SpaceTaxi_1.Entities.Player;
using Image = DIKUArcade.Graphics.Image;

namespace SpaceTaxi_1.States {
    
    public class GameRunning : IGameState {
        
        //Fields
        private readonly Player player;
        private Passenger passenger;
        private readonly Entity backGroundImage;
        private readonly List<float> playerXcoordinates;
        private readonly List<float> playerYcoordinates;
        private Dictionary<char, string> legendsDictionary;
        private static int levelCounter;
        private readonly List<EntityContainer> levels;
        private readonly List<EntityContainer> levelobstacles;
        private readonly List<EntityContainer> levelplatforms;
        private readonly string[] filePath = Directory.GetFiles("Levels");
        
        GameTimer gameTimer = new GameTimer(60, 60);
        private static GameRunning instance;
        private readonly List<CollisionChecker> levelCollisionCheckers;
        private readonly List<TimeHandler> timerHandlers;
        private readonly List<List<Passenger>> levelPassengers;
        private List<Dictionary<int, string>> passengerListInfo;
        private readonly List<Dictionary<char, List<Entity>>> levelDiffrentPlatforms;
        private bool deathBool = true;
        private TimedEvent timeBeforeGameOver;
        private static Text cashLabel;
        private static int cash;

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

        private GameRunning() {
            
            player = new Player();
            player.SetExtent(0.05f, 0.05f);
            
            GameRunning.levelCounter = GameLevels.Levelcount;
            
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "GameSpaceBaground.png")));
            
            GameRunning.cash = 0; 
            GameRunning.cashLabel = new Text($"Cash: {GameRunning.cash}", new Vec2F(0.8f,0.004f),new Vec2F(0.2f,0.2f));
            GameRunning.cashLabel.SetColor(Color.DarkRed);
            PassengerCollision.SetPassengerPickupsToNew();
            SpaceBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
            
            levelCollisionCheckers = new List<CollisionChecker>();
            timerHandlers = new List<TimeHandler>();
            levels = Game.GetLevels();
            levelobstacles = Game.GetLevelObstacles();
            levelplatforms = Game.GetLevelPlatforms();
            levelPassengers = new List<List<Passenger>>();
            levelDiffrentPlatforms = Game.GetLevelDiffrentPlatforms();
               
            playerXcoordinates = Game.GetPlayerXCoordinates();
            playerYcoordinates = Game.GetPlayerYCoordinates();

            levelInfo = Game.GetLevelInfo();
            
            passengerListInfo = new List<Dictionary<int, string>>();

            for (int i = 0; i < filePath.Length; i++){
                levelPassengers.Add(new List<Passenger>());
            }
            
            for (int cp = 0; cp < filePath.Length; cp++){
                CreatePassenger(cp);  
            }
            
            for (int clc = 0; clc < levels.Count; clc++){
                CreateLevelCollision(clc);
            }

            for (int cth = 0; cth < levels.Count; cth++){
                CreateTimerHandlers(cth);
            }
            
            player.SetPosition(playerXcoordinates[GameRunning.levelCounter], playerYcoordinates[GameRunning.levelCounter]);
            
        }

        
        /// <summary>
        /// This method takes two arguments and render the map by using the methods from the level class. 
        /// </summary>
        /// <param name="levelNum"> Number of the level in the level container </param>
        private void CheckGameOver(int levelNum){

            if (levelCollisionCheckers[levelNum].GetGameOverChecker()) {
                ObstacleCollision.CreateExplosion(player);
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.GameStateEvent, this, "GAME_OVER", "", ""));               
            }
        }


        /// <summary>
        /// Creates the passenger and all its features for the game.
        /// </summary>
        /// <param name="filePathNum">The number on the file we are reading</param>
        private void CreatePassenger(int filePathNum){
            Level.UpdatePassengerInfo(levelInfo[filePathNum]);
            
            passengerListInfo = Level.GetPassengerInfo();

            foreach (var passengerinfo in passengerListInfo){
                passenger = new Passenger(passengerinfo[1], int.Parse(passengerinfo[2]), char.Parse(passengerinfo[3]),
                    passengerinfo[4], int.Parse(passengerinfo[5]), int.Parse(passengerinfo[6]),
                    levelDiffrentPlatforms, filePathNum);
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
        /// <param name="levelNum">The number of the current level </param>
        private void CreateLevelCollision( int levelNum){
             levelCollisionCheckers.Add(new CollisionChecker(levelobstacles[levelNum], levelplatforms[levelNum],
             player,levelPassengers[levelNum], levelDiffrentPlatforms));
        }

        /// <summary>
        /// Make a new time handler
        /// </summary>
        /// <param name="levelNum">Level number</param>
        private void CreateTimerHandlers(int levelNum){
            timerHandlers.Add(new TimeHandler(levelPassengers[levelNum]));
        }

        /// <summary>
        /// Adds cash and the label
        /// </summary>
        private void AddCash(){
            foreach (Passenger passenger in PassengerCollision.GetPassengerPickups()){
                if (passenger.GetreleasedWithinTimer()){
                    GameRunning.cash += passenger.GetCash();
                    GameRunning.cashLabel = new Text($"Cash: {GameRunning.cash}", new Vec2F(0.8f,0.004f),new Vec2F(0.2f,0.2f));
                    GameRunning.cashLabel.SetColor(Color.DarkRed);
                    passenger.SetReleasedWithinTimer(false);
                    
                } else if (passenger.PickedUp && passenger.GetReleasePlatformChar() == '^' &&
                           player.GetsShape().Position.Y >= 0.98f && GameRunning.levelCounter == levelInfo.Count - 1 && 
                           passenger.GetChecker()){
                    
                    GameRunning.cash += passenger.GetCash();
                    GameRunning.cashLabel = new Text($"Cash: {GameRunning.cash}", new Vec2F(0.8f,0.004f),new Vec2F(0.2f,0.2f));
                    GameRunning.cashLabel.SetColor(Color.DarkRed);
                    passenger.SetChecker(false);
                }
            }
            
        }

        
        /// <summary>
        ///GetInstance looks if there is any control instance. If it's not the case it returns
        /// new GameControls 
        /// </summary>
        /// <returns>Either the game instance or a new if its null</returns>
        public static void ResetGameInstance(){
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
        public static int GetLevelCounter(){
            return GameRunning.levelCounter;
        }

        public static int GetCash(){
            return GameRunning.cash;
        }


        /// <summary>
        /// Method that updates all logic features, which could be gravity.
        /// </summary>
        public void UpdateGameLogic(){
            if (player.GetsShape().Position.Y >= 0.98f && GameRunning.levelCounter == levelInfo.Count - 1){
                
                AddCash();
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.GameStateEvent, this, "GAME_VICTORY", "", ""));
            }
            
            CheckGameOver(GameRunning.levelCounter);
            RenderState();
            player.Physics();
            AddCash();
            
            foreach (Passenger passenger in levelPassengers[GameRunning.levelCounter]){
                passenger.PassengerMove();
            }

            levelCollisionCheckers[GameRunning.levelCounter].CheckCollsion();
            timerHandlers[GameRunning.levelCounter].SpawnPassengerTimer();
            timerHandlers[GameRunning.levelCounter].SetPickUpTimer();
            CheckGameOver(GameRunning.levelCounter);
            
            if (player.GetsShape().Position.Y >= 1.0f){
                GameRunning.levelCounter++;
                player.SetPosition(playerXcoordinates[GameRunning.levelCounter], playerYcoordinates[GameRunning.levelCounter]);
            } 
        }

        
        /// <summary>
        /// Render the different entities and features.
        /// </summary>
        public void RenderState() {
            backGroundImage.RenderEntity();
            GameRunning.cashLabel.RenderText();
            foreach (Passenger passenger in levelPassengers[GameRunning.levelCounter]){
                passenger.RenderPassenger();
            }
            
            foreach (Passenger passenger in PassengerCollision.GetPassengerPickups()){
                passenger.RenderPassenger();
            }
            
            player.RenderPlayer();  
            levels[GameRunning.levelCounter].RenderEntities();
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


            
    