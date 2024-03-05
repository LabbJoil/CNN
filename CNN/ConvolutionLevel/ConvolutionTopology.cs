using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNM.ConvolutionalLevel;

internal class ConvolutionTopology
{
    //TODO: сделать абстрактеый класс с learningRate и другим дерьмом для этого и layer классов
    public int CoreSize { get; }
    public int CountLayers { get; }
    public int CountMaps { get; }

    public ConvolutionTopology(int convolutionalMatrixSize, int countLayers, int countMaps)
    {
        CoreSize = convolutionalMatrixSize;
        CountLayers = countLayers;
        CountMaps = countMaps;
    }
}
