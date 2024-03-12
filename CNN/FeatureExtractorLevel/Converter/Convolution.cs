
using CNN.Abstract;
using CNN.Interface;
using CNN.Model;
using CNN.NeuralNetworkLevel;

namespace CNN.FeatureExtractorLevel.Converter;

internal class Convolution : ConverterComponent, ITraining
{
    private readonly MatrixHolder<double> CoreMatrix = new("Core");
    private readonly Pooling[] PoolingLayer;

    public Convolution(ConvertLayerParams convertParams) : base(convertParams.StepHeight, convertParams.StepWidth)  // TODO maybe: задавать learningRate статично
    {
        PoolingLayer = new Pooling[convertParams.CountMaps];
        var core = CreateCore(convertParams.CoreHeight, convertParams.CoreWidth);
        CoreMatrix.SetMatrix(core);
    }

    public override void Сollapse(double[,] inputMatrix)
    {
        InputMatrix.SetMatrix(inputMatrix);
        ConvolutionMatrix();
    }

    public override void ReCollapse(double[,] deltas)
    {
        ReConvolutionMatrix(deltas);
        Learn(deltas);
    }

    private void ConvolutionMatrix()
    {
        //TODO: Проверка demo
        var (inputmatrix, heightInputeMatrix, widthInputeMatrix) = InputMatrix.MatrixData;
        var (core, heightCore, widthCore) = CoreMatrix.MatrixData;
        double collapsedMatrixHeight = (double)(heightInputeMatrix - heightCore) / StepHieghtConvertion + 1,
            collapsedMatrixWidth = (double)(widthInputeMatrix - widthCore) / StepHieghtConvertion + 1;

        if (collapsedMatrixHeight % 1 != 0 || collapsedMatrixWidth % 1 != 0)
            throw new Exception($"The step {StepHieghtConvertion} is not suitable for the matrix {heightInputeMatrix}x{widthInputeMatrix}");

        double[,] collapsedMatrix = new double[(int)collapsedMatrixHeight, (int)collapsedMatrixWidth];

        for (int yInputMatrix = 0; yInputMatrix <= collapsedMatrixHeight; yInputMatrix += StepHieghtConvertion)
            for (int xInputMatrix = 0; xInputMatrix <= collapsedMatrixWidth; xInputMatrix += StepHieghtConvertion)
            {
                double sum = 0;
                for (int yCore = 0; yCore < heightCore; yCore++)
                    for (int xCore = 0; xCore < widthCore; xCore++)
                        sum += inputmatrix[yInputMatrix + yCore, xInputMatrix + xCore] * core[yCore, xCore];
                collapsedMatrix[yInputMatrix, xInputMatrix] = sum;
            }
        СollapsedMatrix.SetMatrix(collapsedMatrix);
    }

    private void ReConvolutionMatrix(double[,] deltas)
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

        for (int yConverMatrix = 0; yConverMatrix < deltas.GetLength(0); yConverMatrix += StepHieghtConvertion)
            for (int xConverMatrix = 0; xConverMatrix < deltas.GetLength(1); xConverMatrix += StepHieghtConvertion)
            {
                double sum = 0;
                for (int yCore = 0; yCore < heightCore; yCore++)
                    for (int xCore = 0; xCore < widthCore; xCore++)
                        sum += deltas[yConverMatrix + yCore, xConverMatrix + xCore] * core[yCore, xCore];
                reСollapsedMatrix[yConverMatrix, xConverMatrix] = sum;
            }
        ReСollapsedMatrix.SetMatrix(reСollapsedMatrix);
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
        double[,] newConvertMatrix = new double[convertMatrixHeight, convertMatrixWidth + 2];

        for (int y = 0; y < convertMatrixHeight; y++)
            for (int x = 0; x < convertMatrixWidth; x++)
            {
                if (x == 0 || x == convertMatrixWidth - 1)
                    newConvertMatrix[y, x] = 0;
                newConvertMatrix[y, x] = convertMatrix[y - 1, x - 1];
            }
        return newConvertMatrix;
    }

    public void Learn<T>(T delta)
    {
        if (delta is double[,] deltas)
        {
            var (core, heightCore, widthCore) = CoreMatrix.MatrixData;
            for (int y = 0; y < heightCore; y++)
                for (int x = 0; x < widthCore; x++)
                {
                    var weight = core[y, x];
                    var newWeigth = weight - LearningRate * deltas[y, x]; // TODO: maybe plus
                    core[y, x] = newWeigth;
                }
            CoreMatrix.SetMatrix(core);
        }
        else
            throw new Exception("Unsupported type conversion");
    }

    private static double[,] CreateCore(int width, int height)
    {
        Random rand = new();
        var core = new double[height, width];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                core[y, x] = rand.NextDouble() - 0.5;
        return core;
    }
}
