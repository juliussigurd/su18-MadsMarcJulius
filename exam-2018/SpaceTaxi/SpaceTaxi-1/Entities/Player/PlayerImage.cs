using System.IO;
using DIKUArcade.Graphics;

namespace SpaceTaxi_1.Entities.Player {
    public static class PlayerImage {
        //Field
        private static IBaseImage taxiImage;

        /// <summary>
        /// Decide which image to use for the player. Depending on which key that the user
        /// are pressing.  
        /// </summary>
        /// <param name="value">integer that changes depending on which input it gets from the
        /// player class</param>
        /// <returns></returns>
        public static IBaseImage ImageDecider(int value) {
            
            switch (value) {
            case -2:
                PlayerImage.taxiImage = new ImageStride(80,
                    ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Taxi_Thrust_Back_Right.png")));
                break;

            case -1:
                PlayerImage.taxiImage =
                    new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png"));
                break;

            case 1:
                PlayerImage.taxiImage = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
                break;

            case 2:
                PlayerImage.taxiImage = new ImageStride(80,
                    ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Taxi_Thrust_Back.png")));
                break;

            case 8:
                PlayerImage.taxiImage = new ImageStride(80,
                    ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back_Right.png")));
                break;

            case 9:
                PlayerImage.taxiImage = new ImageStride(80,
                    ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Right.png")));
                break;

            case 11:
                PlayerImage.taxiImage = new ImageStride(80,
                    ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom.png")));
                break;

            case 12:
                PlayerImage.taxiImage = new ImageStride(80,
                    ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back.png")));
                break;

            default:
                PlayerImage.taxiImage = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
                break;
            }
        return PlayerImage.taxiImage;
            
        }
    }
}