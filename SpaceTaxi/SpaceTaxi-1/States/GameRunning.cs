﻿using System;
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
    
    /// <summary>
    /// 
    /// </summary>
    public class GameRunning : IGameEventProcessor<object>, IGameState {
        
        //Fields
        private Player player;
        private Passenger passenger;
        private Entity backGroundImage;
        private List<float> playerXcoordinates;
        private List<float> playerYcoordinates;
        private Dictionary<char, string> legendsDictionary;
        private int LevelCounter = GameLevels.Levelcount;
        private List<EntityContainer> levels;
        private List<EntityContainer> levelobstacles;
        private List<EntityContainer> levelplatforms;
        private string[] filePath = Directory.GetFiles("Levels");
        private string[] levelInfo;
        GameTimer gameTimer = new GameTimer(60, 60);
        private static GameRunning instance;
        private List<CollisionChecker> _levelObstacles;
        private CollisionChecker _collisionChecker;
        private List<List<Passenger>> levelPassengers;
        private List<Dictionary<int, string>> passengerListInfo;
        private System.Timers.Timer deathtimer;


        
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
            player.SetExtent(0.05f, 0.05f);
            
            // defines the different events
            // game assets
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "GameSpaceBaground.png")));
            
            SpaceBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            SpaceBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
            
            _levelObstacles = new List<CollisionChecker>();
            levels = new List<EntityContainer>();
            levelobstacles = new List<EntityContainer>();
            levelplatforms = new List<EntityContainer>();
            levelPassengers = new List<List<Passenger>>();
              
            playerXcoordinates = new List<float>();
            playerYcoordinates = new List<float>();
            passengerListInfo = new List<Dictionary<int, string>>();
            
            deathtimer = new System.Timers.Timer(interval: 1000);
            //deathtimer.Elapsed += TimerMethod;

            for (int i = 0; i < filePath.Length; i++)
            {
                levelPassengers.Add(new List<Passenger>());
            }
            
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
            Level.SetLegendDictionaryToNew();
            Level.ReadLegends(levelInfo);
            return Level.GetLegendsDictionary();
        }

        private void CheckGameOver(int levelNum)
        {
            if (_levelObstacles[levelNum].GetGameOverChecker()) {
                Obstacle.CreateExplosion(player);
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.GameStateEvent, this, "GAME_OVER", "", "")); 
                ResetGameInstance();
            }
        }
        /*private void TimerMethod(object s, System.Timers.ElapsedEventArgs e) {
            deathtimer.Enabled = false;
            SpaceBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent, this, "GAME_OVER", "", "")); 
            
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_filePath"></param>
        /// <param name="filePathNum"></param>
        private void CreateMap(string[] _filePath, int filePathNum)
        {
            legendsDictionary = CreateLegendDictionary(_filePath, filePathNum);
            Level.ReadPlatforms(levelInfo[25]);
            Console.WriteLine(Level.GetDiffrenPlatforms().Count);
            Level.AddAllEntitiesToContainer(levelInfo, legendsDictionary);
            levels.Add(Level.GetLevelEntities());
            levelobstacles.Add(Level.GetLevelObstacles());
            levelplatforms.Add(Level.GetLevelPlatforms());
            
            playerXcoordinates.Add(Level.GetPlayerPosX());
            playerYcoordinates.Add(Level.GetPlayerPosY());
            
            Level.UpdatePassengerInfo(levelInfo);
            
            passengerListInfo = Level.GetPassengerInfo();

            foreach (var passengerinfo in passengerListInfo)
            {
                passenger = new Passenger(passengerinfo[1], int.Parse(passengerinfo[2]),char.Parse(passengerinfo[3]), 
                                          passengerinfo[4], int.Parse(passengerinfo[5]), int.Parse(passengerinfo[6]),
                                          Level.GetDiffrenPlatforms());
                passenger.SetExtent(0.02f,0.05f);
                
                Console.WriteLine(Level.GetDiffrenPlatforms().Count);
                
                passenger.SetPosition(
                Level.GetDiffrenPlatforms()[passenger.GetPlatFormSpawn()][4].Shape.Position.X,
                Level.GetDiffrenPlatforms()[passenger.GetPlatFormSpawn()][4].Shape.Position.Y + 0.045f);
                
                levelPassengers[filePathNum].Add(passenger);

            }
            
          
            
            //Console.WriteLine(levelPassengers[filePathNum].Count);
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

        public static void ResetGameInstance()
        {
            GameRunning.instance = new GameRunning();
        }
        
        public void GameLoop() {
        
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        public void UpdateGameLogic(){
            CheckGameOver(LevelCounter);
            RenderState();
            player.Physics();
            foreach (Passenger passenger in levelPassengers[LevelCounter])
            {
                passenger.PassengerMove();
            }
            _levelObstacles[LevelCounter].CheckCollsion();
            CheckGameOver(LevelCounter);
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
            foreach (Passenger passenger in levelPassengers[LevelCounter])
            {
                passenger.RenderPassenger();
            }
            player.RenderPlayer();  
            levels[LevelCounter].RenderEntities();
            Obstacle.getExplosion().RenderAnimations();
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


            
    