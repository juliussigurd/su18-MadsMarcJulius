using System;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1
{
    public class CollisionChecker
    {
        private EntityContainer MapEntities;
        private Dictionary<char, string> Legends;
        private List<Entity> Obstacles;
        private Player Player;
        private Entity Explosion;

        public CollisionChecker(EntityContainer mapEntities, Dictionary<char, string> legends, Player player)
        {
            MapEntities = mapEntities;
            Legends = legends;
            Player = player;
            Obstacle.AddObstacles(MapEntities,Legends);
            Obstacles = Obstacle.GetObstacles();
        }
        
        public void CheckCollsion()
        {
            if (Obstacle.CollisionObstacle(Player.GetsShape(), Obstacles))
            {
                Obstacle.CreateExplosion(Player);
                Player.SetPosition(0.5f, 0.5f);  
                
            }
        }
    }
}