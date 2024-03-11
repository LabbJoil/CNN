using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNN.Model;

internal class ConvolutionLayerParams(int heightCore, int widthCore, int stepHeight, int stepWidth, int countMaps)
{
    public readonly int HeightCore = heightCore;
    public readonly int WidthCore = widthCore;
    public readonly int StepHeight = stepHeight;
    public readonly int StepWidth = stepWidth;
    public readonly int CountMaps = countMaps;
}
