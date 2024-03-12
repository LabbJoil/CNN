
using CNN.ConnectedNeuralNetwork;
using CNN.ConvolutionalLevel;
using CNN.FeatureExtractorLevel;
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

    private readonly Dictionary<string, int> PathImageValuePairs;
    private readonly int MaxOutputConvolutionNeurons = 100;
    private readonly int HeightImage = 216;    // INFO: maybe среднее значение по всем картинкам | пользователь сам задаёт
    private readonly int WidthImage = 216;

    public Program(Dictionary<string, int> pathImageValuePairs)
    {
        PathImageValuePairs = pathImageValuePairs;
    }

    public void StartLearning(double learningRate, int countEpoch)
    {
        int countOutputNeurons = PathImageValuePairs.Max(kv => kv.Value);
        Dictionary<Bitmap, int[]> bitmapImage2baseValuePairs = [];

        foreach(var kv in PathImageValuePairs)
        {
            var bitmapImage = new Bitmap(new Bitmap(kv.Key), new Size(HeightImage, WidthImage));    // INFO: maybe проверка на размер (не меньше 48p)
            var masbase2 = new int[countOutputNeurons];
            masbase2[kv.Value - 1] = 1;
            bitmapImage2baseValuePairs[bitmapImage] = masbase2;
        }


        FeatureExtractor featureExtractor = new(new(HeightImage, WidthImage));

        //var (convertLayersParam, countInputNeurons) = GetConverterLayerParams();
        //int countNeuronLayers = convertLayersParam.Count;

        //int[] neuronLayers = new int[countNeuronLayers];
        //for (int i = 0, multipleDigit = countNeuronLayers; i < countNeuronLayers; i++, multipleDigit--)
        //    neuronLayers[i] = countInputNeurons * multipleDigit;

        //NeuralNetworkTopology neuralNetworkTopology = new(countInputNeurons, countOutputNeurons, learningRate, neuronLayers);

        //NeuronNetwork neuronNetwork = new(neuralNetworkTopology);

        //for (int epoch = 0; epoch < countEpoch; epoch++)
        //{
        //    foreach(var bi in bitmapImage2baseValuePairs)
        //    {
        //        var inputNeurons = featureExtractor.CollapseImage(bi.Key);
        //    }
        //}



        //var (deltas, differences) = neuronNetwork.Backpropagation(exprected, [.. inputNeurons]);
    }

    

    public void LoadWeights(string filePath)
    {

    }

    public void Predict(string filePath)
    {

    }
}
