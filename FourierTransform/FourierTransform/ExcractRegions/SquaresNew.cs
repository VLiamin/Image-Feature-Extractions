using System.Drawing;

namespace FourierTransformormation.ExcractRegions
{
    public class SquaresNew
    {
        private int _squeryLenght;

        public SquaresNew(int squeryLenght)
        {
            _squeryLenght = squeryLenght;
        }

        public (List<int[,]> nodulars, List<int[,]> normals) DevideOnSquares(Bitmap image, Bitmap originalImage)
        {
            List<int[,]> nodulars = new List<int[,]>();
            List<int[,]> normals = new List<int[,]>();

            image = image.Clone(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);


            List<int> colorsR = new();

            for (int i = 0; i < image.Width - _squeryLenght; i++)
            {
                for (int j = 0; j < image.Height - _squeryLenght; j++)
                {
                    Color color = image.GetPixel(i, j);

                    bool f = true;

                    if (!colorsR.Contains(color.R))
                    {
                        colorsR.Add(color.R);
                    }


                    if (color.R == 16 && color.B == 138 && color.G == 15)
                    {
                        for (int k = 0; k < _squeryLenght; k++)
                        {
                            if (!f)
                            {
                                break;
                            }

                            for (int l = 0; l < _squeryLenght; l++)
                            {
                                color = image.GetPixel(i + k, j + l);
                                if (color.R != 16 || color.B != 138 || color.G != 15)
                                {
                                    f = false; break;
                                }
                            }
                        }

                        if (f)
                        {
                            Bitmap bitmap = new Bitmap(_squeryLenght, _squeryLenght);
                            int[,] nodular = new int[_squeryLenght, _squeryLenght];
                            for (int k = 0; k < _squeryLenght; k++)
                            {
                                for (int l = 0; l < _squeryLenght; l++)
                                {

                                    Color actualColor = originalImage.GetPixel(i + k, j + l);
                                   // (double l, double a, double b) newColor = RGBToLBA.Transform(actualColor.R, actualColor.G, actualColor.B);
                                    //nodular[k, l] = (int)((newColor.a + newColor.b + newColor.l) * 100) / 3;
                                    nodular[k, l] = (int)(0.299 * actualColor.R + 0.587 * actualColor.G + 0.114 * actualColor.B);
                                    image.SetPixel(i + k, j + l, Color.Black);

                                    bitmap.SetPixel(k, l, actualColor);

                                }
                            }

                            nodulars.Add(nodular);
                        }
                    }

                    if (color.B == 33 && color.R == 138 && color.G == 15)
                    {
                        for (int k = 0; k < _squeryLenght; k++)
                        {
                            if (!f)
                            {
                                break;
                            }

                            for (int l = 0; l < _squeryLenght; l++)
                            {
                                color = image.GetPixel(i + k, j + l);
                                if (color.B != 33 || color.R != 138 || color.G != 15)
                                {
                                    f = false; break;
                                }
                            }
                        }

                        if (f)
                        {
                            Bitmap bitmap = new Bitmap(_squeryLenght, _squeryLenght);
                            int[,] normal = new int[_squeryLenght, _squeryLenght];
                            for (int k = 0; k < _squeryLenght; k++)
                            {
                                for (int l = 0; l < _squeryLenght; l++)
                                {
                                    Color actualColor = originalImage.GetPixel(i + k, j + l);
                                   // (double l, double a, double b) newColor = RGBToLBA.Transform(actualColor.R, actualColor.G, actualColor.B);
                                    //normal[k, l] = (int)((newColor.a + newColor.b + newColor.l) * 100) / 3;
                                    normal[k, l] = (int)(0.299 * actualColor.R + 0.587 * actualColor.G + 0.114 * actualColor.B);
                                    image.SetPixel(i + k, j + l, Color.Black);
                                    bitmap.SetPixel(k, l, actualColor);
                                }
                            }

                            normals.Add(normal);
                        }
                    }
                }
            }

            return (nodulars, normals);
        }
    }
}
