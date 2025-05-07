using Haralick;

namespace HaralickTests
{
    public class MatricesCounterTests
    {
        private MatricesCounter matricesCounter;

        [SetUp]
        public void Setup()
        {
            matricesCounter = new MatricesCounter();
        }

        [Test]
        public void Test1()
        {
            int[,] matrix = {
                { 0, 0, 1, 1 },
                { 0, 0, 1, 1 },
                { 0, 2, 2, 2},
                { 2, 2, 3, 3} };
            (double[,] P0, double[,] P45, double[,] P90, double[,] P135) result = matricesCounter.CountMatrices(matrix, 1);

            double[,] P0Expected = {
                { 4, 2, 1, 0 },
                { 2, 4, 0, 0},
                { 1, 0, 6, 1},
                { 0, 0, 1, 2} };

            double[,] P45Expected = {
                { 4, 1, 0, 0 },
                { 1, 2, 2, 0},
                { 0, 2, 4, 1},
                { 0, 0, 1, 0} };

            double[,] P90Expected = {
                { 6, 0, 2, 0 },
                { 0, 4, 2, 0},
                { 2, 2, 2, 2},
                { 0, 0, 2, 0} };

            double[,] P135Expected = {
                { 2, 1, 3, 0 },
                { 1, 2, 1, 0},
                { 3, 1, 0, 2},
                { 0, 0, 2, 0} };


            Assert.AreEqual(P0Expected, result.P0);
            Assert.AreEqual(P45Expected, result.P45);
            Assert.AreEqual(P90Expected, result.P90);
            Assert.AreEqual(P135Expected, result.P135);
        }

        [Test]
        public void Test2()
        {
            int[,] matrix = {
                { 5, 5, 4 },
                { 2, 3, 3 },
                { 2, 5, 5} };
            (double[,] P0, double[,] P45, double[,] P90, double[,] P135) result = matricesCounter.CountMatrices(matrix, 1);

            double[,] P0Expected = {
                { 0, 1, 0, 1 },
                { 1, 2, 0, 0},
                { 0, 0, 0, 1},
                { 1, 0, 1, 4} };

            double[,] P45Expected = {
                { 0, 1, 0, 1 },
                { 1, 0, 1, 1 },
                { 0, 1, 0, 0 },
                { 1, 1, 0, 0 } };

            double[,] P90Expected = {
                { 2, 0, 0, 1 },
                { 0, 0, 1, 3 },
                { 0, 1, 0, 0 },
                { 1, 3, 0, 0 } };

            double[,] P135Expected = {
                { 0, 0, 0, 1 },
                { 0, 0, 0, 3 },
                { 0, 0, 0, 0 },
                { 1, 3, 0, 0 } };


            Assert.AreEqual(P0Expected, result.P0);
            Assert.AreEqual(P45Expected, result.P45);
            Assert.AreEqual(P90Expected, result.P90);
            Assert.AreEqual(P135Expected, result.P135);
        }
    }
}