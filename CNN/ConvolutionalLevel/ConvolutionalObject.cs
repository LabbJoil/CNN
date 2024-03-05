
using static System.Formats.Asn1.AsnWriter;

namespace CNM.ConvolutionalLevel;

internal class ConvolutionalObject
{
    private readonly Matrix2D<double> InputMatrix = new("Input");
    private readonly Matrix2D<double> СollapsedMatrix = new("Collapsed");
    private readonly Matrix2D<double> ReСollapsedMatrix = new("ReCollapsed");
    private readonly Matrix2D<double> CoreMatrix = new("Core");
    private (int, int)[]? MaxElementsPulling;
    private int CollapseStep = 1;

    private ConvolutionalType Type { get; }
    public double[,] СollapsedMatrixProperty
    {
        get => СollapsedMatrix.MatrixTable;
    }

    public ConvolutionalObject(ConvolutionalType type, int? sizeCore = null)
    {
        Type = type;
        if (type == ConvolutionalType.Fold)
        {
            if (sizeCore is int wh)
            {
                var core = CreateCore(wh, wh);
                CoreMatrix.SetMatrix(core);
            }
            else
                throw new Exception("The core size was not passed");
        }
    }

    public void Сollapse(double[,] inputMatrix)
    {
        InputMatrix.SetMatrix(inputMatrix);
        if (Type == ConvolutionalType.Fold)
            ConvolutionMatrix();
        else
            Pulling();
    }

    public void ReCollapse(double[,] error, double learningRate)
    {
        if (Type == ConvolutionalType.Fold)
            ReConvolutionMatrix(error, learningRate);
        else
            RePulling(error);
    }

    private void ConvolutionMatrix()
    {
        //TODO: Проверка demo
        var (inputmatrix, heightInputeMatrix, widthInputeMatrix) = InputMatrix.MatrixData;
        var (core, heightCore, widthCore) = CoreMatrix.MatrixData;
        double collapsedMatrixHeight = (double)(heightInputeMatrix - heightCore) / CollapseStep + 1,
            collapsedMatrixWight = (double)(widthInputeMatrix - widthCore) / CollapseStep + 1;

        if (collapsedMatrixHeight % 1 != 0 || collapsedMatrixWight % 1 != 0)
            throw new Exception($"The step {CollapseStep} is not suitable for the matrix {heightInputeMatrix}x{widthInputeMatrix}");

        double[,] collapsedMatrix = new double[(int)collapsedMatrixHeight, (int)collapsedMatrixWight];

        for (int yInputMatrix = 0; yInputMatrix <= collapsedMatrixHeight; yInputMatrix += CollapseStep)
            for (int xInputMatrix = 0; xInputMatrix <= collapsedMatrixWight; xInputMatrix += CollapseStep)
            {
                double sum = 0;
                for (int yCore = 0; yCore < heightCore; yCore++)
                    for (int xCore = 0; xCore < widthCore; xCore++)
                        sum += inputmatrix[yInputMatrix + yCore, xInputMatrix + xCore] * core[yCore, xCore];
                collapsedMatrix[yInputMatrix, xInputMatrix] = sum;
            }
        СollapsedMatrix.SetMatrix(collapsedMatrix);
    }

    private void ReConvolutionMatrix(double[,] deltas, double learningRate)
    {
        //INFO: работает пока что только с CollapseStep = 1
        //TODO: проверить в demo

        var (core, heightCore, widthCore) = CoreMatrix.MatrixData;
        var (heightInputeMatrix, widthInputeMatrix) = InputMatrix.MatrixSizes;

        var corRot180 = new double[heightCore, widthCore];
        double[,] reСollapsedMatrix = new double[heightInputeMatrix, widthInputeMatrix];

        for (int i = 0; i < heightCore; i++)
            deltas = PaddingUD(deltas);

        for (int i = 0; i < widthCore; i++)
            deltas = PaddingLR(deltas);

        for (int y = 0; y < heightCore; y++)
            for (int x = 0; x < widthCore; x++)
                corRot180[y, x] = core[heightCore - y, widthCore - x];

        for (int yConverMatrix = 0; yConverMatrix < deltas.GetLength(0); yConverMatrix += CollapseStep)
            for (int xConverMatrix = 0; xConverMatrix < deltas.GetLength(1); xConverMatrix += CollapseStep)
            {
                double sum = 0;
                for (int yCore = 0; yCore < heightCore; yCore++)
                    for (int xCore = 0; xCore < widthCore; xCore++)
                        sum += deltas[yConverMatrix + yCore, xConverMatrix + xCore] * core[yCore, xCore];
                reСollapsedMatrix[yConverMatrix, xConverMatrix] = sum;
            }
        ReСollapsedMatrix.SetMatrix(reСollapsedMatrix);

        for (int y = 0; y < heightCore; y++)
            for (int x = 0; x < widthCore; x++)
            {
                var weight = core[y, x];
                var newWeigth = weight - learningRate * deltas[y, x]; // TODO: maybe plus
                core[y, x] = newWeigth;
            }
    }

