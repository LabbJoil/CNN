
namespace CNM;

internal class Activation
{
    public static double Sigmoid(double x)
    {
        var result = 1.0 / (1.0 + Math.Pow(Math.E, -x));
        return result;
    }

    public static double SigmoidDx(double x)
    {
        var sigmoid = Sigmoid(x);
        var result = sigmoid / (1 - sigmoid);
        return result;
    }
}
