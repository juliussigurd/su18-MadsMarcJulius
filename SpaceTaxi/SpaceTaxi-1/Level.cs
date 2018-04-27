using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using DIKUArcade.Entities;
using System;
using System.Dynamic;
using System.IO;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    public class Level
    {

        private EntityContainer EntityList = new EntityContainer();
        private Dictionary<char, string> legendsDictionary = new Dictionary<char, string>();
        
        private string[] LevelInfo;
       
            
        public string[] ReadFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException("File does not exist");
            }
            LevelInfo = File.ReadAllLines(path);
            return LevelInfo;
        }

        public Dictionary<char, string> readLegend (string[] fileText)
        {
            for (int i = 27; i <= fileText.Length - 3; i++)
            {
                if (fileText[i].Length != 0 && fileText[i][1] == ')')
                {
                    var pngName = fileText[i].Substring(3, (fileText[i].Length - 3));                                     
                    legendsDictionary.Add((fileText[i][0]), pngName);
                }
            }
            return legendsDictionary;
        }
      
        public EntityContainer CreateMap(string[] mapString, Dictionary<char, string> legendsDictionary)
        {
            for (int i = 0; i <= mapString[0].Length - 1; i++)
            {
                for (int j = 0; j <= 22; j++)
                {
                    if (legendsDictionary.ContainsKey(mapString[j][i]))
                    {
                        EntityList.AddStationaryEntity(
                            new StationaryShape(
                                new Vec2F((float)(((i + (1.0f)) / mapString[0].Length) - (1.0f/mapString[0].Length)),
                                          (float) ((21 - j + 1.0f) / 23.0f)),
                                new Vec2F((float)(1.0f / mapString[0].Length), 1.0f / 23.0f)),
                                new Image(Path.Combine("Assets", "Images", legendsDictionary[mapString[j][i]])));
                    }
                }
            }
            return EntityList;
        }

  /*  public List<Entity> returnList()
        {
            return EntityList;
        }*/
    }
}