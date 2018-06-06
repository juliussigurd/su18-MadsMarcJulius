using DIKUArcade.EventBus;

namespace SpaceTaxi_1 {
 
    public static class SpaceBus {
        private static GameEventBus<object> _eventBus;
        
        /// <summary>
        /// If eventbus is null create new GameEventBus else return eventbus 
        /// </summary>
        /// <returns>eventBus</returns>
        public static GameEventBus<object> GetBus() {
            return SpaceBus._eventBus ?? (SpaceBus._eventBus =
                       new GameEventBus<object>());
        }
    }
}
