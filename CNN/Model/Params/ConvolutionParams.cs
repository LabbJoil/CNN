using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNN.Model.Params;

internal class ConvolutionParams(int coreheight, int coreWidth, ConverterComponentParams ccParams) : ConverterComponentParams(ccParams)
{
    public readonly int CoreHeight = coreheight;
    public readonly int CoreWidth = coreWidth;
}
