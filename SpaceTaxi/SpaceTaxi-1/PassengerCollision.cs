using System;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Physics;
using SpaceTaxi_1.States;


namespace SpaceTaxi_1
{
    /// <summary>
    /// The features and values that the passenger collsion needs.
    /// </summary>
    public class PassengerCollision
    {
        private static Passenger passengerPickups;
        private static List<Entity> ReleasePlatforms = new List<Entity>();

        public static bool CheckCollisionPassenger(List<Passenger> levelPassengers, Player player, 
            List<Dictionary<Char, List<Entity>>> diffrentPlatforms)
        {
            foreach (Passenger passenger in levelPassengers)
            {
                if (CollisionDetection.Aabb(player.GetsShape(), 
                            passenger.GetShape().AsStationaryShape()).Collision && !passenger.pickedUp && 
                    diffrentPlatforms[passenger.setOffLevel].ContainsKey(passenger.GetReleasePlatform())){
                    
                    Console.WriteLine("setOffLevel: " + passenger.setOffLevel);
                    Console.WriteLine("Current Level: " + GameLevels.Levelcount);
                    passenger.pickedUp = true;
                    passengerPickups = passenger;
                    ReleasePlatforms = diffrentPlatforms[passenger.setOffLevel][passenger.GetReleasePlatform()];
                    
                    return true;
                }
            }
            return false;
        }

        ///TODO: Dette er ufærdigt, marc kan gøre dette haha.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="releasePlatform"></param>
        /// <param name="allPickedUpPassengers"></param>
        public static void CheckDropOffCollision(List<Entity> releasePlatform,
            List<Passenger> allPickedUpPassengers){
            foreach (Passenger passenger in allPickedUpPassengers){
               
                    if (passenger.pickedUp && !passenger.droppedOff &&
                        GameRunning.GetLevelCounter() == passenger.setOffLevel){
                        
                        passenger.SetPosition(
                            releasePlatform[0].Shape.Position.X,
                            releasePlatform[0].Shape.Position.Y + 0.04f);
                        passenger.GetShape().Direction.X = 0.0f;
                        passenger.droppedOff = true;
                    
                }
            }
        }

     

        /// <summary>
        /// Get release platforms which is where a given passenger should be dropped off.
        /// </summary>
        /// <returns>ReleasePlatforms</returns>
        public static List<Entity> GetReleasePlatforms()
        {
            return ReleasePlatforms;
        }
        
        /// <summary>
        /// Get passenger pickups 
        /// </summary>
        /// <returns>passengerPickups</returns>
        public static Passenger GetPassengerPickups()
        {
            return passengerPickups;

        }
    }
}
















