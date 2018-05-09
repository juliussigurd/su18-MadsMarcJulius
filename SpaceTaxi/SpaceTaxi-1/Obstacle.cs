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

        private static List<Entity> obstacles;
        

        public static void AddObstacles(EntityContainer MapEntities, Dictionary<char, string> Legends)
        {
            obstacles = new List<Entity>();
            foreach (Entity entity in MapEntities)
            {
                //if (entity.Image != new Image(Path.Combine("Assets", "Images", Legends['1'])));
                {
                    obstacles.Add(entity);
                }
            }
        }
        public static List<Entity> GetObstacles()
        {
            return obstacles;
        }
        
        public static bool CollisionObstacle (DynamicShape Player, List<Entity> obstacles)
        {
            //var pos = new Vec2F(0.61f, 0.6f);
           // var extent = new Vec2F(0.39f, 0.4f);
            //var shape = new StationaryShape(pos, extent);
            foreach (Entity obstacle in obstacles)
            {
            var check = CollisionDetection.Aabb(Player, obstacle.Shape);
                if (check.Collision)
                {
                    return true;
                }
            }

            
            /*foreach (Entity obstacle in obstacles)
            {
                if (CollisionDetection.Aabb(Player, obstacle.Shape).Collision)
                {
                    Console.WriteLine(obstacle.Shape.Position);
                    return true;
                    
                }
            }*/
            return false;
        }
    }
}