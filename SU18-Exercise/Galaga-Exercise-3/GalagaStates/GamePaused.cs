using DIKUArcade.EventBus;
using DIKUArcade.State;

namespace Galaga_Exercise_3.GalagaStates {
    
    public class GamePaused : IGameEventProcessor<object>, IGameState{
        
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            
        }

        public void GameLoop() {
           
        }

        public void InitializeGameState() {
            
        }

        public void UpdateGameLogic() {
            
        }

        public void RenderState() {
            
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            
        }
    }
}