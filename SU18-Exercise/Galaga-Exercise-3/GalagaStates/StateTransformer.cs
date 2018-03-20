using System;
using Galaga_Exercise_2;

namespace Galaga_Exercise_3.GalagaStates {
    public class StateTransformer {

        public static GameStateType.GameStateTypes TransformStringToState(string state) {

            switch (state) {

            case "GAME_RUNNING":
                return GameStateType.GameStateTypes.GameRunning;

            case "GAME_PAUSED":
                return GameStateType.GameStateTypes.GamePaused;

            case "MAIN_MENU":
                return GameStateType.GameStateTypes.MainMenu;
            default:
                throw new ArgumentException("No match");
            }

        }

        public static string TransformStateToString(GameStateType.GameStateTypes state) {

            switch (state) {

            case GameStateType.GameStateTypes.GameRunning:
                return "GAME_RUNNING";

            case GameStateType.GameStateTypes.GamePaused:
                return "GAME_PAUSED";

            case GameStateType.GameStateTypes.MainMenu:
                return "MAIN_MENU";
            default:
                throw new ArgumentException("No match");

            }
        }
    }
}