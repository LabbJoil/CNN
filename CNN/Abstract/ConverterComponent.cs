
using CNN.NeuralNetworkLevel;

namespace CNN.Abstract;

internal abstract class ConverterComponent(int stepHeight, int stepWidth)
{
    protected readonly MatrixHolder<double> InputMatrix = new("Input");
    protected readonly MatrixHolder<double> СollapsedMatrix = new("Collapsed");
    protected readonly MatrixHolder<double> ReСollapsedMatrix = new("ReCollapsed");
    protected int StepHieghtConvertion = stepHeight;
    protected int StepWidthConvertion = stepWidth;

    public double[,] СollapsedMatrixTable
    {
        get => СollapsedMatrix.MatrixTable;
    }

    public abstract void Сollapse(double[,] inputMatrix);
    public abstract void ReCollapse(double[,] deltas);
}
