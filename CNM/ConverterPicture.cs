
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

    public static double[,] ConvolutionMatrix(double[,] convertMatrix, double[,] coreMatrix)
    {
        int convertMatrixWidth = convertMatrix.GetLength(1),
            convertMatrixHeight = convertMatrix.GetLength(0),
            coreMatrixWidth = coreMatrix.GetLength(1),
            coreMatrixHeight = coreMatrix.GetLength(0);

        double[,] collapsedMatrix = new double[convertMatrixHeight - coreMatrixHeight + 1, convertMatrixWidth - coreMatrixWidth + 1];
        for (int yConverMatrix = 0; yConverMatrix + coreMatrixHeight <= convertMatrixHeight; yConverMatrix++)
            for (int xConverMatrix = 0; xConverMatrix + coreMatrixWidth <= convertMatrixWidth; xConverMatrix++)
            {
                double sum = 0;
                for (int yCore = 0; yCore < coreMatrixHeight; yCore++)
                    for (int xCore = 0; xCore < coreMatrixWidth; xCore++)
                        sum += convertMatrix[yConverMatrix + yCore, xConverMatrix + xCore] * coreMatrix[yCore, xCore];
                collapsedMatrix[yConverMatrix, xConverMatrix] = Activation.Sigmoid(sum);
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

    public static double[,] Puling(double[,] convertMatrix)
    {
        int convertMatrixWidth = convertMatrix.GetLength(1),
            convertMatrixHeight = convertMatrix.GetLength(0),
            pulingMatrixWidth = convertMatrixWidth / 2,
            pulingMatrixHeight = convertMatrixHeight / 2;

        double[,] pulingMatrix = new double[pulingMatrixHeight, pulingMatrixWidth];
        List<(int, int)> maxElementsPlaces = [];

        for (int yConverMatrix = 0, yPuling = 0; yConverMatrix < convertMatrixHeight; yConverMatrix += 2, yPuling++)
            for (int xConverMatrix = 0, xPuling = 0; xConverMatrix < convertMatrixWidth; xConverMatrix += 2, xPuling++)
            {
                double max = convertMatrix[yConverMatrix, xConverMatrix];
                int maxY = yConverMatrix, maxX = xConverMatrix;
                for (int yCore = 0; yCore < 2; yCore++)
                    for (int xCore = 0; xCore < 2; xCore++)
                    {
                        if (convertMatrix[yConverMatrix + yCore, xConverMatrix + xCore] >= max)
                        {
                            maxY = yConverMatrix + yCore;
                            maxX = xConverMatrix + xCore;
                            max = convertMatrix[yConverMatrix + yCore, xConverMatrix + xCore];
                        }
                    }
                maxElementsPlaces.Add((maxX, maxY));
                pulingMatrix[yPuling, xPuling] = max;
            }
        return pulingMatrix;
    }

    public static double[,] Padding(double[,] convertMatrix)
    {
        int convertMatrixWidth = convertMatrix.GetLength(1),
            convertMatrixHeight = convertMatrix.GetLength(0);
        double[,] newConvertMatrix = new double[convertMatrixWidth + 2, convertMatrixHeight + 2];

        for (int y = 0; y < convertMatrixHeight; y++)
            for (int x = 0; x < convertMatrixWidth; x++)
            {
                if (y == 0 || y == convertMatrixHeight - 1 || x == 0 || x == convertMatrixWidth - 1)
                    newConvertMatrix[y, x] = 0;
                newConvertMatrix[y, x] = convertMatrix[y - 1, x - 1];
            }
        return newConvertMatrix;
    }

    public static double[,] CreateCore(int size)
    {
        Random rand = new();
        double[,] core = new double[size, size];
        for (int height = 0; height < size; height++)
            for (int width = 0; width < size; width++)
                core[height, width] = rand.NextDouble() - 0.5;
        return core;
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
}
