using System.Drawing;
using Density.ExcractRegions;

namespace Density
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var fileLabelPaths = Directory.GetFiles("C:\\Users\\Lyami\\Аспирантура\\NewImagesSelected\\Labels", "*.png").ToList();
            var fileImagePaths = Directory.GetFiles("C:\\Users\\Lyami\\Аспирантура\\NewImagesSelected\\Originals", "*.jpeg").ToList();

            fileLabelPaths.Sort();
            fileImagePaths.Sort();

            List<Bitmap> labels = fileLabelPaths.Select(f => new Bitmap(f)).ToList();
            List<Bitmap> images = fileImagePaths.Select(f => new Bitmap(f)).ToList();


            //return;
            SquaresNew squares = new SquaresNew(32);
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

            List<List<Data>> result = new();
            List<bool> isNodular = new();
            int number = 12;

            CountDensity countDensity = new();

            List<int> r = new List<int>() { 2, 3, 4 };

            foreach (var normaL in normalsAll)
            {
                List<Data> normaLResult = countDensity.CountMFSFull(normaL, r, number);
                result.Add(normaLResult);

                isNodular.AddRange(Enumerable.Repeat(false, normaLResult.Count));
            }

            foreach (var nodular in nodularsAll)
            {
                List<Data> nodularResult = countDensity.CountMFSFull(nodular, r, number);
                result.Add(nodularResult);
                isNodular.AddRange(Enumerable.Repeat(true, nodularResult.Count));
            }

            using (StreamWriter writer = new StreamWriter("IsNodularData.txt", false))
            {
                isNodular.ForEach(x => writer.WriteLine((x).ToString()));
            }

            using (StreamWriter writer = new StreamWriter("DataDensity.txt", false))
            {
                foreach (List<Data> den in result)
                {
                    for (int i = 0; i < den.Count; i++)
                    {
                        writer.Write($"{den[i].q} {den[i].D} {den[i].R}");
                        writer.WriteLine();
                    }
                }
            }
        }
    }
}