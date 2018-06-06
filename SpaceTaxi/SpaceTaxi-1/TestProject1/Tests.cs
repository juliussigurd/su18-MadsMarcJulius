using System;
using System.Collections.Generic;
using System.ComponentModel;
using NUnit.Framework;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using NUnit.Framework.Internal;
using SpaceTaxi_1;
using SpaceTaxi_1.Collision;
using SpaceTaxi_1.Entities.Passenger;
using SpaceTaxi_1.Entities.Player;
using SpaceTaxi_1.States;


namespace TestProject1
{
    [TestFixture]
    public class Tests
    {
        private readonly string[] _filePath = Directory.GetFiles("Levels");
        private string[] _levelinfo;
        private Dictionary<char, string> _dictionary = new Dictionary<char, string>();
        private IBaseImage _testImage;
        private EntityContainer _testCollisionObstacle;
        private EntityContainer _testCollisionPlatform;
        private Player _testPlayer;
        private StateMachine _testStateMachine;
        private Game _game;
        private Passenger _testPassenger;
        private List<Passenger> _testPassengerList;
        private List<Dictionary<char, List<Entity>>> _testSpecifiedPlatform;

        [SetUp]
        public void OpenWindows()
        {
            var _win = new Window("Space Taxi Game v0.1", 500, AspectRatio.R1X1);
        }
        
        
        [Test]//Testing it finds the first line right in level 1.
        public void ReadFileTestFirstLineLevel1()
        {
            _levelinfo = Level.ReadFile(_filePath[0]);
            Assert.AreEqual(_levelinfo[0], "%#%#%#%#%#%#%#%#%#^^^^^^%#%#%#%#%#%#%#%#");
        }
        [Test]//Testing it reads the right line in the middle of level 1.
        public void ReadFileTestMiddleLineLevel1()
        {
            _levelinfo = Level.ReadFile(_filePath[0]);
            Assert.AreEqual(_levelinfo[27], "%) white-square.png");
        }
        [Test]//Testing it reads the last line in level 1.
        public void ReadFileTestLastLineLevel1() //
        {
            _levelinfo = Level.ReadFile(_filePath[0]);
            Assert.AreEqual(_levelinfo[_levelinfo.Length-1], "Customer: Alice 10 1 ^J 10 100");
        }
        [Test] //Testing it finds the first line right in level 2.
        public void ReadFileTestFirstLineLevel2()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Assert.AreEqual(_levelinfo[0], "CTTTTTTTTTTTTTTTTD^^^^^^CTTTTTTTTTTTTttt");
        }
        [Test] //Testing it reads the right line in the middle of level 2.
        public void ReadFileTestMiddleLineLevel2()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Assert.AreEqual(_levelinfo[27], "A) aspargus-edge-left.png");
        }

        [Test] //Testing it reads the last line in level 2.
        public void ReadFileTestLastLineLevel2()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Assert.AreEqual(_levelinfo[_levelinfo.Length-1], "Customer: Carol 30 r ^ 10 100");
        }

        [Test] //Testing it will add a valid string.
        public void ReadLegendTestValidString()
        {
            Level.SetLegendsDictionaryToNew();
            Level.ReadLegend("j) ironstone-lower-right.png");
            Assert.AreEqual(Level.GetLegendsDictionary()['j'], "ironstone-lower-right.png");
        }
        [Test] //Testing it wont add anything from an empty string.
        public void ReadLegendTestEmptyString()
        {    
            Level.SetLegendsDictionaryToNew();
            Level.ReadLegend("");
            Assert.AreEqual(Level.GetLegendsDictionary().Count, 0);
        }
        [Test] // Testing it wont add wrong legend.
        public void ReadLegendTestInvalidString()
        {
            Level.SetLegendsDictionaryToNew();
            Level.ReadLegend("asds,jdhbfslkdf");
            Assert.AreEqual(Level.GetLegendsDictionary().Count, 0);
        }
        [Test]//Testing Readlegends with more lines.
        public void ReadLegendTestMultipleStrings()
        {
            Level.SetLegendsDictionaryToNew();
            Level.ReadLegend("019283joijalksasd");
            Level.ReadLegend("l) ironstone-lower-left.png");
            Level.ReadLegend("");
            Level.ReadLegend("r) studio-square.png");
            Assert.AreEqual(Level.GetLegendsDictionary().Count, 2);
        }

        [Test] // add obstacle to "All-Container.
        public void AddObstacleEntityToAllContainerTest()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.SetLegendsDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddEntityToContainer(_levelinfo, _dictionary, 0, 0);
            Assert.AreEqual(Level.GetLevelEntities().CountEntities(),1);
        }
        //testing it wont add Entity add with a space ' '.
        [Test]
        public void TryAddInvalidEntityToAllContainerTest()
        {
            Level.SetAllEntitiesToNew();
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.SetLegendsDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddEntityToContainer(_levelinfo, _dictionary, 12, 3);
            Assert.AreEqual(Level.GetLevelEntities().CountEntities(),0);
        }
        [Test] // Adding platform to the "All-List"
        public void AddPlatformEntityToAllContainerTest()
        {
            Level.SetAllEntitiesToNew();
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.SetLegendsDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddEntityToContainer(_levelinfo, _dictionary, 21, 13);
            Assert.AreEqual(Level.GetLevelEntities().CountEntities(),1);
        }
        
        [Test] // adding obstacle to obstacleContainer.
        public void AddEntityToObstacleContainerTest()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.ReadPlatforms(_levelinfo[25]);
            Level.SetLegendsDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddObstacle(_levelinfo, _dictionary, 0, 0);
            Assert.AreEqual(Level.GetLevelObstacles().CountEntities(),1);
        }
        [Test] //Can't put ' ' in a a obstacleContainer.
        public void TryAddInvalidEntityToObstacleContainerTest2()
        {
            Level.SetObstaclesToNew();
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.ReadPlatforms(_levelinfo[25]);
            Level.SetLegendsDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddObstacle(_levelinfo, _dictionary, 12, 3);
            Assert.AreEqual(Level.GetLevelObstacles().CountEntities(),0);
        }
        [Test] //Can't add platform to obstacleContainer.
        public void TryAddPlatformEntityToObstacleContainerTest3()
        {
            Level.SetObstaclesToNew();
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.ReadPlatforms(_levelinfo[25]);
            Level.SetLegendsDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddObstacle(_levelinfo, _dictionary, 21, 13);
            Assert.AreEqual(Level.GetLevelObstacles().CountEntities(),0);
        }
        //Can't add obstacle to platformContainer
        [Test]
        public void TryAddObstacleEntityToPlatformContainerTest()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.ReadPlatforms(_levelinfo[25]);
            Level.SetLegendsDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddPlatform(_levelinfo, _dictionary, 0, 0);
            Assert.AreEqual(Level.GetLevelPlatforms().CountEntities(),0);
        }
        
        [Test] //Cant put nothing in PlatformConation
        public void NotFoundPlatformAddEntityToPlatformContainerTest()
        {
            Level.SetPlatformsToNew();
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.ReadPlatforms(_levelinfo[25]);
            Level.SetLegendsDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddPlatform(_levelinfo, _dictionary, 12, 3);
            Assert.AreEqual(Level.GetLevelPlatforms().CountEntities(),0);
        }
        [Test] //Putting Platform in platformContainer
        public void AddEntityToPlatformContainerTest()
        {
            Level.SetPlatformsToNew();
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.ReadPlatforms(_levelinfo[25]);
            Level.SetLegendsDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddPlatform(_levelinfo, _dictionary, 21, 13);
            Assert.AreEqual(Level.GetLevelPlatforms().CountEntities(),1);
        }
        [Test]// testing it finds x-coordinate in lvl 2.
        public void PlayerPosOfLevel2TestX()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.PlayerPosOfLevel(_levelinfo, 14, 15);
            Assert.AreEqual(Level.GetPlayerPosX(), 0.375f);
        }
        
        [Test] // testing it finds y-coordinate in lvl 2.
        public void PlayerPosOfLevel2TestY()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.PlayerPosOfLevel(_levelinfo, 14, 15);
            Assert.AreEqual(Level.GetPlayerPosY(), 0.347826093f);
        }
        [Test]//Testing it wont find an x-coordinate, where player is not.
        public void NotFoundPlayerPosOfLevelTestX()
        {
            Level.ChangePosX(0f);
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.PlayerPosOfLevel(_levelinfo, 0, 0);
            Assert.AreEqual(Level.GetPlayerPosX(), 0f);
        }
        
        [Test]//Testing it wont find an y-coordinate, where player is not.
        public void NotFoundPlayerPosOfLevelTestY()
        {
            Level.ChangePosY(0f);
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.PlayerPosOfLevel(_levelinfo, 0, 0);
            Assert.AreEqual(Level.GetPlayerPosY(), 0f);
        }
        [Test]// Testing it finds the right x-coordinate in level 1.
        public void PlayerPosOfLevel1TestX()
        {
            Level.ChangePosX(0f);
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.PlayerPosOfLevel(_levelinfo, 3, 31);
            Assert.AreEqual(Level.GetPlayerPosX(), 0.774999976f);
        }
        
        [Test]// Testing it finds the right y-coordinate in level 1.
        public void PlayerPosOfLevel1TestY()
        {
            Level.ChangePosY(0f);
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.PlayerPosOfLevel(_levelinfo, 3, 31);
            Assert.AreEqual(Level.GetPlayerPosY(), 0.826086938f);
        }

        
        // They return the right types, but tests wont pass.
        [Test]
        public void PlayerImageTest1()
        {
            _testImage = PlayerImage.ImageDecider(-1);
            Assert.AreEqual(_testImage, new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png")));
        }
        
        [Test]
        public void PlayerImageTest2()
        {
            _testImage = PlayerImage.ImageDecider(Int32.MaxValue);
            Assert.AreEqual(_testImage, new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png")));
        }

       [Test]
        public void PlayerImageTest3()
        {
            _testImage = PlayerImage.ImageDecider(12);
            Assert.AreEqual(_testImage, new ImageStride(80,
                ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back.png"))));
        }

        [Test] //Testing collision with obstacle, will lose game. 
        public void CollisionObstacleTest()
        {
            _testCollisionObstacle = new EntityContainer();
            _testCollisionPlatform = new EntityContainer();
            _testSpecifiedPlatform = new List<Dictionary<char, List<Entity>>>();
            _testPassengerList = new List<Passenger>();
            
            var stationaryShape = new StationaryShape(new Vec2F(0.5f,0.0f) , new Vec2F(0.5f,1.0f));
            var image = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
            
            _testPlayer = new Player();
            _testPlayer.SetPosition(0.3f, 0.5f);
            _testPlayer.GetsShape().Direction.X = 0.001f;
            _testPlayer.GetsShape().Direction.Y = 0.00000000001f;
            
            _testCollisionObstacle.AddStationaryEntity(stationaryShape,image);
            
            CollisionChecker collisionTest = new CollisionChecker(_testCollisionObstacle,_testCollisionPlatform,
                _testPlayer,_testPassengerList,_testSpecifiedPlatform);
            
            while (!collisionTest.GetGameOverChecker())
            {
                _testPlayer.PlayerMove();
                collisionTest.CheckCollsion();
            }          
            Assert.AreEqual(collisionTest.GetGameOverChecker(), true);

        }
        [Test] //Testing Collision with platform works.
        public void CollisionWithPlatformTest()
        {
            _testCollisionObstacle = new EntityContainer();
            _testCollisionPlatform = new EntityContainer();
            _testSpecifiedPlatform = new List<Dictionary<char, List<Entity>>>();
            _testPassengerList = new List<Passenger>();
            
            var stationaryShape = new StationaryShape(new Vec2F(0.0f,0.0f) , new Vec2F(1.0f,0.5f));
            var image = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
            
            _testPlayer = new Player();
            _testPlayer.SetPosition(0.5f, 0.6f);
            _testPlayer.GetsShape().Direction.Y = -0.001f;
            
            _testCollisionPlatform.AddStationaryEntity(stationaryShape,image);
            
            CollisionChecker collisionTest = new CollisionChecker(_testCollisionObstacle,_testCollisionPlatform,
                _testPlayer,_testPassengerList,_testSpecifiedPlatform);
            
            while (!collisionTest.GetPlatFormChecker())
            {
                _testPlayer.PlayerMove();
                collisionTest.CheckCollsion();
            }          
            Assert.AreEqual(collisionTest.GetPlatFormChecker(), true);

        }
        [Test] // Testing too much speed collision with platform results in death.
        public void CollisionWithPlatformDeathTest()
        {
            _testCollisionObstacle = new EntityContainer();
            _testCollisionPlatform = new EntityContainer();
            _testSpecifiedPlatform = new List<Dictionary<char, List<Entity>>>();
            _testPassengerList = new List<Passenger>();
            
            var stationaryShape = new StationaryShape(new Vec2F(0.0f,0.0f) , new Vec2F(1.0f,0.5f));
            var image = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
            
            _testPlayer = new Player();
            _testPlayer.SetPosition(0.5f, 0.6f);
            _testPlayer.GetsShape().Direction.Y = -0.005f;
            
            _testCollisionPlatform.AddStationaryEntity(stationaryShape,image);
            
            CollisionChecker collisionTest = new CollisionChecker(_testCollisionObstacle,_testCollisionPlatform,
                _testPlayer,_testPassengerList,_testSpecifiedPlatform);
            
            while (!collisionTest.GetGameOverChecker())
            {
                _testPlayer.PlayerMove();
                collisionTest.CheckCollsion();
            }          
            Assert.AreEqual(collisionTest.GetGameOverChecker(), true);
        }

        [Test] 
        public void CollisionWithPassenger()
        {
            _testCollisionObstacle = new EntityContainer();
            _testCollisionPlatform = new EntityContainer();
            _testSpecifiedPlatform = new List<Dictionary<char, List<Entity>>>();
            _testPassengerList = new List<Passenger>();
            
            Level.SetLegendsDictionaryToNew();
            Level.SetDiffrentPlatformsToNew();
            Level.SetPlatformLegendsToNew();
            
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.ReadPlatforms(_levelinfo[25]);
            Level.ReadLegends(_levelinfo);
            for (int j = 0; j < 23; j++)
            {
                for (int c = 0; c < Level.GetPlatformLegends().Count; c++)
                {
                    for (int i = 0; i < _levelinfo[0].Length; i++)
                    {
                        Level.SpecifyPlatforms(_levelinfo,Level.GetLegendsDictionary(),j,i,c);
                    }
                }
            }
            _testSpecifiedPlatform.Add(Level.GetDiffrenPlatforms());
            
            Level.SetLegendsDictionaryToNew();
            Level.SetDiffrentPlatformsToNew();
            Level.SetPlatformLegendsToNew();
            
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.ReadPlatforms(_levelinfo[25]);
            Level.ReadLegends(_levelinfo);
            for (int j = 0; j < 23; j++)
            {
                for (int c = 0; c < Level.GetPlatformLegends().Count; c++)
                {
                    for (int i = 0; i < _levelinfo[0].Length; i++)
                    {
                        Level.SpecifyPlatforms(_levelinfo,Level.GetLegendsDictionary(),j,i,c);
                    }
                }
            }
            _testSpecifiedPlatform.Add(Level.GetDiffrenPlatforms());
            
            _testPlayer = new Player();
            _testPlayer.SetPosition(0.1f, 0.6f);
            _testPlayer.GetsShape().Direction.X = 0.005f;
            _testPlayer.GetsShape().Direction.Y = 0.0000001f;
            
            _testPassenger = new Passenger("Bob",0,'1',"^",10,100,_testSpecifiedPlatform,0);
            _testPassenger.GetShape().Direction.X = 0.0f;
            _testPassenger.SetPosition(0.9f,0.0f);
            _testPassenger.SetExtent(0.1f,1.0f);
            _testPassenger.StartSpawnTimer();
            
            _testPassengerList.Add(_testPassenger);
            
            CollisionChecker collisionTest = new CollisionChecker(_testCollisionObstacle,_testCollisionPlatform,
                _testPlayer,_testPassengerList,_testSpecifiedPlatform);
            
            while (!_testPassengerList[0].PickedUp)
            {
                _testPlayer.PlayerMove();
                collisionTest.CheckCollsion();
            }          
            Assert.AreEqual(_testPassengerList[0].PickedUp, true);
        }

        [Test] // Testing it finds the passenger info from level 1.
        public void UpdatePassengerInfoTest1()
        {
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.UpdatePassengerInfo(_levelinfo);
            Assert.AreEqual(Level.GetPassengerInfo()[0][1], "Alice");
            Assert.AreEqual(Level.GetPassengerInfo()[0][2], "10");
            Assert.AreEqual(Level.GetPassengerInfo()[0][3], "1");
            Assert.AreEqual(Level.GetPassengerInfo()[0][4], "^J");
            Assert.AreEqual(Level.GetPassengerInfo()[0][5], "10");
            Assert.AreEqual(Level.GetPassengerInfo()[0][6], "100");
        }

        [Test] // Testing it finds the right passenger info in level 2.
        public void UpdatePassengerInfoTest2()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.UpdatePassengerInfo(_levelinfo);
            Assert.AreEqual(Level.GetPassengerInfo()[0][1], "Bob");
            Assert.AreEqual(Level.GetPassengerInfo()[0][2], "10");
            Assert.AreEqual(Level.GetPassengerInfo()[0][3], "J");
            Assert.AreEqual(Level.GetPassengerInfo()[0][4], "r");
            Assert.AreEqual(Level.GetPassengerInfo()[0][5], "10");
            Assert.AreEqual(Level.GetPassengerInfo()[0][6], "100");
        }
        
        [Test]// Testing it finds the right passenger info in level 2.
        public void UpdatePassengerInfoTest3()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.UpdatePassengerInfo(_levelinfo);
            Assert.AreEqual(Level.GetPassengerInfo()[1][1], "Carol");
            Assert.AreEqual(Level.GetPassengerInfo()[1][2], "30");
            Assert.AreEqual(Level.GetPassengerInfo()[1][3], "r");
            Assert.AreEqual(Level.GetPassengerInfo()[1][4], "^");
            Assert.AreEqual(Level.GetPassengerInfo()[1][5], "10");
            Assert.AreEqual(Level.GetPassengerInfo()[1][6], "100");
        }

        [Test] // Testing it adds the right platform to a certain char.
        public void SpecifyplatformTest1()
        {
            Level.SetLegendsDictionaryToNew();
            Level.SetDiffrentPlatformsToNew();
            Level.SetPlatformLegendsToNew();
            
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.ReadPlatforms(_levelinfo[25]);
            Level.ReadLegends(_levelinfo);
            for (int j = 0; j < 23; j++)
            {
                for (int c = 0; c < Level.GetPlatformLegends().Count; c++)
                {
                    for (int i = 0; i < _levelinfo[0].Length; i++)
                    {
                        Level.SpecifyPlatforms(_levelinfo,Level.GetLegendsDictionary(),j,i,c);
                    }
                }
            }
            Assert.AreEqual(Level.GetDiffrenPlatforms()['1'].Count,13);
          }
        
        
        [Test] // Testing it adds the right platform to a certain chars.
        public void SpecifyplatformTest2()
        {
            Level.SetLegendsDictionaryToNew();
            Level.SetDiffrentPlatformsToNew();
            Level.SetPlatformLegendsToNew();
            
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.ReadPlatforms(_levelinfo[25]);
            Level.ReadLegends(_levelinfo);
            for (int j = 0; j < 23; j++)
            {
                for (int c = 0; c < Level.GetPlatformLegends().Count; c++)
                {
                    for (int i = 0; i < _levelinfo[0].Length; i++)
                    {
                        Level.SpecifyPlatforms(_levelinfo,Level.GetLegendsDictionary(),j,i,c);
                    }
                }
            }
            Assert.AreEqual(Level.GetDiffrenPlatforms()['J'].Count,8);
            Assert.AreEqual(Level.GetDiffrenPlatforms()['i'].Count, 14);
            Assert.AreEqual(Level.GetDiffrenPlatforms()['r'].Count,5);;
        }

        
        [Test] //Testing player physics with one tic.
        public void playerPhysics1()
        {
            _testPlayer = new Player();
            _testPlayer.GetsShape().SetPosition(new Vec2F(0.5f,0.9f));
            _testPlayer.GetsShape().Direction.Y = -0.045f;
            _testPlayer.GetsShape().Direction.X = 0.045f;
            _testPlayer.Physics();
            
            Assert.AreEqual(_testPlayer.GetsShape().Position.X, 0.545000017f);
            Assert.AreEqual(_testPlayer.GetsShape().Position.Y, 0.854999959f);
        }
        
        [Test] // Testing player for a decent amount of tics.
        public void playerPhysics2()
        {
            _testPlayer = new Player();
            _testPlayer.GetsShape().SetPosition(new Vec2F(0.5f,0.9f));
            _testPlayer.GetsShape().Direction.Y = -0.045f;
            _testPlayer.Physics();
            _testPlayer.Physics();
            _testPlayer.Physics();
            _testPlayer.Physics();
            _testPlayer.Physics();
            Assert.AreEqual(_testPlayer.GetsShape().Position.X, 0.5f);
            Assert.AreEqual(_testPlayer.GetsShape().Position.Y, 0.674999893f);
        }
        [Test] //Testing physics after 60 iterations, which is the FPS for our game.
        public void playerPhysics3()
        {
            _testPlayer = new Player();
            _testPlayer.GetsShape().SetPosition(new Vec2F(0.5f,0.5f));
            _testPlayer.GetsShape().Direction.X = 0.045f;
            _testPlayer.GetsShape().Direction.Y = 0.001f;
            for (int i = 0; i < 60; i++)
            {
                _testPlayer.Physics();
            }

            Assert.AreEqual(_testPlayer.GetsShape().Position.X, 3.20000124f);
            Assert.AreEqual(_testPlayer.GetsShape().Position.Y, 0.559999228f);
            
        }
        [Test] //Testing Player wont move if, Y.Direction is set to 0.0f.
        public void playerPhysics4()
        {
            _testPlayer = new Player();
            _testPlayer.GetsShape().SetPosition(new Vec2F(0.5f,0.5f));
            _testPlayer.GetsShape().Direction.X = 0.045f;
            _testPlayer.GetsShape().Direction.Y = 0.0f;
            for (int i = 0; i < 60; i++){
                _testPlayer.Physics();
            }

            Assert.AreEqual(_testPlayer.GetsShape().Position.X, 0.5f);
            Assert.AreEqual(_testPlayer.GetsShape().Position.Y, 0.5f);
            
        }
    }
}