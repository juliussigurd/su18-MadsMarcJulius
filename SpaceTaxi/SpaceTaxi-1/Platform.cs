using DIKUArcade.Entities;
using DIKUArcade.Physics;
using System;
using System.Collections.Generic;



namespace SpaceTaxi_1 {
    public static class Platform {

        public static bool CollisionPlatform(DynamicShape Player, EntityContainer platforms)
        {
            foreach (Entity platform in platforms)
            {
                var check = CollisionDetection.Aabb(Player, platform.Shape); 
                {
                    if (check.Collision)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
        public static bool CollisionPlatform(DynamicShape Player, List<Entity> platforms)
        {
            foreach (Entity platform in platforms)
            {
                var check = CollisionDetection.Aabb(Player, platform.Shape); 
                {
                    if (check.Collision)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}