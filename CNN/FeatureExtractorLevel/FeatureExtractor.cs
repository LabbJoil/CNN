﻿
using CNN.ConvolutionalLevel;
using CNN.Model.Topology;
using System.Drawing;

namespace CNN.FeatureExtractorLevel;

internal partial class FeatureExtractor
{
    private FeatureExtractorTopology ConvolutionalTopology { get; }
    private List<ConvolutionLayer> ConvolutionalLayers { get; } = [];

    public FeatureExtractor(FeatureExtractorTopology topology)
    {
        GetConverterLayerParams(topology.HeightImage, topology.WidthImage, topology.MaxInputNeurons);
        ConvolutionalTopology = topology;
    }

    public double Learn(double[][] expected, double[][,] matrixImages, int epoch)
    {
        // TODO: входные параметры изменить на dictionary | скорее всего вынести логику выше, и использовать expected на NeuralNetwork
        var error = 0.0;
        for (int i = 0; i < epoch; i++)
            for (int j = 0; j < expected.GetLength(0); j++)
            {
                var output = expected[j];
                var input = matrixImages[j];
                error += Backpropagation(output, input);
            }
        var result = error / epoch;
        return result;
    }

    public List<double> CollapseImage(Bitmap bitmapImage)
    {
        SetInputLayer(bitmapImage);
        FeedForwardLayers();

        List<double> inputNeurons = []; // TODO: переделать в массив
        foreach (var convObject in ConvolutionalLayers.Last().ConvolutionalObjects)
        {
            var collapsedMatrix = convObject.СollapsedMatrixTable;
            for (int y = 0; y < collapsedMatrix.GetLength(0); y++)
                for (int x = 0; x < collapsedMatrix.GetLength(1); x++)
                    inputNeurons.Add(collapsedMatrix[y, x]);
        }
        return inputNeurons;
    }

    public void FeedForwardLayers()
    {
        for (int layerIndex = 1; layerIndex < ConvolutionalLayers.Count; layerIndex++)
        {
            var layer = ConvolutionalLayers[layerIndex];
            var previousLayer = ConvolutionalLayers[layerIndex - 1];
            for (int conObject = 0; conObject < ConvolutionalTopology.CountMaps; conObject++)
                layer.ConvolutionalObjects[conObject].Сollapse(previousLayer.ConvolutionalObjects[conObject].СollapsedMatrixTable);
        }
    }

    private double Backpropagation(double[] exprected, double[,] matrixImage)
    {

        //TODO: отдельный метод FeedForward
        var inputNeurons = Predict(matrixImage);

        //TODO: необходимо возвращать на уровень выше

        

        foreach (var convObject in ConvolutionalLayers.Last().ConvolutionalObjects)
        {
            var collapseMatrix = convObject.СollapsedMatrixTable;
            int height = collapseMatrix.GetLength(0);
            int width = collapseMatrix.GetLength(1);
            double[,] deltasMatrix = new double[height, width];

            for (int y = 0; y < height; y++)
            {
                var row = y * height;
                for (int x = 0; x < width; x++)
                    deltasMatrix[y, x] = deltas[row + x];
            }
            convObject.ReCollapse(deltasMatrix);
        }


        return 1; // INFO: затычка
    }

    
    private void SetInputLayer(double[,] matrixImage)
    {
        foreach(var conObject in ConvolutionalLayers.First().ConvolutionalObjects)
            conObject.Сollapse(matrixImage);
    }

    //private void CreateLayers()
    //{
    //    for (int i = 0; i < ConvolutionalTopology.CountLayers; i++)
    //    {
    //        ConverterComponent[] layer = new ConverterComponent[ConvolutionalTopology.CountMaps];
    //        if (i % 2 == 0)
    //            for (int j = 0; j < ConvolutionalTopology.CountMaps; j++)
    //                layer[j] = new Convolution(ConvolutionalTopology.CoreSize, 0.1); // INFO: 0.1 - затычка
    //        else
    //            for (int j = 0; j < ConvolutionalTopology.CountMaps; j++)
    //                layer[j] = new Pooling();
    //        ConvolutionalLayers.Add(new ConvolutionLayer(layer));
    //    }
    //}
}
