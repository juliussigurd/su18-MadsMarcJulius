using Galaga_Exercise_2.GalagaEntities;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.EventBus;
using System.Collections.Generic;
using DIKUArcade.Physics;
using DIKUArcade.Timers;


namespace Galaga_Exercise_2.MovementStrategy
{
    public class NoMove : IMovementStrategy
    {
        public void MoveEnemy(Enemy enemy)
        {
            enemy.Shape.Position.Y -= 0.0f;
        }

        public void MoveEnemies(EntityContainer<GalagaEntities.Enemy> enemies)
        {
            foreach (GalagaEntities.Enemy enemy in enemies)
            {
                MoveEnemy(enemy);
            }
        }
    }
}