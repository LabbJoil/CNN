using CNN.ConnectedNeuralNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNN.Model;

internal class Topology(ConvolutionTopology subsamplingLayer, double learningRate)
{
    public ConvolutionTopology SubsamplingLayer { get; } = subsamplingLayer;
    public double LearningRate { get; } = learningRate;
}
