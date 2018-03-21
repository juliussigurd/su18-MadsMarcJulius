using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.EventBus;
using System.Collections.Generic;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using Galaga_Exercise_3.MovementStrategy;
using Galaga_Exercise_3.ISquadrons;

namespace Galaga_Exercise_3 {

    public class Game : IGameEventProcessor<object> {
        private Window win;
        private Player player;
        private GameEventBus<object> eventBus;
        private List<Image> enemyStrides;
        private EntityContainer bullets;
        private List<Image> explosionStrides;
        private AnimationContainer explosions;
        private EntityContainer<GalagaEntities.Enemy> enemies1;
        private EntityContainer<GalagaEntities.Enemy> enemies2;
        private EntityContainer<GalagaEntities.Enemy> enemies3;
        private Squadron1 iSquadron1;
        private Squadron2 iSquadron2;
        private Squadron3 iSquadron3;
        private Down MoveDown;
        private NoMove noMove;
        private ZigZagDown ZigZag;
        GameTimer gameTimer = new GameTimer(60, 60);

        public Game() {
            // using a 500 x 500 window, named Galaga
            win = new Window("Galaga", 500, 500);
            
            // defines the different events
            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.WindowEvent, // messages to the window
            }); // e.g. move, destroy, receive health, etc.
            
            // when the event.x is registered, which place is it processed
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            }

        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    eventBus.ProcessEvents(); // this will call ProcessEvent()
                    player.Move();
                    
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear(); // render game entities win.SwapBuffers(); 
                    
                    explosions.RenderAnimations();
                    player.RenderEntity();
                    enemies1.RenderEntities();
                    ZigZag.MoveEnemies(enemies1);
                    bullets.RenderEntities();
                    
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                                ", FPS: " + gameTimer.CapturedFrames;
                }
            }
        }

        public void KeyPress(string key) {
            switch (key) {
            // sends event if escape is pressed
            case "KEY_ESCAPE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", ""));
                break;
            
        }
    }


        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            // if the event window is called, process it here
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                }
            }
        }
    }
}
