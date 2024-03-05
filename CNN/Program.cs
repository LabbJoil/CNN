using CNM.ConvolutionalLevel;
using CNN.Model;

namespace CNM;

internal class Program
{
    static void Main()
    {
        //var answer = ConverterPicture.Convert(@"C:\Users\levt1\Desktop\PAIS assets\first.jpg");

        double[,] jj = new double[4, 4]{
            { 1, 2, 3, 4 },
            { 5, 6, 7, 8 },
            { 9, 10, 11, 12 },
            { 13, 14, 15, 16 }
        };

        double[,] ii = new double[2, 2]{
            { 1, 2 },
            { 5, 6 }
        };


        //double[,] jj = new double[5, 5]{
        //    { 1, 2, 3, 4, 20 },
        //    { 5, 6, 7, 8, 21 },
        //    { 9, 10, 11, 12, 22 },
        //    { 13, 14, 15, 16, 23 },
        //    { 16, 17, 18, 19, 24 },
        //};

        //double[,] ii = new double[4, 4]{
        //    { 1, 2, 3, 4 },
        //    { 5, 6, 7, 8},
        //    { 9, 10, 11, 12},
        //    {13, 14, 15, 16 }
        //};


        Core oo = new(ConvolutionType.Pulling);
        oo.Сollapse(jj);
        oo.ReCollapse(ii);
    }
}
