﻿using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.EventBus;
using System.Collections.Generic;
using DIKUArcade.Physics;
using DIKUArcade.State;
using DIKUArcade.Timers;
using Galaga_Exercise_3.MovementStrategy;
using Galaga_Exercise_3.ISquadrons;



namespace Galaga_Exercise_3 {
    public class GameRunning : IGameEventProcessor<object>, IGameState {
        private Player player;
        private GameEventBus<object> eventBus;
        private List<Image> enemyStrides;
        private EntityContainer bullets;
        private List<Image> explosionStrides;
        private AnimationContainer explosions;
        private EntityContainer<GalagaEntities.Enemy> enemies;
        private Squadron1 iSquadron1;
        private Squadron2 iSquadron2;
        private Squadron3 iSquadron3;
        private Down MoveDown;
        private NoMove noMove;
        private ZigZagDown ZigZag;
        GameTimer gameTimer = new GameTimer(60, 60);

        public GameRunning() {

            // new player
            player = new Player();
            MoveDown = new Down();
            noMove = new NoMove();
            ZigZag = new ZigZagDown();
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

            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.PlayerEvent, player);

            // the image path of both enemies and explosions,
            // and a container for all sprites
            enemyStrides = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "Elias.png"));
            enemies = new EntityContainer<GalagaEntities.Enemy>();
            bullets = new EntityContainer();
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(8);

            iSquadron1.CreateEnemies(enemyStrides);
            iSquadron2.CreateEnemies(enemyStrides);
            iSquadron3.CreateEnemies(enemyStrides);
            //adding 10 enemies
            //AddEnemies(10);
            enemies = iSquadron3.Enemies;

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

            enemies.Iterate(enemy => {
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

        public void KeyPress(string key) {
            switch (key) {
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
                        GameEventType.InputEvent, this, "Bullet_fire", "", ""));
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
            if (eventType == GameEventType.InputEvent) {
                // if event input is called, process here
                switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;
                case "Bullet_fire":
                    AddBullets();
                    break;
                default:
                    break;
                }
            }
        }

        public void GameLoop() {
            while () {
                bullets.Iterate(IterateShots);
            }
          
            
        }

        public void InitializeGameState() {
            
        }

        public void UpdateGameLogic() {
            
        }

        public void RenderState() {
            player.RenderEntity();
            enemies.RenderEntities();
            bullets.RenderEntities();
            explosions.RenderAnimations();
            
            
            
           
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
           
        }
    }
}


            
    