using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            //Sets new game.
            var game = new Game();
            //Runs game loop.
            game.GameLoop();
        }
    }
}
