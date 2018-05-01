using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.IO;
using DIKUArcade;
using SpaceTaxi_1;


namespace TestProject1
{
    [TestFixture]
    public class Tests
    {
        private string[] _filePath = Directory.GetFiles("Levels");
        private string[] _levelinfo;
        private Dictionary<char, string> dictionaryTest = new Dictionary<char, string>();
        private float playerPosX;
        private float playerPosY;
        
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
            dictionaryTest = Level.GetLegendsDictionary();
            Level.AddEntityToContainer(_levelinfo, dictionaryTest, 0, 0);
            Assert.AreEqual(Level.GetLevelEntities().CountEntities(),1);
        }
        
        [Test]
        public void AddEntityToContainerTest2()
        {
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.SetDictionaryToNew();
            Level.ReadLegends(_levelinfo);
            dictionaryTest = Level.GetLegendsDictionary();
            Level.AddEntityToContainer(_levelinfo, dictionaryTest, 12, 3);
            Assert.AreEqual(Level.GetLevelEntities().CountEntities(),1);
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
            Level.changePosX(0f);
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.PlayerPosOfLevel(_levelinfo, 0, 0);
            Assert.AreEqual(Level.GetPlayerPosX(), 0f);
        }
        
        [Test]
        public void PlayerPosOfLevelTestY2()
        {
            Level.changePosY(0f);
            _levelinfo = Level.ReadFile(_filePath[1]);
            Level.PlayerPosOfLevel(_levelinfo, 0, 0);
            Assert.AreEqual(Level.GetPlayerPosY(), 0f);
        }
        [Test]
        public void PlayerPosOfLevelTestX3()
        {
            Level.changePosX(0f);
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.PlayerPosOfLevel(_levelinfo, 3, 31);
            Assert.AreEqual(Level.GetPlayerPosX(), 0.774999976f);
        }
        
        [Test]
        public void PlayerPosOfLevelTestY3()
        {
            Level.changePosY(0f);
            _levelinfo = Level.ReadFile(_filePath[0]);
            Level.PlayerPosOfLevel(_levelinfo, 3, 31);
            Assert.AreEqual(Level.GetPlayerPosY(), 0.826086938f);
        }
        
        [SetUp]
        public void OpenWindows()
        {
            var _win = new Window("Space Taxi Game v0.1", 500, AspectRatio.R1X1);
        }
    }
}