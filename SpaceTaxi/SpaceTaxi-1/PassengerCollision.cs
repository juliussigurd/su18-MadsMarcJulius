using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        private static List<Passenger> passengerPickups = new List<Passenger>();
        private static List<Entity> ReleasePlatforms = new List<Entity>();
        //private static List<List<Entity>> ReleasePlatforms = new List<List<Entity>>();

        public static bool CheckCollisionPassenger(List<Passenger> levelPassengers, Player player, 
            List<Dictionary<Char, List<Entity>>> diffrentPlatforms)
        {
            foreach (Passenger passenger in levelPassengers)
            {
                 if  (CollisionDetection.Aabb(passenger.GetShape(), 
                      player.GetsShape().AsStationaryShape()).Collision && !passenger.pickedUp && 
                      diffrentPlatforms[passenger.setOffLevel].ContainsKey(passenger.GetReleasePlatformChar()) &&
                      player.GetsShape().Direction.Y == 0 && player.GetsShape().Direction.Y == 0) {
                    
                    passenger.pickedUp = true;
                    passenger.GetShape().Direction.X = 0.0f;
                    passengerPickups.Add(passenger);
                    ReleasePlatforms = diffrentPlatforms[passenger.setOffLevel][passenger.GetReleasePlatformChar()];
                    
                    return true;
                }if (CollisionDetection.Aabb(player.GetsShape(), 
                    passenger.GetShape().AsStationaryShape()).Collision && !passenger.pickedUp && 
                    diffrentPlatforms[passenger.setOffLevel].ContainsKey(passenger.GetReleasePlatformChar())){
                    
                    Console.WriteLine("setOffLevel: " + passenger.setOffLevel);
                    Console.WriteLine("Current Level: " + GameLevels.Levelcount);
                    passenger.pickedUp = true;
                    passengerPickups.Add(passenger);
                    ReleasePlatforms = diffrentPlatforms[passenger.setOffLevel][passenger.GetReleasePlatformChar()];
                    
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
        public static void CheckDropOffCollision(){
            foreach (Passenger passenger in passengerPickups){
               
                    if (passenger.pickedUp && !passenger.droppedOff &&
                        GameRunning.GetLevelCounter() == passenger.setOffLevel){
                        
                        passenger.SetPosition(
                            passenger.GetReleasePlatform()[0].Shape.Position.X,
                            passenger.GetReleasePlatform()[0].Shape.Position.Y + 0.04f);
                        //passenger.GetShape().Direction.X = 0.0f;
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
        public static List<Passenger> GetPassengerPickups()
        {
            return passengerPickups;
        }
    }
}