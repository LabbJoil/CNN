namespace CNN.NeuralNetworkLevel;

internal class MatrixHolder<T>(string name)
{
    private T[,]? Matrix;
    private int Height;
    private int Width;
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
        Matrix = matrix;
        Height = matrix.GetLength(0);
        Width = matrix.GetLength(1);
    }
}
