﻿using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;

namespace SpaceTaxi_1.Collision {
    public static class ObstacleCollision {
        //Fields
        private static StationaryShape explsionShape;
        private static List<Image> explosionStrides = new List<Image>();
        private static readonly AnimationContainer EXPLOSION = new AnimationContainer(8);
        
        
        /// <summary>
        /// Checks collision between obstacle and player
        /// </summary>
        /// <param name="player">DynamicShape player</param>
        /// <param name="obstacles">EntityContainer of obstacles</param>
        /// <returns>bool if the collsion is true or false</returns>
        public static bool CollisionObstacle (DynamicShape player, EntityContainer obstacles){
            foreach (Entity obstacle in obstacles){
            var check = CollisionDetection.Aabb(player, obstacle.Shape);
                if (check.Collision){
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// If collision happens with obstacle or hits the platform to hard
        /// explosion happends.
        /// </summary>
        /// <param name="player">Player</param>
        public static void CreateExplosion(Entities.Player.Player player){
            
            ObstacleCollision.explsionShape = new StationaryShape(new Vec2F(player.GetsShape().Position.X, player.GetsShape().Position.Y),
                                                new Vec2F(player.GetsShape().Extent.X, player.GetsShape().Extent.Y));
            ObstacleCollision.explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));

            ObstacleCollision.EXPLOSION.AddAnimation(ObstacleCollision.explsionShape, 500,
                                   new ImageStride(500 / 8, ObstacleCollision.explosionStrides));

        }

        /// <summary>
        /// get explosion 
        /// </summary>
        /// <returns>explosion</returns>
        public static AnimationContainer GetExplosion(){
            return ObstacleCollision.EXPLOSION;
        }
    }
}