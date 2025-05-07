using System.Drawing;

namespace ExcludeNodles.Squares
{
    public class Squares
    {
        private int _squeryLenght;

        public Squares(int squeryLenght)
        {
            _squeryLenght = squeryLenght;
        }

        public (List<int[,]> nodulars, List<int[,]> normals) DevideOnSquares(Bitmap image, Bitmap originalImage)
        {
            List<int[,]> nodulars = new List<int[,]>();
            List<int[,]> normals = new List<int[,]>();

            for (int i = 0; i < image.Width - _squeryLenght; i++)
            {
                for (int j = 0; j < image.Height - _squeryLenght; j++)
                {
                    Color color = image.GetPixel(i, j);

                    bool f = true;

                    if (color.R == 255 && color.B == 0 && color.G == 0)
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
                                if (color.R != 255 || color.B != 0 || color.G != 0)
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
                                    nodular[k, l] = (int)(0.299 * actualColor.R + 0.587 * actualColor.G + 0.114 * actualColor.B);
                                    image.SetPixel(i + k, j + l, Color.Black);

                                    bitmap.SetPixel(k, l, actualColor);

                                }
                            }

                            nodulars.Add(nodular);
                        }
                    }

                    if (color.B == 255 && color.R == 0 && color.G == 0)
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
                                if (color.B != 255 || color.R != 0 || color.G != 0)
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
