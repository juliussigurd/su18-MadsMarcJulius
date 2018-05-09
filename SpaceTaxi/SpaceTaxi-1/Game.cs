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
        private Window _win;
        private GameTimer _gameTimer;
        private Player _player;
        private StateMachine stateMachine;
        public static int keepTrackOfUpdates;
        
      
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
            _gameTimer = new GameTimer(60); // 60 UPS, no FPS limit

            }

        /// <summary>
        /// This method takes two arguments and render the map by using the methods from the level class. 
        /// </summary>
        /// <param name="_filePath"> Directory of levels </param>
        /// <param name="filePathNum"> Number of the level in the level container </param>

                
        public void GameLoop(){
            while (_win.IsRunning()){
                _gameTimer.MeasureTime();
                while (_gameTimer.ShouldUpdate()){
                    _win.PollEvents();
                    SpaceBus.GetBus().ProcessEvents();
                    stateMachine.ActiveState.UpdateGameLogic();  

                }

                if (_gameTimer.ShouldRender()){
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
            else if (eventType == GameEventType.InputEvent){
                    stateMachine.ActiveState.HandleKeyEvent(gameEvent.Message, gameEvent.Parameter1);
            }
        }
    }
}
