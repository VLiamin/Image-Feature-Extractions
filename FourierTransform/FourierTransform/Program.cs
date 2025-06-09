using FourierTransformormation.ExcractRegions;
using System.Drawing;

namespace FourierTransformormation
{
    internal class Program
    {
        private static int Size = 32;
        static void Main(string[] args)
        {

            var fileLabelPaths = Directory.GetFiles("C:\\Users\\Lyami\\Аспирантура\\NewImagesSelected\\Labels", "*.png").ToList();
            var fileImagePaths = Directory.GetFiles("C:\\Users\\Lyami\\Аспирантура\\NewImagesSelected\\Originals", "*.jpeg").ToList();

            fileLabelPaths.Sort();
            fileImagePaths.Sort();

            List<Bitmap> labels = fileLabelPaths.Select(f => new Bitmap(f)).ToList();
            List<Bitmap> images = fileImagePaths.Select(f => new Bitmap(f)).ToList();

            SquaresNew squares = new SquaresNew(Size);
            List<int[,]> nodularsAll = new List<int[,]>();
            List<int[,]> normalsAll = new List<int[,]>();

            for (int i = 0; i < labels.Count; i++)
            {
                (List<int[,]> nodulars, List<int[,]> normals) places = squares.DevideOnSquares(labels[i], images[i]);

                if (places.nodulars.Count > 0)
                {
                    nodularsAll.AddRange(places.nodulars);
                }
                else
                {
                    normalsAll.AddRange(places.normals);
                }
            }

            if (normalsAll.Count > nodularsAll.Count)
            {
                normalsAll = normalsAll.GetRange(0, nodularsAll.Count);
            }
            else
            {
                nodularsAll = nodularsAll.GetRange(0, normalsAll.Count);
            }

            FourierTransformation fourierTransformation = new FourierTransformation();

            List<(double mean, double stdDev, double maxVal, double energy)> result = new();
            List<bool> isNodular = new();

            foreach (int[,] nodular in nodularsAll)
            {
                (double mean, double stdDev, double maxVal, double energy) value = fourierTransformation.Transform(nodular, Size);
                result.Add(value);
                isNodular.Add(true);
            }

            foreach (int[,] normal in normalsAll)
            {
                (double mean, double stdDev, double maxVal, double energy) value = fourierTransformation.Transform(normal, Size);
                result.Add(value);
                isNodular.Add(false);
            }

            using (StreamWriter writer = new StreamWriter("IsNodularData.txt", false))
            {
                isNodular.ForEach(x => writer.WriteLine((x).ToString()));
            }

            using (StreamWriter writer = new StreamWriter("DataFourier.txt", false))
            {
                foreach ((double mean, double stdDev, double maxVal, double energy) fourier in result)
                {
                    writer.Write($"{fourier.mean} {fourier.stdDev} {fourier.maxVal} {fourier.energy}");

                    writer.WriteLine();
                }
            }
        }
    }
}
