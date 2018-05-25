using System;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using SpaceTaxi_1.States;

namespace SpaceTaxi_1
{
    /// <summary>
    /// 
    /// </summary>
    public class CollisionChecker
    {
        //Fields
        private EntityContainer MapEntities;
        private Dictionary<char, string> Legends;
        private EntityContainer Obstacles;
        private EntityContainer Platforms;
        private Player Player;
        private Entity Explosion;
        private bool GameOverChecker;
        private bool PlatformChecker;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapEntities"></param>
        /// <param name="legends"></param>
        /// <param name="player"></param>
        public CollisionChecker(EntityContainer mapObstacles, EntityContainer mapPlatforms, Player player)
        {
            Player = player;
            Obstacles = mapObstacles;
            Platforms = mapPlatforms;

        }
        

        public void CheckCollsion()
        {
            if (Obstacle.CollisionObstacle(Player.GetsShape(), Obstacles))
            {
                //TODO: lav det i player
                Player.alive = false;
                GameOverChecker = true;
                
            } else if (Platform.CollisionPlatform(Player.GetsShape(), Platforms) &&
                       Player.GetsShape().Direction.Y > -0.004f)
            {
                Player.Changephysics();
                PlatformChecker = true;

            }
            else if (Platform.CollisionPlatform(Player.GetsShape(), Platforms) && Player.GetsShape().Direction.Y < -0.004f)
            {
                Obstacle.CreateExplosion(Player);
                GameOverChecker = true;
            }
        }

        public bool GetGameOverChecker()
        {
            return GameOverChecker;
        }

        public bool GetPlatFormChecker()
        {
            return PlatformChecker;
        }
    }
}