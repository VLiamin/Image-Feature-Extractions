using System;
using System.Drawing;

public class HaarWavelet2
{
    public static (double[,], double[,], double[,], double[,]) DWT(Bitmap bmp)
    {
        int width = bmp.Width;
        int height = bmp.Height;

        // Преобразуем изображение в серый массив
        double[,] gray = new double[width, height];
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                gray[i, j] = bmp.GetPixel(i, j).R;

        int halfWidth = width / 2;
        int halfHeight = height / 2;

        double[,] LL = new double[halfWidth, halfHeight];
        double[,] LH = new double[halfWidth, halfHeight];
        double[,] HL = new double[halfWidth, halfHeight];
        double[,] HH = new double[halfWidth, halfHeight];

        // Применим 1 уровень DWT по строкам и столбцам
        for (int y = 0; y < height; y += 2)
        {
            for (int x = 0; x < width; x += 2)
            {
                double a = gray[x, y];
                double b = gray[x + 1, y];
                double c = gray[x, y + 1];
                double d = gray[x + 1, y + 1];

                // Вейвлет Хаара: вычисляем усреднение и разности
                double avg = (a + b + c + d) / 4;
                double hor = (a + b - c - d) / 4;
                double vert = (a - b + c - d) / 4;
                double diag = (a - b - c + d) / 4;

                int i = x / 2;
                int j = y / 2;

                LL[i, j] = avg;
                LH[i, j] = hor;
                HL[i, j] = vert;
                HH[i, j] = diag;
            }
        }

        return (LL, LH, HL, HH);
    }
}
