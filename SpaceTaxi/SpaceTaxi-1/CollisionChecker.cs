using System;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

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
                Obstacle.CreateExplosion(Player);
                Player.SetPosition(0.5f, 0.5f);  
                
            }
        }
    }
}