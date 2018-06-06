using System;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Physics;
using SpaceTaxi_1.Entities;
using SpaceTaxi_1.Entities.Passenger;
using SpaceTaxi_1.Entities.Player;
using SpaceTaxi_1.States;

namespace SpaceTaxi_1.Collision
{
    /// <summary>
    /// The features and values that the passenger collsion needs.
    /// </summary>
    public static class PassengerCollision
    {
        private static List<Passenger> _passengerPickups = new List<Passenger>();
        
        public static bool CheckCollisionPassenger(List<Passenger> levelPassengers, Player player, 
            List<Dictionary<Char, List<Entity>>> diffrentPlatforms)
        {
            foreach (Passenger passenger in levelPassengers)
            {
                //If Passenger player flies into passenger.
                 if  (CollisionDetection.Aabb(passenger.GetShape(), 
                      player.GetsShape().AsStationaryShape()).Collision && !passenger.PickedUp && 
                      diffrentPlatforms[passenger.SetOffLevel].ContainsKey(passenger.GetReleasePlatformChar()) &&
                      player.GetsShape().Direction.Y == 0.0f && player.GetsShape().Direction.Y == 0.0f && 
                      passenger.SpawnTimer()) {
                    
                    passenger.PickedUp = true;
                    passenger.GetShape().Direction.X = 0.0f;
                    _passengerPickups.Add(passenger);
                    
                    return true;
                 //If Passenger walks into Player.
                }if (CollisionDetection.Aabb(player.GetsShape(), 
                    passenger.GetShape().AsStationaryShape()).Collision && !passenger.PickedUp && 
                    diffrentPlatforms[passenger.SetOffLevel].ContainsKey(passenger.GetReleasePlatformChar())&& 
                     passenger.SpawnTimer()){

                    passenger.PickedUp = true;
                    _passengerPickups.Add(passenger);
                    
                    return true;
                //If Passenger player flies into passenger. 
                } if ((CollisionDetection.Aabb(player.GetsShape(), 
                           passenger.GetShape().AsStationaryShape()).Collision && !passenger.PickedUp && 
                       passenger.GetReleasePlatformChar() == '^') && passenger.SpawnTimer()){
                    _passengerPickups.Add(passenger);
                    passenger.PickedUp = true;
                }
                //If Passenger walks into Player.
                if ((CollisionDetection.Aabb(passenger.GetShape(),
                         player.GetsShape().AsStationaryShape()).Collision && !passenger.PickedUp &&
                     passenger.GetReleasePlatformChar() == '^') && passenger.SpawnTimer())
                {
                    _passengerPickups.Add(passenger);
                    passenger.PickedUp = true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks which platform the player is supposed to be collided with.
        /// </summary>
        /// <param name="player">player</param>
        public static void CheckDropOffCollision(Player player){
            foreach (Passenger passenger in _passengerPickups){

                if (passenger.PickedUp && !passenger.DroppedOff &&
                        GameRunning.GetLevelCounter() == passenger.SetOffLevel &&
                    PlatformCollision.CollisionReleasePlatformSingle(player.GetsShape(),passenger))
                {
                    if (passenger.GetReleasePlatformChar() != '^'){
                            passenger.SetPosition(
                                passenger.GetReleasePlatform()[0].Shape.Position.X,
                                passenger.GetReleasePlatform()[0].Shape.Position.Y + 0.04f);
                            passenger.GetShape().Direction.X = 0.0f;
                    }
                    passenger.DroppedOff = true;
                    passenger.SetReleasedWithinTimer(!passenger.ReleaseTimer());
                }
            }
        }

        /// <summary>
        /// Get passenger pickups 
        /// </summary>
        /// <returns>passengerPickups</returns>
        public static List<Passenger> GetPassengerPickups()
        {
            return _passengerPickups;
        }

        public static void SetPassengerPickupsToNew()
        {
            _passengerPickups = new List<Passenger>();
        }
    }
}