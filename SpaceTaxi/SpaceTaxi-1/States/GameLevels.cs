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
    public class GameLevels : IGameState {


        //Fields
        private static GameLevels instance;

        private Entity backGroundImage;
        private Entity spaceTaxiLogo;
        private Text[] menuButtons;
        private int activeMenuButton;
        private int maxMenuButtons;
        public static int Levelcount;


        //Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static GameLevels GetInstance() {
            return GameLevels.instance ?? (GameLevels.instance = new GameLevels());
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
            maxMenuButtons = 3;
            menuButtons = new Text[maxMenuButtons];
            backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
            spaceTaxiLogo = new Entity(new StationaryShape(0.05f, 0.45f, 0.9f, 0.5f),
                new Image(Path.Combine("Assets", "Images", "SpaceTaxiLogo.png")));

            //Size of the buttons
            menuButtons[0] = new Text("Level 1", new Vec2F(0.31f, 0.15f), new Vec2F(0.4f, 0.3f));
            menuButtons[1] = new Text("Level 2", new Vec2F(0.31f, 0.05f), new Vec2F(0.4f, 0.3f));
            menuButtons[2] = new Text("Back", new Vec2F(0.31f, -0.05f), new Vec2F(0.4f, 0.3f));
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
            menuButtons[(activeMenuButton + 1) % 3].SetColor(Color.White);
            menuButtons[(activeMenuButton + 2) % 3].SetColor(Color.White);
            menuButtons[activeMenuButton].SetColor(Color.Yellow);


            backGroundImage.RenderEntity();
            spaceTaxiLogo.RenderEntity();

            //Draws the button 
            menuButtons[0].RenderText();
            menuButtons[1].RenderText();
            menuButtons[2].RenderText();
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
                        GameLevels.Levelcount = 0;
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "GAME_RUNNING", "", ""));
                        break;
                        
                    case 1:
                        GameLevels.Levelcount = 1;
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "GAME_RUNNING", "", ""));
                        
                        break;
                        
                    case 2:
                        GameLevels.Levelcount = 1;
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