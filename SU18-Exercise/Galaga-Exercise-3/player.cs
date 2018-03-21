using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_3 {
   
    public class Player : IGameEventProcessor<object> {
        private Entity player;
        private DynamicShape playerDynamicShape;

        // Our processor for event of the player type
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                    case "Move_right" :
                        playerDynamicShape.Direction.X = 0.0045f; // moves a little step right
                        break;
                    case "Move_left" :
                        playerDynamicShape.Direction.X = -0.0045f; // moves a little step left
                        break;
                    case "release_right" :
                        playerDynamicShape.Direction.X = 0.0f;
                        break;
                    case "release_left" :
                        playerDynamicShape.Direction.X = 0.0f;
                        break;
                    default:
                        break;
                }
            }
        }
        // defines our player as a entity
        public Player() {
            player = new Entity(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            playerDynamicShape = player.Shape as DynamicShape;
        }

        public void Move() {
            // only moves if it's within the x.pos of board, so it doesnt go off screen
            if (playerDynamicShape.Position.X >= 0.0f &&
                playerDynamicShape.Position.X <= 0.9f) {
                player.Shape.Move();
                //moves a tiny bit so its not stuck out of the if statement above
                if (playerDynamicShape.Position.X <= 0.0f) {
                    playerDynamicShape.Position.X = 0.000000001f;
                }
                //moves a tiny bit, the other side
                if (playerDynamicShape.Position.X >= 0.9f) {
                    playerDynamicShape.Position.X = 0.89999999f;
                }
            }
        }

        public void RenderEntity() {
            player.RenderEntity();
        }
        
        //so we can get our positions from player
        public float GetPositionY() {
            return playerDynamicShape.Position.Y;
        }
        public float GetPositionX() {
            return playerDynamicShape.Position.X;
        }
        public float GetExtendX() {
            return playerDynamicShape.Extent.Y;
        }
        public float GetExtendY() {
            return playerDynamicShape.Extent.X;
        }
    }
}