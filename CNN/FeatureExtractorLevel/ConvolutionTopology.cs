using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNN.ConvolutionalLevel;

internal class ConvolutionTopology(int coreSize, int countLayers, int countMaps)
{
    //TODO: объединить в одну 3x матрицу, добавить learningRate -> убрать другие топологии
    public int CoreSize { get; } = coreSize;
    public int CountLayers { get; } = countLayers;
    public int CountMaps { get; } = countMaps;
}
