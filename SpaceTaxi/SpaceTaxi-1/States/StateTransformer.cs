using System;

namespace SpaceTaxi_1.States {
    
    public class StateTransformer {

        /// <summary>
        /// Transforms a string to a enum, of the GameStateType.
        /// </summary>
        /// <param name="state">The string representing the state</param>
        /// <returns>Game state type</returns>
        /// <exception cref="ArgumentException">If there are not any string to match</exception>
        public static GameStateType TransformStringToState(string state) {

            switch (state) {

            case "GAME_RUNNING":
                return GameStateType.GameRunning;

            case "GAME_PAUSED":
                return GameStateType.GamePaused;

            case "MAIN_MENU":
                return GameStateType.MainMenu;
                
            case "GAME_CONTROLS":
                return GameStateType.GameControls;
                
            case "GAME_LEVELS":
                return GameStateType.Gamelevels;
            
            case "GAME_OVER":
                return GameStateType.GameOver;
                
            case "GAME_VICTORY":
                return GameStateType.GameVictory;
                
            default:
                throw new ArgumentException("No match");
            }
        }
    }
}