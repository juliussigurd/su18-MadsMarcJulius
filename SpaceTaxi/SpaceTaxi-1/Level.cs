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
        //Fields
        private static EntityContainer EntityList = new EntityContainer();
        private static Dictionary<char, string> legendsDictionary = new Dictionary<char, string>();
        private static float PlayerPosX = 0f;
        private static float PlayerPosY = 0f;
        
        private static string[] LevelInfo;
       

        public static void SetDictionaryToNew()
        {
            legendsDictionary = new Dictionary<char, string>();
        }

        public static void SetEntitiContainerToNew()
        {
            EntityList = new EntityContainer();
        }
        
        
        public static string[] ReadFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException("File does not exist");
            }
            LevelInfo = File.ReadAllLines(path);
            return LevelInfo;
        }
        
        public static void ReadLegend(string line)
        {
            if (line.Length != 0 && line[1] == ')')
            {
                var pngName = line.Substring(3, (line.Length - 3));                                     
                legendsDictionary.Add((line[0]), pngName);
            } 
        }

        public static void ReadLegends (string[] fileText)
        {
            SetDictionaryToNew();
            for (int i = 27; i < fileText.Length; i++)
            {
                ReadLegend(fileText[i]);
            }
        }

        public static void AddEntityToContainer(string[] mapString, Dictionary<char, string> legendsDictionary, int row,
            int indeks)
        {
            if (legendsDictionary.ContainsKey(mapString[row][indeks]) && mapString[row][indeks] != ' ')
            {
                EntityList.AddStationaryEntity(
                    new StationaryShape(
                        new Vec2F((float) (((indeks + (1.0f)) / mapString[0].Length) - (1.0f / mapString[0].Length)),
                            (float) ((21 - row + 1.0f) / 23.0f)),
                        new Vec2F((float) (1.0f / mapString[0].Length), 1.0f / 23.0f)),
                    new Image(Path.Combine("Assets", "Images", legendsDictionary[mapString[row][indeks]])));
            }
        }

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
            SetEntitiContainerToNew();
            for (int j = 0; j < 23; j++)
            {
                for (int i = 0; i < mapString[0].Length; i++)
                {
                     AddEntityToContainer(mapString,legendsDictionary,j,i);
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
            return EntityList;
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