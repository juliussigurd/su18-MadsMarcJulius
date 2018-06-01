using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Security;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using SpaceTaxi_1.States;

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
        private string platformRelease;
        public bool pickedUp = false;
        public bool droppedOff = false;
        public int setOffLevel = 0;
        

        private Dictionary<char, List<Entity>> thePlatform;
      //  private System.Timers.Timer spawnTimer;

        
        public Passenger(
            string name, int timeBeforeSpawning, char platformSpawn, string platformRelease, int timeBeforeRelease,
            int points, Dictionary<char, List<Entity>> thePlatform, int spawnLevel) {
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            imageWalk = new ImageStride(80, ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkLeft.png")));
            _passenger = new Entity(shape, imageWalk);
            this.name = name;
            this.timeBeforeSpawning = timeBeforeSpawning;
            this.platformSpawn = platformSpawn;
            this.platformRelease = platformRelease;
            this.timeBeforeReleased = timeBeforeRelease;
            this.points = points;
            this.thePlatform = thePlatform;
            if (platformRelease.Length > 1)
            {
                setOffLevel = spawnLevel + 1;
            }
            else
            {
                setOffLevel = spawnLevel;
            }
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
            if (!pickedUp || droppedOff)
            {
                if (shape.Direction.X >= 0.0f)
                {
                    _passenger.Image = new ImageStride(80,
                        ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkRight.png")));
                }
                else if (shape.Direction.X <= -0.0f)
                {
                    _passenger.Image = new ImageStride(80,
                        ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkLeft.png")));
                }
                else
                {
                    _passenger.Image = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png"));
                }

                _passenger.RenderEntity();
            }
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

        public char GetReleasePlatform()
        {
            if (platformRelease.Length == 2)
            {
                var platformReleaseLength = platformRelease.Length;
                
                return platformRelease[platformReleaseLength - 1];
            }

            return Char.Parse(platformRelease);
        }
    }
}