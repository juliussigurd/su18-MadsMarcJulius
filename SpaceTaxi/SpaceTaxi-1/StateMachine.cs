using DIKUArcade.EventBus;
using DIKUArcade.State;
using SpaceTaxi_1.States;

namespace SpaceTaxi_1 {
    
    public class StateMachine : IGameEventProcessor<object> {

        /// <summary>
        /// get and set the activeState 
        /// </summary>
        public IGameState ActiveState { get; private set; }

        
        /// <summary>
        /// Handles GameStateEvents in this class.
        /// </summary>
        public StateMachine() {
            SpaceBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            SpaceBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);
            // Sets state to MainMenu to start off with.
            ActiveState = MainMenu.GetInstance();
        }


        /// <summary>
        /// Swicthes state depending on which stateType given by the GameStateType.
        /// </summary>
        /// <param name="stateType">The state to switch to</param>
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
                
            case GameStateType.GameVictory:
                ActiveState = GameVictory.GetInstance();
                ActiveState.InitializeGameState();
                break;
            }
        }
        
        /// <summary>
        /// Procceses the events of the SwitchState, and in which string it needs to be transformed to.
        /// </summary>
        /// <param name="eventType">Which kind of event, like a GameStateEvent</param>
        /// <param name="gameEvent">In which state of the game the message is at</param>
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
