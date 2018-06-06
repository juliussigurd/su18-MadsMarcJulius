using System.Collections.Generic;
using DIKUArcade.Entities;
using System;
using System.IO;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    
    public static class Level
    {
        private static EntityContainer _obstacles = new EntityContainer();
        private static EntityContainer _platforms = new EntityContainer();
        private static EntityContainer _allEntities = new EntityContainer();
        private static List<char> _platformLegends = new List<char>();
        private static Dictionary<int, string> _passengerInfo = new Dictionary<int, string>();
        private static Dictionary<char, string> _legendsDictionary = new Dictionary<char, string>();
        private static List<Dictionary<int, string>> _passengerInfoList = new List<Dictionary<int, string>>();
        private static Dictionary<char, List<Entity>> _diffrentPlatforms = new Dictionary<char, List<Entity>>();
        private static float _playerPosX;
        private static float _playerPosY;

        private static string[] _levelInfo;

        /// <summary>
        /// Set PlatformLegends to new.
        /// </summary>
        public static void SetPlatformLegendsToNew()
        {
            _platformLegends = new List<char>();
        }
        
        /// <summary>
        /// set legend dictionary to new 
        /// </summary>
        public static void SetLegendsDictionaryToNew()
        {
            _legendsDictionary = new Dictionary<char, string>();
        }
        
        /// <summary>
        /// set passegner infor to new 
        /// </summary>
        public static void SetPassengerInfoToNew()
        {
            _passengerInfo = new Dictionary<int, string>();
        }

        /// <summary>
        /// set passenger info list to new 
        /// </summary>
        public static void SetPassengerInfoListToNew()
        {
            _passengerInfoList = new List<Dictionary<int, string>>();
        }

        /// <summary>
        /// set all entities to new 
        /// </summary>
        public static void SetAllEntitiesToNew()
        {
            _allEntities = new EntityContainer();
        }
        
        /// <summary>
        /// set obstacles to new 
        /// </summary>
        public static void SetObstaclesToNew()
        {
            _obstacles = new EntityContainer();
        }
        
        /// <summary>
        /// set platforms to new 
        /// </summary>
        public static void SetPlatformsToNew()
        {
            _platforms = new EntityContainer();
        }

        /// <summary>
        /// set diffrent platforms to new 
        /// </summary>
        public static void SetDiffrentPlatformsToNew()
        {
            _diffrentPlatforms = new Dictionary<char, List<Entity>>();
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

            _levelInfo = File.ReadAllLines(path);
            return _levelInfo;
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
                var pngName = line.Substring(3, (line.Length - 3));
                _legendsDictionary.Add((line[0]), pngName);
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
            SetLegendsDictionaryToNew();
            for (int i = 27; i < fileText.Length; i++)
            {
                ReadLegend(fileText[i]);
            }
        }

        /// <summary>
        /// Updates the info on each passenger in the dictionary.
        /// Such as names, spawn time, position, drop off, time for points and points.
        /// </summary>
        /// <param name="mapString">The map/level string of all the ASCII.</param>
        public static void UpdatePassengerInfo(string[] mapString) {
            SetPassengerInfoListToNew();
            {
                for (int row = 50; row < mapString.Length; row++) {
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
                        SetPassengerInfoToNew();
                        for (int i = 8; i < mapString[row].Length; i++) {
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

                        _passengerInfo.Add(1, name);
                        _passengerInfo.Add(2, spawnTime);
                        _passengerInfo.Add(3, spawnPlatform);
                        _passengerInfo.Add(4, releasePlatform);
                        _passengerInfo.Add(5, timeForPoints);
                        _passengerInfo.Add(6, pointsRecived);
                        _passengerInfoList.Add(_passengerInfo);
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
            SetDiffrentPlatformsToNew();
            for (int i = 10; i < platformLine.Length; i++)
            {
                if (platformLine[i - 1] == ' ')
                {
                    _platformLegends.Add(platformLine[i]);
                    _diffrentPlatforms.Add(platformLine[i], new List<Entity>());                    
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
            if (_diffrentPlatforms.ContainsKey(mapString[row][stringIndeks]) &&
                mapString[row][stringIndeks] == _platformLegends[platformIndeks]) {
                _diffrentPlatforms[mapString[row][stringIndeks]].Add(new Entity(new StationaryShape(
                        new Vec2F(
                            ((stringIndeks + (1.0f)) / mapString[0].Length) -
                            (1.0f / mapString[0].Length),
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
                _allEntities.AddStationaryEntity(
                    new StationaryShape(new Vec2F(((indeks + (1.0f)) / mapString[0].Length) - (1.0f / mapString[0].Length),
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
            if (!_platformLegends.Contains(mapString[row][indeks]) && legendsDictionary.ContainsKey(mapString[row][indeks]))
                _obstacles.AddStationaryEntity(
                    new StationaryShape(
                        new Vec2F(
                            ((indeks + (1.0f)) / mapString[0].Length) - (1.0f / mapString[0].Length),
                            (21 - row + 1.0f) / 23.0f),
                        new Vec2F(1.0f / mapString[0].Length, 1.0f / 23.0f)),
                    new Image(Path.Combine("Assets", "Images", legendsDictionary[mapString[row][indeks]])));
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
            if (_platformLegends.Contains(mapString[row][indeks]) && legendsDictionary.ContainsKey(mapString[row][indeks]))
                _platforms.AddStationaryEntity(
                    new StationaryShape(
                        new Vec2F(
                            (((indeks + (1.0f)) / mapString[0].Length) - (1.0f / mapString[0].Length)),
                            (21 - row + 1.0f) / 23.0f),
                        new Vec2F(1.0f / mapString[0].Length, 1.0f / 23.0f)),
                        new Image(Path.Combine("Assets", "Images", legendsDictionary[mapString[row][indeks]])));
            
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
                _playerPosX = (((indeks + (1.0f)) / mapString[0].Length) - (1.0f / mapString[0].Length));
                _playerPosY = (21 - row + 1.0f) / 23.0f;
            }
        }

        /// <summary>
        /// Adds all the entities to a container.
        /// </summary>
        /// <param name="mapString"> array of strings of the map</param>
        /// <param name="legendsDictionary">Dictionary of the legends</param>
        public static void AddAllEntitiesToContainer(string[] mapString, Dictionary<char, string> legendsDictionary)
        {
            SetAllEntitiesToNew();
            SetObstaclesToNew();
            SetPlatformsToNew();
            
            for (int j = 0; j < 23; j++)
            {
                for (int c = 0; c < _platformLegends.Count; c++)
                {
                    for (int i = 0; i < mapString[0].Length; i++)
                    {
                        AddEntityToContainer(mapString, legendsDictionary, j, i);
                        AddObstacle(mapString, legendsDictionary, j, i);
                        AddPlatform(mapString, legendsDictionary, j, i);
                        SpecifyPlatforms(mapString, legendsDictionary, j, i,c);
                        PlayerPosOfLevel(mapString, j, i);     
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
            _playerPosX = newX;
        }
        
        
        /// <summary>
        /// Change player position Y
        /// </summary>
        /// <param name="newY"></param>
        public static void ChangePosY(float newY)
        {
            _playerPosY = newY;
        }

        /// <summary>
        /// Get legends dictionary 
        /// </summary>
        /// <returns>legendsDictionary</returns>
        public static Dictionary<char, string> GetLegendsDictionary()
        {
            return _legendsDictionary;
        }

        public static List<char> GetPlatformLegends()
        {
            return _platformLegends;
        }
        /// <summary>
        /// get diffrenplatform
        /// </summary>
        /// <returns>diffrentplatforms</returns>
        public static Dictionary<char ,List<Entity>> GetDiffrenPlatforms()
        {
            return _diffrentPlatforms;
        }
        
        /// <summary>
        /// get passenger info 
        /// </summary>
        /// <returns>passengerInfoList</returns>
        public static List<Dictionary<int, string>> GetPassengerInfo()
        {
            return _passengerInfoList;
        }
        
        /// <summary>
        /// get all level entities 
        /// </summary>
        /// <returns>AllEntities</returns>
        public static EntityContainer GetLevelEntities()
        {
            return _allEntities;
        }

        /// <summary>
        /// get all level obstacles 
        /// </summary>
        /// <returns>Obstacles</returns>
        public static EntityContainer GetLevelObstacles()
        {
            return _obstacles;
        }
        
        /// <summary>
        /// get all level platforms 
        /// </summary>
        /// <returns>Platforms</returns>
        public static EntityContainer GetLevelPlatforms()
        {
            return _platforms;
        }

        /// <summary>
        /// get player position X
        /// </summary>
        /// <returns>PlayerPosX</returns>
        public static float GetPlayerPosX()
        {
            return _playerPosX;
        }

        /// <summary>
        /// get player position Y
        /// </summary>
        /// <returns>PlayerPosY</returns>
        public static float GetPlayerPosY()
        {
            return _playerPosY;
        }
        
    }
}