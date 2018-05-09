using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;

namespace SpaceTaxi_1 {
    public static class Obstacle
    {
        //Fields
        private static List<Entity> obstacles;

        private static Entity Explosion;
        private static StationaryShape explsionShape;
        private static IBaseImage explosionAnimation;


        

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
            explosionAnimation = new ImageStride(80,
                ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png")));
                
            Explosion = new Entity(explsionShape,explosionAnimation);
            Explosion.RenderEntity();
            Explosion.DeleteEntity();
        }

        public static Entity getExplosion()
        {
            return Explosion;
        }
    }
}