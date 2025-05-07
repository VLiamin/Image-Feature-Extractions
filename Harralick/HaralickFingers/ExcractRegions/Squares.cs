using System.Drawing;

namespace HaralickFingers.ExcractRegions
{
    public class Squares
    {
        public int[,] DevideOnSquares(Bitmap image)
        {
            int[,] greyImage = new int[image.Width, image.Height];

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color actualColor = image.GetPixel(i, j);
                    greyImage[i, j] = (int)(0.299 * actualColor.R + 0.587 * actualColor.G + 0.114 * actualColor.B);
                }
            }

            return greyImage;
        }
    }
}
