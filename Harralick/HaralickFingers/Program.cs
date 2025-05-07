using System.Drawing;
using HaralickFingers.ExcractRegions;

namespace HaralickFingers
{
    internal class Program
    {
        public static MatricesCounter matricesCounter = new MatricesCounter();

        static void Main(string[] args)
        {
            MatricesCounter counter = new MatricesCounter();

            double[,] matrix = {
                { 4, 2, 1, 0 },
                { 2, 4, 0, 0},
                { 1, 0, 6, 1},
                { 0, 0, 1, 2} };

            var res = counter.CountMoments(matrix);
            var res2 = counter.CountVariances(matrix, res.momentX, res.momentY);
            var res3 = counter.CountSecondMoment(matrix);


            var filePathsLiveTraining = Directory.GetFiles("C:\\Users\\Lyami\\Аспирантура\\Spoof_data\\Training Biometrika Live\\live", "*.png");
            var filePathsLiveTest = Directory.GetFiles("C:\\Users\\Lyami\\Аспирантура\\Spoof_data\\Testing Biometrika Live\\live", "*.png");
            var filePathsSpoofTraining = Directory.GetFiles("C:\\Users\\Lyami\\Аспирантура\\Spoof_data\\Training Biometrika Spoof\\Training Biometrika Spoof\\spoof", "*.png");
            var filePathsSpoofTest = Directory.GetFiles("C:\\Users\\Lyami\\Аспирантура\\Spoof_data\\Testing Biometrika Spoof\\Testing Biometrika Spoof\\spoof", "*.png");

            List<int[,]> liveTrainingImages = new List<int[,]>();
            List<int[,]> liveTestImages = new List<int[,]>();
            List<int[,]> spoofTrainingImages = new List<int[,]>();
            List<int[,]> spoofTestImages = new List<int[,]>();

            Squares squares = new Squares();

            foreach (string path in filePathsLiveTraining)
            {
                var r = new Bitmap(path);
                liveTrainingImages.Add(squares.DevideOnSquares(new Bitmap(r, new Size(40, 40))));
            }

            foreach (string path in filePathsLiveTest)
            {
                var r = new Bitmap(path);
                liveTestImages.Add(squares.DevideOnSquares(new Bitmap(r, new Size(40, 40))));
            }

            foreach (string path in filePathsSpoofTraining)
            {
                var r = new Bitmap(path);
                spoofTrainingImages.Add(squares.DevideOnSquares(new Bitmap(r, new Size(40, 40))));
            }

            foreach (string path in filePathsSpoofTest)
            {
                var r = new Bitmap(path);
                spoofTestImages.Add(squares.DevideOnSquares(new Bitmap(r, new Size(40, 40))));
            }

            

            List<(double moment, double contr, double corr, double entr)> resultTest = new();
            List<(double moment, double contr, double corr, double entr)> resultTraining = new();
            List<bool> isLiveTest = new();
            List<bool> isLiveTraining = new();

            foreach (var liveTraining in liveTrainingImages)
            {
                var liveTrainingResult = Program.CountStatistics(liveTraining);
                resultTraining.Add((
                    liveTrainingResult.averageSecondMoment,
                    liveTrainingResult.averageContrast,
                    liveTrainingResult.averageCorrelation,
                    liveTrainingResult.averageEntropy));
                isLiveTraining.Add(true);
            }

            foreach (var liveTest in liveTestImages)
            {
                var liveTestResult = Program.CountStatistics(liveTest);
                resultTest.Add((
                    liveTestResult.averageSecondMoment,
                    liveTestResult.averageContrast,
                    liveTestResult.averageCorrelation,
                    liveTestResult.averageEntropy));
                isLiveTest.Add(true);
            }

            foreach (var spoofTraining in spoofTrainingImages)
            {
                var spoofTrainingResult = Program.CountStatistics(spoofTraining);
                resultTraining.Add((
                    spoofTrainingResult.averageSecondMoment,
                    spoofTrainingResult.averageContrast,
                    spoofTrainingResult.averageCorrelation,
                    spoofTrainingResult.averageEntropy));
                isLiveTraining.Add(false);
            }

            foreach (var spoofTest in spoofTestImages)
            {
                var spoofTestResult = Program.CountStatistics(spoofTest);
                resultTest.Add((
                    spoofTestResult.averageSecondMoment,
                    spoofTestResult.averageContrast,
                    spoofTestResult.averageCorrelation,
                    spoofTestResult.averageEntropy));
                isLiveTest.Add(true);
            }

            using (StreamWriter writer = new StreamWriter("IsLiveTraining.txt", false))
            {
                isLiveTraining.ForEach(x => writer.WriteLine((x).ToString()));
            }

            using (StreamWriter writer = new StreamWriter("DataTraining.txt", false))
            {
                foreach ((double moment, double contr, double corr, double entr) har in resultTraining)
                {
                    writer.Write($"{har.moment} {har.contr} {har.corr} {har.entr}");

                    writer.WriteLine();
                }
            }

            using (StreamWriter writer = new StreamWriter("IsLiveTest.txt", false))
            {
                isLiveTest.ForEach(x => writer.WriteLine((x).ToString()));
            }

            using (StreamWriter writer = new StreamWriter("DataTest.txt", false))
            {
                foreach ((double moment, double contr, double corr, double entr) har in resultTest)
                {
                    writer.Write($"{har.moment} {har.contr} {har.corr} {har.entr}");

                    writer.WriteLine();
                }
            }
        }

        public static (double averageSecondMoment, double averageContrast, double averageCorrelation, double averageEntropy) 
            CountStatistics(int[,] image)
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

            double averageSecondMoment = (secondMomentP0 + secondMomentP45 + secondMomentP90 + secondMomentP135) / 4;
            double averageContrast = (contrastP0 + contrastP45 + contrastP90 + contrastP135) / 4;
            double averageCorrelation = (correlationP0 + correlationP45 + correlationP90 + correlationP135) / 4;
            double averageEntropy = (entropyP0 + entropyP45 + entropyP90 + entropyP135) / 4;

            return (averageSecondMoment, averageContrast, averageCorrelation, averageEntropy);
        }
    }
}
