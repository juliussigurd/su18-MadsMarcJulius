﻿using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.EventBus;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using Galaga_Exercise_2.GalagaEntities;


namespace Galaga_Exercise_2
{
    public class Squadron3 : Squadrons.ISquadron
    {
        public int maxEnemies;

        public EntityContainer<Enemy> Enemies { get; }

        public int MaxEnemies
        {
            get { return maxEnemies; }
        }

        public Squadron3(int maxEnemies)
        {
            this.maxEnemies = maxEnemies;
            Enemies = new EntityContainer<Enemy>();
        }

        public void CreateEnemies(List<Image> enemystrides)
        {
            float check = 0.0f;
            float check2 = 0.0f;
            for (int i = 0; i <= maxEnemies / 2 -1 ; i++)
            {
                if (i <= maxEnemies)
                {
                    check = 0.1f;
                }
                

                Enemy e = new Enemy(
                    new StationaryShape((0.1f * i), (0.9f - (check * i )) , 0.1f, 0.1f),
                    new ImageStride(80, enemystrides));
                Enemies.AddStationaryEntity(e);
                //new ImageStride(80, enemystrides)
                
            }

            for (int i = 0; i <= maxEnemies / 2 - 1; i++)
            {
                if (i <= maxEnemies)
                {
                    check += 0.1f;
                }


                Enemy e = new Enemy(
                    new StationaryShape((0.3f + check), (0.3f + check), 0.1f, 0.1f),
                    new ImageStride(80, enemystrides));
                Enemies.AddStationaryEntity(e);
            }
        }
    }
}                                                       