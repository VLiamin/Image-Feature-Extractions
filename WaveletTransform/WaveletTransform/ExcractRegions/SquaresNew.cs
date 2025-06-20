using System.Drawing;

namespace WaveletTransform.ExcractRegions
{
    public class SquaresNew
    {
        private int _squeryLenght;

        public SquaresNew(int squeryLenght)
        {
            _squeryLenght = squeryLenght;
        }

        public (List<double[,]> nodulars, List<double[,]> normals) DevideOnSquares(Bitmap image, Bitmap originalImage)
        {
            List<double[,]> nodulars = new List<double[,]>();
            List<double[,]> normals = new List<double[,]>();

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
                            double[,] nodular = new double[_squeryLenght, _squeryLenght];
                            for (int k = 0; k < _squeryLenght; k++)
                            {
                                for (int l = 0; l < _squeryLenght; l++)
                                {

                                    Color actualColor = originalImage.GetPixel(i + k, j + l);
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
                            double[,] normal = new double[_squeryLenght, _squeryLenght];
                            for (int k = 0; k < _squeryLenght; k++)
                            {
                                for (int l = 0; l < _squeryLenght; l++)
                                {
                                    Color actualColor = originalImage.GetPixel(i + k, j + l);
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
