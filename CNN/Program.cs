using CNM.ConnectedNeuralNetwork;
using CNM.ConvolutionalLevel;
using CNN.Model;

namespace CNM;

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


        Core oo = new(ConvolutionType.Fold, 0.1, 2);
        oo.Сollapse(jj);
        oo.ReCollapse(ii);
    }

    static void StartLearning(Dictionary<string, int> pathImageKeyMeanbase10Value, double learningRate, int countEpoch)
    {
        int maxMean = pathImageKeyMeanbase10Value.Max(kv => kv.Value);
        Dictionary<byte[,,], int[]> pathImageKeyMean2baseValue = [];
        foreach(var kv in pathImageKeyMeanbase10Value)
        {
            var preparedMatrix = ConverterPicture.(kv.Key);
            var masbase2 = new int[maxMean];
            masbase2[kv.Value - 1] = 1;
            pathImageKeyMean2baseValue[kv.Key] = masbase2;
        }

        ConvolutionTopology topology = new(3, 3, 2);
        Convolution convolution = new (topology);
        convolution.Learn();

        NeuralNetworkTopology neuralNetworkTopology = new(inputNeurons.Count, 3, 0.1, [inputNeurons.Count / 2, inputNeurons.Count / 2]);
        NeuronNetwork connectedNN = new(neuralNetworkTopology);
        var (deltas, differences) = connectedNN.Backpropagation(exprected, [.. inputNeurons]);
    }
}
