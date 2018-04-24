using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Security.Permissions;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace Galaga_Exercise_3.GalagaStates {
    public class GamePaused : IGameState {


        private static GamePaused instance = null;

        //Fields
        private Entity backGroundImage;
        private Text[] menuButtons;
        private int activeMenuButton = 0;
        private int maxMenuButtons;


        //Methods
        public static GamePaused GetInstance() {
            return GamePaused.instance ?? (GamePaused.instance = new GamePaused());

        }

        public void GameLoop() { }

        public void InitializeGameState() {
            maxMenuButtons = 2;
            menuButtons = new Text[maxMenuButtons];
            backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));

            //Size of the buttons
            menuButtons[0] = new Text("Resume", new Vec2F(0.15f, 0.2f), new Vec2F(0.4f, 0.3f));
            menuButtons[1] = new Text("Main Menu", new Vec2F(0.15f, 0.1f), new Vec2F(0.4f, 0.3f));
        }

        public void UpdateGameLogic() { }

        public void RenderState() {
            //Sets the color of the active button to green
            InitializeGameState();
            menuButtons[(activeMenuButton + 1) % 2].SetColor(Color.White);
            menuButtons[activeMenuButton].SetColor(Color.PaleVioletRed);
            

            backGroundImage.RenderEntity();

            //Draws the two buttons 
            menuButtons[0].RenderText();
            menuButtons[1].RenderText();
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {

            if (keyAction == "KEY_PRESS") {
                switch (keyValue) {

                case "KEY_UP":
                    if (activeMenuButton == maxMenuButtons - 1) {
                        activeMenuButton = 0;
                    } else {
                        activeMenuButton += 1;
                    }

                    break;
                case "KEY_DOWN":
                    Console.WriteLine(activeMenuButton);
                    if (activeMenuButton == 0) {
                        activeMenuButton = maxMenuButtons - 1;
                    } else {
                        activeMenuButton -= 1;
                    }

                    break;
                case "KEY_ENTER":
                    switch (activeMenuButton) {
                    case 0:
                        GalagaBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "GAME_RUNNING", "", ""));
                        break;
                    case 1:
                        GalagaBus.GetBus().RegisterEvent(
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