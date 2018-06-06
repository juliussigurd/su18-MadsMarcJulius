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
    
    public class GameLevels : IGameState {


        //Fields
        private static GameLevels _instance;
        private Entity _backGroundImage;
        private Entity _spaceTaxiLogo;
        private Text[] _menuButtons;
        private int _activeMenuButton;
        private int _maxMenuButtons;
        public static int Levelcount;


        /// <summary>
        ///GetInstance looks if there is any control instance. If it's not the case it returns
        /// new GameControls 
        /// </summary>
        /// <returns>Either the game instance or a new if its null</returns>
        public static GameLevels GetInstance() {
            return GameLevels._instance ?? (GameLevels._instance = new GameLevels());
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
            _maxMenuButtons = 3;
            _menuButtons = new Text[_maxMenuButtons];
            _backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
            _spaceTaxiLogo = new Entity(new StationaryShape(0.05f, 0.45f, 0.9f, 0.5f),
                new Image(Path.Combine("Assets", "Images", "SpaceTaxiLogo.png")));

            //Size of the buttons
            _menuButtons[0] = new Text("Level 1", new Vec2F(0.31f, 0.15f), new Vec2F(0.4f, 0.3f));
            _menuButtons[1] = new Text("Level 2", new Vec2F(0.31f, 0.05f), new Vec2F(0.4f, 0.3f));
            _menuButtons[2] = new Text("Back", new Vec2F(0.31f, -0.05f), new Vec2F(0.4f, 0.3f));
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
            _menuButtons[(_activeMenuButton + 1) % 2].SetColor(Color.White);
            _menuButtons[(_activeMenuButton + 1) % 3].SetColor(Color.White);
            _menuButtons[(_activeMenuButton + 2) % 3].SetColor(Color.White);
            _menuButtons[_activeMenuButton].SetColor(Color.Yellow);


            _backGroundImage.RenderEntity();
            _spaceTaxiLogo.RenderEntity();

            //Draws the button 
            _menuButtons[0].RenderText();
            _menuButtons[1].RenderText();
            _menuButtons[2].RenderText();
    }

        
        /// <summary>
        /// Handles the key events. For key up, key down and enter.
        /// </summary>
        /// <param name="keyValue">The given key pressed</param>
        /// <param name="keyAction">Registers if a certain button is pressed or released</param>
        public void HandleKeyEvent(string keyValue, string keyAction) {

            if (keyAction == "KEY_PRESS") {
                switch (keyValue) {
                case "KEY_UP":
                    if (_activeMenuButton ==  0) {
                        _activeMenuButton = _maxMenuButtons -1;
                    } 
                    else {
                        _activeMenuButton -= 1;
                    }

                    break;
                case "KEY_DOWN":
                    Console.WriteLine(_activeMenuButton);
                    if (_activeMenuButton == _maxMenuButtons - 1) {
                        _activeMenuButton = 0;
                    } else {
                        _activeMenuButton += 1;
                    }

                    break;

                case "KEY_ENTER":
                    switch (_activeMenuButton) {
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