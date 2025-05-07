using System.Drawing;

namespace Sharpness
{
    public class IncreaseTreble
    {
        private Bitmap MakeLinearFiltration(Bitmap oldImage, int[,] array, int divider, int size)
        {
            if (oldImage is null)
            {
                return null;
            }

            Bitmap image = new Bitmap(oldImage);

            Color actualColor;

            byte red = 0;
            byte blue = 0;
            byte green = 0;
            int brightness = 0;

            double[,] brightnesses = new double[image.Width, image.Height];

            for (int i = size / 2; i < image.Width - size / 2; i++)
            {
                for (int j = size / 2; j < image.Height - size / 2; j++)
                {
                    int sum = 0;

                    for (int l = i - size / 2; l <= i + size / 2; l++)
                    {
                        for (int k = j - size / 2; k <= j + size / 2; k++)
                        {
                            actualColor = image.GetPixel(l, k);

                            red = actualColor.R;
                            blue = actualColor.B;
                            green = actualColor.G;

                            // Get image brightness
                            brightness = (int)(0.2126 * red + 0.7152 * green + 0.0722 * blue);

                            sum += brightness * array[l - i + size / 2, k - j + size / 2];
                        }
                    }

                    brightnesses[i, j] = sum / (double)divider;
                }
            }

            for (int i = size / 2; i < image.Width - size / 2; i++)
            {
                for (int j = size / 2; j < image.Height - size / 2; j++)
                {
                    actualColor = image.GetPixel(i, j);

                    red = actualColor.R;
                    blue = actualColor.B;
                    green = actualColor.G;

                    // Get image brightness
                    brightness = (int)(0.2126 * red + 0.7152 * green + 0.0722 * blue);

                    double t = brightnesses[i, j];

                    brightness = brightness == 0 ? 1 : brightness;

                    double value = brightnesses[i, j] == 0 ? 1 : brightnesses[i, j];

                    double coef = (double)value / brightness;

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

        private Bitmap MakegMask(Bitmap originalImage, Bitmap smoothImage)
        {
            if (originalImage is null || smoothImage is null)
            {
                return null;
            }

            Bitmap image = new Bitmap(originalImage);

            Color actualColor;
            Color smoothColor;

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

                    smoothColor = smoothImage.GetPixel(i, j);

                    byte redS = smoothColor.R;
                    byte blueS = smoothColor.B;
                    byte greenS = smoothColor.G;

                    int smoothBrightness = (int)(0.2126 * redS + 0.7152 * blueS + 0.0722 * greenS);

                    int newBrightness = brightness - smoothBrightness;

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

        public Bitmap MakeIncreaseTreble(Bitmap originalImage, double k)
        {
            int[,] array = new int[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    array[i, j] = 1;
                }
            }

            Bitmap smoothImage = MakeLinearFiltration(originalImage, array, 25, 5);

            Bitmap gMaskImage = MakegMask(originalImage, smoothImage);

            Bitmap image = new Bitmap(originalImage);

            Color actualColor;
            Color gMaskColor;

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

                    gMaskColor = gMaskImage.GetPixel(i, j);

                    byte redM = gMaskColor.R;
                    byte blueM = gMaskColor.B;
                    byte greenM = gMaskColor.G;

                    int gMaskBrightness = (int)(0.2126 * redM + 0.7152 * blueM + 0.0722 * greenM);

                    int newBrightness = brightness + (int)(gMaskBrightness * k);

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
