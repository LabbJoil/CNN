
using CNN.Abstract;
using CNN.Model.Params;

namespace CNN.FeatureExtractorLevel.Converter;

internal class Pooling(ConverterComponentParams ccParams) : ConverterComponent(ccParams)
{
    private (int, int)[]? MaxElementsPooling;
    private Convolution[] ConvolutionLayer = new Convolution[ccParams.CountMaps];

    public override void Сollapse(double[,] inputMatrix)
    {
        InputMatrix.SetMatrix(inputMatrix);
        PoolingMatrix();
    }

    public override void ReCollapse(double[,] deltas)
    {
        RePoolingMatrix(deltas);
    }

    private void PoolingMatrix()
    {
        var (inputMatrix, inputMatrixHeight, inputMatrixWidth) = InputMatrix.MatrixData;
        var (collapsedMatrixHeight, collapsedMatrixWidth) = СollapsedMatrix.MatrixSizes;
        double[,] collapsedMatrix = new double[collapsedMatrixHeight, collapsedMatrixWidth];
        (int, int)[] maxElementsPlaces = new (int, int)[collapsedMatrixHeight * collapsedMatrixWidth];

        for (int yConverMatrix = 0, yPooling = 0; yConverMatrix < inputMatrixHeight - 1; yConverMatrix += StepConvertionHieght, yPooling++)
        {
            var rowPooling = yPooling * collapsedMatrixHeight;
            for (int xConverMatrix = 0, xPooling = 0; xConverMatrix < inputMatrixWidth - 1; xConverMatrix += StepConvertionWidth, xPooling++)
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
                maxElementsPlaces[rowPooling + xPooling] = (maxX, maxY);
                collapsedMatrix[yPooling, xPooling] = max;
            }
        }
        СollapsedMatrix.SetMatrix(collapsedMatrix);
        MaxElementsPooling = maxElementsPlaces;
    }

    private void RePoolingMatrix(double[,] deltas)
    {
        if (MaxElementsPooling == null)
            throw new Exception("Max elements of Pooling not found");

        var (heightInputeMatrix, widthInputeMatrix) = InputMatrix.MatrixSizes;
        var deltasHeight = deltas.GetLength(0);
        var deltasWidth = deltas.GetLength(1);
        double[,] reCollapsMatrix = new double[heightInputeMatrix, widthInputeMatrix];

        for (int yError = 0, yInput = 0; yError < deltasHeight; yError++, yInput += StepConvertionHieght)
        {
            var errorRow = yError * deltasHeight;
            for (int xError = 0, xInput = 0; xError < deltasWidth; xError++, xInput += StepConvertionHieght)
            {
                var (maxElementX, maxElementY) = MaxElementsPooling[errorRow + xError];
                for (int yPoolingMatrix = 0; yPoolingMatrix < 2; yPoolingMatrix++)
                    for (int xPoolingMatrix = 0; xPoolingMatrix < 2; xPoolingMatrix++)
                    {
                        int y = yInput + yPoolingMatrix,
                            x = xInput + xPoolingMatrix;
                        if (maxElementX == x && maxElementY == y)
                            reCollapsMatrix[y, x] += deltas[yError, xError];
                    }
            }
        }
        ReСollapsedMatrix.SetMatrix(reCollapsMatrix);
    }
}
