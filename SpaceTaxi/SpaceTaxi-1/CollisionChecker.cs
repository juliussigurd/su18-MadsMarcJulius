using System;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using SpaceTaxi_1.States;

namespace SpaceTaxi_1
{
    public class CollisionChecker
    {
        //Fields
        private EntityContainer Obstacles;
        private EntityContainer Platforms;
        private List<Passenger> _passengers;
        private List<Dictionary<char, List<Entity>>> _specifiedPlatform;
        private static Player Player;
        private bool GameOverChecker;
        private bool PlatformChecker;
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
            Player = player;
            Obstacles = mapObstacles;
            Platforms = mapPlatforms;
            _passengers = passengers;
            _specifiedPlatform = specifiedPlatform;
            
        }

        /// <summary>
        /// Method that checks the collision between the player and the different entities on the
        /// different levels. 
        /// </summary>
        /// TODO: Kunne denne kode deles op i flere funktioner?
        public void CheckCollsion()
        {
            if (Obstacle.CollisionObstacle(Player.GetsShape(), Obstacles))
            {
                //TODO: lav det i player
                Player.alive = false;
                GameOverChecker = true;
            } else if (Platform.CollisionReleasePlatform(Player.GetsShape(), _specifiedPlatform) &&
                       Player.GetsShape().Direction.Y > -0.004f){
                PassengerCollision.CheckDropOffCollision();
                Player.Changephysics();
                PlatformChecker = true;
                Console.WriteLine("Du har ramt platformen");
                foreach (var passenger in PassengerCollision.GetPassengerPickups())
                {
                    Console.WriteLine(passenger.droppedOff);
                    Console.WriteLine("X: " + passenger.GetShape().Position.X + " Y: " + passenger.GetShape().Position.Y);
                }
                      
            }
            else if (Platform.CollisionPlatform(Player.GetsShape(), Platforms) &&
                     Player.GetsShape().Direction.Y > -0.004f)
            {
                Player.Changephysics();
                PlatformChecker = true;
            }
            else if (Platform.CollisionPlatform(Player.GetsShape(), Platforms) &&
                     Player.GetsShape().Direction.Y < -0.004f)
            {
                Obstacle.CreateExplosion(Player);
                GameOverChecker = true;
            }
            else if (PassengerCollision.CheckCollisionPassenger(_passengers, Player, _specifiedPlatform))
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
            return GameOverChecker;
        }

        /// <summary>
        /// Get platform checker
        /// </summary>
        /// <returns>PlatformChecker</returns>
        public bool GetPlatFormChecker()
        {
            return PlatformChecker;
        }
    }
}