
using CNM.ConnectedNeuralNetwork;
using CNN.Model.Topology;
using CNN.Model.Type;

namespace CNN.ConnectedNeuralNetwork;

internal class NeuronNetwork
{
    private NeuralNetworkTopology Topology { get; }
    private List<NeuralNetworkLayer> Layers { get; } = [];

    public NeuronNetwork(NeuralNetworkTopology topology)
    {
        Topology = topology;
        CreateInputLayer();
        CreateHiddenLayers();
        CreateOutputLayer();
    }

    public (double[], double[]) Backpropagation(double[] exprected, double[] inputs)
    {
        var outputNeurons = Predict(inputs);
        double[] differences = new double[outputNeurons.Length];

        for (int neuronIndex = 0; neuronIndex < outputNeurons.Length; neuronIndex++)
        {
            var difference = outputNeurons[neuronIndex].Output - exprected[neuronIndex];
            differences[neuronIndex] = (difference * difference);
            outputNeurons[neuronIndex].Learn(difference);
        }

        for (int j = Layers.Count - 2; j >= 0; j--)
        {
            var layer = Layers[j];
            var previousLayer = Layers[j + 1];

            for (int i = 0; i < layer.NeuronCount; i++)
            {
                var neuron = layer.NeuronsProperty[i];
                double neuronDelta = previousLayer.NeuronsProperty.Sum(n => n.Weights[i] * n.Delta);
                neuron.Learn(neuronDelta);
            }
        }
        var deltas = Layers.First().NeuronsProperty.Select(n => n.Delta).ToArray();
        return (deltas, differences);
    }

    public TempNeuron[] Predict(params double[] inputSignals)
    {
        SendInputsSignals(inputSignals);
        FeedForwardLayers();
        //return [.. Layers.Last().NeuronsProperty.OrderByDescending(n => n.Output)];
        return [.. Layers.Last().NeuronsProperty];
    }

    private void FeedForwardLayers()
    {
        for (int i = 1; i < Layers.Count; i++)
        {
            var layer = Layers[i];
            var previousLayerSingals = Layers[i - 1].GetSignals();

            foreach (var neuron in layer.NeuronsProperty)
                neuron.FeedForward(previousLayerSingals);
        }
    }

    private void SendInputsSignals(params double[] inputSignals)
    {
        for (int i = 0; i < inputSignals.Length; i++)
        {
            var signal = new List<double>() { inputSignals[i] };
            var neuron = Layers[0].NeuronsProperty[i];
            neuron.FeedForward(signal);
        }
    }

    private void CreateOutputLayer()
    {
        var outputNeurons = new List<TempNeuron>();
        var lastLayer = Layers.Last();
        for (int i = 0; i < Topology.OutputCount; i++)
        {
            var neuron = new TempNeuron(lastLayer.NeuronCount, Topology.LearningRate, NeuronType.Output);
            outputNeurons.Add(neuron);
        }
        var outputLayer = new NeuralNetworkLayer(outputNeurons, NeuronType.Output);
        Layers.Add(outputLayer);
    }

    private void CreateHiddenLayers()
    {
        for (int j = 0; j < Topology.HiddenLayers.Count; j++)
        {
            var hiddenNeurons = new List<TempNeuron>();
            var lastLayer = Layers.Last();
            for (int i = 0; i < Topology.HiddenLayers[j]; i++)
            {
                var neuron = new TempNeuron(lastLayer.NeuronCount, Topology.LearningRate);
                hiddenNeurons.Add(neuron);
            }
            var hiddenLayer = new NeuralNetworkLayer(hiddenNeurons);
            Layers.Add(hiddenLayer);
        }
    }

    private void CreateInputLayer()
    {
        var inputNeurons = new List<TempNeuron>();
        for (int i = 0; i < Topology.InputCount; i++)
        {
            var neuron = new TempNeuron(1, Topology.LearningRate, NeuronType.Input);
            inputNeurons.Add(neuron);
        }
        var inputLayer = new NeuralNetworkLayer(inputNeurons, NeuronType.Input);
        Layers.Add(inputLayer);
    }
}
