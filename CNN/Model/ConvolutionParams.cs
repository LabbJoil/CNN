using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNN.Model;

internal class ConvolutionParams(int heightCore, int widthCore, int convertionStep)
{
    public readonly int HeightCore = heightCore;
    public readonly int WidthCore = widthCore;
    public readonly int ConvertionStep = convertionStep;
}
