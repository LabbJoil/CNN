
using CNM.ConnectedNeuralNetwork;
using CNN.ConnectedNeuralNetwork;
using CNN.ConvolutionalLevel;
using CNN.Model;
using System.Drawing;

namespace CNN;

internal class Program
{
    static void Main()
    {
        //var answer = ConverterPicture.Convert(@"C:\Users\levt1\Desktop\PAIS assets\first.jpg");

        double[,] jj = new double[4, 4]{
            { 1, 2, 3, 4 },
            { 5, 6, 7, 8 },
            { 9, 10, 11, 12 },
            { 13, 14, 15, 16 }
        };

        double[,] ii = new double[2, 2]{
            { 1, 2 },
            { 5, 6 }
        };


        //double[,] jj = new double[5, 5]{
        //    { 1, 2, 3, 4, 20 },
        //    { 5, 6, 8, 7, 21 },
        //    { 9, 10, 11, 12, 22 },
        //    { 13, 14, 15, 16, 23 },
        //    { 16, 17, 18, 19, 24 },
        //};

        //double[,] ii = new double[4, 4]{
        //    { 1, 2, 3, 4 },
        //    { 5, 6, 7, 8},
        //    { 9, 10, 11, 12},
        //    {13, 14, 15, 16 }
        //};


        //Core oo = new(ConvolutionType.Fold, 0.1, 2);
        //oo.Сollapse(jj);
        //oo.ReCollapse(ii);
    }

    private const int MaxOutputConvolutionNeurons = 20;
    private const int HeightImage = 216;    // INFO: maybe среднее значение по всем картинкам | пользователь сам задаёт
    private const int WidthImage = 216;

    public void StartLearning(Dictionary<string, int> pathImageKeyMeanbase10Value, double learningRate, int countEpoch)
    {
        int maxMean = pathImageKeyMeanbase10Value.Max(kv => kv.Value);
        Dictionary<Bitmap, int[]> pathImageKeyMean2baseValue = [];

        foreach(var kv in pathImageKeyMeanbase10Value)
        {
            var bitmapImage = new Bitmap(new Bitmap(kv.Key), new Size(HeightImage, WidthImage));    // INFO: maybe проверка на размер (не меньше 48p)
            var masbase2 = new int[maxMean];
            masbase2[kv.Value - 1] = 1;
            pathImageKeyMean2baseValue[bitmapImage] = masbase2;
        }

        int countMaps = HeightImage / 20;
        var (heightOutput, widthOutput) = (HeightImage, WidthImage);
        List<ConvolutionLayerParams> convolutionParams = []; 

        for (int iteration = 0, multipleHWO = heightOutput * widthOutput; multipleHWO > MaxOutputConvolutionNeurons; iteration++, multipleHWO = heightOutput * widthOutput)
        {
            if (iteration % 2 == 0)
            {
                var (height, stepHeight) = GetStepSize(heightOutput);
                var (width, stepWidth) = GetStepSize(widthOutput);

                convolutionParams.Add(new(height, width, stepHeight, stepWidth));

            }
            
        }


        ConvolutionTopology convolutionTopology = new(3, 3, 2);
        NeuralNetworkTopology neuralNetworkTopology = new(inputNeurons.Count, 3, 0.1, [inputNeurons.Count / 2, inputNeurons.Count / 2]);

        FeatureExtractor featureExtractor = new(convolutionTopology);
        NeuronNetwork neuronNetwork = new(neuralNetworkTopology);

        for (int epoch = 0; epoch < countEpoch; epoch++)
        {
            foreach(var bi in pathImageKeyMean2baseValue)
            {
                var inputNeurons = featureExtractor.CollapseImage(bi.Key);
            }
        }


        
        var (deltas, differences) = neuronNetwork.Backpropagation(exprected, [.. inputNeurons]);
    }

    private static (int, int) GetStepSize(int size)
    {
        var coreSize = (size) switch
        {
            > 400 => 7,
            > 300 => 6,
            > 200 => 5,
            > 100 => 4,
            _ => 3
        };

        double newSize = -1;
        int step = -1;
        for (int maybeStep = coreSize - 1; maybeStep > 1; maybeStep--)
        {
            newSize = (double)(size - coreSize) / maybeStep + 1;
            if (newSize % 1 != 0)
            {
                step = maybeStep;
                break;
            }
        }
        return ((int)newSize, step);
    }

    public void LoadWeights(string filePath)
    {

    }

    public void Predict(string filePath)
    {

    }
}
