
namespace CNM.ConvolutionalLevel;

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
        foreach (var convolutionalObjects in ConvolutionalObjects)
        {
            result.Add(convolutionalObjects.СollapsedMatrix ?? throw new Exception("Core not found"));
        }
        return result;
    }
}
