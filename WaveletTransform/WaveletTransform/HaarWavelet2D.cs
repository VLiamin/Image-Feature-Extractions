namespace WaveletTransform;

public class HaarWavelet2D
{
    // Прямое 2D преобразование Хаара
    // input: 2D массив double (например, яркость пикселей)
    // размер width и height должны быть степенями двойки
    public static double[,] ForwardTransform(double[,] input)
    {
        int width = input.GetLength(1);
        int height = input.GetLength(0);

        if (!IsPowerOfTwo(width) || !IsPowerOfTwo(height))
            throw new ArgumentException("Размеры массива должны быть степенями двойки.");

        double[,] temp = new double[height, width];
        double[,] output = (double[,])input.Clone();

        int h = height;
        int w = width;

        while (h > 1 || w > 1)
        {
            if (w > 1)
            {
                for (int i = 0; i < h; i++)
                {
                    double[] row = new double[w];
                    for (int j = 0; j < w; j++)
                        row[j] = output[i, j];

                    var transformedRow = ForwardTransform1D(row);

                    for (int j = 0; j < w; j++)
                        temp[i, j] = transformedRow[j];
                }
                for (int i = 0; i < h; i++)
                    for (int j = 0; j < w; j++)
                        output[i, j] = temp[i, j];
            }

            if (h > 1)
            {
                for (int j = 0; j < w; j++)
                {
                    double[] col = new double[h];
                    for (int i = 0; i < h; i++)
                        col[i] = output[i, j];

                    var transformedCol = ForwardTransform1D(col);

                    for (int i = 0; i < h; i++)
                        temp[i, j] = transformedCol[i];
                }
                for (int i = 0; i < h; i++)
                    for (int j = 0; j < w; j++)
                        output[i, j] = temp[i, j];
            }

            if (w > 1) w /= 2;
            if (h > 1) h /= 2;
        }

        return output;
    }

    // Вспомогательное 1D прямое преобразование Хаара
    private static double[] ForwardTransform1D(double[] data)
    {
        int length = data.Length;
        double[] output = new double[length];
        double[] temp = new double[length];

        Array.Copy(data, output, length);
        int h = length;

        while (h > 1)
        {
            h /= 2;
            for (int i = 0; i < h; i++)
            {
                temp[i] = (output[2 * i] + output[2 * i + 1]) / Math.Sqrt(2);
                temp[h + i] = (output[2 * i] - output[2 * i + 1]) / Math.Sqrt(2);
            }
            Array.Copy(temp, 0, output, 0, h * 2);
        }
        return output;
    }

    private static bool IsPowerOfTwo(int x)
    {
        return (x & (x - 1)) == 0 && x > 0;
    }
}
