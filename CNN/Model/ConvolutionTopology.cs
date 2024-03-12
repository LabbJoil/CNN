using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNN.Model;

internal class ConvolutionTopology(int heightImage, int widthImage, int maxInputNeurons)
{
    //TODO: добавить learningRate -> убрать другие топологии
    public int HeightImage { get; } = heightImage;
    public int WidthImage { get; } = widthImage;
    public int MaxInputNeurons { get; } = maxInputNeurons;
}
