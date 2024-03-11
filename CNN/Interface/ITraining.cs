
namespace CNN.Interface;

internal interface ITraining
{
    public abstract void Learn<T>(T delta);
}
