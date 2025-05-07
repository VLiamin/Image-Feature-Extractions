using System.Drawing;
using LBPFingers.ExcractRegions;

namespace LBPFingers
{
    public class Program
    {
        static void Main(string[] args)
        {
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
                liveTrainingImages.Add(squares.DevideOnSquares(new Bitmap(r, new Size(300, 300))));
            }

            foreach (string path in filePathsLiveTest)
            {
                var r = new Bitmap(path);
                liveTestImages.Add(squares.DevideOnSquares(new Bitmap(r, new Size(300, 300))));
            }

            foreach (string path in filePathsSpoofTraining)
            {
                var r = new Bitmap(path);
                spoofTrainingImages.Add(squares.DevideOnSquares(new Bitmap(r, new Size(300, 300))));
            }

            foreach (string path in filePathsSpoofTest)
            {
                var r = new Bitmap(path);
                spoofTestImages.Add(squares.DevideOnSquares(new Bitmap(r, new Size(300, 300))));
            }

            CounterBP counterFLBP = new CounterBP();

            List<List<Models.Element>> resultTraining = new();
            List<List<Models.Element>> resultTest = new();
            List<bool> isLiveTraining = new();
            List<bool> isLiveTest = new();

            foreach (int[,] liveTraining in liveTrainingImages)
            {
                resultTraining.Add(counterFLBP.CountLBP(liveTraining));
                isLiveTraining.Add(true);
            }

            foreach (int[,] spoofTraining in spoofTrainingImages)
            {
                resultTraining.Add(counterFLBP.CountLBP(spoofTraining));
                isLiveTraining.Add(false);
            }

            foreach (int[,] liveTest in liveTestImages)
            {
                resultTest.Add(counterFLBP.CountLBP(liveTest));
                isLiveTest.Add(true);
            }

            foreach (int[,] spoofTest in spoofTestImages)
            {
                resultTest.Add(counterFLBP.CountLBP(spoofTest));
                isLiveTest.Add(false);
            }

            using (StreamWriter writer = new StreamWriter("IsNodularDataTest.txt", false))
            {
                isLiveTest.ForEach(x => writer.WriteLine((x).ToString()));
            }

            using (StreamWriter writer = new StreamWriter("DataTest.txt", false))
            {
                foreach (List<Models.Element> flbp in resultTest)
                {
                    flbp.ForEach(x => writer.Write(x.FLBP + " "));
                    writer.WriteLine();
                }
            }

            using (StreamWriter writer = new StreamWriter("IsNodularDataTraining.txt", false))
            {
                isLiveTraining.ForEach(x => writer.WriteLine((x).ToString()));
            }

            using (StreamWriter writer = new StreamWriter("DataTraining.txt", false))
            {
                foreach (List<Models.Element> flbp in resultTraining)
                {
                    flbp.ForEach(x => writer.Write(x.FLBP + " "));
                    writer.WriteLine();
                }
            }
        }
    }
}
