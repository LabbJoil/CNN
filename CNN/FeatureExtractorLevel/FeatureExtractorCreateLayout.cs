using CNN.FeatureExtractorLevel.Converter;
using CNN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNN.FeatureExtractorLevel;

internal partial class FeatureExtractor
{
    private (List<ConvertLayerParams>, int) GetConverterLayerParams(int heightImage, int widthImage, int maxInputNeurons)
    {
        int countMaps = heightImage / 20;
        var (matrixHeight, stepHeight) = GetConvolutionStepSize(heightImage);
        var (matrixWidth, stepWidth) = GetConvolutionStepSize(widthImage);
        List<ConvertLayerParams> convertLayerParams = [new(matrixHeight, matrixWidth, stepHeight, stepWidth, countMaps)];
        int multipleHWO = matrixHeight * matrixWidth;

        for (int iteration = 1; multipleHWO > maxInputNeurons; iteration++)
        {
            ConvertLayerParams newLayerParams;
            int newMatrixHeight, newMatrixWidth;
            if (iteration % 2 == 0)
            {
                (newMatrixHeight, stepHeight) = GetConvolutionStepSize(matrixHeight);
                (newMatrixWidth, stepWidth) = GetConvolutionStepSize(matrixWidth);
                //ConvolutionalLayers.Add(new Convolution(new(matr), 0.1));   // INFO: 0.1 - затычка | TODO: карты на следующем слое должны подбираться 
                newLayerParams = new(matrixHeight, matrixWidth, stepHeight, stepWidth, 1);
            }
            else
            {
                (newMatrixHeight, stepHeight) = GetPoolingStepSize(matrixHeight);
                (newMatrixWidth, stepWidth) = GetPoolingStepSize(matrixWidth);
                newLayerParams = new(matrixHeight, matrixWidth, stepHeight, stepWidth, 1);
            }
            convertLayerParams.Add(newLayerParams);
            (matrixHeight, matrixWidth) = (newMatrixHeight, newMatrixWidth);
            multipleHWO = matrixHeight * matrixWidth;
        }
        return (convertLayerParams, multipleHWO * countMaps);
    }

    private static (int, int) GetPoolingStepSize(int size)
    {
        int step = size % 2 == 0 ? 2 : 1;
        int newSize = (size - 2) % step + 1;
        return (step, newSize);
    }

    private static (int, int) GetConvolutionStepSize(int size)
    {
        var coreSize = (size) switch
        {
            > 400 => 7,
            > 300 => 6,
            > 200 => 5,
            > 100 => 4,
            _ => 3
        };

        double newSize = -1;
        int step = -1;
        for (int maybeStep = coreSize - 1; maybeStep > 0; maybeStep--)
        {
            newSize = (double)(size - coreSize) / maybeStep + 1;
            if (newSize % 1 != 0)
            {
                step = maybeStep;
                break;
            }
        }
        return ((int)newSize, step);
    }
}
