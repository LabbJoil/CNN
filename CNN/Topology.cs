using CNN.ConnectedNeuralNetwork;
using CNN.ConvolutionalLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNN;

internal class Topology(ConvolutionTopology subsamplingLayer, double learningRate)
{
    public ConvolutionTopology SubsamplingLayer { get; } = subsamplingLayer;
    public double LearningRate { get; } = learningRate;
}
