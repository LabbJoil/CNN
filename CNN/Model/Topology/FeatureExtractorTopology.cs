namespace CNN.Model.Topology;

internal class FeatureExtractorTopology(int heightImage, int widthImage, int maxInputNeurons)
{
    public int HeightImage { get; } = heightImage;
    public int WidthImage { get; } = widthImage;
    public int MaxInputNeurons { get; } = maxInputNeurons;
}
