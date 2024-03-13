using CNN.Interface;
using CNN.Model.Type;

namespace CNN.ConnectedNeuralNetwork;

internal class TempNeuron : ITraining
{
    public List<double> Weights { get; } = [];
    public List<double> Inputs { get; private set; } = [];
    public NeuronType NeuronType { get; }
    public double Output { get; private set; }
    public double Delta { get; private set; }

    public TempNeuron(int inputCount, double learningRate, NeuronType type = NeuronType.Hidden)
    {
        NeuronType = type;
        InitWeightsRandomValue(inputCount);
    }

    private void InitWeightsRandomValue(int inputCount)
    {
        var rand = new Random();
        Inputs = new(inputCount);
        for (int i = 0; i < inputCount; i++)
        {
            if (NeuronType == NeuronType.Input)
                Weights.Add(1);
            else
                Weights.Add(rand.NextDouble());
        }
    }

    public double FeedForward(List<double> inputs)
    {
        for (int i = 0; i < inputs.Count; i++)
            Inputs[i] = inputs[i];

        double sum = 0;
        for (int i = 0; i < inputs.Count; i++)
            sum += inputs[i] * Weights[i];

        if (NeuronType != NeuronType.Input)
            Output = Activation.Sigmoid(sum);
        else
            Output = sum;

        return Output;
    }

    public void Learn<T>(T deltaT)
    {
        if (deltaT is double delta)
        {
            Delta = delta;

            if (NeuronType == NeuronType.Input)
                return;

            for (int i = 0; i < Weights.Count; i++)
            {
                var weight = Weights[i];
                var input = Inputs[i];
                var newWeigth = weight - LearningRate * Delta * Activation.SigmoidDx(Output) * input; // TODO: maybe minus
                Weights[i] = newWeigth;
            }
        }
        else
            throw new InvalidOperationException("Unsupported type conversion");
    }
}
