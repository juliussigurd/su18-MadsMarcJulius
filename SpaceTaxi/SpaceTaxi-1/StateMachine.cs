﻿using DIKUArcade.EventBus;
using DIKUArcade.State;
using SpaceTaxi_1.States;

namespace SpaceTaxi_1 {
    
    /// <summary>
    /// 
    /// </summary>
    public class StateMachine : IGameEventProcessor<object> {

        /// <summary>
        /// 
        /// </summary>
        public IGameState ActiveState { get; private set; }

        
        /// <summary>
        /// 
        /// </summary>
        public StateMachine() {
            SpaceBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            SpaceBus.GetBus().Subscribe(GameEventType.InputEvent, this);


            // Sets state to MainMenu
            ActiveState = MainMenu.GetInstance();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateType"></param>
        private void SwitchState(GameStateType stateType) {
            switch (stateType) {

            case GameStateType.MainMenu:
                ActiveState = MainMenu.GetInstance();
                ActiveState.InitializeGameState();
                break;
                
            case GameStateType.Gamelevels:
                ActiveState = GameLevels.GetInstance();
                ActiveState.InitializeGameState();
                break;
                
            case GameStateType.GameControls:
                ActiveState = GameControls.GetInstance();
                ActiveState.InitializeGameState();
                break;

            case GameStateType.GameRunning:
                ActiveState = GameRunning.GetInstance();
                break;

            case GameStateType.GamePaused:
                ActiveState = GamePaused.GetInstance();
                ActiveState.InitializeGameState();
                break;
                
            case GameStateType.GameOver:
                ActiveState = GameOver.GetInstance();
                ActiveState.InitializeGameState();
                break;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="gameEvent"></param>
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