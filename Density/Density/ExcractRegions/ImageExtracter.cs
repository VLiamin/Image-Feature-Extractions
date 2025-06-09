using System.Drawing;

namespace Density.ExcractRegions
{
    public class ImageExtracter
    {
        public List<(Bitmap original, Bitmap extracted)> ExtractImages(string storagePath)
        {
            List<(Bitmap original, Bitmap extracted)> images = new();
            var fileStorage = new List<string[]>();
            try
            {
                var filePaths = Directory.GetFiles(storagePath, "*.png"); //string[]

                foreach (string path in filePaths)
                {
                    Bitmap original = new Bitmap(path);

                    Bitmap tissueExtracted = new Bitmap(original.Width, original.Height);

                    Bitmap newImage = new Bitmap(original.Width / 2, original.Height);
                    int[,] array = new int[original.Width / 2, original.Height];

                    for (int u = original.Width / 2; u < original.Width; u++)
                    {
                        for (int v = 0; v < original.Height; v++)
                        {
                            Color actualColor = original.GetPixel(u, v);

                            byte red = actualColor.R;
                            byte blue = actualColor.B;
                            byte green = actualColor.G;

                            ///Узел
                            if (red >= 70 && red <= 160 &&
                                blue >= 185 && blue <= 220 &&
                                green >= 52 && green <= 82)
                            {
                                //Color actualColor2 = original.GetPixel(u - original.Width / 2, v);

                                newImage.SetPixel(u - original.Width / 2, v, Color.Red);
                                tissueExtracted.SetPixel(u - original.Width / 2, v, original.GetPixel(u - original.Width / 2, v));
                                tissueExtracted.SetPixel(u, v, actualColor);
                            }
                            /// щитовидная железа
                            else if (red >= 140 && red <= 199 &&
                                blue >= 125 && blue <= 185 &&
                                green >= 110 && green <= 165 &&
                                red - green >= 11 && red - blue >= -10)
                            {
                                newImage.SetPixel(u - original.Width / 2, v, Color.Blue);
                                tissueExtracted.SetPixel(u - original.Width / 2, v, original.GetPixel(u - original.Width / 2, v));
                                tissueExtracted.SetPixel(u, v, actualColor);
                            }
                            // обычная ткань
                            else if (red >= 20 && red <= 148 &&
                                blue >= 38 && blue <= 168 &&
                                green >= 68 && green <= 198 &&
                                green > red + 31 && green > blue)
                            {
                                newImage.SetPixel(u - original.Width / 2, v, Color.Black);
                                tissueExtracted.SetPixel(u - original.Width / 2, v, original.GetPixel(u - original.Width / 2, v));
                                tissueExtracted.SetPixel(u, v, actualColor);
                            }
                            // артерия
                            else if (red >= 140 && red <= 223 &&
                                blue >= 20 && blue <= 85 &&
                                green >= 130 && green <= 192 &&
                                blue < red - 90 && red - blue < 145)
                            {
                                newImage.SetPixel(u - original.Width / 2, v, Color.Black);
                                tissueExtracted.SetPixel(u - original.Width / 2, v, original.GetPixel(u - original.Width / 2, v));
                                tissueExtracted.SetPixel(u, v, actualColor);
                            }
                            else
                            {
                                newImage.SetPixel(u - original.Width / 2, v, Color.Black);
                            }
                        }
                    }

                    string newPath = path.Split("\\")[path.Split("\\").Length - 1];
                    //tissueExtracted.Save(newPath);

                    images.Add((original, newImage));

                    //	newImage.Save($"Test.png");
                }
            }
            catch (Exception e)
            {
                /* todo */
            }

            return images;
        }
    }
}
