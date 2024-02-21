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
                layer.ConvolutionalObjects[conObject].Сollapse(previousLayer.ConvolutionalObjects[conObject].СollapsedMatrix ?? throw new Exception("Ошибка при поиске матрицы прошлого уровня"));
        }
        var lastLayer = ConvolutionalLayers.Last();
        List<double> inputNeurons = [];
        foreach(var convObject in lastLayer.ConvolutionalObjects)
        {
            double[,] collapsedMatrix = convObject.СollapsedMatrix ?? throw new Exception("Ошибка при поиске матрицы выходного уровня");
            for (int y = 0; y < collapsedMatrix.GetLength(0); y++)
            {
                for (int x = 0; x < collapsedMatrix.GetLength(1); x++)
                {
                    inputNeurons.Add(collapsedMatrix[y, x]);
                }
            }
        }
        NeuralNetworkTopology neuralNetworkTopology = new(inputNeurons.Count, 3, 0.1, [inputNeurons.Count / 2, inputNeurons.Count / 2]);
        ConnectedNeuronNetwork connectedNN = new(neuralNetworkTopology);
        var errors = connectedNN.Backpropagation(exprected, [.. inputNeurons]);

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
            ConvolutionalType type = i % 2 == 0 ? ConvolutionalType.Fold : ConvolutionalType.Padding;
            ConvolutionalObject[] layer = new ConvolutionalObject[ConvolutionalTopology.CountMaps];
            for (int j = 0; j < ConvolutionalTopology.CountMaps; j++)
            {
                if (type == ConvolutionalType.Fold)
                    layer[j] = new ConvolutionalObject(type, ConvolutionalTopology.CoreSize);
            }
            ConvolutionalLayers.Add(new ConvolutionalLayer(layer));
        }
    }
   
    private void SendToConnectedNeuronNetwork()
    {
        //NeuralNetworkTopology neuralNetworkTopology = new();
    }

}
