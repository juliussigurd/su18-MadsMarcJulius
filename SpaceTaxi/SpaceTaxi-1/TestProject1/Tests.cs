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
        private StateMachine _testStateMachine; //
        private Game _game;
        
        [SetUp]
        public void OpenWindows()
        {
            var _win = new Window("Space Taxi Game v0.1", 500, AspectRatio.R1X1);
        }
        
        [Test]
        public void ReadFileTest1()
        {
            _levelinfo = Level.ReadFile(_filePath[0]);
            Assert.AreEqual(_levelinfo[0], "%#%#%#%#%#%#%#%#%#^^^^^^%#%#%#%#%#%#%#%#");
        }
        [Test]
        public void ReadFileTest2()
        {
            _levelinfo = Level.ReadFile(_filePath[0]);
            Assert.AreEqual(_levelinfo[27], "%) white-square.png");
        }
        [Test]
        public void ReadFileTest3()
        {
            _levelinfo = Level.ReadFile(_filePath[0]);
            Assert.AreEqual(_levelinfo[_levelinfo.Length-1], "Customer: Alice 10 1 ^J 10 100");
        }
        [Test]
        public void ReadFileTest4()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Assert.AreEqual(_levelinfo[0], "CTTTTTTTTTTTTTTTTD^^^^^^CTTTTTTTTTTTTttt");
        }
        [Test]
        public void ReadFileTest5()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Assert.AreEqual(_levelinfo[27], "A) aspargus-edge-left.png");
        }

        [Test]
        public void ReadFileTest6()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Assert.AreEqual(_levelinfo[_levelinfo.Length-1], "Customer: Carol 30 r ^ 10 100");
        }

        [Test]
        public void ReadLegendTest1()
        {
            Level.SetDictionaryToNew();
            Level.ReadLegend("j) ironstone-lower-right.png");
            Assert.AreEqual(Level.GetLegendsDictionary()['j'], "ironstone-lower-right.png");
        }
        [Test]
        public void ReadLegendTest2()
        {    
            Level.SetDictionaryToNew();
            Level.ReadLegend("");
            Assert.AreEqual(Level.GetLegendsDictionary().Count, 0);
        }
        [Test]
        public void ReadLegendTest3()
        {
            Level.SetDictionaryToNew();
            Level.ReadLegend("asds,jdhbfslkdf");
            Assert.AreEqual(Level.GetLegendsDictionary().Count, 0);
        }
        [Test]
        public void ReadLegendTest4()
        {
            Level.SetDictionaryToNew();
            Level.ReadLegend("019283joijalksasd");
            Level.ReadLegend("l) ironstone-lower-left.png");
            Level.ReadLegend("");
            Level.ReadLegend("r) studio-square.png");
            Assert.AreEqual(Level.GetLegendsDictionary().Count, 2);
        }

        [Test]
        public void AddEntityToContainerTest1()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.SetDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddEntityToContainer(_levelinfo, _dictionary, 0, 0);
            Assert.AreEqual(Level.GetLevelEntities().CountEntities(),1);
        }
        //Tester at den ikke tilføjer en enitity ved et mellemrum.
        [Test]
        public void AddEntityToContainerTest2()
        {
            Level.SetAllEntitiesToNew();
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.SetDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddEntityToContainer(_levelinfo, _dictionary, 12, 3);
            Assert.AreEqual(Level.GetLevelEntities().CountEntities(),0);
        }
        [Test]
        public void AddEntityToContainerTest3()
        {
            Level.SetAllEntitiesToNew();
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.SetDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddEntityToContainer(_levelinfo, _dictionary, 21, 13);
            Assert.AreEqual(Level.GetLevelEntities().CountEntities(),1);
        }
        
        [Test]
        public void AddEntityToObstacleContainerTest1()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.ReadPlatforms(_levelinfo[25]);
            Level.SetDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddObstacle(_levelinfo, _dictionary, 0, 0);
            Assert.AreEqual(Level.GetLevelObstacles().CountEntities(),1);
        }
        [Test]
        public void AddEntityToObstacleContainerTest2()
        {
            Level.SetObstaclesToNew();
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.ReadPlatforms(_levelinfo[25]);
            Level.SetDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddObstacle(_levelinfo, _dictionary, 12, 3);
            Assert.AreEqual(Level.GetLevelObstacles().CountEntities(),0);
        }
        [Test]
        public void AddEntityToObstacleContainerTest3()
        {
            Level.SetObstaclesToNew();
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.ReadPlatforms(_levelinfo[25]);
            Level.SetDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddObstacle(_levelinfo, _dictionary, 21, 13);
            Assert.AreEqual(Level.GetLevelObstacles().CountEntities(),0);
        }
        
        [Test]
        public void AddEntityToPlatformContainerTest1()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.ReadPlatforms(_levelinfo[25]);
            Level.SetDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddPlatform(_levelinfo, _dictionary, 0, 0);
            Assert.AreEqual(Level.GetLevelPlatforms().CountEntities(),0);
        }
        
        [Test]
        public void AddEntityToPlatformContainerTest2()
        {
            Level.SetPlatformsToNew();
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.ReadPlatforms(_levelinfo[25]);
            Level.SetDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddPlatform(_levelinfo, _dictionary, 12, 3);
            Assert.AreEqual(Level.GetLevelPlatforms().CountEntities(),0);
        }
        [Test]
        public void AddEntityToPlatformContainerTest3()
        {
            Level.SetPlatformsToNew();
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.ReadPlatforms(_levelinfo[25]);
            Level.SetDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            _dictionary = Level.GetLegendsDictionary();
            Level.AddPlatform(_levelinfo, _dictionary, 21, 13);
            Assert.AreEqual(Level.GetLevelPlatforms().CountEntities(),1);
        }
        [Test]
        public void PlayerPosOfLevelTestX1()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.PlayerPosOfLevel(_levelinfo, 14, 15);
            Assert.AreEqual(Level.GetPlayerPosX(), 0.375f);
        }
        
        [Test]
        public void PlayerPosOfLevelTestY1()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.PlayerPosOfLevel(_levelinfo, 14, 15);
            Assert.AreEqual(Level.GetPlayerPosY(), 0.347826093f);
        }
        [Test]
        public void PlayerPosOfLevelTestX2()
        {
            Level.ChangePosX(0f);
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.PlayerPosOfLevel(_levelinfo, 0, 0);
            Assert.AreEqual(Level.GetPlayerPosX(), 0f);
        }
        
        [Test]
        public void PlayerPosOfLevelTestY2()
        {
            Level.ChangePosY(0f);
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.PlayerPosOfLevel(_levelinfo, 0, 0);
            Assert.AreEqual(Level.GetPlayerPosY(), 0f);
        }
        [Test]
        public void PlayerPosOfLevelTestX3()
        {
            Level.ChangePosX(0f);
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.PlayerPosOfLevel(_levelinfo, 3, 31);
            Assert.AreEqual(Level.GetPlayerPosX(), 0.774999976f);
        }
        
        [Test]
        public void PlayerPosOfLevelTestY3()
        {
            Level.ChangePosY(0f);
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.PlayerPosOfLevel(_levelinfo, 3, 31);
            Assert.AreEqual(Level.GetPlayerPosY(), 0.826086938f);
        }

        
        // Vi vidste ikke helt hvordan vi skulle teste nedenstående tests.
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

        [Test]
        public void CollisionTest1()
        {
            _testCollisionObstacle = new EntityContainer();
            _testCollisionPlatform = new EntityContainer();
            
            var stationaryShape = new StationaryShape(new Vec2F(0.5f,0.0f) , new Vec2F(0.5f,1.0f));
            var image = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
            
            _testPlayer = new Player();
            _testPlayer.SetPosition(0.3f, 0.5f);
            _testPlayer.GetsShape().Direction.X = 0.01f;
            
            _testCollisionObstacle.AddStationaryEntity(stationaryShape,image);
            
            CollisionChecker collisionTest = new CollisionChecker(_testCollisionObstacle,_testCollisionPlatform,_testPlayer);
            
            while (!collisionTest.GetGameOverChecker())
            {
                _testPlayer.playerMove();
                collisionTest.CheckCollsion();
            }          
            Assert.AreEqual(collisionTest.GetGameOverChecker(), true);

        }
        [Test]
        public void CollisionTest2()
        {
            _testCollisionObstacle = new EntityContainer();
            _testCollisionPlatform = new EntityContainer();
            
            var stationaryShape = new StationaryShape(new Vec2F(0.0f,0.0f) , new Vec2F(1.0f,0.5f));
            var image = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
            
            _testPlayer = new Player();
            _testPlayer.SetPosition(0.5f, 0.6f);
            _testPlayer.GetsShape().Direction.Y = -0.001f;
            
            _testCollisionPlatform.AddStationaryEntity(stationaryShape,image);
            
            CollisionChecker collisionTest = new CollisionChecker(_testCollisionObstacle,_testCollisionPlatform,_testPlayer);
            
            while (!collisionTest.GetPlatFormChecker())
            {
                _testPlayer.playerMove();
                collisionTest.CheckCollsion();
            }          
            Assert.AreEqual(collisionTest.GetPlatFormChecker(), true);

        }
        [Test]
        public void CollisionTest3()
        {
            _testCollisionObstacle = new EntityContainer();
            _testCollisionPlatform = new EntityContainer();
            
            var stationaryShape = new StationaryShape(new Vec2F(0.0f,0.0f) , new Vec2F(1.0f,0.5f));
            var image = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
            
            _testPlayer = new Player();
            _testPlayer.SetPosition(0.5f, 0.6f);
            _testPlayer.GetsShape().Direction.Y = -0.005f;
            
            _testCollisionPlatform.AddStationaryEntity(stationaryShape,image);
            
            CollisionChecker collisionTest = new CollisionChecker(_testCollisionObstacle,_testCollisionPlatform,_testPlayer);
            
            while (!collisionTest.GetGameOverChecker())
            {
                _testPlayer.playerMove();
                collisionTest.CheckCollsion();
            }          
            Assert.AreEqual(collisionTest.GetGameOverChecker(), true);

        }

    }
}