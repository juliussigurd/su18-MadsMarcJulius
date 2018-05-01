using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Physics;

namespace SpaceTaxi_1 {
    public static class Obstacle
    {

        private static List<Entity> obstacles = new List<Entity>();
        

        public static void AddObstacles(EntityContainer Map, Dictionary<char, string> Legends)
        {
            foreach (Entity entity in Map )
            {
                if (entity.Image != new Image(Path.Combine("Assets", "Images", Legends['1'])));
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
            foreach (Entity obstacle in obstacles)
            {
                if (CollisionDetection.Aabb(Player, obstacle.Shape).Collision)
                {
                    return true;
                }
            }
            return false;
        }
    }
}