using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNN.Model.Params;

internal class ConverterComponentParams
{
    public readonly int InputMatrixHeight;
    public readonly int InputMatrixWidth ;
    public readonly int OutputMatrixHeight;
    public readonly int OutputMatrixWidth;
    public readonly int StepHeight;
    public readonly int StepWidth;
    public readonly int CountMaps;

    public ConverterComponentParams(ConverterComponentParams newParams)
    {
        InputMatrixHeight = newParams.InputMatrixHeight;
        InputMatrixWidth = newParams.InputMatrixWidth;
        OutputMatrixHeight = newParams.OutputMatrixHeight;
        OutputMatrixWidth = newParams.OutputMatrixWidth;
        StepHeight = newParams.StepHeight;
        StepWidth = newParams.StepWidth;
        CountMaps = newParams.CountMaps;
    }

    public ConverterComponentParams(int inputMatrixHeight, int inputMatrixWidth, int outputMatrixHeight, int outputMatrixWidth, int stepHeight, int stepWidth, int countMaps)
    {
        InputMatrixHeight = inputMatrixHeight;
        InputMatrixWidth = inputMatrixWidth;
        OutputMatrixHeight = outputMatrixHeight;
        OutputMatrixWidth = outputMatrixWidth;
        StepHeight = stepHeight;
        StepWidth = stepWidth;
        CountMaps = countMaps;
    }
}
