using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;

namespace SpaceTaxi_1.Entities.Passenger {
    public class Passenger {
        private Entity _passenger { get; set; }
        private readonly DynamicShape shape;
        private readonly IBaseImage imageWalk;
        private string name;
        private readonly int timeBeforeSpawning;
        private readonly char platformSpawn;
        private readonly int timeBeforeRelease;
        private readonly int cash;
        private readonly string platformRelease;
        public bool PickedUp = false;
        public bool DroppedOff = false;
        public readonly int SetOffLevel = 0;
        private readonly int spawnLevel;
        private TimedEvent spawnTimer;
        private TimedEvent releaseTimer = new TimedEvent(TimeSpanType.Minutes,10,"");
        private bool spawnTimerStarted;
        private bool releaseTimerStarted;
        private bool releasedWithinTimer;
        private bool checker = true; 
        
        private readonly List<Dictionary<char, List<Entity>>> _thePlatform;
        
        /// <summary>
        /// Parameters and methods the passenger needs to be created
        /// </summary>
        /// <param name="name">Name of passenger</param>
        /// <param name="timeBeforeSpawning">Spawn timer</param>
        /// <param name="platformSpawn">Which platform to spawn on</param>
        /// <param name="platformRelease">Which platform to be dropped off at</param>
        /// <param name="timeBeforeRelease">Time before release</param>
        /// <param name="cash">Points given</param>
        /// <param name="thePlatform">The platform to be dropped at</param>
        /// <param name="spawnLevel">In which level to spawn</param>
        public Passenger(
            string name, int timeBeforeSpawning, char platformSpawn, string platformRelease, int timeBeforeRelease,
            int cash, List<Dictionary<char, List<Entity>>> thePlatform, int spawnLevel) {
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            imageWalk = new ImageStride(80, ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkLeft.png")));
            _passenger = new Entity(shape, imageWalk);
            this.name = name;
            this.timeBeforeSpawning = timeBeforeSpawning;
            this.platformSpawn = platformSpawn;
            this.platformRelease = platformRelease;
            this.timeBeforeRelease = timeBeforeRelease;
            this.cash = cash;
            _thePlatform = thePlatform;
            this.spawnLevel = spawnLevel;
            
            
            if (platformRelease.Length > 1){
                SetOffLevel = spawnLevel + 1;
            }
            else{
                SetOffLevel = spawnLevel;
            }

          
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
        /// Get shape of the player
        /// </summary>
        /// <returns>shape</returns>
        public DynamicShape GetShape(){
            return shape;
        }

        /// <summary>
        /// Get spawn timer
        /// </summary>
        /// <returns>spwan timer</returns>
        public bool GetSpawnTimerStarted(){
            return spawnTimerStarted;
        }
        
        /// <summary>
        /// Set spawn timer bool
        /// </summary>
        /// <param name="setBool">boolean</param>
        public void SetSpawnTimerStarted(bool setBool){
            spawnTimerStarted = setBool;
        }

        /// <summary>
        /// reset spawn timer
        /// </summary>
        public void ResetSpawnTimer(){
            spawnTimer.ResetTimer();
        }
        
        /// <summary>
        /// Start spawn timer
        /// </summary>
        public void StartSpawnTimer(){
            spawnTimer = new TimedEvent(TimeSpanType.Seconds, timeBeforeSpawning, "");
        }
        
        /// <summary>
        /// Has spawn timer expired
        /// </summary>
        /// <returns>boolean telling if timer has expired</returns>
        public bool SpawnTimer(){
            return spawnTimer.HasExpired();
        }

        /// <summary>
        /// Reset timer for release
        /// </summary>
        public void ResetReleaseTimer(){
            releaseTimer.ResetTimer();
        }
        
        /// <summary>
        /// Start timer
        /// </summary>
        public void StartRelaseTimer(){
            releaseTimer = new TimedEvent(TimeSpanType.Seconds, timeBeforeRelease,"");
        }

        /// <summary>
        /// Checks if release timer has not expired
        /// </summary>
        /// <returns>Bool checking if it has not expired</returns>
        public bool ReleaseTimer(){
            return !releaseTimer.HasExpired();
        }
        
        /// <summary>
        /// get release timer
        /// </summary>
        /// <returns>_releaseTimerStarted</returns>
        public bool GetReleaseTimerStarted(){
            return releaseTimerStarted;
        }

        /// <summary>
        /// Makes a bool giving if release timer has started
        /// </summary>
        /// <param name="setBool">bool to be set</param>
        public void SetReleaseTimerStarted(bool setBool){
            releaseTimerStarted = setBool;
        }

        /// <summary>
        /// Render the passenger and the image depending on which direction the player is going.
        /// </summary>
        public void RenderPassenger(){
        
            if ((!PickedUp || DroppedOff) && SpawnTimer()){
                
                if (shape.Direction.X > 0.0f){
                    _passenger.Image = new ImageStride(80,
                        ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkRight.png")));
                }
                
                else if (shape.Direction.X < -0.0f){
                    _passenger.Image = new ImageStride(80,
                        ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkLeft.png")));
                }
                
                else{
                    _passenger.Image = new Image(Path.Combine("Assets", "Images", "CustomerStandRight.png"));
                }

                _passenger.RenderEntity();
            }
        }

        
        /// <summary>
        /// How fast the passenger should move and when to switch directions.
        /// </summary>
        public void PassengerMove(){
            
            if (shape.Direction.X != 0.0f){
                var platformLength = _thePlatform[spawnLevel][platformSpawn].Count;
                if (shape.Position.X >= _thePlatform[spawnLevel][platformSpawn][platformLength - 1].Shape.Position.X){
                    shape.Direction.X = -0.00045f;
                }
                
                else if (shape.Position.X <= _thePlatform[spawnLevel][platformSpawn][0].Shape.Position.X){
                    shape.Direction.X = 0.00045f;
                }
            }
            
            shape.Move();
        }

        /// <summary>
        /// Gets the spawn of the platform
        /// </summary>
        /// <returns>platformSpawn</returns>
        public char GetPlatFormSpawn(){
            return platformSpawn;
        }


        /// <summary>
        /// Get platform release, which platform the passenger is to be dropped at.
        /// The entire length of the platform is counted to be one platform. 
        /// </summary>
        /// <returns></returns>
        public char GetReleasePlatformChar(){
            if (platformRelease.Length == 2){
                var platformReleaseLength = platformRelease.Length;
                
                return platformRelease[platformReleaseLength - 1];
            }

            return Char.Parse(platformRelease);
        }

        /// <summary>
        /// Get release platform
        /// </summary>
        /// <returns>The platform to be set off at</returns>
        public List<Entity> GetReleasePlatform(){
            var platformReleaseLength = platformRelease.Length;
            return _thePlatform[SetOffLevel][platformRelease[platformReleaseLength - 1]];
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setBool"></param>
        public void SetReleasedWithinTimer(bool setBool){
            releasedWithinTimer = setBool;
        }

        /// <summary>
        /// Get bool if released within the time of drop of
        /// </summary>
        /// <returns>_releasedWithinTimer</returns>
        public bool GetreleasedWithinTimer(){
            return releasedWithinTimer;
        }

        /// <summary>
        /// Get amount of cash
        /// </summary>
        /// <returns>cash</returns>
        public int GetCash(){
            return cash;
        }
        
        /// <summary>
        /// Checks false or true
        /// </summary>
        /// <param name="setBool">bool to be set</param>
        public void SetChecker(bool setBool){
            checker = setBool;
        }
        
        /// <summary>
        /// a checker
        /// </summary>
        /// <returns>checks</returns>
        public bool GetChecker(){
            return checker;
        }

    }
}