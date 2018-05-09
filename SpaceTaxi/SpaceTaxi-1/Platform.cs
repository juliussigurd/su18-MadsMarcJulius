using DIKUArcade.Entities;
using DIKUArcade.Physics;

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
    }
}