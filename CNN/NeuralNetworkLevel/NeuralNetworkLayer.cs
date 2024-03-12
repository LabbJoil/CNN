using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNN.Model;

namespace CNN.ConnectedNeuralNetwork;

internal class NeuralNetworkLayer
{
    public List<TempNeuron> NeuronsProperty { get => new(Neurons); }
    public int NeuronCount => NeuronsProperty?.Count ?? 0;

    public NeuronType Type;
    private readonly List<TempNeuron> Neurons;

    public NeuralNetworkLayer(List<TempNeuron> neurons, NeuronType type = NeuronType.Hidden)
    {
        // TODO: проверить все входные нейроны на соответствие типу

        Neurons = neurons;
        Type = type;
    }

    public List<double> GetSignals()
    {
        var result = new List<double>();
        foreach (var neuron in NeuronsProperty)
        {
            result.Add(neuron.Output);
        }
        return result;
    }
}
