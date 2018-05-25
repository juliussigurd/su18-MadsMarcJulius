using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    public class Passenger {
        private new List<EntityContainer> passengerList;
        public Entity _passenger { get; private set; }
        private DynamicShape shape;
        private IBaseImage imageWalk;
        private string name;
        private int timeBeforeSpawning;
        private char platformSpawn;
        private int timeBeforeReleased;
        private int points;

        private Dictionary<char, List<Entity>> thePlatform;
      //  private System.Timers.Timer spawnTimer;

        
        public Passenger(
            string name, int timeBeforeSpawning, char platformSpawn, string platformRelease, int timeBeforeRelease,
            int points, Dictionary<char, List<Entity>> thePlatform) {
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            imageWalk = new ImageStride(80, ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkLeft.png")));
            _passenger = new Entity(shape, imageWalk);
            this.name = name;
            this.timeBeforeSpawning = timeBeforeSpawning;
            this.platformSpawn = platformSpawn;
            this.timeBeforeReleased = timeBeforeRelease;
            this.points = points;
            this.thePlatform = thePlatform;
          //spawnTimer = new System.Timers.Timer(interval: timeBeforeSpawning*1000);
            shape.Direction.X = 0.00045f;
        }
        
        public void SetPosition(float x, float y){
            shape.Position.X = x;
            shape.Position.Y = y;
            
        }
        
        public void SetExtent(float x, float y){
            shape.Extent.X = x;
            shape.Extent.Y = y;
            
        }

        public void SpawnTimer()
        {
           // spawnTimer = new System.Timers.Timer(interval: timeBeforeSpawning*1000);
            //spawnTimer.Elapsed += RenderPassenger;
        }

        public DynamicShape GetShape()
        {
            return shape;
        }

        public IBaseImage GetImage()
        {
            return imageWalk;
        }
        
        public void RenderPassenger()
        {
            if (shape.Direction.X >= 0.0f){
                imageWalk = new ImageStride(80, ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkRight.png")));
            }
            else if (shape.Direction.X <= -0.0f){
                imageWalk = new ImageStride(80, ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkLeft.png")));
            }
            _passenger.RenderEntity();
        }

        public void PassengerMove()
        {
            var platformLength = thePlatform[platformSpawn].Count;
            if (shape.Position.X >= thePlatform[platformSpawn][platformLength - 1].Shape.Position.X)
            {
                shape.Direction.X = -0.00045f;
            } else if (shape.Position.X <= thePlatform[platformSpawn][0].Shape.Position.X)
            {
                shape.Direction.X = 0.00045f;
            }
            shape.Move();
        }

        public char GetPlatFormSpawn()
        {
            return platformSpawn;
        }

    }
}