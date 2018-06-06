using System.Collections.Generic;
using DIKUArcade.Entities;
using SpaceTaxi_1.Entities;
using SpaceTaxi_1.Entities.Passenger;
using SpaceTaxi_1.Entities.Player;

namespace SpaceTaxi_1.Collision
{
    public class CollisionChecker
    {
        //Fields
        private readonly EntityContainer _obstacles;
        private readonly EntityContainer _platforms;
        private readonly List<Passenger> _passengers;
        private readonly List<Dictionary<char, List<Entity>>> _specifiedPlatform;
        private static Player _player;
        private bool _gameOverChecker;
        private bool _platformChecker;
        private bool _passengerChecker;
      
        /// <summary>
        ///  /// Method that checks what kind of collision. Is the taxi colliding
        /// with obstacles or passengers? 
        /// </summary>
        /// <param name="mapObstacles">Obstacles in the different levels</param>
        /// <param name="mapPlatforms">Platform in the different levels</param>
        /// <param name="player">The taxi which the user can control</param>
        /// <param name="passengers">The passengers that the taxi needs to pick up</param>
        /// <param name="specifiedPlatform">A certain platform that the passenger should be
        /// dropped of at</param>
        public CollisionChecker(EntityContainer mapObstacles, EntityContainer mapPlatforms, Player player,
            List<Passenger> passengers, List<Dictionary<char, List<Entity>>> specifiedPlatform)
        {
            _player = player;
            _obstacles = mapObstacles;
            _platforms = mapPlatforms;
            _passengers = passengers;
            _specifiedPlatform = specifiedPlatform;
            
        }

        /// <summary>
        /// Method that checks the collision between the player and the different entities on the
        /// different levels. 
        /// </summary>
        public void CheckCollsion()
        {
            if (ObstacleCollision.CollisionObstacle(_player.GetsShape(), _obstacles))
            {
                _player.Alive = false;
                _gameOverChecker = true;
                
            } else if (PlatformCollision.CollisionReleasePlatform(_player.GetsShape()) &&
                       _player.GetsShape().Direction.Y > -0.004f){
                PassengerCollision.CheckDropOffCollision(_player);
                _player.Changephysics();
                _platformChecker = true;
            }
            else if (PlatformCollision.CollisionPlatform(_player.GetsShape(), _platforms) &&
                     _player.GetsShape().Direction.Y > -0.004f)
            {
                _player.Changephysics();
                _platformChecker = true;
            }
            else if (PlatformCollision.CollisionPlatform(_player.GetsShape(), _platforms) &&
                     _player.GetsShape().Direction.Y < -0.004f)
            {
                ObstacleCollision.CreateExplosion(_player);
                _gameOverChecker = true;
            }
            else if (PassengerCollision.CheckCollisionPassenger(_passengers, _player, _specifiedPlatform))
            {
                _passengerChecker = true;
                //lav en tidscounter, der holder øje med hvor lang tid der går før han bliver sat af.
            }
            
        }
        /// <summary>
        /// Get game over checker 
        /// </summary>
        /// <returns>GameOverChecker</returns>
        public bool GetGameOverChecker()
        {
            return _gameOverChecker;
        }

        /// <summary>
        /// Get platform checker
        /// </summary>
        /// <returns>PlatformChecker</returns>
        public bool GetPlatFormChecker()
        {
            return _platformChecker;
        }

        public bool GetPassengerChecker()
        {
            return _passengerChecker;
        }
    }
}