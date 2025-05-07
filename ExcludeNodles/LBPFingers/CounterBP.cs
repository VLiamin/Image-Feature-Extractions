using LBPFingers.Models;

namespace LBPFingers
{
	public class CounterBP
	{
		private List<Element> elements;

		public List<Element> CountLBP(int[,] area)
		{
			elements = new();
			int rows = area.GetUpperBound(0) + 1;    // количество строк
			int columns = area.Length / rows;

			for (int i = 1; i < rows - 1; i++)
			{
				for (int j = 1; j < columns - 1; j++)
				{
					int[] texton = new int[8];
					int counter = 0;

					texton[0] = area[i - 1, j - 1] - area[i, j];
					texton[1] = area[i - 1, j] - area[i, j];
					texton[2] = area[i - 1, j + 1] - area[i, j];

					texton[3] = area[i, j + 1] - area[i, j];
					texton[4] = area[i + 1, j + 1] - area[i, j];
					texton[5] = area[i + 1, j] - area[i, j];
					texton[6] = area[i + 1, j - 1] - area[i, j];
					texton[7] = area[i, j - 1] - area[i, j];

					FillElements(0, 0, texton, new Element());
				}
			}

			return elements;
		}

		public List<Element> CountFLBP(int[,] area, int f)
		{
			elements = new();
			int rows = area.GetUpperBound(0) + 1;    // количество строк
			int columns = area.Length / rows;

			for (int i = 1; i < rows - 1; i++)
			{
				for (int j = 1; j < columns - 1; j++)
				{
					int[] texton = new int[8];
					int counter = 0;

					texton[0] = area[i - 1, j - 1] - area[i, j];
					texton[1] = area[i - 1, j] - area[i, j];
					texton[2] = area[i - 1, j + 1] - area[i, j];

					texton[3] = area[i, j + 1] - area[i, j];
					texton[4] = area[i + 1, j + 1] - area[i, j];
					texton[5] = area[i + 1, j] - area[i, j];
					texton[6] = area[i + 1, j - 1] - area[i, j];
					texton[7] = area[i, j - 1] - area[i, j];

					FillElements(0, f, texton, new Element());
				}
			}

			return elements;
		}

		private double CountFunction(int f, int delta)
		{
			return (f + delta) / ((double)2 * f);
		}

		private void FillElements(int i, int f, int[] texton, Element element)
		{
			if (i == 8)
			{
				elements.Add(element);

				return;
			}

			if (texton[i] >= f)
			{
				element.FLBP += (int)Math.Pow(2, i);
				FillElements(++i, f, texton, element);
			}
			else if (texton[i] > -f)
			{
				Element newElement = new Element();

				newElement.FLBP = element.FLBP;
				newElement.Weight = element.Weight;

				double weight = CountFunction(f, texton[i]);
				element.FLBP += (int)Math.Pow(2, i);
				element.Weight *= weight;
				newElement.Weight *= 1 - weight;

				i = i + 1;
				FillElements(i, f, texton, element);
				FillElements(i, f, texton, newElement);
			}
			else
			{
				FillElements(++i, f, texton, element);
			}
		}
	}
}
