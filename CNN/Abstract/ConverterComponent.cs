
using CNN.NeuralNetworkLevel;

namespace CNN.Abstract;

internal abstract class ConverterComponent
{
    protected readonly MatrixHolder<double> InputMatrix = new("Input");
    protected readonly MatrixHolder<double> СollapsedMatrix = new("Collapsed");
    protected readonly MatrixHolder<double> ReСollapsedMatrix = new("ReCollapsed");
    protected int CollapseStep = 1;

    public double[,] СollapsedMatrixTable
    {
        get => СollapsedMatrix.MatrixTable;
    }

    public abstract void Сollapse(double[,] inputMatrix);
    public abstract void ReСollapse(double[,] deltas);
}
