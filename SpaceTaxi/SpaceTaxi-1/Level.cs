using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using DIKUArcade.Entities;
using System;
using System.Dynamic;
using System.IO;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System;

namespace SpaceTaxi_1 {
    
    public static class Level
    {


        private static EntityContainer Obstacles = new EntityContainer();
        private static EntityContainer Platforms = new EntityContainer();
        private static EntityContainer AllEntities = new EntityContainer();
        private static List<char> PlatformLegends = new List<char>();

        //Fields


        private static Dictionary<char, string> legendsDictionary = new Dictionary<char, string>();
        private static float PlayerPosX = 0f;
        private static float PlayerPosY = 0f;

        private static string[] LevelInfo;

        public static void SetDictionaryToNew()
        {
            legendsDictionary = new Dictionary<char, string>();
        }

        public static void SetAllEntitiesToNew()
        {
            AllEntities = new EntityContainer();
        }
        
        public static void SetObstaclesToNew()
        {
            Obstacles = new EntityContainer();
        }
        
        public static void SetPlatformsToNew()
        {
            Platforms = new EntityContainer();
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

            LevelInfo = File.ReadAllLines(path);
            return LevelInfo;
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
                legendsDictionary.Add((line[0]), pngName);
            }
        }


        /// <summary>
        /// The method "Dictionary" takes a string[] "filetext" as an argument and
        /// returns a dictionary and uses the method "ReadLegend" to add all legends to
        /// a dictionary. 
        /// </summary>
        /// <param name="fileText"> Array of each line of the file in a string </param>
        /// <returns> Dictionary containing the Legends </returns>
        /// 
        public static void ReadLegends (string[] fileText)

        {
            SetDictionaryToNew();
            for (int i = 27; i < fileText.Length; i++)
            {
                ReadLegend(fileText[i]);
            }
        }


        public static void ReadPlatforms(string platformLine)
        {
            
            for (int i = 10; i < platformLine.Length; i++)
            {
                if (platformLine[i - 1] == ' ')
                {
                    PlatformLegends.Add(platformLine[i]);
                }
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
                AllEntities.AddStationaryEntity(
                    new StationaryShape(
                        new Vec2F((float) (((indeks + (1.0f)) / mapString[0].Length) - (1.0f / mapString[0].Length)),
                            (float) ((21 - row + 1.0f) / 23.0f)),
                        new Vec2F((float) (1.0f / mapString[0].Length), 1.0f / 23.0f)),
                    new Image(Path.Combine("Assets", "Images", legendsDictionary[mapString[row][indeks]])));
            }
        }


        public static void AddObstacle(string[] mapString, Dictionary<char, string> legendsDictionary, int row,
            int indeks)
        {
            if (!PlatformLegends.Contains(mapString[row][indeks]) && legendsDictionary.ContainsKey(mapString[row][indeks]))
                Obstacles.AddStationaryEntity(
                    new StationaryShape(
                        new Vec2F(
                            (float) (((indeks + (1.0f)) / mapString[0].Length) - (1.0f / mapString[0].Length)),
                            (float) ((21 - row + 1.0f) / 23.0f)),
                        new Vec2F((float) (1.0f / mapString[0].Length), 1.0f / 23.0f)),
                    new Image(Path.Combine("Assets", "Images", legendsDictionary[mapString[row][indeks]])));
            
        }
        public static void AddPlatform(string[] mapString, Dictionary<char, string> legendsDictionary, int row,
            int indeks)
        {
            if (PlatformLegends.Contains(mapString[row][indeks]) && legendsDictionary.ContainsKey(mapString[row][indeks]))
                Platforms.AddStationaryEntity(
                    new StationaryShape(
                        new Vec2F(
                            (float) (((indeks + (1.0f)) / mapString[0].Length) - (1.0f / mapString[0].Length)),
                            (float) ((21 - row + 1.0f) / 23.0f)),
                        new Vec2F((float) (1.0f / mapString[0].Length), 1.0f / 23.0f)),
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
                PlayerPosX = (((indeks + (1.0f)) / mapString[0].Length) - (1.0f / mapString[0].Length));
                PlayerPosY = (float) ((21 - row + 1.0f) / 23.0f);
            }
        }

        public static void AddAllEntitiesToContainer(string[] mapString, Dictionary<char, string> legendsDictionary)
        {
            SetAllEntitiesToNew();
            SetObstaclesToNew();
            SetPlatformsToNew();
            for (int j = 0; j < 23; j++)
            {
                for (int i = 0; i < mapString[0].Length; i++)
                {
                    AddEntityToContainer(mapString,legendsDictionary,j,i);
                    AddObstacle(mapString,legendsDictionary,j,i);
                    AddPlatform(mapString,legendsDictionary,j,i);
                    PlayerPosOfLevel(mapString, j, i);
                }
            }
        }       

        public static void changePosX(float newX)
        {
            PlayerPosX = newX;
        }
        
        public static void changePosY(float newY)
        {
            PlayerPosY = newY;
        }


        public static Dictionary<char, string> GetLegendsDictionary()
        {
            return legendsDictionary;
        }
        
        public static EntityContainer GetLevelEntities()
        {
            return AllEntities;
        }

        public static EntityContainer GetLevelObstacles()
        {
            return Obstacles;
        }
        
        public static EntityContainer GetLevelPlatforms()
        {
            return Platforms;
        }

        public static float GetPlayerPosX()
        {
            return PlayerPosX;
        }

        public static float GetPlayerPosY()
        {
            return PlayerPosY;
        }
        
    }
}