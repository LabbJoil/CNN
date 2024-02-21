
namespace CNM.ConvolutionalLevel;

internal class ConvolutionalObject
{
    private double[,]? InputMatrix { get; set; }
    public double[,]? СollapsedMatrix { get; private set; }
    private double[,]? Core { get; }
    private ConvolutionalType Type { get; }

    public ConvolutionalObject(ConvolutionalType type, int? sizeCore = null)
    {
        Type = type;
        if (type == ConvolutionalType.Fold)
            Core = ConverterPicture.CreateCore(sizeCore ?? throw new Exception("The core size was not passed"));
    }

    public void Сollapse(double[,] inputMatrix)
    {
        InputMatrix = inputMatrix;
        if (Type == ConvolutionalType.Fold)
            СollapsedMatrix = ConverterPicture.ConvolutionMatrix(InputMatrix, Core ?? throw new Exception("Core not found"));
        else
            СollapsedMatrix = ConverterPicture.Puling(InputMatrix);
    }
}
