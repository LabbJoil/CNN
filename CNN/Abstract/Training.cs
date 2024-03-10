
namespace CNN.Abstract;

internal abstract class Training (double learningRate)
{
    public double LearningRate { get; } = learningRate;
    public abstract void Learn<T>(T delta);
}
