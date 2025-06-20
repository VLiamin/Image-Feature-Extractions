using System.Drawing;
using Haralick.ExcractRegions;
using Sharpness;

namespace Haralick
{
    public class Program
    {
        public static MatricesCounter matricesCounter = new MatricesCounter();

        static void Main(string[] args)
        {
            /*            ImageExtracter imageExtracter = new ImageExtracter();

                        List<(Bitmap original, Bitmap extracted)> imagesRight = imageExtracter.ExtractImages("C:\\Users\\Lyami\\Аспирантура\\FolRightImages");
                        List<(Bitmap original, Bitmap extracted)> imagesleft = imageExtracter.ExtractImages("C:\\Users\\Lyami\\Аспирантура\\FolLeftImages");*/

            var fileLabelPaths = Directory.GetFiles("C:\\Users\\Lyami\\Аспирантура\\NewImagesSelected\\Labels", "*.png").ToList();
            var fileImagePaths = Directory.GetFiles("C:\\Users\\Lyami\\Аспирантура\\NewImagesSelected\\Originals", "*.jpeg").ToList();

            fileLabelPaths.Sort();
            fileImagePaths.Sort();

            List<Bitmap> labels = fileLabelPaths.Select(f => new Bitmap(f)).ToList();
            List<Bitmap> images = fileImagePaths.Select(f => new Bitmap(f)).ToList();

            /*            SharpnessByLaplacian sharpnessByLalpacian = new();

                        for (int i = 0; i < images.Count; i++)
                        {
                            var laplacian = sharpnessByLalpacian.MakeLaplacian(images[i]);
                            images[i] = sharpnessByLalpacian.MakeSharpnessByLapalacian(images[i], laplacian);
                        }*/

            IncreaseTreble increaseTreble = new IncreaseTreble();

            for (int i = 0; i < images.Count; i++)
            {
                images[i] = increaseTreble.MakeIncreaseTreble(images[i], 1.5);
            }


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

            /*            foreach (var image in imagesRight)
                        {
                            (List<int[,]> nodulars, List<int[,]> normals) places = squares.DevideOnSquares(image.extracted, image.original);
                            nodularsAll.AddRange(places.nodulars);
                            normalsAll.AddRange(places.normals);
                        }

                        foreach (var image in imagesleft)
                        {
                            (List<int[,]> nodulars, List<int[,]> normals) places = squares.DevideOnSquares(image.extracted, image.original);
                            nodularsAll.AddRange(places.nodulars);
                            normalsAll.AddRange(places.normals);
                        }*/

            if (normalsAll.Count > nodularsAll.Count)
            {
                normalsAll = normalsAll.GetRange(0, nodularsAll.Count);
            }
            else
            {
                nodularsAll = nodularsAll.GetRange(0, normalsAll.Count);
            }

            MatricesCounter matricesCounter = new MatricesCounter();

            List<(double moment, double contr, double corr, double entr, double gom)> result = new();
            List<bool> isNodular = new();

            //  using (StreamWriter writer = new StreamWriter("DataHaralick.txt", false))
            //  {

            foreach (var normaL in normalsAll)
            {
                var normaLResult = Program.CountStatistics(normaL);
                result.Add((
                    normaLResult.averageSecondMoment,
                    normaLResult.averageContrast,
                    normaLResult.averageCorrelation,
                    normaLResult.averageEntropy,
                    normaLResult.gomogeneity));
                isNodular.Add(false);
            }

            foreach (var nodular in nodularsAll)
            {
                var nodularResult = Program.CountStatistics(nodular);
                result.Add((
                    nodularResult.averageSecondMoment,
                    nodularResult.averageContrast,
                    nodularResult.averageCorrelation,
                    nodularResult.averageEntropy,
                    nodularResult.gomogeneity));
                isNodular.Add(true);
            }
            //   }

            using (StreamWriter writer = new StreamWriter("IsNodularData.txt", false))
            {
                isNodular.ForEach(x => writer.WriteLine((x).ToString()));
            }

            using (StreamWriter writer = new StreamWriter("DataHaralick.txt", false))
            {
                foreach ((double moment, double contr, double corr, double entr, double gom) har in result)
                {
                    writer.Write($"{har.moment} {har.contr} {har.corr} {har.entr} {har.gom}");

                    writer.WriteLine();
                }
            }
        }

