namespace CNN.Model.Topology;

internal class NeuralNetworkTopology(int inputCount, int outputCount, double learningRate, int[] layers)
{
    public int InputCount { get; } = inputCount;
    public int OutputCount { get; } = outputCount;
    public double LearningRate { get; } = learningRate; // TODO: вынести
    public List<int> HiddenLayers { get; } = [.. layers];   // TODO: переделать в массив
}
