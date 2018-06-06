using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace SpaceTaxi_1.States {
    
    /// <summary>
    /// The class "GameVictory" 
    /// </summary>
    public class GameVictory : IGameState {

        //Fields
        private static GameVictory _instance;
        private Entity _backGroundImage;
        private Entity _gameVictory;
        private Text _button;
        private Text _cash;


        /// <summary>
        ///GetInstance looks if there is any control instance. If it's not the case it returns
        /// new GameControls 
        /// </summary>
        /// <returns>Either the game instance or a new if its null</returns>
        public static GameVictory GetInstance() {
            return GameVictory._instance ?? (GameVictory._instance = new GameVictory());

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
            _backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f),
                new Image(Path.Combine("Assets", "Images", "GameSpaceBaground.png")));
            _gameVictory = new Entity(new StationaryShape(0.0f, 0.2f, 1.1f, 0.65f),
                new Image(Path.Combine("Assets", "Images", "GameVictory.png")));
            _button = new Text("Main Menu", new Vec2F(0.31f, 0.0f), new Vec2F(0.4f, 0.3f));
            _button.SetColor(Color.Yellow);
            _cash = new Text($"Total Cash: {GameRunning.GetCash()}",new Vec2F(0.31f, 0.1f), new Vec2F(0.4f, 0.3f));
            _cash.SetColor(Color.Red);
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
            _backGroundImage.RenderEntity();
            _gameVictory.RenderEntity();
            _button.RenderText();
            _cash.RenderText();
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