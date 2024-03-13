
using CNN.Model.Params;
using CNN.NeuralNetworkLevel;

namespace CNN.Abstract;

internal abstract class ConverterComponent(ConverterComponentParams ccParams)
{
    protected readonly MatrixHolder<double> InputMatrix = new("Input", ccParams.InputMatrixHeight, ccParams.InputMatrixWidth);
    protected readonly MatrixHolder<double> СollapsedMatrix = new("Collapsed", ccParams.OutputMatrixHeight, ccParams.OutputMatrixWidth);
    protected readonly MatrixHolder<double> ReСollapsedMatrix = new("ReCollapsed", ccParams.InputMatrixHeight, ccParams.InputMatrixWidth);
    protected int StepConvertionHieght = ccParams.StepHeight;
    protected int StepConvertionWidth = ccParams.StepWidth;

    public double[,] СollapsedMatrixTable
    {
        get => СollapsedMatrix.MatrixTable;
    }

    public abstract void Сollapse(double[,] inputMatrix);
    public abstract void ReCollapse(double[,] deltas);
}
