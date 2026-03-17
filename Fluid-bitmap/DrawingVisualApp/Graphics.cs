using System;

namespace DrawingVisualApp
{
    internal class Graphics
    {
        public static int Perlin2DSeed;

        public static double Noise(double x, double y = 0.0)
        {
            return new Perlin2D(Perlin2DSeed).Noise(x * 0.01, y * 0.01, 4);
        }


    }

    public class Perlin2D
    {
        private byte[] permutationTable;

        public Perlin2D(int seed = 0)
        {
            Random random = new Random(seed);
            permutationTable = new byte[1024];
            random.NextBytes(permutationTable);
        }

        private static double QunticCurve(double t)
        {
            return t * t * t * (t * (t * 6.0 - 15.0) + 10.0);
        }

        //
        // Summary:
        //     хэш-функция с Простыми числами, обрезкой результата до размера массива со случайными
        //     байтами
        //
        // Parameters:
        //   x:
        //
        //   y:
        private double[] GetPseudoRandomGradientVector(int x, int y)
        {
            unchecked
            {
                long yMultiplied = y * 2971215073u + 4807526976L;
                int xorResult = (x * 1836311903) ^ (int)yMultiplied;
                int num = xorResult & 0x3FF;

                int gradientIndex = permutationTable[num] & 3;

                switch (gradientIndex)
                {
                    case 0:
                        return new double[] { 1.0, 0.0 };
                    case 1:
                        return new double[] { -1.0, 0.0 };
                    case 2:
                        return new double[] { 0.0, 1.0 };
                    default:
                        return new double[] { 0.0, -1.0 };
                }
            }
        }

        //
        // Summary:
        //     Скалярное произведение векторов
        //
        // Parameters:
        //   a:
        //
        //   b:
        private static double Dot(double[] a, double[] b)
        {
            return a[0] * b[0] + a[1] * b[1];
        }

        //
        // Summary:
        //     Главный метод
        //
        // Parameters:
        //   x:
        //
        //   y:
        public double Noise(double fx, double fy)
        {
            int num = (int)Math.Floor(fx);
            int num2 = (int)Math.Floor(fy);
            double num3 = fx - (double)num;
            double num4 = fy - (double)num2;
            double[] pseudoRandomGradientVector = GetPseudoRandomGradientVector(num, num2);
            double[] pseudoRandomGradientVector2 = GetPseudoRandomGradientVector(num + 1, num2);
            double[] pseudoRandomGradientVector3 = GetPseudoRandomGradientVector(num, num2 + 1);
            double[] pseudoRandomGradientVector4 = GetPseudoRandomGradientVector(num + 1, num2 + 1);
            double[] a = new double[2] { num3, num4 };
            double[] a2 = new double[2]
            {
            num3 - 1.0,
            num4
            };
            double[] a3 = new double[2]
            {
            num3,
            num4 - 1.0
            };
            double[] a4 = new double[2]
            {
            num3 - 1.0,
            num4 - 1.0
            };
            double start = Dot(a, pseudoRandomGradientVector);
            double end = Dot(a2, pseudoRandomGradientVector2);
            double start2 = Dot(a3, pseudoRandomGradientVector3);
            double end2 = Dot(a4, pseudoRandomGradientVector4);
            num3 = QunticCurve(num3);
            num4 = QunticCurve(num4);
            double start3 = Lerp(start, end, num3);
            double end3 = Lerp(start2, end2, num3);
            return Lerp(start3, end3, num4);
        }

        public double Noise(double fx, double fy, int octaves, double persistence = 0.5)
        {
            double num = 1.0;
            double num2 = 0.0;
            double num3 = 0.0;
            while (octaves-- > 0)
            {
                num2 += num;
                num3 += Noise(fx, fy) * num;
                num *= persistence;
                fx *= 2.0;
                fy *= 2.0;
            }

            return num3 / num2;
        }

        /// <summary>
        /// Линейная интерполяция
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private double Lerp(double start, double end, double t)
        {
            // return start * (t - 1) + end * t; можно переписать с одним умножением (раскрыть скобки, взять в другие скобки):
            return start + (end - start) * t;
        }
        public static float Lerp(float a, float b, float t) => a + (b - a) * t;
    }

}
