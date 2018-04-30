using System;
using System.Xml.Serialization;
using DIKUArcade.EventBus;
using DIKUArcade.State;
using Galaga_Exercise_3;
using Galaga_Exercise_3.GalagaStates;

namespace GalagaGame.GalagaState {
    public class StateMachine : IGameEventProcessor<object> {


        public IGameState ActiveState { get; private set; }

        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);


            // Sets state to MainMenu
            ActiveState = MainMenu.GetInstance();
        }



        private void SwitchState(GameStateType stateType) {
            switch (stateType) {

            case GameStateType.MainMenu:
                ActiveState = MainMenu.GetInstance();
                ActiveState.InitializeGameState();
                break;

            case GameStateType.GameRunning:
                ActiveState = GameRunning.GetInstance();
                break;

            case GameStateType.GamePaused:
                ActiveState = GamePaused.GetInstance();
                ActiveState.InitializeGameState();
                break;
            }
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            // if the event window is called, process it here
            if (eventType == GameEventType.GameStateEvent) {
                    SwitchState(StateTransformer.TransformStringToState(gameEvent.Message));
               
            }
            else if (eventType == GameEventType.InputEvent) {
                ActiveState.HandleKeyEvent(gameEvent.Message, gameEvent.Parameter1);
            }
        }
    }
}
