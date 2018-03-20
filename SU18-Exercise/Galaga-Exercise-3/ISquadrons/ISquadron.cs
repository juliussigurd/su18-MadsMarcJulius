using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.EventBus;
using System.Collections.Generic;


namespace Galaga_Exercise_2.Squadrons
{
    public interface ISquadron
    {
        EntityContainer<GalagaEntities.Enemy> Enemies { get; }
        int MaxEnemies { get; }

        void CreateEnemies(List<Image> enemyStrides);
    }
}