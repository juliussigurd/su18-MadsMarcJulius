using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Galaga_Exercise_3.ISquadrons
{
    public interface ISquadron
    {
        EntityContainer<GalagaEntities.Enemy> Enemies { get; }
        int MaxEnemies { get; }

        void CreateEnemies(List<Image> enemyStrides);
    }
}