    private void Pulling()
    {
        int pullingMatrixWidth, pullingMatrixHeight;
        var (inputMatrix, heightInputeMatrix, widthInputeMatrix) = InputMatrix.MatrixData;

        //TODO: сделать step на ширину и высоту
        if (heightInputeMatrix % 2 == 0)
        {
            CollapseStep = 2;
            pullingMatrixWidth = widthInputeMatrix / 2;
            pullingMatrixHeight = heightInputeMatrix / 2;
        }
        else
        {
            pullingMatrixWidth = widthInputeMatrix - 1;
            pullingMatrixHeight = heightInputeMatrix - 1;
        }

        double[,] pullingMatrix = new double[pullingMatrixHeight, pullingMatrixWidth];
        (int, int)[] maxElementsPlaces = new (int, int)[pullingMatrixHeight * pullingMatrixWidth];

        for (int yConverMatrix = 0, yPulling = 0; yConverMatrix < heightInputeMatrix - 1; yConverMatrix += CollapseStep, yPulling++)
        {
            var rowPulling = yPulling * pullingMatrixHeight;
            for (int xConverMatrix = 0, xPulling = 0; xConverMatrix < widthInputeMatrix - 1; xConverMatrix += CollapseStep, xPulling++)
            {
                double max = inputMatrix[yConverMatrix, xConverMatrix];
                int maxY = yConverMatrix, maxX = xConverMatrix;
                for (int yCore = 0; yCore < 2; yCore++)
                    for (int xCore = 0; xCore < 2; xCore++)
                    {
                        double newMax = inputMatrix[yConverMatrix + yCore, xConverMatrix + xCore];
                        if (newMax >= max)
                        {
                            maxY = yConverMatrix + yCore;
                            maxX = xConverMatrix + xCore;
                            max = newMax;
                        }
                    }
                maxElementsPlaces[rowPulling + xPulling] = (maxX, maxY);
                pullingMatrix[yPulling, xPulling] = max;
            }
        }
        СollapsedMatrix.SetMatrix(pullingMatrix);
        MaxElementsPulling = maxElementsPlaces;
    }

    private void RePulling(double[,] deltas)
    {
        //TODO: проверить в demo

        if (MaxElementsPulling == null)
            throw new Exception("Max elements of pulling not found");

        var (heightInputeMatrix, widthInputeMatrix) = InputMatrix.MatrixSizes;
        var deltasHeight = deltas.GetLength(0);
        var deltasWidth = deltas.GetLength(1);
        double[,] reCollapsMatrix = new double[heightInputeMatrix, widthInputeMatrix];

        for (int yError = 0, yInput = 0; yError < deltasHeight; yError++, yInput += CollapseStep)
        {
            var errorRow = yError * deltasHeight;
            for (int xError = 0, xInput = 0; xError < deltasWidth; xError++, xInput += CollapseStep)
            {
                var (maxElementX, maxElementY) = MaxElementsPulling[errorRow + xError];
                for (int yPullingMatrix = 0; yPullingMatrix < 2; yPullingMatrix++)
                    for (int xPullingMatrix = 0; xPullingMatrix < 2; xPullingMatrix++)
                    {
                        int y = yInput + yPullingMatrix,
                            x = xInput + xPullingMatrix;
                        if (maxElementX == x && maxElementY == y)
                            reCollapsMatrix[y, x] += deltas[yError, xError];
                    }
            }
        }
        ReСollapsedMatrix.SetMatrix(reCollapsMatrix);
    }

    private static double[,] PaddingUD(double[,] convertMatrix)
    {
        int convertMatrixWidth = convertMatrix.GetLength(1),
            convertMatrixHeight = convertMatrix.GetLength(0);
        double[,] newConvertMatrix = new double[convertMatrixHeight + 2, convertMatrixWidth];

        for (int y = 0; y < convertMatrixHeight; y++)
            for (int x = 0; x < convertMatrixWidth; x++)
            {
                if (y == 0 || y == convertMatrixHeight - 1)
                    newConvertMatrix[y, x] = 0;
                newConvertMatrix[y, x] = convertMatrix[y - 1, x - 1];
            }
        return newConvertMatrix;
    }

    private static double[,] PaddingLR(double[,] convertMatrix)
    {
        int convertMatrixWidth = convertMatrix.GetLength(1),
            convertMatrixHeight = convertMatrix.GetLength(0);
        double[,] newConvertMatrix = new double[ convertMatrixHeight, convertMatrixWidth + 2];

        for (int y = 0; y < convertMatrixHeight; y++)
            for (int x = 0; x < convertMatrixWidth; x++)
            {
                if (x == 0 || x == convertMatrixWidth - 1)
                    newConvertMatrix[y, x] = 0;
                newConvertMatrix[y, x] = convertMatrix[y - 1, x - 1];
            }
        return newConvertMatrix;
    }

    private static double[,] CreateCore(int width, int height)
    {
        Random rand = new();
        var core = new double[height, width];
        for (int y = 0; y < height; height++)
            for (int x = 0; x < width; width++)
                core[y, x] = rand.NextDouble() - 0.5;
        return core;
    }
}
