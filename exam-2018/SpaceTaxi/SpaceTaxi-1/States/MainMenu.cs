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
    
    public class MainMenu : IGameState {

        //Fields
        private static MainMenu instance;
        private Entity spaceTaxiLogo;
        private Entity backGroundImage;
        private Text[] menuButtons;
        private int activeMenuButton;
        private int maxMenuButtons;


        /// <summary>
        ///GetInstance looks if there is any control instance. If it's not the case it returns
        /// new GameControls 
        /// </summary>
        /// <returns>Either the game instance or a new if its null</returns>
        public static MainMenu GetInstance() {
            return MainMenu.instance ?? (MainMenu.instance = new MainMenu());

        }

        /// <summary>
        /// Left empty because use of IGameState.
        /// </summary>
        public void GameLoop() { }

        
        /// <summary>
        /// Sets the GameState features and the entities as new.
        /// </summary>
        public void InitializeGameState() {
            maxMenuButtons = 3;
            menuButtons = new Text[maxMenuButtons];
            backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
            spaceTaxiLogo = new Entity(new StationaryShape(0.05f, 0.45f, 0.9f, 0.5f),
                new Image(Path.Combine("Assets", "Images", "SpaceTaxiLogo.png")));

            //Size of the buttons
            menuButtons[0] = new Text("New Game", new Vec2F(0.31f, 0.15f), new Vec2F(0.4f, 0.3f));
            menuButtons[1] = new Text("Controls", new Vec2F(0.31f, 0.05f), new Vec2F(0.4f, 0.3f));
            menuButtons[2] = new Text("Quit", new Vec2F(0.31f, -0.05f), new Vec2F(0.4f, 0.3f));

        }

        
        /// <summary>
        /// Left empty because use of IGameState.
        /// </summary>
        public void UpdateGameLogic() {     
        // Left empty on purpose
        }

        
        /// <summary>
        /// Render the different entities and features.
        /// </summary>
        public void RenderState() {
            //Sets the color of the active button to blue
            InitializeGameState();
            menuButtons[(activeMenuButton + 1) % 2].SetColor(Color.White);
            menuButtons[(activeMenuButton + 1) % 3].SetColor(Color.White);
            menuButtons[(activeMenuButton + 2) % 3].SetColor(Color.White);
            menuButtons[activeMenuButton].SetColor(Color.Yellow);
            backGroundImage.RenderEntity();
            spaceTaxiLogo.RenderEntity();

            //Draws the three buttons 
            menuButtons[0].RenderText();
            menuButtons[1].RenderText();
            menuButtons[2].RenderText();
        }
        
        /// <summary>
        /// Handles the key events. Such as the key enter, sets the player back to main menu.
        /// </summary>
        /// <param name="keyValue">The given key pressed</param>
        /// <param name="keyAction">Registers if a certain button is pressed or released</param>
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
                    if (activeMenuButton == maxMenuButtons - 1) {
                        activeMenuButton = 0;
                    } else {
                        activeMenuButton += 1;
                    }

                    break;
                case "KEY_ENTER":
                    switch (activeMenuButton) {
                    case 0:
                        GameRunning.ResetGameRunning();
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "GAME_LEVELS", "", ""));
                        break;
                    case 1:
                        GameRunning.ResetGameRunning();
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "GAME_CONTROLS", "", ""));
                        break;
                    case 2:
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", ""));
                        break;
                    }

                    break;
                }
            }
        }
    }
}