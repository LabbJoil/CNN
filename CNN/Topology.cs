using CNM.ConnectedNeuralNetwork;
using CNM.ConvolutionalLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNM;

internal class Topology
{
    public NeuralNetworkTopology NetworkTopology { get; }
    public ConvolutionTopology SubsamplingLayer { get; }
    public double LearningRate { get; }

    public Topology(NeuralNetworkTopology networkTopology, ConvolutionTopology subsamplingLayer, double learningRate)
    {
        NetworkTopology = networkTopology;
        SubsamplingLayer = subsamplingLayer;
        LearningRate = learningRate;
    }
}
