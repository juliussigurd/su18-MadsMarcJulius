using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace SpaceTaxi_1.States {
    
    public class GameControls : IGameState {

        //Fields
        private static GameControls instance;
        private Entity backGroundImage;
        private Entity gameControls;
        private Text button;


        /// <summary>
        ///GetInstance looks if there is any control instance. If it's not the case it returns
        /// new GameControls 
        /// </summary>
        /// <returns>Either the game instance or a new if its null</returns>
        public static GameControls GetInstance() {
            return GameControls.instance ?? (GameControls.instance = new GameControls());

        }

        /// <summary>
        /// Left empty because use of IGameState.
        /// </summary>
        public void GameLoop() {
        // Left empty on purpose
        }

        
        /// <summary>
        /// Sets the GameState features and the entities as new.
        /// </summary>
        public void InitializeGameState() {
            backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
            gameControls = new Entity(new StationaryShape(0.05f, 0.40f, 0.9f, 0.6f),
                new Image(Path.Combine("Assets", "Images", "GameControls.png")));
            button = new Text("Back", new Vec2F(0.31f, 0.0f), new Vec2F(0.4f, 0.3f));
            button.SetColor(Color.Yellow);
        }

        
        /// <summary>
        /// Left empty because use of IGameState.
        /// </summary>
        public void UpdateGameLogic() {
            //Left empty on purpose
        }

        
        /// <summary>
        /// Render the different entities and features.
        /// </summary>
        public void RenderState() {
            //Sets the color of the active button to green
            InitializeGameState();
            backGroundImage.RenderEntity();
            gameControls.RenderEntity();
            button.RenderText();
        }
        
        
        /// <summary>
        /// Handles the key events. Such as the key enter, sets the player back to main menu.
        /// </summary>
        /// <param name="keyValue">The given key pressed</param>
        /// <param name="keyAction">Registers if a certain button is pressed or released</param>
        public void HandleKeyEvent(string keyValue, string keyAction) {
            if (keyAction == "KEY_PRESS") {
                switch (keyValue) {

                case "KEY_ENTER":
                    SpaceBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent, this, "MAIN_MENU", "", ""));
                    break;
                default:
                    break;
                }
            }
        }
    }
}