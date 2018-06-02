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
        private EntityContainer MapEntities;
        private Dictionary<char, string> Legends;
        private EntityContainer Obstacles;
        private EntityContainer Platforms;
        private List<Passenger> _passengers;
        private List<Dictionary<char, List<Entity>>> _specifiedPlatform;
        private static Player Player;
        private Entity Explosion;
        private bool GameOverChecker;
        private bool PlatformChecker;
        private bool _passengerChecker;
        private static List<Passenger> passengerPickups;
        private static List<List<Entity>> passengerReleasePlatforms;

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
            passengerPickups = new List<Passenger>();
            passengerReleasePlatforms = new List<List<Entity>>();
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

            }
            else if (Platform.CollisionPlatform(Player.GetsShape(), Platforms) &&
                     Player.GetsShape().Direction.Y > -0.004f)
            {
                Player.Changephysics();
                PlatformChecker = true;
                Console.WriteLine(passengerPickups.Count + " Marc er swag");
                Console.WriteLine(passengerReleasePlatforms.Count + "ReleasePlatforms");
                if (passengerReleasePlatforms.Count > 0)
                {
                    Console.WriteLine(passengerReleasePlatforms[0].Count
                                      + " ReleasePlatforms Entities");
                }
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
                passengerPickups.Add(PassengerCollision.GetPassengerPickups());
                passengerReleasePlatforms.Add(PassengerCollision.GetReleasePlatforms());
                if (passengerReleasePlatforms.Count < 0)
                {
                    Console.WriteLine(passengerReleasePlatforms[0].Count
                                      + "ReleasePlatforms");
                }

                Console.WriteLine(passengerPickups.Count + "Amount of passengers picked up");
                //lav en counter, der holder øje med hvor lang tid der går før han bliver sat af.
            }
            
            foreach (var A in passengerReleasePlatforms)
            {
                if (Platform.CollisionPlatform(Player.GetsShape(), A)) {
                    Console.WriteLine("Du har ramt platformen");
                    PassengerCollision.CheckDropOffCollision(A , passengerPickups);
                }
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