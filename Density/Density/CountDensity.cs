using System.Drawing;

namespace Density
{
    public class CountDensity
    {
        private int[,] FillArrayFromImage(Bitmap original, int[,] array)
        {
            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    Color actualColor = original.GetPixel(i, j);

                    byte red = actualColor.R;
                    byte blue = actualColor.B;
                    byte green = actualColor.G;

                    // Get image brightness
                    int brightness = (int)(0.2126 * red + 0.7152 * green + 0.0722 * blue);

                    array[i, j] = brightness;
                }
            }

            return array;
        }

        private int CountМ(int[,] array, int r, int i, int j)
        {
            int brightness = 0;
            for (int l = i - r; l <= i + r; l++)
            {
                for (int k = j - r; k <= j + r; k++)
                {
                    brightness += array[l, k];
                }
            }

            return brightness;
        }

        private double[,] CountD(int[,] array, int r)
        {
            double[,] d = new double[array.GetLength(0), array.GetLength(1)];
            for (int i = r; i < array.GetLength(0) - r; i++)
            {
                for (int j = r; j < array.GetLength(1) - r; j++)
                {
                    int m = CountМ(array, r, i, j);
                    d[i, j] = Math.Log(m) / Math.Log(r);
                }
            }

            return d;
        }

        private double CountCapacitive(int[,] newD, int r)
        {
            int count = 0;

            for (int i = 0; i < newD.GetLength(0) - r; i = i + r)
            {
                for (int j = 0; j < newD.GetLength(1) - r; j = j + r)
                {
                    for (int l = i; l <= i + r; l++)
                    {
                        for (int k = j; k <= j + r; k++)
                        {
                            if (newD[l, k] == 0)
                            {
                                count++;
                                l = newD.GetLength(0);
                                break;
                            }
                        }
                    }
                }
            }

            return Math.Log2(count) / -Math.Log2(r / (double)newD.GetLength(0));
        }

        private void MakeNewImages(Bitmap original, List<int[,]> intervals)
        {
            int count = 0;
            foreach (int[,] inter in intervals)
            {
                count++;
                Bitmap image = new Bitmap(original);

                for (int i = 0; i < inter.GetLength(0); i++)
                {
                    for (int j = 0; j < inter.GetLength(1); j++)
                    {
                        if (inter[i, j] != 0)
                        {
                            image.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                        }
                        else
                        {
                            image.SetPixel(i, j, Color.FromArgb(0, 0, 0));

                        }
                    }
                }
                image.Save($"FolRight2{count}.jpg");
            }
        }

        public List<Data> CountMFSFull(int[,] array, List<int> radii, int intervalCount)
        {
            List<Data> allResults = new();

            foreach (int r in radii)
            {
                Console.WriteLine($"\n=== Анализ для радиуса r = {r} ===");

                double[,] d = CountD(array, r);

                double min = double.MaxValue;
                double max = double.MinValue;

                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        if (d[i, j] != 0)
                        {
                            if (d[i, j] < min) min = d[i, j];
                            if (d[i, j] > max) max = d[i, j];
                        }
                    }
                }

                Console.WriteLine($"min = {Math.Round(min, 3)}, max = {Math.Round(max, 3)}");

                double e = (max - min) / intervalCount;

                for (int intervalIndex = 0; intervalIndex < intervalCount; intervalIndex++)
                {
                    double q = min + intervalIndex * e;
                    int[,] mask = new int[array.GetLength(0), array.GetLength(1)];

                    for (int i = 0; i < array.GetLength(0); i++)
                    {
                        for (int j = 0; j < array.GetLength(1); j++)
                        {
                            if (d[i, j] > q && d[i, j] <= q + e)
                                mask[i, j] = 0;
                            else
                                mask[i, j] = 255;
                        }
                    }

                    double D = CountCapacitive(mask, r);
                    if (D > -100) // отсекаем невалидные значения
                    {
                        allResults.Add(new Data
                        {
                            q = q + e / 2,
                            D = D,
                            R = r
                        });

                        Console.WriteLine($"r = {r}, α = {Math.Round(q + e / 2, 3)}, D = {Math.Round(D, 4)}");
                    }
                }
            }

            return allResults;
        }
    }
}
