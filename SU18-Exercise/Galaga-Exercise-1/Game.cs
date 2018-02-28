namespace Galaga_Exercise_1 {

    using System.IO;
    using DIKUArcade;
    using DIKUArcade.Entities;
    using DIKUArcade.Graphics;
    using DIKUArcade.Math;
    using DIKUArcade.EventBus;
    using System.Collections.Generic;
    

    public class Game : IGameEventProcessor<object> {
        private Window win;
        private Entity player;
        private GameEventBus<object> eventBus;
        //Fik af vide, fra instruktor, at det skulle gøres på nedstående måde
        private DynamicShape playerDynamicShape;

        public Game() {
            // look at the Window.cs file for possible constructors.
            // We recommend using 500×500 as window dimensions,
            // which is most easily done using a predefined aspect ratio.
            win = new Window("Galaga", 500, 500);
            
            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent,// messages to the window
                GameEventType.PlayerEvent // commands issued to the player object,
            });                           // e.g. move, destroy, receive health, etc.
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);


            player = new Entity(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            DynamicShape playerdDynamicShape = player.Shape as DynamicShape;

        }

        public void GameLoop() {
            while (win.IsRunning()) {
                eventBus.ProcessEvents(); // this will call ProcessEvent()
                win.PollEvents();
                win.Clear();
                player.RenderEntity();
                win.SwapBuffers();
                playerDynamicShape.Move();
                player.RenderEntity();
            }
        }

        public void KeyPress(string key) {
            switch (key) {
            case "KEY_ESCAPE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", ""));
                break;
            }

            // match on e.g. "KEY_UP", "KEY_1", "KEY_A", etc.
            // TODO: use this method to start moving your player object
            playerDynamicShape.Direction.X = 0.0001f; // choose a fittingly small number
        }

        public void KeyRelease(string key) {
            // match on e.g. "KEY_UP", "KEY_1", "KEY_A", etc.
            playerDynamicShape.Direction.X = 0.0f;
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                default:
                    break;
                    
                }
            } else if (eventType == GameEventType.InputEvent) {
                switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;
                }
            }
        }
        
    }
}
