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
        private readonly DynamicShape _shape;
        private readonly IBaseImage _imageWalk;
        private string name;
        private readonly int _timeBeforeSpawning;
        private readonly char _platformSpawn;
        private readonly int _timeBeforeRelease;
        private readonly int _points;
        private readonly string _platformRelease;
        public bool PickedUp = false;
        public bool DroppedOff = false;
        public readonly int SetOffLevel = 0;
        private readonly int _spawnLevel;
        private TimedEvent _spawnTimer;
        private TimedEvent _releaseTimer = new TimedEvent(TimeSpanType.Minutes,10,"");
        private bool _spawnTimerStarted;
        private bool _releaseTimerStarted;
        private bool _releasedWithinTimer;
        private bool _checker = true; 
        
        private readonly List<Dictionary<char, List<Entity>>> _thePlatform;
        
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
            _shape = new DynamicShape(new Vec2F(), new Vec2F());
            _imageWalk = new ImageStride(80, ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkLeft.png")));
            _passenger = new Entity(_shape, _imageWalk);
            this.name = name;
            _timeBeforeSpawning = timeBeforeSpawning;
            _platformSpawn = platformSpawn;
            _platformRelease = platformRelease;
            _timeBeforeRelease = timeBeforeRelease;
            _points = points;
            _thePlatform = thePlatform;
            _spawnLevel = spawnLevel;
            
            
            if (platformRelease.Length > 1)
            {
                SetOffLevel = spawnLevel + 1;
            }
            else
            {
                SetOffLevel = spawnLevel;
            }

          
          //spawnTimer = new System.Timers.Timer(interval: timeBeforeSpawning*1000);
            _shape.Direction.X = 0.00045f;
        }
        
        /// <summary>
        /// sets position of the player
        /// </summary>
        /// <param name="x">shape position x value</param>
        /// <param name="y">shape position y value </param>
        public void SetPosition(float x, float y){
            _shape.Position.X = x;
            _shape.Position.Y = y;
            
        }
        
        /// <summary>
        /// sets extent of the player
        /// </summary>
        /// <param name="x">The players extent x value</param>
        /// <param name="y">The players extent y value</param>
        public void SetExtent(float x, float y){
            _shape.Extent.X = x;
            _shape.Extent.Y = y;
        }
        
        /// <summary>
        /// Get shape of the player
        /// </summary>
        /// <returns>shape</returns>
        public DynamicShape GetShape()
        {
            return _shape;
        }

        /// <summary>
        /// Get the image of the player
        /// </summary>
        /// <returns>imageWalk</returns>

        public bool GetSpawnTimerStarted()
        {
            return _spawnTimerStarted;
        }
        
        public void SetSpawnTimerStarted(bool setBool)
        {
            _spawnTimerStarted = setBool;
        }

        public void ResetSpawnTimer()
        {
            _spawnTimer.ResetTimer();
        }
        
        public void StartSpawnTimer()
        {
            _spawnTimer = new TimedEvent(TimeSpanType.Seconds, _timeBeforeSpawning, "");
        }
        
        public bool SpawnTimer()
        {
            return _spawnTimer.HasExpired();
        }

        public void ResetReleaseTimer()
        {
            _releaseTimer.ResetTimer();
        }
        public void StartRelaseTimer()
        {
            _releaseTimer = new TimedEvent(TimeSpanType.Seconds, _timeBeforeRelease,"");
        }

        public bool ReleaseTimer()
        {
            return !_releaseTimer.HasExpired();
        }
        
        public bool GetReleaseTimerStarted()
        {
            return _releaseTimerStarted;
        }

        public void SetReleaseTimerStarted(bool setBool)
        {
            _releaseTimerStarted = setBool;
        }

        /// <summary>
        /// Render the passenger and the image depending on which direction the player is going.
        /// </summary>
        public void RenderPassenger()
        {
        
            if ((!PickedUp || DroppedOff) && SpawnTimer())
            {
                if (_shape.Direction.X > 0.0f)
                {
                    _passenger.Image = new ImageStride(80,
                        ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkRight.png")));
                }
                else if (_shape.Direction.X < -0.0f)
                {
                    _passenger.Image = new ImageStride(80,
                        ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkLeft.png")));
                }
                else
                {
                    _passenger.Image = new Image(Path.Combine("Assets", "Images", "CustomerStandRight.png"));
                }

                _passenger.RenderEntity();
            }
        }

        
        /// <summary>
        /// How fast the passenger should move and when to switch directions.
        /// </summary>
        public void PassengerMove()
        {
            if (_shape.Direction.X != 0.0f)
            {
                var platformLength = _thePlatform[_spawnLevel][_platformSpawn].Count;
                if (_shape.Position.X >= _thePlatform[_spawnLevel][_platformSpawn][platformLength - 1].Shape.Position.X)
                {
                    _shape.Direction.X = -0.00045f;
                }
                else if (_shape.Position.X <= _thePlatform[_spawnLevel][_platformSpawn][0].Shape.Position.X)
                {
                    _shape.Direction.X = 0.00045f;
                }
            }
            
            _shape.Move();
        }

        /// <summary>
        /// Gets the spawn of the platform
        /// </summary>
        /// <returns>platformSpawn</returns>
        public char GetPlatFormSpawn()
        {
            return _platformSpawn;
        }


        /// <summary>
        /// Get platform release, which platform the passenger is to be dropped at.
        /// The entire length of the platform is counted to be one platform. 
        /// </summary>
        /// <returns></returns>
        public char GetReleasePlatformChar()
        {
            if (_platformRelease.Length == 2)
            {
                var platformReleaseLength = _platformRelease.Length;
                
                return _platformRelease[platformReleaseLength - 1];
            }

            return Char.Parse(_platformRelease);
        }

        public List<Entity> GetReleasePlatform()
        {
            
            var platformReleaseLength = _platformRelease.Length;
            return _thePlatform[SetOffLevel][_platformRelease[platformReleaseLength - 1]];
        }

        public bool GetreleasedWithinTimer()
        {
            return _releasedWithinTimer;
        }

        public void SetReleasedWithinTimer(bool setBool)
        {
            _releasedWithinTimer = setBool;
        }

        public int GetPoints()
        {
            return _points;
        }
        
        public bool GetChecker()
        {
            return _checker;
        }

        public void SetChecker(bool setBool)
        {
            _checker = setBool;
        }
    }
}