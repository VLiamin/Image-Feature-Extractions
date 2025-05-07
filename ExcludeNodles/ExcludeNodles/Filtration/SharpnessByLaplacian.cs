using System.Drawing;

namespace Sharpness
{
    public class SharpnessByLaplacian
    {
        public Bitmap MakeLaplacian(Bitmap oldImage)
        {
            if (oldImage is null)
            {
                return null;
            }

            int[,] laplacian = new int[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    laplacian[i, j] = 1;
                }
            }

            laplacian[1, 1] = -8;

            Bitmap image = new Bitmap(oldImage);

            Color actualColor;

            byte red = 0;
            byte blue = 0;
            byte green = 0;
            int brightness = 0;
            // Get image brightness

            double[,] brightnesses = new double[image.Width, image.Height];

            for (int i = 1; i < image.Width - 1; i++)
            {
                for (int j = 1; j < image.Height - 1; j++)
                {
                    int sum = 0;

                    for (int l = i - 1; l <= i + 1; l++)
                    {
                        for (int k = j - 1; k <= j + 1; k++)
                        {
                            actualColor = image.GetPixel(l, k);

                            red = actualColor.R;
                            blue = actualColor.B;
                            green = actualColor.G;

                            // Get image brightness
                            brightness = (int)(0.2126 * red + 0.7152 * green + 0.0722 * blue);

                            sum += brightness * laplacian[l - i + 1, k - j + 1];
                        }
                    }

                    if (sum < 0)
                    {
                        sum = 0;
                    }

                    if (sum > 255)
                    {
                        sum = 255;
                    }

                    brightnesses[i, j] = sum;
                }
            }

            for (int i = 1; i < image.Width - 1; i++)
            {
                for (int j = 1; j < image.Height - 1; j++)
                {
                    actualColor = image.GetPixel(i, j);

                    red = actualColor.R;
                    blue = actualColor.B;
                    green = actualColor.G;

                    // Get image brightness
                    brightness = (int)(0.2126 * red + 0.7152 * green + 0.0722 * blue);

                    double value =  brightnesses[i, j];

                    brightness = brightness < 1 ? 1 : brightness;

                    double coef = value / brightness;

                    byte newRed = 0;
                    byte newBlue = 0;
                    byte newGreen = 0;

                    newRed = red * coef > 255
                        ? (byte)255
                        : (byte)(red * coef);

                    newBlue = blue * coef > 255
                        ? (byte)255
                        : (byte)(blue * coef);

                    newGreen = green * coef > 255
                        ? (byte)255
                        : (byte)(green * coef);

                    image.SetPixel(i, j, Color.FromArgb(newRed, newGreen, newBlue));
                }
            }

            return image;
        }

        public Bitmap MakeSharpnessByLapalacian(Bitmap originalImage, Bitmap laplacianImage)
        {
            if (originalImage is null || laplacianImage is null)
            {
                return null;
            }

            Bitmap image = new Bitmap(originalImage);

            Color actualColor;
            Color laplacianColor;

            byte red = 0;
            byte blue = 0;
            byte green = 0;
            int brightness = 0;
            // Get image brightness

            for (int i = 1; i < originalImage.Width - 1; i++)
            {
                for (int j = 1; j < originalImage.Height - 1; j++)
                {
                    actualColor = originalImage.GetPixel(i, j);

                    red = actualColor.R;
                    blue = actualColor.B;
                    green = actualColor.G;

                    // Get image brightness
                    brightness = (int)(0.2126 * red + 0.7152 * green + 0.0722 * blue);

                    laplacianColor = laplacianImage.GetPixel(i, j);

                    byte redL = laplacianColor.R;
                    byte blueL = laplacianColor.B;
                    byte greenL = laplacianColor.G;

                    int laplacianBrightness = (int)(0.2126 * redL + 0.7152 * blueL + 0.0722 * greenL);

                    int newBrightness = brightness - laplacianBrightness;

                    double value = brightness == 0 ? 1 : brightness;

                    double newValue = newBrightness <= 0 ? 1 : newBrightness;

                    double coef = newValue / value;

                    if (coef == 0)
                    {
                        coef = 1;
                    }

                    byte newRed = 1;
                    byte newBlue = 1;
                    byte newGreen = 1;

                    red = red == 0 ? (byte)1 : red;
                    blue = blue == 0 ? (byte)1 : blue;
                    green = green == 0 ? (byte)1 : green;

                    newRed = red * coef > 255
                        ? (byte)255
                        : (byte)(red * coef);

                    newBlue = blue * coef > 255
                        ? (byte)255
                        : (byte)(blue * coef);

                    newGreen = green * coef > 255
                        ? (byte)255
                        : (byte)(green * coef);

                    image.SetPixel(i, j, Color.FromArgb(newRed, newGreen, newBlue));
                }
            }

            return image;
        }
    }
}
