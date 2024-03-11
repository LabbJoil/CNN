
using CNN.Abstract;

namespace CNN.ConvolutionalLevel;

internal class ConvolutionLayer
{
    public ConverterComponent[] ConvolutionalObjects { get; }

    public ConvolutionLayer(ConverterComponent[] convolutionalObjects)
    {
        ConvolutionalObjects = convolutionalObjects;
    }

    public List<double[,]> GetSignals()
    {
        var result = new List<double[,]>();
        foreach (var convolutionalObject in ConvolutionalObjects)
            result.Add(convolutionalObject.СollapsedMatrixTable);
        return result;
    }
}
