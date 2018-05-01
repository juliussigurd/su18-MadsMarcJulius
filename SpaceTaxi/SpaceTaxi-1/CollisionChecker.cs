using System;
using System.Collections.Generic;
using DIKUArcade.Entities;

namespace SpaceTaxi_1
{
    public class CollisionChecker
    {
        private EntityContainer MapEntities;
        private Dictionary<char, string> Legends;
        private DynamicShape PlayerShape;
        private List<Entity> Obstacles;
        private Player Player;

        public CollisionChecker(EntityContainer mapEntities, Dictionary<char, string> legends, Player player)
        {
            MapEntities = mapEntities;
            Legends = legends;
            PlayerShape = player.GetsShape();
            Player = player;
            Obstacle.AddObstacles(MapEntities,Legends);
            Obstacles = Obstacle.GetObstacles();
        }

        public void CheckCollsion()
        {
            if (Obstacle.CollisionObstacle(PlayerShape, Obstacles))
            {
                Player.SetPosition(0.5f,0.5f);
            }
        }
    }
}