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
    
    /// <summary>
    /// 
    /// </summary>
    public class GameOver : IGameState {


        //Fields
        private static GameOver instance;

        private Entity backGroundImage;
        private Text[] menuButtons;
        private int activeMenuButton;
        private int maxMenuButtons;
        public static int Levelcount;


        //Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static GameOver GetInstance() {
            return GameOver.instance ?? (GameOver.instance = new GameOver());
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
            maxMenuButtons = 2;
            menuButtons = new Text[maxMenuButtons];
            backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));

            //Size of the buttons
            menuButtons[0] = new Text("Restart", new Vec2F(0.15f, 0.1f), new Vec2F(0.4f, 0.3f));
            menuButtons[1] = new Text("Main Menu", new Vec2F(0.15f, 0.0f), new Vec2F(0.4f, 0.3f));
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
            menuButtons[(activeMenuButton + 1) % 2].SetColor(Color.White);
            menuButtons[activeMenuButton].SetColor(Color.Blue);
            

            backGroundImage.RenderEntity();

            //Draws the button 
            menuButtons[0].RenderText();
            menuButtons[1].RenderText();
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="keyAction"></param>
        public void HandleKeyEvent(string keyValue, string keyAction) {

            if (keyAction == "KEY_PRESS") {
                switch (keyValue) {
                case "KEY_UP":
                    if (activeMenuButton ==  0) {
                        activeMenuButton = maxMenuButtons -1;
                    } 
                    else {
                        activeMenuButton -= 1;
                    }

                    break;
                case "KEY_DOWN":
                    Console.WriteLine(activeMenuButton);
                    if (activeMenuButton == maxMenuButtons - 1) {
                        activeMenuButton = 0;
                    } else {
                        activeMenuButton += 1;
                    }

                    break;

                case "KEY_ENTER":
                    switch (activeMenuButton) {
                    case 0:
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "GAME_RUNNING", "", ""));
                        break;
                        
                    case 1:
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "MAIN_MENU", "", ""));
                        
                        break;
                    default: 
                        break;
                    }

                    break;
                    
                default: 
                    break;
                }
            }
        }
    }
}