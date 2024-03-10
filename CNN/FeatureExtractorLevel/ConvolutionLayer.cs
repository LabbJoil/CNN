
namespace CNN.ConvolutionalLevel;

internal class ConvolutionLayer
{
    public Core[] ConvolutionalObjects { get; }

    public ConvolutionLayer(Core[] convolutionalObjects)
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
