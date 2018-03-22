using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.EventBus;
using System.Collections.Generic;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using GalagaGame.GalagaState;
using Galaga_Exercise_3.MovementStrategy;
using Galaga_Exercise_3.ISquadrons;

namespace Galaga_Exercise_3 {

    public class Game : IGameEventProcessor<object> {
        private Window win;
        private StateMachine stateMachine;
        GameTimer gameTimer = new GameTimer(60, 60);

        public Game() {
            // using a 500 x 500 window, named Galaga
            win = new Window("Galaga", 500, 500);
            stateMachine = new StateMachine();
            
            // defines the different events
            GalagaBus.GetBus().InitializeEventBus(new List<GameEventType>() {
                GameEventType.WindowEvent, // messages to the window
                GameEventType.InputEvent,
                GameEventType.GameStateEvent,
                GameEventType.PlayerEvent
            }); // e.g. move, destroy, receive health, etc.
                 
            win.RegisterEventBus(GalagaBus.GetBus());
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, stateMachine);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, stateMachine);
            
            // when the event.x is registered, which place is it processed
            GalagaBus.GetBus().Subscribe(GameEventType.WindowEvent, this);
            }

        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    GalagaBus.GetBus().ProcessEvents(); // this will call ProcessEvent()
                    stateMachine.ActiveState.UpdateGameLogic();
                    
                    
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear(); // render game entities win.SwapBuffers(); 
                    stateMachine.ActiveState.RenderState();
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                                ", FPS: " + gameTimer.CapturedFrames;
                }
            }
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            // if the event window is called, process it here
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                case "EnterQuit":
                    win.CloseWindow();
                    break;
                }
            }
            if (eventType == GameEventType.InputEvent) {
                stateMachine.ActiveState.HandleKeyEvent(gameEvent.Message, gameEvent.Parameter1);
            }
        }
    }
}
