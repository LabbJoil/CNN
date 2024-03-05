using CNM.ConnectedNeuralNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNM.ConvolutionalLevel;

internal class ConvolutionalNeuronNetwork
{
    public ConvolutionalTopology ConvolutionalTopology { get; }
    private List<ConvolutionalLayer> ConvolutionalLayers { get; } = [];

    public ConvolutionalNeuronNetwork(ConvolutionalTopology topology)
    {
        ConvolutionalTopology = topology;
        CreateLayers();
    }

    public double Learn(double[,] expected, double[][,] inputeMatrixImages, int epoch)
    {
        var error = 0.0;
        for (int i = 0; i < epoch; i++)
        {
            for (int j = 0; j < expected.GetLength(0); j++)
            {
                var output = ConverterPicture.GetRow(expected, j);
                var input = inputeMatrixImages[j];
                error += Backpropagation(output, input);
            }
        }
        var result = error / epoch;
        return result;
    }

    private double Backpropagation(double[] exprected, double[,] matrixImage)
    {
        SetInputLayer(matrixImage);
        for (int layerIndex = 1; layerIndex < ConvolutionalLayers.Count; layerIndex++)
        {
            var layer = ConvolutionalLayers[layerIndex];
            var previousLayer = ConvolutionalLayers[layerIndex - 1];
            for (int conObject = 0; conObject < ConvolutionalTopology.CountMaps; conObject++)
                layer.ConvolutionalObjects[conObject].Сollapse(previousLayer.ConvolutionalObjects[conObject].СollapsedMatrixProperty);
        }

        var lastLayer = ConvolutionalLayers.Last();
        List<double> inputNeurons = [];
        foreach(var convObject in lastLayer.ConvolutionalObjects)
        {
            var collapsedMatrix = convObject.СollapsedMatrixProperty;
            for (int y = 0; y < collapsedMatrix.GetLength(0); y++)
                for (int x = 0; x < collapsedMatrix.GetLength(1); x++)
                    inputNeurons.Add(collapsedMatrix[y, x]);
        }

        //TODO: необходимо возвращать на уровень выше

        NeuralNetworkTopology neuralNetworkTopology = new(inputNeurons.Count, 3, 0.1, [inputNeurons.Count / 2, inputNeurons.Count / 2]);
        ConnectedNeuronNetwork connectedNN = new(neuralNetworkTopology);
        var (deltas, differences) = connectedNN.Backpropagation(exprected, [.. inputNeurons]);

        foreach (var convObject in lastLayer.ConvolutionalObjects)
        {
            var collapseMatrix = convObject.СollapsedMatrixProperty;
            int height = collapseMatrix.GetLength(0);
            int width = collapseMatrix.GetLength(1);
            double[,] deltasMatrix = new double[height, width];

            for (int y = 0; y < height; y++)
            {
                var row = y * height;
                for (int x = 0; x < width; x++)
                    deltasMatrix[y, x] = deltas[row + x];
            }
            convObject.ReCollapse(deltasMatrix, ConvolutionalTopology.jjj);
        }


        return 1; // TODO: затычка
    }

    
    private void SetInputLayer(double[,] matrixImage)
    {
        foreach(var conObject in ConvolutionalLayers.First().ConvolutionalObjects)
            conObject.Сollapse(matrixImage);
    }

    private void CreateLayers()
    {
        for (int i = 0; i < ConvolutionalTopology.CountLayers; i++)
        {
            ConvolutionalType type = i % 2 == 0 ? ConvolutionalType.Fold : ConvolutionalType.Pulling;
            ConvolutionalObject[] layer = new ConvolutionalObject[ConvolutionalTopology.CountMaps];
            for (int j = 0; j < ConvolutionalTopology.CountMaps; j++)
            {
                if (type == ConvolutionalType.Fold)
                    layer[j] = new ConvolutionalObject(type, ConvolutionalTopology.CoreSize);
            }
            ConvolutionalLayers.Add(new ConvolutionalLayer(layer));
        }
    }
}
