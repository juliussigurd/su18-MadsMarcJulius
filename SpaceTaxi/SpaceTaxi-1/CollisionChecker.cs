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
                Player.DeletePlayer();
                GameOverChecker = true;
                

            } else if (Platform.CollisionPlatform(Player.GetsShape(), Platforms) && Player.GetsShape().Direction.Y > -0.004f)
            {
                Player.ChangeGravity(0.0f, 0.0f, new Vec2F(0.0f,0.0f));
            }
            else if (Platform.CollisionPlatform(Player.GetsShape(), Platforms) && Player.GetsShape().Direction.Y < -0.004f)
            {
                GameOverChecker = true;
            }
        }

        public bool GetGameOverChecker()
        {
            return GameOverChecker;
        }
    }
}