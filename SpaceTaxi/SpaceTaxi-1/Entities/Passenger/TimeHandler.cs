using System.Collections.Generic;

namespace SpaceTaxi_1.Entities.Passenger
{
    public class TimeHandler
    {
        private List<Passenger> passengers;
        
        public TimeHandler(List<Passenger> passengers)
        {
            this.passengers = passengers;
        }
        

        public void SpawnPassengerTimer()
        {
            foreach (Passenger passenger in passengers)
            {
                if (!passenger.GetSpawnTimerStarted())
                {
                    passenger.ResetSpawnTimer();
                    passenger.StartSpawnTimer();
                    
                    passenger.SetSpawnTimerStarted(true);
                }
            }
        }

        public void SetPickUpTimer()
        {
            foreach (Passenger passenger in passengers)
            {
                if (!passenger.GetReleaseTimerStarted() && passenger.PickedUp)
                {
                    passenger.ResetReleaseTimer();
                    passenger.StartRelaseTimer();
                    
                    passenger.SetReleaseTimerStarted(true);
                }
            }
        }
    }
}
