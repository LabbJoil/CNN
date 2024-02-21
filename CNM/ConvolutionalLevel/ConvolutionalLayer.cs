
namespace CNM.ConvolutionalLevel;

internal class ConvolutionalLayer
{
    public ConvolutionalObject[] ConvolutionalObjects { get; }

    public ConvolutionalLayer(ConvolutionalObject[] convolutionalObjects)
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
