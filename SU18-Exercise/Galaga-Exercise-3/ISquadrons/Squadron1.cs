﻿using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Galaga_Exercise_3.GalagaEntities;

namespace Galaga_Exercise_3.ISquadrons {
    public class Squadron1 : ISquadron {
        public int maxEnemies;

        public EntityContainer<Enemy> Enemies { get; }

        public int MaxEnemies {
            get { return maxEnemies; }
        }

        public Squadron1(int maxEnemies) {
            this.maxEnemies = maxEnemies;
            Enemies = new EntityContainer<Enemy>();
        }

        public void CreateEnemies(List<Image> enemystrides) {
            float check = 0.0f;
            for (int i = 0; i <= maxEnemies; i++) {
                if (i % 2 == 0) {
                    check = 0.0f;
                } else {
                    check = 0.1f;
                }

                Enemy e = new Enemy(
                    new StationaryShape((0.1f * i), (0.9f - check), 0.1f, 0.1f),
                    new ImageStride(80, enemystrides));
                Enemies.AddStationaryEntity(e);
                //new ImageStride(80, enemystrides)
            }
        }
    }
}