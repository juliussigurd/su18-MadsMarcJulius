using Galaga_Exercise_2.GalagaEntities;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.EventBus;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using DIKUArcade.Physics;
using DIKUArcade.Timers;



namespace Galaga_Exercise_2.MovementStrategy
{
    public class ZigZagDown : IMovementStrategy
    {               
        public void MoveEnemy(Enemy enemy)
        {
            enemy.Shape.Position.X = (float) (enemy.position.X + (0.05f * System.Math.Sin(
                                                                      (2.0 * System.Math.PI * (enemy.position.Y - (enemy.Shape.Position.Y - 0.0003f)))/ 0.045f)));
            enemy.Shape.Position.Y -= 0.0003f;
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