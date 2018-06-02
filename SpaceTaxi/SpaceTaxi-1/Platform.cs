using DIKUArcade.Entities;
using DIKUArcade.Physics;
using System;
using System.Collections.Generic;



namespace SpaceTaxi_1 {
    public static class Platform {

        /// <summary>
        /// Checks the collision between the player and the platform. 
        /// </summary>
        /// <param name="Player"> Dynamic shape player</param>
        /// <param name="platforms">Entity container with all the platforms</param>
        /// <returns></returns>
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
       
        //TODO: samme funktion navn, samme funktion?
        /// <summary>
        /// Checks the collision between the player and the platform. 
        /// </summary>
        /// <param name="Player"> Dynamic shape player</param>
        /// <param name="platforms">Entity container with all the platforms</param>
        /// <returns></returns>
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