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
        private int spawnLevel;
        
        private List<Dictionary<char, List<Entity>>> thePlatform;
        //  private System.Timers.Timer spawnTimer;
        
        /// <summary>
        /// Parameters and methods the passenger needs to be created
        /// </summary>
        /// <param name="name">Name of passenger</param>
        /// <param name="timeBeforeSpawning">Spawn timer</param>
        /// <param name="platformSpawn">Which platform to spawn on</param>
        /// <param name="platformRelease">Which platform to be dropped off at</param>
        /// <param name="timeBeforeRelease">Time before release</param>
        /// <param name="points">Points given</param>
        /// <param name="thePlatform">The platform to be dropped at</param>
        /// <param name="spawnLevel">In which level to spawn</param>
        public Passenger(
            string name, int timeBeforeSpawning, char platformSpawn, string platformRelease, int timeBeforeRelease,
            int points, List<Dictionary<char, List<Entity>>> thePlatform, int spawnLevel) {
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
            this.spawnLevel = spawnLevel;
            
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
        
        /// <summary>
        /// sets position of the player
        /// </summary>
        /// <param name="x">shape position x value</param>
        /// <param name="y">shape position y value </param>
        public void SetPosition(float x, float y){
            shape.Position.X = x;
            shape.Position.Y = y;
            
        }
        
        /// <summary>
        /// sets extent of the player
        /// </summary>
        /// <param name="x">The players extent x value</param>
        /// <param name="y">The players extent y value</param>
        public void SetExtent(float x, float y){
            shape.Extent.X = x;
            shape.Extent.Y = y;
            
        }

        /// <summary>
        /// Spawn timer
        /// </summary>
        public void SpawnTimer()
        {
           // spawnTimer = new System.Timers.Timer(interval: timeBeforeSpawning*1000);
            //spawnTimer.Elapsed += RenderPassenger;
        }

        /// <summary>
        /// Get shape of the player
        /// </summary>
        /// <returns>shape</returns>
        public DynamicShape GetShape()
        {
            return shape;
        }

        /// <summary>
        /// Get the image of the player
        /// </summary>
        /// <returns>imageWalk</returns>
        public IBaseImage GetImage()
        {
            return imageWalk;
        }
        
        /// <summary>
        /// Render the passenger and the image depending on which direction the player is going.
        /// </summary>
        public void RenderPassenger()
        {
            if (!pickedUp || droppedOff)
            {
                if (shape.Direction.X > 0.0f)
                {
                    _passenger.Image = new ImageStride(80,
                        ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkRight.png")));
                }
                else if (shape.Direction.X < -0.0f)
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

        
        /// <summary>
        /// How fast the passenger should move and when to switch directions.
        /// </summary>
        public void PassengerMove()
        {
            if (shape.Direction.X != 0.0f)
            {
                var platformLength = thePlatform[spawnLevel][platformSpawn].Count;
                if (shape.Position.X >= thePlatform[spawnLevel][platformSpawn][platformLength - 1].Shape.Position.X)
                {
                    shape.Direction.X = -0.00045f;
                }
                else if (shape.Position.X <= thePlatform[spawnLevel][platformSpawn][0].Shape.Position.X)
                {
                    shape.Direction.X = 0.00045f;
                }
            }
            
            shape.Move();
        }

        /// <summary>
        /// Gets the spawn of the platform
        /// </summary>
        /// <returns>platformSpawn</returns>
        public char GetPlatFormSpawn()
        {
            return platformSpawn;
        }

        /// <summary>
        /// Get platform release, which platform the passenger is to be dropped at.
        /// The entire length of the platform is counted to be one platform. 
        /// </summary>
        /// <returns></returns>
        public char GetReleasePlatformChar()
        {
            if (platformRelease.Length == 2)
            {
                var platformReleaseLength = platformRelease.Length;
                
                return platformRelease[platformReleaseLength - 1];
            }

            return Char.Parse(platformRelease);
        }

        public List<Entity> GetReleasePlatform()
        {
            var platformReleaseLength = platformRelease.Length;
            return thePlatform[setOffLevel][platformRelease[platformReleaseLength - 1]];
        }
    }
}