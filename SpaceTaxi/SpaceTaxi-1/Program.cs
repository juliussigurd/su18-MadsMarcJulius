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
        public static void Main(string[] args)
        {
            string[] _filePath = Directory.GetFiles("Levels");
            string[] _teStrings;
            Level Testlevel = new Level();

            _teStrings = Testlevel.ReadFile(_filePath[0]);


            for (int i = 27; i <= _teStrings.Length - 3; i++)
            {
                if (_teStrings[i] != "")
                {
                    Console.WriteLine(_teStrings[i][0]);
                }
            }

            var game = new Game();
            game.GameLoop();
        }
    }
}
