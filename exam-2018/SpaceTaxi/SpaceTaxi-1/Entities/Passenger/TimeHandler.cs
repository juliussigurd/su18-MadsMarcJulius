using System.Collections.Generic;

namespace SpaceTaxi_1.Entities.Passenger
{
    
    public class TimeHandler{
        private List<Passenger> passengers;
        
        public TimeHandler(List<Passenger> passengers){
            this.passengers = passengers;
        }
        

        /// <summary>
        /// Handles each passengers spawn timer.
        /// </summary>
        public void SpawnPassengerTimer(){
            foreach (var passenger in passengers){

                if (!passenger.GetSpawnTimerStarted()){
                    passenger.ResetSpawnTimer();
                    passenger.StartSpawnTimer();
                    
                    passenger.SetSpawnTimerStarted(true);
                }
            }
        }

        /// <summary>
        /// Starts the passenger pick up/release time.
        /// </summary>
        public void SetPickUpTimer(){
            foreach (var passenger in passengers){
                if (!passenger.GetReleaseTimerStarted() && passenger.PickedUp){
                    passenger.ResetReleaseTimer();
                    passenger.StartRelaseTimer();
                    
                    passenger.SetReleaseTimerStarted(true);
                }
            }
        }
    }
}
