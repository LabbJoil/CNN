using CNN.Abstract;
using CNN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNN.ConvolutionLevel.Convolution;

internal class Pooling : ConverterComponent
{
    private (int, int)[]? MaxElementsPulling;

    public override void Сollapse(double[,] inputMatrix)
    {
        InputMatrix.SetMatrix(inputMatrix);
        PoolingMatrix();
    }

    public override void ReСollapse(double[,] deltas)
    {
        RePoolingMatrix(deltas);
    }

    private void PoolingMatrix()
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

    private void RePoolingMatrix(double[,] deltas)
    {
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
}
