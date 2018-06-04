using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using SpaceTaxi_1.States;

namespace SpaceTaxi_1 {
    public static class Obstacle
    {
        //Fields
        private static List<Entity> obstacles;

        private static StationaryShape explsionShape;
        private static List<Image> explosionStrides = new List<Image>();
        private static AnimationContainer explosion = new AnimationContainer(8);

        

        /// <summary>
        /// get obstacles
        /// </summary>
        /// <returns>obstacles</returns>
        public static List<Entity> GetObstacles()
        {
            return obstacles;
        }
        
        
        /// <summary>
        /// Checks collision between obstacle and player
        /// </summary>
        /// <param name="Player">DynamicShape player</param>
        /// <param name="obstacles">EntityContainer of obstacles</param>
        /// <returns>bool if the collsion is true or false</returns>
        public static bool CollisionObstacle (DynamicShape Player, EntityContainer obstacles)
        {
            foreach (Entity obstacle in obstacles)
            {
            var check = CollisionDetection.Aabb(Player, obstacle.Shape);
                if (check.Collision)
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// If collision happens with obstacle or hits the platform to hard
        /// explosion happends.
        /// </summary>
        /// <param name="player">Player</param>
        public static void CreateExplosion(Player player)
        {
            
            explsionShape = new StationaryShape(new Vec2F(player.GetsShape().Position.X, player.GetsShape().Position.Y),
                                                new Vec2F(player.GetsShape().Extent.X, player.GetsShape().Extent.Y));
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));

            explosion.AddAnimation(explsionShape, 500,
                                   new ImageStride(500 / 8, explosionStrides));

        }

        /// <summary>
        /// get explosion 
        /// </summary>
        /// <returns>explosion</returns>
        public static AnimationContainer getExplosion()
        {
            return explosion;
        }
    }
}