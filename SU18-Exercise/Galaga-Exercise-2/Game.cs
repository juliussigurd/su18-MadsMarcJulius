using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.EventBus;
using System.Collections.Generic;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using Galaga_Exercise_2.MovementStrategy;

namespace Galaga_Exercise_2 {

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
        GameTimer gameTimer = new GameTimer(60, 60);

        public Game() {
            // using a 500 x 500 window, named Galaga
            win = new Window("Galaga", 500, 500);
            
            // new player
            player = new Player();
            MoveDown = new Down();
            iSquadron1 = new Squadron1(10);
            iSquadron2 = new Squadron2(15);
            iSquadron3 = new Squadron3(10);
            // defines the different events
            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window
                GameEventType.PlayerEvent // commands issued to the player objec-t,
            }); // e.g. move, destroy, receive health, etc.
            
            // when the event.x is registered, which place is it processed
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            eventBus.Subscribe(GameEventType.PlayerEvent, player);

            // the image path of both enemies and explosions,
            // and a container for all sprites
            enemyStrides = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "Elias.png"));
            enemies1 = new EntityContainer<GalagaEntities.Enemy>();
            enemies2 = new EntityContainer<GalagaEntities.Enemy>();
            enemies3 = new EntityContainer<GalagaEntities.Enemy>();
            bullets = new EntityContainer();
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(8);
            
            iSquadron1.CreateEnemies(enemyStrides);
            iSquadron2.CreateEnemies(enemyStrides);
            iSquadron3.CreateEnemies(enemyStrides);
            //adding 10 enemies
            //AddEnemies(10);
            enemies1 = iSquadron3.Enemies;
            
        }
        
        private int explosionLength = 500;

        public void AddExplosion(float posX, float posY, float extentX, float extentY) {
            // sets the explosoin to the container, with the position, and animation
            explosionLength = 500;
            explosions.AddAnimation( 
                new StationaryShape(posX, posY, extentX, extentY), explosionLength, 
                new ImageStride(explosionLength / 8, explosionStrides));
        }


        private void AddBullets() {
            // sets the bullet to dynamic shape, with a position from where to start
            // direction and a speed
            bullets.AddDynamicEntity(
                new DynamicShape(new Vec2F(player.GetPositionX() + (player.GetExtendX() / 2.0f),
                    player.GetPositionY() + player.GetExtendY()),
                new Vec2F(0.008f, 0.027f),
            new Vec2F(0.0f, 0.01f)),
            new Image(Path.Combine("Assets", "Images", "BulletRed2.png")));
        }

        public void IterateShots(Entity bullet) {
            // uses iterate instead of foreach
            // moves the bullet, sets every bullet to dynamic
            bullet.Shape.Move();
            DynamicShape shot = bullet.Shape as DynamicShape;
            if (bullet.Shape.Position.Y >= 1.0f) {
                // when above 1.0 in Y, remove
                bullet.DeleteEntity();
            }
            enemies1.Iterate(enemy => {
                if (CollisionDetection.Aabb(shot, enemy.Shape).Collision) {
                    // uses the collision method from physics,
                    // to determan if bullet hits enemy, then remove both
                    AddExplosion(
                        enemy.Shape.Position.X, 
                        enemy.Shape.Position.Y, 
                        enemy.Shape.Extent.X, 
                        enemy.Shape.Extent.Y);
                    enemy.DeleteEntity();
                    bullet.DeleteEntity();
                }
            });
        }

        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    eventBus.ProcessEvents(); // this will call ProcessEvent()
                    player.Move();
                    bullets.Iterate(IterateShots);
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear(); // render game entities win.SwapBuffers(); 
                    explosions.RenderAnimations();
                    player.RenderEntity();
                    enemies1.RenderEntities();
                    MoveDown.MoveEnemies(enemies1);
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

            case "KEY_RIGHT":
                // Sends event if right key is pressed
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "Move_right", "", ""));
                break;

            case "KEY_LEFT":
                // Sends event if left key is pressed
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "Move_left", "", ""));
                break;
            case "KEY_SPACE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.WindowEvent, this, "Bullet_fire", "", ""));
                break;
            }
        }

        public void KeyRelease(string key) {
            switch (key) {

            case "KEY_RIGHT":
                // event on release of right key
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "release_right", "", ""));
                break;

            case "KEY_LEFT":
                // event on release of left key
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "release_left", "", ""));
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
                case "Bullet_fire":
                    AddBullets();
                    break;

                }
            } else if (eventType == GameEventType.InputEvent) {
                // if event input is called, process here
                switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;
                default:
                    break;
                }
            }
        }
    }
}
