
namespace CNN.Abstract;

internal abstract class NetworkComponent
{
    public double LearningRate { get; }
    public abstract void Learn<T>(T delta);
}
