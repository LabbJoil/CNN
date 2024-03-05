
using System.Drawing;

namespace CNM;

internal static class ConverterPicture
{
    public static int[][,] ConvertToMatrix(string path)
    {
        var image = new Bitmap(path);
        var resizeImage = new Bitmap(image, new Size(216, 216));
        int[][,] pictureMatrixRGB = [
            new int[resizeImage.Height, resizeImage.Width],
            new int[resizeImage.Height, resizeImage.Width],
            new int[resizeImage.Height, resizeImage.Width]
        ];
        for (int y = 0; y < resizeImage.Height; y++)
            for (int x = 0; x < resizeImage.Width; x++)
            {
                var pixel = resizeImage.GetPixel(x, y);
                int[] colors = [pixel.R, pixel.G, pixel.B];
                for (var color = 0; color < 3; color++)
                    pictureMatrixRGB[color][y, x] = colors[color] / 255;
            }
        return pictureMatrixRGB;
    }

    public static int[,] ConvolutionMatrixRGB(int[][,] pictureMatrixRGB, int[,] coreMatrix)
    {
        // TODO: проверка на равенство всех слоёв
        int pictureMatrixWidth = pictureMatrixRGB[0].GetLength(1),
            pictureMatrixHeight = pictureMatrixRGB[0].GetLength(0),
            coreMatrixWidth = coreMatrix.GetLength(1),
            coreMatrixHeight = coreMatrix.GetLength(0);

        int[,] collapsedMatrix = new int[pictureMatrixHeight - 2, pictureMatrixWidth - 2];

        for (int rowP = 0; rowP + coreMatrixHeight <= pictureMatrixHeight; rowP++)
            for (int colP = 0; colP + coreMatrixWidth <= pictureMatrixWidth; colP++)
            {
                int sum = 0;
                for (int rowC = 0; rowC < coreMatrixHeight; rowC++)
                    for (int colC = 0; colC < coreMatrixWidth; colC++)
                        for (int color = 0; color < 3; color++)
                            sum += pictureMatrixRGB[color][rowP + rowC, colP + colC] * coreMatrix[rowC, colC];
                collapsedMatrix[rowP, colP] = sum;
            }
        return collapsedMatrix;
    }

    public static double[] GetRow(double[,] matrix, int row)
    {
        var columns = matrix.GetLength(1);
        var array = new double[columns];
        for (int i = 0; i < columns; ++i)
            array[i] = matrix[row, i];
        return array;
    }
}
