using System.Xml.Linq;

namespace CNN.NeuralNetworkLevel;

internal class MatrixHolder<T>(string name, int height, int width)
{
    private T[,]? Matrix;
    private readonly int Height = height;
    private readonly int Width = width;
    private readonly string Name = name;

    public (T[,], int, int) MatrixData
    {
        get => (Matrix ?? throw new Exception($"{Name} matrix not found"),
            Height, Width);
    }

    public T[,] MatrixTable
    {
        get => Matrix ?? throw new Exception($"{Name} matrix not found");
    }

    public (int, int) MatrixSizes
    {
        get => Matrix == null ? throw new Exception($"{Name} matrix not found")
            : (Height, Width);
    }

    public void SetMatrix(T[,] matrix)
    {
        int height = matrix.GetLength(0),
            width = matrix.GetLength(1);
        if (height != Height)
            throw new Exception($"Высота матрицы не совпадает с изначальными расчётами. Расчёт - <{Height}> Вход - <{height}>");
        if (width != Width)
            throw new Exception($"Ширина матрицы не совпадает с изначальными расчётами. Расчёт - <{Width}> Вход - <{width}>");
        Matrix = matrix;
    }
}
