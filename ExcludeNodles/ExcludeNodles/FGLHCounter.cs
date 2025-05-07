namespace ExcludeNodles
{
	public class FGLHCounter
	{
		public (List<double> histogramm, List<int> levels) CountFGLH(int[,] area, int parameter)
		{
			List<int> levels = new List<int>();
			int rows = area.GetUpperBound(0) + 1;    // количество строк
			int columns = area.Length / rows;

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					levels.Add(area[i, j]);
				}
			}

			levels = levels.Distinct().ToList();
			levels.Sort();
			int numberOfLevels = levels.Count;
			List<double> histogramm = new List<double>();

			for (int k = 0; k < levels.Count; k++)
			{
				double hg = 0;
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{						
						double mg = CountMg(parameter, area[i, j], levels[k]);
						hg += mg;

					}
				}

				hg /= area.Length;

				histogramm.Add(hg);
			}

			return (histogramm, levels);
		}

		private double CountMg(int parametr, int gi, int g)
		{
			if (Math.Abs(gi - g) >= parametr) { return 0; }

			return (double)parametr - Math.Abs(gi - g) / Math.Pow(parametr, 2);
		}
	}
}
