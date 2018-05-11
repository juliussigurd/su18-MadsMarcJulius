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
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Entity> GetObstacles()
        {
            return obstacles;
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Player"></param>
        /// <param name="obstacles"></param>
        /// <returns></returns>
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
        public static void CreateExplosion(Player player)
        {
            
            explsionShape = new StationaryShape(new Vec2F(player.GetsShape().Position.X, player.GetsShape().Position.Y),
                                                new Vec2F(player.GetsShape().Extent.X, player.GetsShape().Extent.Y));
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));

            explosion.AddAnimation(explsionShape, 500,
                                   new ImageStride(500 / 8, explosionStrides));

        }

        public static AnimationContainer getExplosion()
        {
            return explosion;
        }
    }
}