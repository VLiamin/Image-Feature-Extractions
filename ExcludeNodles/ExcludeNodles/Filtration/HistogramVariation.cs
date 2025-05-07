using System.Drawing;
using System.Text;

namespace Histogram
{
    public class HistogramVariation
    {
        public void MakeGlobalHistogram(Bitmap original)
        {
            if (original is null)
            {
                return;
            }
            Bitmap image = new Bitmap(original);

            int[] brightnesses = new int[255];

            using var sw = new StreamWriter(@"Original.csv", false, Encoding.Default);
            using var sw2 = new StreamWriter(@"NewImage.csv", false, Encoding.Default);

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color actualColor = image.GetPixel(i, j);

                    byte red = actualColor.R;
                    byte blue = actualColor.B;
                    byte green = actualColor.G;

                    // Get image brightness
                    int brightness = (int)(0.2126 * red + 0.7152 * green + 0.0722 * blue);

                    brightnesses[brightness]++;

                    if (i < image.Height / 8)
                    {
                        sw.WriteLine(brightness);
                    }                    
                }
            }

            int pixels = image.Height * image.Width;

            int[] transformation = new int[255];

            for (int i = 0; i < 255; i++)
            {
                int sum = 0;
                for (int j = 0; j <= i; j++)
                {
                    sum += brightnesses[j];
                }

                transformation[i] = 254 * sum / pixels;
            }

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color actualColor = image.GetPixel(i, j);

                    byte red = actualColor.R;
                    byte blue = actualColor.B;
                    byte green = actualColor.G;

                    // Get image brightness
                    int brightness = (int)(0.2126 * red + 0.7152 * green + 0.0722 * blue);

                    int newBrightness = transformation[brightness];

                    double coef = (double)newBrightness / brightness;

                    byte newRed = 1;
                    byte newBlue = 1;
                    byte newGreen = 1;

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

                    if (i < image.Height / 8)
                    {
                        sw2.WriteLine(newBrightness);
                    }
                }
            }
        }

        public Bitmap MakeLocalHistogram(Bitmap oldImage)
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
            // Get image brightness

            ColorValues[,] values = new ColorValues[image.Width, image.Height];

            for (int i = 1; i < image.Width - 1; i++)
            {
                for (int j = 1; j < image.Height - 1; j++)
                {
                    int[] brightnesses = new int[255];

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

                            brightnesses[brightness]++;
                        }
                    }

                    int pixels = 9;

                    int[] transformation = new int[255];

                    for (int l = 0; l < 255; l++)
                    {
                        int sum = 0;
                        for (int k = 0; k <= l; k++)
                        {
                            sum += brightnesses[k];
                        }

                        transformation[l] = 254 * sum / pixels;
                    }


                    actualColor = image.GetPixel(i, j);

                    red = actualColor.R;
                    blue = actualColor.B;
                    green = actualColor.G;

                    // Get image brightness
                    brightness = (int)(0.2126 * red + 0.7152 * green + 0.0722 * blue);

                    int newBrightness = transformation[brightness];

                    double coef = (double)newBrightness / brightness;

                    byte newRed = 1;
                    byte newBlue = 1;
                    byte newGreen = 1;

                    newRed = red * coef > 255
                        ? (byte)255
                        : (byte)(red * coef);

                    newBlue = blue * coef > 255
                        ? (byte)255
                        : (byte)(blue * coef);

                    newGreen = green * coef > 255
                        ? (byte)255
                        : (byte)(green * coef);

                    //      image.SetPixel(i, j, Color.FromArgb(newRed, newGreen, newBlue));

                    values[i, j] = new ColorValues
                    {
                        Red = newRed,
                        Green = newGreen,
                        Blue = newBlue
                    };
                }
            }

            for (int i = 1; i < image.Width - 1; i++)
            {
                for (int j = 1; j < image.Height - 1; j++)
                {
                    image.SetPixel(i, j, Color.FromArgb(values[i, j].Red, values[i, j].Green, values[i, j].Blue));
                }
            }

            return image;
        }
    }

    struct ColorValues
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
    }
}
