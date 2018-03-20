using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.EventBus;
using System.Collections.Generic;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using Galaga_Exercise_2.GalagaEntities;


namespace Galaga_Exercise_2.MovementStrategy
{
    public interface IMovementStrategy
    {
        void MoveEnemy(Enemy enemy);
        void MoveEnemies(EntityContainer<Enemy> enemies);
      
    }
}
