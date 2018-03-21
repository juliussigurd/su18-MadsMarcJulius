using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace Galaga_Exercise_3.GalagaStates {
    public class MainMenu : IGameState {
        
        //Constructor
        private static MainMenu instance = null;

        //Fields
        private Entity backGroundImage;   
        private Text[] menuButtons;      
        private int activeMenuButton;     
        private int maxMenuButtons;
        private GameEventBus<object> eventBus;

        
        
        
        //Methods
        public static MainMenu GetInstance() {
            return MainMenu.instance ?? MainMenu.instance;
            
        }

        public void GameLoop() {
            
        }

        public void InitializeGameState() {
            eventBus = new GameEventBus<object>();
            activeMenuButton = 0;
            maxMenuButtons = 2;

        }

        public void UpdateGameLogic() {
            
        }

        public void RenderState() {
            backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f), 
                new Image(Path.Combine("Assets", "Images", "TitleImage.png")));
            backGroundImage.RenderEntity();
            
            //Size of the buttons
            menuButtons[0] = new Text("New Game", new Vec2F(0.3f, 0.6f), new Vec2F(0.4f, 0.2f));
            menuButtons[1] = new Text("Quit", new Vec2F(0.3f, 0.2f), new Vec2F(0.4f, 0.2f));
            
            //Sets the color of the active button to green
            menuButtons[activeMenuButton].SetColor(Color.Aquamarine);
            
            //Draws the two buttons 
            menuButtons[0].RenderText();
            menuButtons[1].RenderText();
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyValue) {
            
            case "KEY_UP":
                activeMenuButton = (activeMenuButton + 1) % maxMenuButtons;
                break;
            case "KEY_DOWN":
                activeMenuButton = (activeMenuButton - 1) % maxMenuButtons;
                break;
            case "KEY_ENTER":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.GameStateEvent, this, "Enter", "", ""));
                break;
            }
            
        }

        public void GameRunning() {
            
        }
    }
}