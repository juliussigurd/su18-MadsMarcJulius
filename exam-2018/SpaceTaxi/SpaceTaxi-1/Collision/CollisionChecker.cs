using System.Collections.Generic;
using DIKUArcade.Entities;
using SpaceTaxi_1.Entities.Passenger;
using SpaceTaxi_1.Entities.Player;

namespace SpaceTaxi_1.Collision {
    
    public class CollisionChecker {
        //Fields
        private readonly EntityContainer obstacles;
        private readonly EntityContainer platforms;
        private readonly List<Passenger> passengers;
        private readonly List<Dictionary<char, List<Entity>>> specifiedPlatform;
        private static Player player;
        private bool gameOverChecker;
        private bool platformChecker;
        private bool passengerChecker;
      
        /// <summary>
        /// Method that checks what kind of collision.
        /// </summary>
        /// <param name="mapObstacles">Obstacles in the different levels</param>
        /// <param name="mapPlatforms">Platform in the different levels</param>
        /// <param name="player">The taxi which the user can control</param>
        /// <param name="passengers">The passengers that the taxi needs to pick up</param>
        /// <param name="specifiedPlatform">A certain platform that the passenger should be
        /// dropped of at</param>
        public CollisionChecker(EntityContainer mapObstacles, EntityContainer mapPlatforms, Player player,
            List<Passenger> passengers, List<Dictionary<char, List<Entity>>> specifiedPlatform){
            
            CollisionChecker.player = player;
            obstacles = mapObstacles;
            platforms = mapPlatforms;
            this.passengers = passengers;
            this.specifiedPlatform = specifiedPlatform;
            
        }

        /// <summary>
        /// Method that checks the collision between the player and the different entities on the
        /// different levels. Such as the platform or obstacles. Also checks if the platform is to be dropped of at.
        /// The direction check of platform checks the speed to land.
        /// </summary>
        public void CheckCollsion(){
            
            if (ObstacleCollision.CollisionObstacle(CollisionChecker.player.GetsShape(), obstacles)){
                CollisionChecker.player.Alive = false;
                gameOverChecker = true;
                
            } else if (PlatformCollision.CollisionReleasePlatform(CollisionChecker.player.GetsShape()) &&
                       CollisionChecker.player.GetsShape().Direction.Y > -0.004f){
                PassengerCollision.CheckDropOffCollision(CollisionChecker.player);
                CollisionChecker.player.Changephysics();
                platformChecker = true;
            }
            else if (PlatformCollision.CollisionPlatform(CollisionChecker.player.GetsShape(), platforms) &&
                     CollisionChecker.player.GetsShape().Direction.Y > -0.004f){
                
                CollisionChecker.player.Changephysics();
                platformChecker = true;
            }
            else if (PlatformCollision.CollisionPlatform(CollisionChecker.player.GetsShape(), platforms) &&
                     CollisionChecker.player.GetsShape().Direction.Y < -0.004f){
                
                ObstacleCollision.CreateExplosion(CollisionChecker.player);
                gameOverChecker = true;
            }
            else if (PassengerCollision.CheckCollisionPassenger(passengers, CollisionChecker.player, specifiedPlatform)){
                
                passengerChecker = true;
            }
            
        }
        /// <summary>
        /// Get game over checker 
        /// </summary>
        /// <returns>GameOverChecker</returns>
        public bool GetGameOverChecker(){
            return gameOverChecker;
        }

        /// <summary>
        /// Get platform checker
        /// </summary>
        /// <returns>PlatformChecker</returns>
        public bool GetPlatFormChecker(){
            return platformChecker;
        }

        public bool GetPassengerChecker(){
            return passengerChecker;
        }
    }
}