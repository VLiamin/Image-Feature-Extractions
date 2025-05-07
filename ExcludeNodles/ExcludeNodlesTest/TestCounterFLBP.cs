using ExcludeNodles;
using ExcludeNodles.Models;

namespace ExcludeNodlesTest
{
	[TestClass]
	public sealed class TestCounterFLBP
	{
		private CounterBP counterFLBP;

		public TestCounterFLBP()
		{
			counterFLBP = new();
		}

		[TestMethod]
		public void TestMethodLBP()
		{
			int[,] area =
				{
					{ 90, 200, 120},
					{ 180, 172, 100},
					{ 170, 181, 152}
				};

			List<ExcludeNodles.Models.Element> result = counterFLBP.CountLBP(area);

			List<ExcludeNodles.Models.Element> expected = new List<ExcludeNodles.Models.Element>
			{
				new Element
				{
					FLBP = 162,
					Weight = 1
				}
			};

			Assert.AreEqual(expected.Count, result.Count);

			Assert.IsTrue(Math.Abs(expected[0].Weight - result[0].Weight) < 0.0001);
			Assert.AreEqual(expected[0].FLBP, result[0].FLBP);
		}


		[TestMethod]
		public void TestMethodWithOneTexton()
		{
			int[,] area =
				{
					{ 90, 200, 120},
					{ 180, 142, 100},
					{ 182, 181, 144}
				};

			List<ExcludeNodles.Models.Element> result = counterFLBP.CountFLBP(area, 5);

			List<ExcludeNodles.Models.Element> expected = new List<ExcludeNodles.Models.Element>
			{
				new Element
				{
					FLBP = 242,
					Weight = 0.7
				},
				new Element
				{
					FLBP = 226,
					Weight = 0.3
				}
			};

			Assert.AreEqual(expected.Count, result.Count);

			Assert.IsTrue(Math.Abs(expected[0].Weight - result[0].Weight) < 0.0001);
			Assert.AreEqual(expected[0].FLBP, result[0].FLBP);
			Assert.IsTrue(Math.Abs(expected[1].Weight - result[1].Weight) < 0.0001);
			Assert.AreEqual(expected[1].FLBP, result[1].FLBP);
		}

		[TestMethod]
		public void TestMethodWithOneTexton2()
		{
			int[,] area =
				{
					{ 90, 200, 120},
					{ 180, 142, 140},
					{ 182, 181, 144}
				};

			List<ExcludeNodles.Models.Element> result = counterFLBP.CountFLBP(area, 5);

			List<ExcludeNodles.Models.Element> expected = new List<ExcludeNodles.Models.Element>
			{
				new Element
				{
					FLBP = 250,
					Weight = 0.21
				},
				new Element
				{
					FLBP = 234,
					Weight = 0.09
				},
				new Element
				{
					FLBP = 242,
					Weight = 0.49
				},
				new Element
				{
					FLBP = 226,
					Weight = 0.21
				}
			};

			Assert.AreEqual(expected.Count, result.Count);

			Assert.IsTrue(Math.Abs(expected[0].Weight - result[0].Weight) < 0.0001);
			Assert.AreEqual(expected[0].FLBP, result[0].FLBP);
			Assert.IsTrue(Math.Abs(expected[1].Weight - result[1].Weight) < 0.0001);
			Assert.AreEqual(expected[1].FLBP, result[1].FLBP);
		}

		[TestMethod]
		public void TestMethodWithTwoTextons()
		{
			int[,] area =
	{
					{ 90, 200, 120, 103},
					{ 180, 142, 100, 160},
					{ 182, 181, 144, 188}
				};

			List<Element> result = counterFLBP.CountFLBP(area, 5);

			List<ExcludeNodles.Models.Element> expected = new List<ExcludeNodles.Models.Element>
			{
				new Element
				{
					FLBP = 242,
					Weight = 0.7
				},
				new Element
				{
					FLBP = 226,
					Weight = 0.3
				},
				new Element
				{
					FLBP = 255,
					Weight = 0.8
				},
				new Element
				{
					FLBP = 251,
					Weight = 0.2
				}
			};

			Assert.AreEqual(4, result.Count);

			Assert.IsTrue(Math.Abs(expected[0].Weight - result[0].Weight) < 0.0001);
			Assert.AreEqual(expected[0].FLBP, result[0].FLBP);
			Assert.IsTrue(Math.Abs(expected[1].Weight - result[1].Weight) < 0.0001);
			Assert.AreEqual(expected[1].FLBP, result[1].FLBP);

			Assert.IsTrue(Math.Abs(expected[2].Weight - result[2].Weight) < 0.0001);
			Assert.AreEqual(expected[2].FLBP, result[2].FLBP);
			Assert.IsTrue(Math.Abs(expected[3].Weight - result[3].Weight) < 0.0001);
			Assert.AreEqual(expected[3].FLBP, result[3].FLBP);
		}
	}
}
