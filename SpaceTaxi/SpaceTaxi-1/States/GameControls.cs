using System;
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


        private static GameControls instance = null;

        //Fields
        private Entity backGroundImage;
        private Text[] menuButtons;
        private int activeMenuButton = 0;
        private int maxMenuButtons;


        //Methods
        public static GameControls GetInstance() {
            return GameControls.instance ?? (GameControls.instance = new GameControls());

        }

        public void GameLoop() {
        // Left empty on purpose
        }

        public void InitializeGameState() {
            maxMenuButtons = 1;
            menuButtons = new Text[maxMenuButtons];
            backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));

            //Size of the buttons
            menuButtons[0] = new Text("Back", new Vec2F(0.15f, 0.0f), new Vec2F(0.4f, 0.3f));
        }

        public void UpdateGameLogic() {
            //Left empty on purpose
        }

        public void RenderState() {
            //Sets the color of the active button to green
            InitializeGameState();
            menuButtons[activeMenuButton].SetColor(Color.Blue);
            

            backGroundImage.RenderEntity();

            //Draws the button 
            menuButtons[0].RenderText();
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {

            if (keyAction == "KEY_PRESS") {
                switch (keyValue) {

                case "KEY_ENTER":
                    switch (activeMenuButton) {
                    case 0:
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "MAIN_MENU", "", ""));
                        break;
                    }

                    break;
                }
            }
        }
    }
}