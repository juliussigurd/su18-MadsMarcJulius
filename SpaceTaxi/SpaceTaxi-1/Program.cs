using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTaxi_1
{
    internal class Program
    {
        private static string[] _filePath = Directory.GetFiles("Levels");
        private static string[] _levelInfo;
        private static int count = 0;
        private static int countall = 0;
        
        public static void Main(string[] args)
        {
            _levelInfo = Level.ReadFile(_filePath[0]);
            for (int j = 0; j < 23; j++)
            {
                for (int i = 0; i < _levelInfo[0].Length; i++)
                {
                    if (_levelInfo[j][i] != ' ' && _levelInfo[j][i] != '^' && _levelInfo[j][i] != '>')
                    {
                        count += 1;
                    }

                    countall += 1;
                }
            }
            
            Console.WriteLine(count);
            Console.WriteLine(countall);
            var game = new Game();
            game.GameLoop();
        }
    }
}
