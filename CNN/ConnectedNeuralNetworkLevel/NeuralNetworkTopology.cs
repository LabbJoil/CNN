using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNM.ConnectedNeuralNetwork;

internal class NeuralNetworkTopology
{
    public int InputCount { get; }
    public int OutputCount { get; }
    public double LearningRate { get; }
    public List<int> HiddenLayers { get; }

    public NeuralNetworkTopology(int inputCount, int outputCount, double learningRate, int[] layers)
    {
        InputCount = inputCount;
        OutputCount = outputCount;
        LearningRate = learningRate;
        HiddenLayers = [.. layers];
    }
}
