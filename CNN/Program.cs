
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

        int countMaps = HeightImage / 20,
            convertionStep = 3,
            coreSize;   // TODO: переделать под height и width (определить наибольшую сторону и относительно её задавать размер 2 стороны через пропорцию)
        var (heightOutput, widthOutput) = (HeightImage, WidthImage);
        List<ConvolutionParams> convolutionParams = []; 

        for (int iteration = 0, multipleHWO = heightOutput * widthOutput; multipleHWO > MaxOutputConvolutionNeurons; iteration++, multipleHWO = heightOutput * widthOutput)
        {
            if (iteration % 2 == 0)
            {
                coreSize = (multipleHWO) switch
                {
                    > 400 => 7,
                    > 300 => 6,
                    > 200 => 5,
                    > 100 => 4,
                    _ => 3
                };

                double collapsedMatrixHeight, collapsedMatrixWidth;
                for (int step = coreSize; step > 1; step--)
                {
                    collapsedMatrixHeight = (double)(heightOutput - coreSize) / step + 1;
                    collapsedMatrixWidth = (double)(widthOutput - coreSize) / step + 1;   // INFO: coreSize - изменить на coreWidth
                    if (collapsedMatrixHeight % 1 != 0 || collapsedMatrixWidth % 1 != 0)
                    {
                        convertionStep = step;
                        break;
                    }
                }

                convolutionParams.Add(new(convertionStep));

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

    public void LoadWeights(string filePath)
    {

    }

    public void Predict(string filePath)
    {

    }
}
