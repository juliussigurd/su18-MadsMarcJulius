﻿using System;
using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace SpaceTaxi_1.States {
    
    public class GamePaused : IGameState {


        //Fields
        private static GamePaused instance;
        private Entity backGroundImage;
        private Entity gamePaused;
        private Text[] menuButtons;
        private int activeMenuButton;
        private int maxMenuButtons;


        /// <summary>
        ///GetInstance looks if there is any control instance. If it's not the case it returns
        /// new GameControls 
        /// </summary>
        /// <returns>Either the game instance or a new if its null</returns>
        public static GamePaused GetInstance() {
            return GamePaused.instance ?? (GamePaused.instance = new GamePaused());

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
            maxMenuButtons = 2;
            menuButtons = new Text[maxMenuButtons];
            backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
            gamePaused = new Entity(new StationaryShape(0.1f, 0.4f, 0.8f, 0.5f),
                new Image(Path.Combine("Assets", "Images", "GamePaused.png")));

            //Size of the buttons
            menuButtons[0] = new Text("Resume", new Vec2F(0.31f, 0.2f), new Vec2F(0.4f, 0.3f));
            menuButtons[1] = new Text("Main Menu", new Vec2F(0.31f, 0.1f), new Vec2F(0.4f, 0.3f));
        }

        
        /// <summary>
        /// Left empty because use of IGameState./ 
        /// </summary>
        public void UpdateGameLogic() { }

        
        /// <summary>
        /// Render the different entities and features.
        /// </summary>
        public void RenderState() {
            //Sets the color of the active button to green
            InitializeGameState();
            menuButtons[(activeMenuButton + 1) % 2].SetColor(Color.White);
            menuButtons[activeMenuButton].SetColor(Color.Yellow);
            

            backGroundImage.RenderEntity();
            gamePaused.RenderEntity();

            //Draws the two buttons 
            menuButtons[0].RenderText();
            menuButtons[1].RenderText();
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
                    }
                    break;
                }
            }
        }
    }
}