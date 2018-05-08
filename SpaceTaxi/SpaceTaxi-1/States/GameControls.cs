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


        private static GameControls instance;

        //Fields
        private Entity backGroundImage;
        private Text button;


        //Methods
        public static GameControls GetInstance() {
            return GameControls.instance ?? (GameControls.instance = new GameControls());

        }

        public void GameLoop() {
        // Left empty on purpose
        }

        public void InitializeGameState() {
            backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
            button = new Text("Back", new Vec2F(0.15f, 0.0f), new Vec2F(0.4f, 0.3f));
            button.SetColor(Color.Blue);
        }

        public void UpdateGameLogic() {
            //Left empty on purpose
        }

        public void RenderState() {
            //Sets the color of the active button to green
            InitializeGameState();
            backGroundImage.RenderEntity();
            button.RenderText();
        }

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