using System.Numerics;
using Accord.Math;
using Accord.Math.Transforms;

namespace FourierTransformormation
{
    public class FourierTransformation
    {
        public (double mean,
            double stdDev,
            double maxVal,
            double energy) Transform(int[,] image, int size)
        {
            // === 2. Конвертируем в матрицу double ===
            double[,] pixels = new double[size, size];
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    pixels[y, x] = (double)image[x, y] / 255.0; // нормализация

            // === 3. Применяем 2D Fourier Transform ===
            Complex[,] complexImage = new Complex[size, size];
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    complexImage[y, x] = new Complex(pixels[y, x], 0);

            FourierTransform2.FFT2(ConvertToJaggedArray(complexImage), FourierTransform.Direction.Forward);

            // === 4. Вычисляем амплитудный спектр ===
            double[,] magnitude = new double[size, size];
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    magnitude[y, x] = complexImage[y, x].Magnitude;

            // === 5. Извлечение признаков ===
            double sum = 0;
            double sumSq = 0;
            double maxVal = 0;
            int count = size * size;

            foreach (var val in magnitude)
            {
                sum += val;
                sumSq += val * val;
                if (val > maxVal)
                    maxVal = val;
            }

            double mean = sum / count;
            double stdDev = Math.Sqrt(sumSq / count - mean * mean);
            double energy = sumSq;

            // === 6. Вывод результатов ===
            Console.WriteLine("Извлечённые признаки:");
            Console.WriteLine($"Среднее значение: {mean}");
            Console.WriteLine($"Стандартное отклонение: {stdDev}");
            Console.WriteLine($"Максимальное значение: {maxVal}");
            Console.WriteLine($"Энергия спектра: {energy}");

            return (mean, stdDev, maxVal, energy);
        }

        private Complex[][] ConvertToJaggedArray(Complex[,] input)
        {
            int rows = input.GetLength(0);
            int cols = input.GetLength(1);
            Complex[][] result = new Complex[rows][];
            for (int i = 0; i < rows; i++)
            {
                result[i] = new Complex[cols];
                for (int j = 0; j < cols; j++)
                {
                    result[i][j] = input[i, j];
                }
            }
            return result;
        }
    }
}
