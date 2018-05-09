using DIKUArcade.EventBus;

namespace SpaceTaxi_1 {
    
    /// <summary>
    /// 
    /// </summary>
    public static class SpaceBus {
        private static GameEventBus<object> eventBus;
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static GameEventBus<object> GetBus() {
            return SpaceBus.eventBus ?? (SpaceBus.eventBus =
                       new GameEventBus<object>());
        }
    }
}
