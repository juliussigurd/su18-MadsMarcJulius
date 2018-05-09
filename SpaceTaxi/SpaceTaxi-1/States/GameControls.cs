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
    /// The class "GameControls" 
    /// </summary>
    public class GameControls : IGameState {

        //Fields
        private static GameControls instance;

        private Entity backGroundImage;
        private Text button;


        /// <summary>
        ///GetInstance looks if there is any control instance. If it's not the case it returns
        /// new GameControls (Kom tilbage til det her)
        /// </summary>
        /// <returns></returns>
        public static GameControls GetInstance() {
            return GameControls.instance ?? (GameControls.instance = new GameControls());

        }

        /// <summary>
        /// 
        /// </summary>
        public void GameLoop() {
        // Left empty on purpose
        }

        
        /// <summary>
        /// 
        /// </summary>
        public void InitializeGameState() {
            backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
            button = new Text("Back", new Vec2F(0.15f, 0.0f), new Vec2F(0.4f, 0.3f));
            button.SetColor(Color.Blue);
        }

        
        /// <summary>
        /// 
        /// </summary>
        public void UpdateGameLogic() {
            //Left empty on purpose
        }

        
        /// <summary>
        /// 
        /// </summary>
        public void RenderState() {
            //Sets the color of the active button to green
            InitializeGameState();
            backGroundImage.RenderEntity();
            button.RenderText();
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="keyAction"></param>
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