using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Physics;
using SpaceTaxi_1.Entities.Passenger;
using SpaceTaxi_1.States;

namespace SpaceTaxi_1.Collision {
    public static class PlatformCollision {
        /// <summary>
        ///     Checks the collision between the player and the platform.
        /// </summary>
        /// <param name="player"> Dynamic shape player</param>
        /// <param name="platforms">Entity container with all the platforms</param>
        /// <returns></returns>
        public static bool CollisionPlatform(DynamicShape player,
            EntityContainer platforms) {
            foreach (Entity platform in platforms) {
                var check = CollisionDetection.Aabb(player,
                    platform.Shape);{
                    
                    if (check.Collision) {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     Checks the collision between the player and the platform.
        /// </summary>
        /// <param name="Player"> Dynamic shape player</param>
        /// <param name="platforms">Entity container with all the platforms</param>
        /// <returns></returns>
        private static bool CollisionPlatform(DynamicShape Player,
            List<Entity> platforms) {
            foreach (var platform in platforms) {
                var check = CollisionDetection.Aabb(Player,
                    platform.Shape);{
                    
                    if (check.Collision) {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     Checks if the player is at the right platform
        /// </summary>
        /// <param name="player">player</param>
        /// <returns>bool checking if its the right platform</returns>
        public static bool CollisionReleasePlatform(DynamicShape player) {
            foreach (var passenger in PassengerCollision.GetPassengerPickups()) {
                if (passenger.GetReleasePlatformChar() != '^' &&
                    PlatformCollision.CollisionPlatform(player,
                        passenger.GetReleasePlatform()) &&
                    GameRunning.GetLevelCounter() == passenger.SetOffLevel) {
                    return true;
                }
            }

            return false;
        }

        public static bool
            CollisionReleasePlatformSingle(DynamicShape player,
                Passenger passenger) {
            if (passenger.GetReleasePlatformChar() != '^' &&
                PlatformCollision.CollisionPlatform(player,
                    passenger.GetReleasePlatform()) &&
                GameRunning.GetLevelCounter() == passenger.SetOffLevel) {
                return true;
            }

            return false;
        }
    }
}