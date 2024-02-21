
namespace CNM.ConnectedNeuralNetwork;

internal class Neuron
{
    public List<double> Weights { get; } = [];
    public List<double> Inputs { get; } = [];
    public NeuronType NeuronType { get; }
    public double Output { get; private set; }
    public double Delta { get; private set; }

    public Neuron(int inputCount, NeuronType type = NeuronType.Hidden)
    {
        NeuronType = type;
        InitWeightsRandomValue(inputCount);
    }

    private void InitWeightsRandomValue(int inputCount)
    {
        var rand = new Random();

        for (int i = 0; i < inputCount; i++)
        {
            if (NeuronType == NeuronType.Input)
            {
                Weights.Add(1);
            }
            else
            {
                Weights.Add(rand.NextDouble());
            }
            Inputs.Add(0);
        }
    }

    public double FeedForward(List<double> inputs)
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            Inputs[i] = inputs[i];
        }

        var sum = 0.0;
        for (int i = 0; i < inputs.Count; i++)
        {
            sum += inputs[i] * Weights[i];
        }

        if (NeuronType != NeuronType.Input)
        {
            Output = Activation.Sigmoid(sum);
        }
        else
        {
            Output = sum;
        }

        return Output;
    }

    public void Learn(double error, double learningRate)
    {
        if (NeuronType == NeuronType.Input)
        {
            return;
        }

        Delta = error * Activation.SigmoidDx(Output);

        for (int i = 0; i < Weights.Count; i++)
        {
            var weight = Weights[i];
            var input = Inputs[i];

            var newWeigth = weight - input * Delta * learningRate;
            Weights[i] = newWeigth;
        }
    }
}
