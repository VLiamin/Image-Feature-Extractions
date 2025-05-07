namespace HaralickFingers
{
    public class MatricesCounter
    {
        public (double[,] P0, double[,] P45, double[,] P90, double[,] P135) CountMatrices(int[,] matrix, int d)
        {
            List<int> levels = new List<int>();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    levels.Add(matrix[i, j]);
                }
            }

            levels = levels.Distinct().ToList();
            levels.Sort();

            double[,] P0 = new double[levels.Count, levels.Count];
            double[,] P45 = new double[levels.Count, levels.Count];
            double[,] P90 = new double[levels.Count, levels.Count];
            double[,] P135 = new double[levels.Count, levels.Count];

            for (int i = 0; i < levels.Count; i++)
            {
                for (int j = 0; j < levels.Count; j++)
                {
                    //PO
                    int sum = 0;
                    for (int k = 0; k < matrix.GetLength(0); k++)
                    {
                        for (int l = 0; l < matrix.GetLength(1); l++)
                        {
                            if (matrix[k, l] == levels[i] &&
                            (l - d >= 0 && matrix[k, l - d] == levels[j] || l + d < matrix.GetLength(1) && matrix[k, l + d] == levels[j]))
                            {
                                sum++;
                            }

                            if (matrix[k, l] == levels[i] &&
                           (l - d >= 0 && matrix[k, l - d] == levels[j] && l + d < matrix.GetLength(1) && matrix[k, l + d] == levels[j]))
                            {
                                sum++;
                            }
                        }
                    }

                    P0[i, j] = sum;
                    sum = 0;

                    //P90

                    for (int k = 0; k < matrix.GetLength(0); k++)
                    {
                        for (int l = 0; l < matrix.GetLength(1); l++)
                        {
                            if (matrix[k, l] == levels[i] &&
                            (k - d >= 0 && matrix[k - d, l] == levels[j] || k + d < matrix.GetLength(0) && matrix[k + d, l] == levels[j]))
                            {
                                sum++;
                            }

                            if (matrix[k, l] == levels[i] &&
                            (k - d >= 0 && matrix[k - d, l] == levels[j] && k + d < matrix.GetLength(0) && matrix[k + d, l] == levels[j]))
                            {
                                sum++;
                            }
                        }
                    }

                    P90[i, j] = sum;
                    sum = 0;

                    //P45

                    for (int k = 0; k < matrix.GetLength(0); k++)
                    {
                        for (int l = 0; l < matrix.GetLength(1); l++)
                        {
                            if (matrix[k, l] == levels[i] &&
                            (k - d >= 0 && l + d < matrix.GetLength(0) && matrix[k - d, l + d] == levels[j] ||
                            k + d < matrix.GetLength(0) && l - d >= 0 && matrix[k + d, l - d] == levels[j]))
                            {
                                sum++;
                            }

                            if (matrix[k, l] == levels[i] &&
                            (k - d >= 0 && l + d < matrix.GetLength(0) && matrix[k - d, l + d] == levels[j] &&
                            k + d < matrix.GetLength(0) && l - d >= 0 && matrix[k + d, l - d] == levels[j]))
                            {
                                sum++;
                            }
                        }
                    }

                    P45[i, j] = sum;
                    sum = 0;

                    //P135

                    for (int k = 0; k < matrix.GetLength(0); k++)
                    {
                        for (int l = 0; l < matrix.GetLength(1); l++)
                        {
                            if (matrix[k, l] == levels[i] &&
                            (k - d >= 0 && l - d >= 0 && matrix[k - d, l - d] == levels[j] ||
                            k + d < matrix.GetLength(0) && l + d < matrix.GetLength(0) && matrix[k + d, l + d] == levels[j]))
                            {
                                sum++;
                            }

                            if (matrix[k, l] == levels[i] &&
                            (k - d >= 0 && l - d >= 0 && matrix[k - d, l - d] == levels[j] &&
                            k + d < matrix.GetLength(0) && l + d < matrix.GetLength(0) && matrix[k + d, l + d] == levels[j]))
                            {
                                sum++;
                            }
                        }
                    }

                    P135[i, j] = sum;
                }
            }

            return (P0, P45, P90, P135);
        }

        public void MakeNormal(double[,] matrix)
        {
            double sum = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    sum += matrix[i, j];
                }
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] /= sum;
                }
            }
        }

        public (double momentX, double momentY) CountMoments(double[,] matrix)
        {
            double momentX = 0;
            double momentY = 0;

            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    momentX += i * matrix[i, j];
                }
            }

            for (int j = 1; j < matrix.GetLength(1); j++)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    momentY += j * matrix[i, j];
                }
            }

            return (momentX, momentY);
        }

        public (double varianceX, double varianceY) CountVariances(double[,] matrix, double momentX, double momentY)
        {
            double varianceX = 0;
            double varianceY = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                double multiplier = Math.Pow(1 - momentX, 2);
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    varianceX += multiplier * matrix[i, j];
                }
            }

            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                double multiplier = Math.Pow(1 - momentY, 2);
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    varianceY += multiplier * matrix[i, j];
                }
            }

            return (varianceX, varianceY);
        }

        public double CountSecondMoment(double[,] matrix)
        {
            double sum = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    sum += Math.Pow(matrix[i, j], 2);
                }
            }

            return sum;
        }

        public double CountContrast(double[,] matrix)
        {
            double sum = 0;
            for (int n = 1; n < matrix.GetLength(0) - 1; n++)
            {
                for (int i = 1; i < matrix.GetLength(0); i++)
                {
                    for (int j = 1; j < matrix.GetLength(1); j++)
                    {
                        if (Math.Abs(i - j) == n)
                        {
                            sum += n * n + matrix[i, j];
                        }
                    }
                }
            }

            return sum;
        }

        public double CountEntropy(double[,] matrix)
        {
            double sum = 0;
            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                for (int j = 1; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] != 0)
                    {
                        sum += matrix[i, j] * Math.Log(matrix[i, j]);
                    }
                    
                }
            }

            return -sum;
        }

        public double CountCorrelation(
            double[,] matrix,
            double momentX,
            double momentY,
            double varianceX,
            double varianceY)
        {
            double sum = 0;
            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                for (int j = 1; j < matrix.GetLength(1); j++)
                {
                    sum += i * j + matrix[i, j];
                }
            }

            sum = (sum - momentX * momentY) / (varianceX * varianceY);

            return sum;
        }
    }
}
