using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;
using DIKUArcade.Timers;

namespace SpaceTaxi_1.States {
    public class GameRunning : IGameEventProcessor<object>, IGameState {
        private Player player;
        private Entity backGroundImage;
        private List<Image> enemyStrides;
        private EntityContainer bullets;
        private List<Image> explosionStrides;
        private AnimationContainer explosions;
        private CollisionChecker collisionChecker;
        private List<float> playerXcoordinates;
        private List<float> playerYcoordinates;
        private Dictionary<char, string> legendsDictionary;
        private int levelCounter = 0;
        private List<EntityContainer> levels;
        private string[] filePath = Directory.GetFiles("Levels");
        private string[] levelInfo;
        GameTimer gameTimer = new GameTimer(60, 60);
        private static GameRunning instance = null;

        public static void ResetGameRunning() {
            GameRunning.instance = null;
        }
        
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
            
            levels = new List<EntityContainer>();
            playerXcoordinates = new List<float>();
            playerYcoordinates = new List<float>();

            for (int i = 0; i < filePath.Length; i++)
            {
                CreateMap(filePath,i);  
            }
            
            player.SetPosition(playerXcoordinates[levelCounter], playerYcoordinates[levelCounter]);
            /*_collisionChecker = new CollisionChecker(_levels[levelCounter],
                                                     _legendsDictionary,
                                                     _player);
            */
        }
        /// <summary>
        /// This method takes two arguments and render the map by using the methods from the level class. 
        /// </summary>
        /// <param name="_filePath"> Directory of levels </param>
        /// <param name="filePathNum"> Number of the level in the level container </param>
        private void CreateMap(string[] _filePath, int filePathNum)
        {
            levelInfo = Level.ReadFile((_filePath[filePathNum]));
            legendsDictionary = new Dictionary<char, string>();
            Level.ReadLegends(levelInfo);
            legendsDictionary = Level.GetLegendsDictionary();
            Level.AddAllEntitiesToContainer(levelInfo, legendsDictionary);
            levels.Add(Level.GetLevelEntities());
            playerXcoordinates.Add(Level.GetPlayerPosX());
            playerYcoordinates.Add(Level.GetPlayerPosY());
        }

        
        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }
        
        public void GameLoop() {
        // Left empty on purpose
        }
        
        public void UpdateGameLogic() {
            RenderState();
            player.PlayerMove();
            player.Gravity();
            //  _collisionChecker.CheckCollsion();
            if (player.GetsShape().Position.Y >= 1.0f){
                levelCounter++;
                player.SetPosition(playerXcoordinates[levelCounter], playerYcoordinates[levelCounter]);
            }
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            player.RenderPlayer();  
            levels[levelCounter].RenderEntities();
        }


        public void InitializeGameState() {
            // Left empty on purpose
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            {
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
                    }
                }
            }
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            throw new System.NotImplementedException();
        }
    }
}


            
    