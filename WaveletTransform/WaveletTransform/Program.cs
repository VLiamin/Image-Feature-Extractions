using System.Drawing;
using WaveletTransform.ExcractRegions;

namespace WaveletTransform
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fileLabelPaths = Directory.GetFiles("C:\\Users\\Lyami\\Аспирантура\\NewImagesSelected\\Labels", "*.png").ToList();
            var fileImagePaths = Directory.GetFiles("C:\\Users\\Lyami\\Аспирантура\\NewImagesSelected\\Originals", "*.jpeg").ToList();

            fileLabelPaths.Sort();
            fileImagePaths.Sort();

            List<Bitmap> labels = fileLabelPaths.Select(f => new Bitmap(f)).ToList();
            List<Bitmap> images = fileImagePaths.Select(f => new Bitmap(f)).ToList();


            //return;
            SquaresNew squares = new SquaresNew(32);
            List<double[,]> nodularsAll = new List<double[,]>();
            List<double[,]> normalsAll = new List<double[,]>();

            for (int i = 0; i < labels.Count; i++)
            {
                (List<double[,]> nodulars, List<double[,]> normals) places = squares.DevideOnSquares(labels[i], images[i]);

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

            HaarWavelet2D haarWavelet2D = new HaarWavelet2D();
            List<List<double>> result = new();
            List<bool> isNodular = new();

            foreach (var nodular in nodularsAll)
            {
                result.Add(HaarWavelet2D.ForwardTransform(nodular).Cast<double>().ToList());
                isNodular.Add(true);
            }

            foreach (var normal in normalsAll)
            {
                result.Add(HaarWavelet2D.ForwardTransform(normal).Cast<double>().ToList());
                isNodular.Add(false);
            }

            using (StreamWriter writer = new StreamWriter("IsNodularData.txt", false))
            {
                isNodular.ForEach(x => writer.WriteLine((x).ToString()));
            }

            using (StreamWriter writer = new StreamWriter("DataWavelet.txt", false))
            {
                foreach (List<double> wavelet in result)
                {
                    foreach (var v in wavelet)
                    {
                        writer.Write($"{v} ");
                    }

                    writer.WriteLine();
                }
            }
        }
    }
}
