using Aspose.Svg.Drawing;

namespace Haralick.ImageTransformation
{
    public class RGBToHCV
    {
        public static (double H, double S, double V) Transform(double R, double G, double B)
        {
            Color color = Color.FromRgb((byte)R, (byte)G, (byte)B);

            var r = color.Convert(ColorModel.Hsv);

            float[] list = r.Components;


            return (float.IsNaN(list[0]) ? 1 : list[0], list[1], list[2]);
        }
    }
}
