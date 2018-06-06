using System.Collections.Generic;
using DIKUArcade.Entities;
using System;
using System.IO;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    
    public static class Level
    {
        private static EntityContainer obstacles = new EntityContainer();
        private static EntityContainer platforms = new EntityContainer();
        private static EntityContainer allEntities = new EntityContainer();
        private static List<char> platformLegends = new List<char>();
        private static Dictionary<int, string> passengerInfo = new Dictionary<int, string>();
        private static Dictionary<char, string> legendsDictionary = new Dictionary<char, string>();
        private static List<Dictionary<int, string>> passengerInfoList = new List<Dictionary<int, string>>();
        private static Dictionary<char, List<Entity>> diffrentPlatforms = new Dictionary<char, List<Entity>>();
        private static float playerPosX;
        private static float playerPosY;

        private static string[] levelInfo;

        /// <summary>
        /// Set PlatformLegends to new.
        /// </summary>
        public static void SetPlatformLegendsToNew()
        {
            Level.platformLegends = new List<char>();
        }
        
        /// <summary>
        /// set legend dictionary to new 
        /// </summary>
        public static void SetLegendsDictionaryToNew()
        {
            Level.legendsDictionary = new Dictionary<char, string>();
        }
        
        /// <summary>
        /// set passegner infor to new 
        /// </summary>
        public static void SetPassengerInfoToNew()
        {
            Level.passengerInfo = new Dictionary<int, string>();
        }

        /// <summary>
        /// set passenger info list to new 
        /// </summary>
        public static void SetPassengerInfoListToNew()
        {
            Level.passengerInfoList = new List<Dictionary<int, string>>();
        }

        /// <summary>
        /// set all entities to new 
        /// </summary>
        public static void SetAllEntitiesToNew()
        {
            Level.allEntities = new EntityContainer();
        }
        
        /// <summary>
        /// set obstacles to new 
        /// </summary>
        public static void SetObstaclesToNew()
        {
            Level.obstacles = new EntityContainer();
        }
        
        /// <summary>
        /// set platforms to new 
        /// </summary>
        public static void SetPlatformsToNew()
        {
            Level.platforms = new EntityContainer();
        }

        /// <summary>
        /// set diffrent platforms to new 
        /// </summary>
        public static void SetDiffrentPlatformsToNew()
        {
            Level.diffrentPlatforms = new Dictionary<char, List<Entity>>();
        }


        /// <summary>
        /// This method takes takes a string "path" as an argument and returns
        /// the file as a string array and reads the file.  
        /// </summary>
        /// <param name="path"> Path to file </param>
        /// <returns> Returns file as a string array </returns>
        /// <exception cref="ArgumentException"> If the file
        /// doesn't exist, it throw an exception </exception>
        public static string[] ReadFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException("File does not exist");
            }

            Level.levelInfo = File.ReadAllLines(path);
            return Level.levelInfo;
        }


        /// <summary>
        /// The method "ReadLegend" takes a string "line" as an
        /// argument and checks if it is a legend and finds the png. 
        /// </summary>
        /// <param name="line"> Is a certain line in the file </param>
        public static void ReadLegend(string line)
        {
            if (line.Length != 0 && line[1] == ')')
            {
                var pngName = line.Substring(3, line.Length - 3);
                Level.legendsDictionary.Add(line[0], pngName);
            }
        }
        
        /// <summary>
        /// The method "Dictionary" takes a string[] "filetext" as an argument and
        /// returns a dictionary and uses the method "ReadLegend" to add all legends to
        /// a dictionary. 
        /// </summary>
        /// <param name="fileText"> Array of each line of the file in a string </param>
        /// <returns> Dictionary containing the Legends </returns>
        public static void ReadLegends (string[] fileText)
        {
            Level.SetLegendsDictionaryToNew();
            for (var i = 27; i < fileText.Length; i++)
            {
                Level.ReadLegend(fileText[i]);
            }
        }

        /// <summary>
        /// Updates the info on each passenger in the dictionary.
        /// Such as names, spawn time, position, drop off, time for points and points.
        /// </summary>
        /// <param name="mapString">The map/level string of all the ASCII.</param>
        public static void UpdatePassengerInfo(string[] mapString) {
            Level.SetPassengerInfoListToNew();
            {
                for (var row = 50; row < mapString.Length; row++) {
                    if (!mapString[row].StartsWith("Customer:")) {
                        continue;
                    }

                    var counter = 0;
                    var name = "";
                    var spawnTime = "";
                    var spawnPlatform = "";
                    var releasePlatform = "";
                    var timeForPoints = "";
                    var pointsRecived = "";
                    {
                        Level.SetPassengerInfoToNew();
                        for (var i = 8; i < mapString[row].Length; i++) {
                            if (mapString[row][i] == ' ') {
                                counter += 1;
                            }

                            if (mapString[row][i] == ' ') {
                                continue;
                            }

                            switch (counter) {
                            case 1:
                                name += mapString[row][i];
                                break;
                            case 2:
                                spawnTime += mapString[row][i];
                                break;
                            case 3:
                                spawnPlatform += mapString[row][i];
                                break;
                            case 4:
                                releasePlatform += mapString[row][i];
                                break;
                            case 5:
                                timeForPoints += mapString[row][i];
                                break;
                            case 6:
                                pointsRecived += mapString[row][i];
                                break;
                            }
                        }

                        Level.passengerInfo.Add(1, name);
                        Level.passengerInfo.Add(2, spawnTime);
                        Level.passengerInfo.Add(3, spawnPlatform);
                        Level.passengerInfo.Add(4, releasePlatform);
                        Level.passengerInfo.Add(5, timeForPoints);
                        Level.passengerInfo.Add(6, pointsRecived);
                        Level.passengerInfoList.Add(Level.passengerInfo);
                    }
                }
            }
        }

        

        /// <summary>
        /// Reads the platform and sets PlatformLegends and differentPlatforms to hold info about platforms.
        /// </summary>
        /// <param name="platformLine">The line of platform symbols</param>
        public static void ReadPlatforms(string platformLine)
        {  
            Level.SetDiffrentPlatformsToNew();
            for (var i = 10; i < platformLine.Length; i++)
            {
                if (platformLine[i - 1] == ' ')
                {
                    Level.platformLegends.Add(platformLine[i]);
                    Level.diffrentPlatforms.Add(platformLine[i], new List<Entity>());                    
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapString"></param>
        /// <param name="legendsDictionary"></param>
        /// <param name="row"></param>
        /// <param name="stringIndeks"></param>
        /// <param name="platformIndeks"></param>
        public static void SpecifyPlatforms(string[] mapString,
            Dictionary<char, string> legendsDictionary, int row,
            int stringIndeks, int platformIndeks) {
            if (Level.diffrentPlatforms.ContainsKey(mapString[row][stringIndeks]) &&
                mapString[row][stringIndeks] == Level.platformLegends[platformIndeks]) {
                Level.diffrentPlatforms[mapString[row][stringIndeks]].Add(new Entity(new StationaryShape(
                        new Vec2F(
                            (stringIndeks + 1.0f) / mapString[0].Length -
                            1.0f / mapString[0].Length,
                            (21 - row + 1.0f) / 23.0f),
                        new Vec2F(1.0f / mapString[0].Length, 1.0f / 23.0f)),
                    new Image(Path.Combine("Assets", "Images",
                        legendsDictionary[mapString[row][stringIndeks]]))));
            }
        }
        
        /// <summary>
        /// The method takes four arguments and adds a stationary entity to an entity container.
        /// </summary>
        /// <param name="mapString"> array of strings of the map </param>
        /// <param name="legendsDictionary"> dictionary of legends </param>
        /// <param name="row"> defines a certain row </param>
        /// <param name="indeks"> indexing in the row </param>

        public static void AddEntityToContainer(string[] mapString, Dictionary<char, string> legendsDictionary, int row,
            int indeks)
        {
            if (legendsDictionary.ContainsKey(mapString[row][indeks]))
            {
                Level.allEntities.AddStationaryEntity(
                    new StationaryShape(new Vec2F((indeks + 1.0f) / mapString[0].Length - 1.0f / mapString[0].Length,
                        (21 - row + 1.0f) / 23.0f),
                    new Vec2F(1.0f / mapString[0].Length, 1.0f / 23.0f)),
                    new Image(Path.Combine("Assets", "Images", legendsDictionary[mapString[row][indeks]])));
            }
        }

        /// <summary>
        /// If it's not a Plateform then sets the obstacles in the ASCII file to stationary shapes.
        /// And gives the image path.
        /// </summary>
        /// <param name="mapString">Array of strings of the map</param>
        /// <param name="legendsDictionary">Dictionary of the legends</param>
        /// <param name="row">The row in the ASCII file</param>
        /// <param name="indeks"> The indeks in the ASCII file</param>
        public static void AddObstacle(string[] mapString, Dictionary<char, string> legendsDictionary, int row,
            int indeks)
        {
            if (!Level.platformLegends.Contains(mapString[row][indeks]) && legendsDictionary.ContainsKey(mapString[row][indeks])) {
                Level.obstacles.AddStationaryEntity(
                    new StationaryShape(
                        new Vec2F(
                            (indeks + 1.0f) / mapString[0].Length - 1.0f / mapString[0].Length,
                            (21 - row + 1.0f) / 23.0f),
                        new Vec2F(1.0f / mapString[0].Length, 1.0f / 23.0f)),
                    new Image(Path.Combine("Assets", "Images", legendsDictionary[mapString[row][indeks]])));
            }
        }
        
        /// <summary>
        /// If it is a Plateform then sets the platform in the ASCII file to stationary shapes.
        /// And gives the image path.
        /// </summary>
        /// <param name="mapString">Array of strings of the map</param>
        /// <param name="legendsDictionary">Dictionary of the legends</param>
        /// <param name="row">The row in the ASCII file</param>
        /// <param name="indeks"> The indeks in the ASCII file</param>
        public static void AddPlatform(string[] mapString, Dictionary<char, string> legendsDictionary, int row,
            int indeks)
        {
            if (Level.platformLegends.Contains(mapString[row][indeks]) && legendsDictionary.ContainsKey(mapString[row][indeks])) {
                Level.platforms.AddStationaryEntity(
                    new StationaryShape(
                        new Vec2F(
                            (indeks + 1.0f) / mapString[0].Length - 1.0f / mapString[0].Length,
                            (21 - row + 1.0f) / 23.0f),
                        new Vec2F(1.0f / mapString[0].Length, 1.0f / 23.0f)),
                    new Image(Path.Combine("Assets", "Images", legendsDictionary[mapString[row][indeks]])));
            }
        }

    /// <summary>
        /// The method takes three arguments and places the player a certain place
        /// on the map, with the help of coordinates. 
        /// </summary>
        /// <param name="mapString"> array of strings of the map</param>
        /// <param name="row"> dictionary of legends </param>
        /// <param name="indeks"> indexing in the row </param>
        public static void PlayerPosOfLevel(string[] mapString, int row, int indeks)
        {
            if (mapString[row][indeks] == '>')
            {
                Level.playerPosX = (indeks + 1.0f) / mapString[0].Length - 1.0f / mapString[0].Length;
                Level.playerPosY = (21 - row + 1.0f) / 23.0f;
            }
        }

        /// <summary>
        /// Adds all the entities to a container.
        /// </summary>
        /// <param name="mapString"> array of strings of the map</param>
        /// <param name="legendsDictionary">Dictionary of the legends</param>
        public static void AddAllEntitiesToContainer(string[] mapString, Dictionary<char, string> legendsDictionary)
        {
            Level.SetAllEntitiesToNew();
            Level.SetObstaclesToNew();
            Level.SetPlatformsToNew();
            
            for (var j = 0; j < 23; j++)
            {
                for (var c = 0; c < Level.platformLegends.Count; c++)
                {
                    for (var i = 0; i < mapString[0].Length; i++)
                    {
                        Level.AddEntityToContainer(mapString, legendsDictionary, j, i);
                        Level.AddObstacle(mapString, legendsDictionary, j, i);
                        Level.AddPlatform(mapString, legendsDictionary, j, i);
                        Level.SpecifyPlatforms(mapString, legendsDictionary, j, i,c);
                        Level.PlayerPosOfLevel(mapString, j, i);     
                    }
                }
            }
        }       

        /// <summary>
        /// Change player position X
        /// </summary>
        /// <param name="newX"></param>
        public static void ChangePosX(float newX)
        {
            Level.playerPosX = newX;
        }
        
        
        /// <summary>
        /// Change player position Y
        /// </summary>
        /// <param name="newY"></param>
        public static void ChangePosY(float newY)
        {
            Level.playerPosY = newY;
        }

        /// <summary>
        /// Get legends dictionary 
        /// </summary>
        /// <returns>legendsDictionary</returns>
        public static Dictionary<char, string> GetLegendsDictionary()
        {
            return Level.legendsDictionary;
        }

        public static List<char> GetPlatformLegends()
        {
            return Level.platformLegends;
        }
        /// <summary>
        /// get diffrenplatform
        /// </summary>
        /// <returns>diffrentplatforms</returns>
        public static Dictionary<char ,List<Entity>> GetDiffrenPlatforms()
        {
            return Level.diffrentPlatforms;
        }
        
        /// <summary>
        /// get passenger info 
        /// </summary>
        /// <returns>passengerInfoList</returns>
        public static List<Dictionary<int, string>> GetPassengerInfo()
        {
            return Level.passengerInfoList;
        }
        
        /// <summary>
        /// get all level entities 
        /// </summary>
        /// <returns>AllEntities</returns>
        public static EntityContainer GetLevelEntities()
        {
            return Level.allEntities;
        }

        /// <summary>
        /// get all level obstacles 
        /// </summary>
        /// <returns>Obstacles</returns>
        public static EntityContainer GetLevelObstacles()
        {
            return Level.obstacles;
        }
        
        /// <summary>
        /// get all level platforms 
        /// </summary>
        /// <returns>Platforms</returns>
        public static EntityContainer GetLevelPlatforms()
        {
            return Level.platforms;
        }

        /// <summary>
        /// get player position X
        /// </summary>
        /// <returns>PlayerPosX</returns>
        public static float GetPlayerPosX()
        {
            return Level.playerPosX;
        }

        /// <summary>
        /// get player position Y
        /// </summary>
        /// <returns>PlayerPosY</returns>
        public static float GetPlayerPosY()
        {
            return Level.playerPosY;
        }
        
    }
}