        public static (double averageSecondMoment, double averageContrast, double averageCorrelation, double averageEntropy, double gomogeneity)
            CountStatistics(int[,] image, StreamWriter writer = default)
        {
            (double[,] P0, double[,] P45, double[,] P90, double[,] P135) matrices = matricesCounter.CountMatrices(image, 1);
            matricesCounter.MakeNormal(matrices.P0);
            matricesCounter.MakeNormal(matrices.P45);
            matricesCounter.MakeNormal(matrices.P90);
            matricesCounter.MakeNormal(matrices.P135);

            (double momentX, double momentY) P0moments = matricesCounter.CountMoments(matrices.P0);
            (double momentX, double momentY) P45moments = matricesCounter.CountMoments(matrices.P45);
            (double momentX, double momentY) P90moments = matricesCounter.CountMoments(matrices.P90);
            (double momentX, double momentY) P135moments = matricesCounter.CountMoments(matrices.P135);

            (double varianceX, double varianceY) P0variances = matricesCounter.CountVariances(matrices.P0, P0moments.momentX, P0moments.momentY);
            (double varianceX, double varianceY) P45variances = matricesCounter.CountVariances(matrices.P45, P45moments.momentX, P45moments.momentY);
            (double varianceX, double varianceY) P90variances = matricesCounter.CountVariances(matrices.P90, P90moments.momentX, P90moments.momentY);
            (double varianceX, double varianceY) P135variances = matricesCounter.CountVariances(matrices.P135, P135moments.momentX, P135moments.momentY);

            var secondMomentP0 = matricesCounter.CountSecondMoment(matrices.P0);
            var secondMomentP45 = matricesCounter.CountSecondMoment(matrices.P45);
            var secondMomentP90 = matricesCounter.CountSecondMoment(matrices.P90);
            var secondMomentP135 = matricesCounter.CountSecondMoment(matrices.P135);

            var contrastP0 = matricesCounter.CountContrast(matrices.P0);
            var contrastP45 = matricesCounter.CountContrast(matrices.P45);
            var contrastP90 = matricesCounter.CountContrast(matrices.P90);
            var contrastP135 = matricesCounter.CountContrast(matrices.P135);

            var correlationP0 = matricesCounter.CountCorrelation(matrices.P0, P0moments.momentX, P0moments.momentY, P0variances.varianceX, P0variances.varianceY);
            var correlationP45 = matricesCounter.CountCorrelation(matrices.P45, P45moments.momentX, P45moments.momentY, P45variances.varianceX, P45variances.varianceY);
            var correlationP90 = matricesCounter.CountCorrelation(matrices.P90, P90moments.momentX, P90moments.momentY, P90variances.varianceX, P90variances.varianceY);
            var correlationP135 = matricesCounter.CountCorrelation(matrices.P135, P135moments.momentX, P135moments.momentY, P135variances.varianceX, P135variances.varianceY);

            var entropyP0 = matricesCounter.CountEntropy(matrices.P0);
            var entropyP45 = matricesCounter.CountEntropy(matrices.P45);
            var entropyP90 = matricesCounter.CountEntropy(matrices.P90);
            var entropyP135 = matricesCounter.CountEntropy(matrices.P135);

            var gomogeneityP0 = matricesCounter.CountGomogeneity(matrices.P0);
            var gomogeneityP45 = matricesCounter.CountGomogeneity(matrices.P45);
            var gomogeneityP90 = matricesCounter.CountGomogeneity(matrices.P90);
            var gomogeneityP135 = matricesCounter.CountGomogeneity(matrices.P135);

            double averageSecondMoment = (secondMomentP0 + secondMomentP45 + secondMomentP90 + secondMomentP135) / 4;
            double averageContrast = (contrastP0 + contrastP45 + contrastP90 + contrastP135) / 4;
            double averageCorrelation = (correlationP0 + correlationP45 + correlationP90 + correlationP135) / 4;
            double averageEntropy = (entropyP0 + entropyP45 + entropyP90 + entropyP135) / 4;
            double gomogeneity = (gomogeneityP0 + gomogeneityP45 + gomogeneityP90 + gomogeneityP135) / 4;

            if (writer is not null)
            {
                writer.Write($"{secondMomentP0} {secondMomentP45} {secondMomentP90} {secondMomentP135} " +
                    /*$"{contrastP0} {contrastP45} {contrastP90} {contrastP135} " +*/
                    $"{correlationP0} {correlationP45} {correlationP90} {correlationP135} "
                    /*$"{entropyP0} {entropyP45} {entropyP90} {entropyP135}"*/);
                writer.WriteLine();
            }

            return (averageSecondMoment, averageContrast, averageCorrelation, averageEntropy, gomogeneity);
        }
    }
}
