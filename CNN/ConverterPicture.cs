
using System.Drawing;

namespace CNM;

internal class ConverterPicture
{
    const byte CountColors = 3;
    private int Height;
    private int Width;

    public int[,] ConvertPicture(string path)
    {
        var image = new Bitmap(path);
        var resixeBitmapImage = new Bitmap(image, new Size(216, 216));
        Height = resixeBitmapImage.Height;
        Width = resixeBitmapImage.Width;
       var rgbValues = ConvertToRGBValues(resixeBitmapImage);
        ConvolutionRGBValues(rgbValues);
    }

    public byte[,,] ConvertToRGBValues(Bitmap bitmapImage)
    {
        byte[,,] pictureMatrixRGB = new byte[CountColors, Height, Width];
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
            {
                var pixel = bitmapImage.GetPixel(x, y);
                byte[] colors = [pixel.R, pixel.G, pixel.B];
                for (var color = 0; color < CountColors; color++)
                    pictureMatrixRGB[color, y, x] = (byte)(colors[color] / byte.MaxValue);
            }
        return pictureMatrixRGB;
    }

    public int[,] ConvolutionRGBValues(int[,,] rgbValues, int[,] coreMatrix)
    {
        // TODO: demo | проверка на равенство всех слоёв
        int coreMatrixWidth = coreMatrix.GetLength(1),
            coreMatrixHeight = coreMatrix.GetLength(0);

        int[,] collapsedMatrix = new int[Height - 2, Width - 2];

        for (int rowP = 0; rowP + coreMatrixHeight <= Height; rowP++)
            for (int colP = 0; colP + coreMatrixWidth <= Width; colP++)
            {
                int sum = 0;
                for (int rowC = 0; rowC < coreMatrixHeight; rowC++)
                    for (int colC = 0; colC < coreMatrixWidth; colC++)
                        for (int color = 0; color < CountColors; color++)
                            sum += rgbValues[color, rowP + rowC, colP + colC] * coreMatrix[rowC, colC];
                collapsedMatrix[rowP, colP] = sum;
            }
        return collapsedMatrix;
    }
}
