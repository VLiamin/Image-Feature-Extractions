using System.Drawing;
using ExcludeNodles.Squares;

namespace ExcludeNodles
{
    public class Program
    {
        static void Main(string[] args)
        {
            int f = 13;
            /*			ImageExtracter imageExtracter = new ImageExtracter();

                        List<(Bitmap original, Bitmap extracted)> imagesRight = imageExtracter.ExtractImages("C:\\Users\\Lyami\\Аспирантура\\FolRightImages");
                        List<(Bitmap original, Bitmap extracted)> imagesleft = imageExtracter.ExtractImages("C:\\Users\\Lyami\\Аспирантура\\FolLeftImages");


                        //return;
                        Squares squares = new Squares(20);
                        List<int[,]> nodularsAll = new List<int[,]>();
                        List<int[,]> normalsAll = new List<int[,]>();

                        SharpnessByLaplacian sharpnessByLaplacian = new();
                        IncreaseTreble increaseTreble = new IncreaseTreble();
                        HistogramVariation histogramVariation = new HistogramVariation();*/

            /*            var fileLabelPaths = Directory.GetFiles("C:\\Users\\Lyami\\Аспирантура\\VOCdevkit\\VOC\\SegmentationClass", "*.png");
                        var fileImagePaths = Directory.GetFiles("C:\\Users\\Lyami\\Аспирантура\\VOCdevkit\\VOC\\JPEGImages", "*.jpeg");*/

            var fileLabelPaths = Directory.GetFiles("C:\\Users\\Lyami\\Аспирантура\\NewImagesSelected\\Labels", "*.png");
            var fileImagePaths = Directory.GetFiles("C:\\Users\\Lyami\\Аспирантура\\NewImagesSelected\\Originals", "*.jpeg");

            List<Bitmap> labels = fileLabelPaths.Select(f => new Bitmap(f)).ToList();
            List<Bitmap> images = fileImagePaths.Select(f => new Bitmap(f)).ToList();

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

            FGLHCounter fGLHCounter = new FGLHCounter();


            //	return;

            if (normalsAll.Count > nodularsAll.Count)
            {
                normalsAll = normalsAll.GetRange(0, nodularsAll.Count);
            }
            else
            {
                nodularsAll = nodularsAll.GetRange(0, normalsAll.Count);
            }

            //return;

            CounterBP counterFLBP = new CounterBP();

            List<List<Models.Element>> flbps = new();
            List<bool> isNodular = new();


            //	List<double[]> fglhList = new();

            foreach (int[,] nodular in nodularsAll)
            {
                flbps.Add(counterFLBP.CountLBP(nodular));
                isNodular.Add(true);

                double[] fglh = new double[256];

                /*				(List<double> histogramm, List<int> levels) res = fGLHCounter.CountFGLH(nodular, 5);
                                int j = 0;
                                for (int i = 0; i < 256; i++)
                                {
                                    if (j < res.levels.Count && res.levels[j] == i)
                                    {
                                        fglh[i] = res.histogramm[j];
                                        j++;
                                    }

                                }*/
                //	fglhList.Add(fglh);
            }


            int[,] nodularTest = nodularsAll[0];
            int[] nodularIntens = new int[256];

            foreach (var normal in normalsAll)
            {
                //flbps.Add(counterFLBP.CountFLBP(normal, f));
                flbps.Add(counterFLBP.CountLBP(normal));
                isNodular.Add(false);

                double[] fglh = new double[256];

                /*				(List<double> histogramm, List<int> levels) res = fGLHCounter.CountFGLH(normal, 5);
                                int j = 0;
                                for (int i = 0; i < 256; i++)
                                {
                                    if (j < res.levels.Count && res.levels[j] == i)
                                    {
                                        fglh[i] = res.histogramm[j];
                                        j++;
                                    }
                                }

                                fglhList.Add(fglh);*/
            }

            using (StreamWriter writer = new StreamWriter("IsNodularData.txt", false))
            {
                isNodular.ForEach(x => writer.WriteLine((x).ToString()));
            }

            using (StreamWriter writer = new StreamWriter("Data.txt", false))
            {
                foreach (List<Models.Element> flbp in flbps)
                {
                    flbp.ForEach(x => writer.Write(x.FLBP + " "));
                    //flbp.ForEach(x => writer.Write("\\" + x.FLBP + " " + x.Weight));
                    writer.WriteLine();
                }
            }

            /*            using (StreamWriter writer = new StreamWriter("DataFGLH.txt", false))
                        {
                            foreach (double[] fglh in fglhList)
                            {
                                for (int i = 0; i < fglh.Length; i++)
                                {
                                    writer.Write(fglh[i] + " ");
                                }

                                writer.WriteLine();
                                writer.WriteLine();
                            }
                        }*/
        }
    }
}
