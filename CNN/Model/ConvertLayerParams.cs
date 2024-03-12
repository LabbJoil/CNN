using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNN.Model;

internal class ConvertLayerParams(int coreheight, int coreWidth, int stepHeight, int stepWidth, int countMaps)
{
    public readonly int CoreHeight = coreheight;
    public readonly int CoreWidth = coreWidth;
    public readonly int StepHeight = stepHeight;
    public readonly int StepWidth = stepWidth;
    public readonly int CountMaps = countMaps;
}
