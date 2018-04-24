using DIKUArcade.Entities;
using Galaga_Exercise_3.GalagaEntities;

namespace Galaga_Exercise_3.MovementStrategy
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