using System.Collections.Generic;
using System.IO;
using System.Xml.Schema;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;


namespace SpaceTaxi_1
{
    
    /// <summary>
    /// 
    /// </summary>
    public static class PlayerImage
    {
        //Field
        private static IBaseImage _taxiImage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static IBaseImage ImageDecider(int Value)
        {

            // rightstill -1
            // leftstill 1
            // rightMove -2
            // Leftmove 2
            // up 10
            
            switch (Value)
            {

                case -2:
                    _taxiImage = new ImageStride(80,
                        ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Back_Right.png")));
                    break;

                case -1:
                    _taxiImage = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png"));
                    break;

                case 1:
                    _taxiImage = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
                    break;

                case 2:
                    _taxiImage = new ImageStride(80,
                        ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Back.png")));
                    break;

                case 8:
                    _taxiImage = new ImageStride(80,
                        ImageStride.CreateStrides(2,
                            Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back_Right.png")));
                    break;

                case 9:
                    _taxiImage = new ImageStride(80,
                        ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Right.png")));
                    break;

                case 11:
                    _taxiImage = new ImageStride(80,
                        ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom.png")));
                    break;

                case 12:
                    _taxiImage = new ImageStride(80,
                        ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back.png")));
                    break;
                default:
                    _taxiImage = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
                    break;
            }
            return _taxiImage;
        }
    }
}