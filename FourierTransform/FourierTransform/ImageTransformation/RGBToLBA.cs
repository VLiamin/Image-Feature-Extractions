using Aspose.Svg.Drawing;
using Color = Aspose.Svg.Drawing.Color;

namespace FourierTransformormation.ImageTransformation
{
    internal class RGBToLBA
    {
        public static (double l, double a, double b) Transform(double R, double G, double B)
        {
            Color color = Color.FromRgb((byte)R, (byte)G, (byte)B);

            // Convert RGB to LAB
            var labColor = color.Convert(ColorModel.Lab);

            var list = labColor.Components;

            return (list[0], list[1], list[2]);
        }
    }
}
