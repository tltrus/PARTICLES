using System;


namespace DrawingVisualApp
{
    public static class Randoms
    {
        public static Random rnd = new Random();

        public static double[,] Random(double[][,] val)
        {
            int len = val.Length;
            double[,] result = val[rnd.Next(len)];
            return result;
        }

        public static double[,] Random(int rows, int cols)
        {
            double[,] result = new double[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[i, j] = rnd.NextDoubleRange(-3, 3);

            return result;
        }

        public static double NextDoubleRange(this System.Random random, double minNumber, double maxNumber)
        {
            return random.NextDouble() * (maxNumber - minNumber) + minNumber;
        }
    }
}
