using System;

namespace SpaceTaxi_1.States {
    
    /// <summary>
    /// 
    /// </summary>
    public class StateTransformer {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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
                
            default:
                throw new ArgumentException("No match");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string TransformStateToString(GameStateType state) {

            switch (state) {

            case GameStateType.GameRunning:
                return "GAME_RUNNING";

            case GameStateType.GamePaused:
                return "GAME_PAUSED";

            case GameStateType.MainMenu:
                return "MAIN_MENU";
                
            case GameStateType.GameControls:
                return "GAME_CONTROLS";
                
            case GameStateType.Gamelevels:
                return "GAME_LEVELS";
                
            case GameStateType.GameOver:
                return "GAME_OVER";
                
            default:
                throw new ArgumentException("No match");

            }
        }
    }
}