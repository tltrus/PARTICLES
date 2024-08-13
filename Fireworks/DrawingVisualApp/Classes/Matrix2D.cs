using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingVisualApp
{
    /// <summary>
    /// Базовый класс
    /// </summary>
    public class MatrixBase
    {
        /// <summary>
        /// Создание матрицы
        /// </summary>
        /// <param name="rows">Количество строк</param>
        /// <param name="cols">Количество столбцов</param>
        /// <returns>Новая матрица</returns>
        public static double[,] Create(int rows, int cols) => new double[rows, cols];
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static double[,] FillRandoms(double[,] m)
        {
            int rows = m.GetLength(0);
            int cols = m.GetLength(1);
            double[,] result = new double[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[i, j] = Randoms.rnd.NextDouble();
            return result;
        }

        /// <summary>
        /// Создание копии матрицы вида double[,]
        /// </summary>
        /// <param name="matrix">Копируемая матрица</param>
        /// <returns>Новая матрица</returns>
        public static T[,] Copy<T>(T[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            T[,] result = new T[rows, cols];
            for (int i = 0; i < rows; ++i) // copy the values
                for (int j = 0; j < cols; ++j)
                    result[i, j] = matrix[i, j];
            return result;
        }
        public static T[] Copy<T>(T[] matrix)
        {
            int len = matrix.Length;
            T[] result = new T[len];
            for (int i = 0; i < len; ++i) // copy the values
                result[i] = matrix[i];
            return result;
        }

        // ======================================
        //    МАТРИЦА В ВИДЕ СТРОКИ
        // ======================================
        public static string ToString<T>(T[,] matrix, int padding = 4)
        {
            string s = "";
            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                for (int j = 0; j < matrix.GetLength(1); ++j)
                {
                    string str = (string)Convert.ChangeType(matrix[i, j], typeof(string));
                    s += str.PadLeft(padding) + " ";
                }

                s += Environment.NewLine;
            }
            return s;
        }

        // ======================================
        //    УМНОЖЕНИЕ
        // ======================================

        /// <summary>
        /// Умножение двух матриц
        /// </summary>
        /// <param name="matrixA">Первая матрица</param>
        /// <param name="matrixB">Вторая матрица</param>
        /// <returns>Новая матрица</returns>
        public static double[,] Mult(double[,] matrixA, double[,] matrixB)
        {
            int aRows = matrixA.GetLength(0); int aCols = matrixA.GetLength(1);
            int bRows = matrixB.GetLength(0); int bCols = matrixB.GetLength(1);
            if (aCols != bRows)
                throw new Exception("Non-conformable matrices in MatrixProduct");
            double[,] result = Create(aRows, bCols);
            Parallel.For(0, aRows, i =>
            {
                for (int j = 0; j < bCols; ++j)
                    for (int k = 0; k < aCols; ++k)
                        result[i, j] += matrixA[i, k] * matrixB[k, j];
            }
            );
            return result;
        }
        public static double[,] Mult(double[,] matrix, double val)
        {
            int aRows = matrix.GetLength(0); int aCols = matrix.GetLength(1);
            double[,] result = Create(aRows, aCols);
            for (int i = 0; i < aRows; ++i)
                for (int j = 0; j < aCols; ++j)
                    result[i, j] += matrix[i, j] * val;
            return result;
        }

        /// <summary>
        /// Умножение вектора-строки на двумерную матрицу
        /// </summary>
        /// <param name="vector">Вектор-строка</param>
        /// <param name="matrixA">Двумерная матрица</param>
        /// <returns>Новый вектор-строка</returns>
        public static double[] Mult(double[] vector, double[,] matrixA)
        {
            int aRows = matrixA.GetLength(0); int aCols = matrixA.GetLength(1);
            int vRows = 1; int vCols = vector.Length;
            if (aRows != vCols)
                throw new Exception("Non-conformable matrices in MatrixProduct");
            //double[] result = new double[3] {0, 0, 0};
            double[] result = new double[vCols];
            for (int i = 0; i < aCols; ++i)
            {
                for (int j = 0; j < vCols; ++j)
                {
                    result[i] += vector[j] * matrixA[j, i];
                }
            }
            return result;
        }

        public static double[] Mult(double[] a, double[] b)
        {
            int aLen = a.Length;
            int bLen = b.Length;
            if (aLen != bLen)
                throw new Exception("Размерность матриц не одинакова");
            double[] result = new double[aLen];

            for (int i = 0; i < aLen; i++)
                result[i] = a[i] * b[i];

            return result;
        }
        public static double[] Mult(double[] m, double n)
        {
            int mLen = m.Length;
            double[] result = new double[mLen];

            for (int i = 0; i < mLen; i++)
                result[i] = m[i] * n;

            return result;
        }

        // ======================================
        //    ДЕЛЕНИЕ
        // ======================================

        public static double[,] Div(double[,] m, double val)
        {
            int rows = m.GetLength(0); int cols = m.GetLength(1);

            double[,] result = new double[rows, cols];
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    result[i, j] = m[i, j] / val;
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Matrix : MatrixBase
    {
        // =================================================
        // ЕДИНИЧНАЯ МАТРИЦА
        // =================================================
        /// <summary>
        /// Создание единичной квадратной матрицы double[,]
        /// </summary>
        /// <param name="n">Количество строк и столбцов</param>
        /// <returns>Новая матрица</returns>
        public static double[,] Identity(int n)
        {
            // return an n x n Identity matrix
            double[,] result = Create(n, n);
            for (int i = 0; i < n; ++i)
                result[i, i] = 1.0;

            return result;
        }

        // ======================================
        //    ТРАНСПОНИРОВАНИЕ
        // ======================================

        /// <summary>
        /// Транспонирование ОДНОМЕРНОГО массива
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static T[,] T<T>(T[] m)
        {
            var result = new T[m.GetLength(0), 1];

            for (int y = 0; y < m.GetLength(0); y++)
                result[y, 0] = m[y];

            return result;
        }
        //public static double[,] T(double[] m)
        //{
        //    var result = new double[m.GetLength(0), 1];

        //    for (int y = 0; y < m.GetLength(0); y++)
        //        result[y, 0] = m[y];

        //    return result;
        //}

        // Транспонирование ДВУМЕРНОГО массива
        public static T[,] T<T>(T[,] m)
        {
            var result = new T[m.GetLength(1), m.GetLength(0)];

            for (int y = 0; y < m.GetLength(1); y++)
                for (int x = 0; x < m.GetLength(0); x++)
                    result[y, x] = m[x, y];

            return result;
        }

        // В РАЗРАБОТКЕ
        // Транспонирование ДВУМЕРНОГО ступенчатого массива
        // ВАЖНО! количество столбцов во всех строках исходного массива должно быть одинаково
        //public static T[][] T<T>(T[][] m)
        //{
        //    var mRows = m.Length;
        //    var result = new T[mRows][];

        //    for (int y = 0; y < mlen; y++)
        //        for (int x = 0; x < m[y].Length; x++)
        //        {
        //            result[y] = new T[m[y].Length];
        //            result[y][x] = m[x][y];
        //        }
        //    return result;
        //}

        public static T[] GetRow<T>(T[,] m, int index)
        {
            T[] result = new T[m.GetLength(1)];

            for (int i = 0; i < m.GetLength(1); i++)
                result[i] = m[index, i];
            return result;
        }

        // ======================================
        //    ВЫЧИТАНИЕ
        // ======================================
        // This function subtracts B[,] from A[,]
        public static double[,] Subtract(double[,] matrixA, double[,] matrixB)
        {
            int aRows = matrixA.GetLength(0); int aCols = matrixA.GetLength(1);
            int bRows = matrixB.GetLength(0); int bCols = matrixB.GetLength(1);
            if (aCols != bCols || aRows != bRows)
                throw new Exception("Размерность матриц не одинакова");
            double[,] result = Create(aRows, bCols);

            for (int i = 0; i < aRows; i++)
                for (int j = 0; j < aCols; j++)
                    result[i, j] += matrixA[i, j] - matrixB[i, j];

            return result;
        }

        public static double[] Subtract(double[] a, double[] b)
        {
            int aLen = a.Length;
            int bLen = b.Length;
            if (aLen != bLen)
                throw new Exception("Размерность матриц не одинакова");
            double[] result = new double[aLen];

            for (int i = 0; i < aLen; i++)
                result[i] = a[i] - b[i];

            return result;
        }

        // ======================================
        //    СЛОЖЕНИЕ
        // ======================================
        public static double[,] Add(double[,] matrixA, double[,] matrixB)
        {
            int aRows = matrixA.GetLength(0); int aCols = matrixA.GetLength(1);
            int bRows = matrixB.GetLength(0); int bCols = matrixB.GetLength(1);
            if (aCols != bCols || aRows != bRows)
                throw new Exception("Размерность матриц не одинакова");
            double[,] result = Create(aRows, bCols);

            for (int i = 0; i < aRows; i++)
                for (int j = 0; j < aCols; j++)
                    result[i, j] += matrixA[i, j] + matrixB[i, j];

            return result;
        }

        public static double[,] Add(double[,] matrixA, double[,] matrixB, double[,] matrixC)
        {
            int aRows = matrixA.GetLength(0);
            int bCols = matrixB.GetLength(1);

            double[,] result = Create(aRows, bCols);

            var sum1 = Matrix.Add(matrixA, matrixB);
            result = Matrix.Add(sum1, matrixC);

            return result;
        }

        public static double[] Add(double[] a, double[] b)
        {
            int aLen = a.Length;
            int bLen = b.Length;
            if (aLen != bLen)
                throw new Exception("Размерность матриц не одинакова");
            double[] result = new double[aLen];

            for (int i = 0; i < aLen; i++)
                result[i] = a[i] + b[i];

            return result;
        }

        // ======================================
        //    CLIP МАТРИЦЫ
        // ======================================

        public static double[,] Clip(double[,] matrix, double a_min, double a_max)
        {
            int aRows = matrix.GetLength(0); int aCols = matrix.GetLength(1);
            double[,] result = Create(aRows, aCols);

            for (int i = 0; i < aRows; i++)
                for (int j = 0; j < aCols; j++)
                {
                    var val = result[i, j];
                    if (val < a_min) result[i, j] = a_min;
                    else
                    if (val > a_max) result[i, j] = a_max;
                    else
                        result[i, j] = matrix[i, j];
                }
            return result;
        }

        // ======================================
        //    МАТРИЦЫ ЭКВИВАЛЕНТНЫ?
        // ======================================
        public static bool IsEqual(double[,] matrixA, double[,] matrixB, double epsilon = 0.0001)
        {
            // true if all values in matrixA == corresponding values in matrixB
            int aRows = matrixA.Length; int aCols = matrixA.GetLength(0);
            int bRows = matrixB.Length; int bCols = matrixB.GetLength(0);
            if (aRows != bRows || aCols != bCols)
                throw new Exception("Non-conformable matrices in MatrixAreEqual");

            for (int i = 0; i < aRows; ++i) // each row of A and B
                for (int j = 0; j < aCols; ++j) // each col of A and B
                                                //if (matrixA[i][j] != matrixB[i][j])
                    if (Math.Abs(matrixA[i, j] - matrixB[i, j]) > epsilon)
                        return false;
            return true;
        }
        public static bool IsEqual(int[] matrixA, List<int> matrixB, double epsilon = 0.001)
        {
            int aLen = matrixA.Length;
            int bLen = matrixB.Count;
            if (aLen != bLen)
                throw new Exception("Non-conformable matrices in MatrixAreEqual");

            for (int i = 0; i < aLen; ++i) // each row of A and B
                if (Math.Abs(matrixA[i] - matrixB[i]) > epsilon)
                    return false;
            return true;
        }
        // ======================================
        //    ОПРЕДЕЛИТЕЛЬ (ДЕТЕРМИНАНТ)
        // ======================================
        public static double Determinant(double[,] matrix)
        {
            int[] perm;
            int toggle;
            double[,] lum = Decompose(matrix, out perm, out toggle);
            if (lum == null)
                throw new Exception("Unable to compute MatrixDeterminant");
            double result = toggle;
            for (int i = 0; i < lum.GetLength(0); ++i)
                result *= lum[i, i];
            return result;
        }

        // ======================================
        //    РАЗЛОЖЕНИЕ МАТРИЦЫ - МЕТОД ДУЛЛИТЛА
        // ======================================
        public static double[,] Decompose(double[,] matrix, out int[] perm, out int toggle)
        {
            // Doolittle LUP decomposition with partial pivoting.
            // rerturns: result is L (with 1s on diagonal) and U; perm holds row permutations; toggle is +1 or -1 (even or odd)
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1); // assume all rows have the same number of columns so just use row [0].
            if (rows != cols)
                throw new Exception("Attempt to MatrixDecompose a non-square mattrix");

            int n = rows; // convenience

            double[,] result = Copy(matrix); // make a copy of the input matrix

            perm = new int[n]; // set up row permutation result
            for (int i = 0; i < n; ++i) { perm[i] = i; }

            toggle = 1; // toggle tracks row swaps. +1 -> even, -1 -> odd. used by MatrixDeterminant

            for (int j = 0; j < n - 1; ++j) // each column
            {
                double colMax = Math.Abs(result[j, j]); // find largest value in col j
                int pRow = j;
                for (int i = j + 1; i < n; ++i)
                {
                    if (result[i, j] > colMax)
                    {
                        colMax = result[i, j];
                        pRow = i;
                    }
                }

                if (pRow != j) // if largest value not on pivot, swap rows
                {
                    //double[] rowPtr = result[pRow];
                    double[] rowPtr = GetColsInRow(result, pRow);
                    //result[pRow] = result[j];
                    result = SetColsInRow(result, pRow, GetColsInRow(result, j));
                    //result[j] = rowPtr;
                    result = SetColsInRow(result, j, rowPtr);

                    int tmp = perm[pRow]; // and swap perm info
                    perm[pRow] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }

                if (Math.Abs(result[j, j]) < 1.0E-20) // if diagonal after swap is zero . . .
                    return null; // consider a throw

                for (int i = j + 1; i < n; ++i)
                {
                    result[i, j] /= result[j, j];
                    for (int k = j + 1; k < n; ++k)
                    {
                        result[i, k] -= result[i, j] * result[j, k];
                    }
                }
            } // main j column loop

            return result;
        } // MatrixDecompose

        /*
           МОИ ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ДЛЯ ДЕКОМПОЗИЦИИ (ОБРАЩЕНИЯ)
        */
        // Вспомогательный метод получения столбцов двумерного массива в строке
        private static double[] GetColsInRow(double[,] m, int row)
        {
            double[] result = new double[m.GetLength(1)];
            for (int i = 0; i < m.GetLength(1); i++)
                result[i] = m[row, i];
            return result;
        }

        // Вспомогательный метод записи столбцов двумерного массива в строку
        private static double[,] SetColsInRow(double[,] m, int row, double[] cols)
        {
            double[,] duplm = Copy(m);
            for (int i = 0; i < duplm.GetLength(1); i++)
                duplm[row, i] = cols[i];
            return duplm;
        }

        // ======================================
        //    ОБРАЩЕНИЕ МАТРИЦЫ
        // ======================================

        public static double[,] Inverse(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[,] result = Copy(matrix);

            int[] perm;
            int toggle;
            double[,] lum = Decompose(matrix, out perm, out toggle);
            if (lum == null)
                throw new Exception("Unable to compute inverse");

            double[] b = new double[n];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (i == perm[j])
                        b[j] = 1.0;
                    else
                        b[j] = 0.0;
                }

                double[] x = HelperSolve(lum, b); // 

                for (int j = 0; j < n; ++j)
                    result[j, i] = x[j];
            }
            return result;
        }

        // Вспомогательный метод для обращения матрицы
        private static double[] HelperSolve(double[,] luMatrix, double[] b) // helper
        {
            // before calling this helper, permute b using the perm array from MatrixDecompose that generated luMatrix
            int n = luMatrix.GetLength(0);
            double[] x = new double[n];
            b.CopyTo(x, 0);

            for (int i = 1; i < n; ++i)
            {
                double sum = x[i];
                for (int j = 0; j < i; ++j)
                    sum -= luMatrix[i, j] * x[j];
                x[i] = sum;
            }

            x[n - 1] /= luMatrix[n - 1, n - 1];
            for (int i = n - 2; i >= 0; --i)
            {
                double sum = x[i];
                for (int j = i + 1; j < n; ++j)
                    sum -= luMatrix[i, j] * x[j];
                x[i] = sum / luMatrix[i, i];
            }

            return x;
        }
    }

    // ======================================
    //    2D МАТРИЦА
    // ======================================

    /// <summary>
    /// Класс для 2D матричных трансформаций
    /// </summary>
    public class Matrix2D : MatrixBase
    {
        public double M11;
        public double M12;
        public double M13;
        public double M21;
        public double M22;
        public double M23;
        public double M31;
        public double M32;
        public double M33;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public Matrix2D()
        {

        }

        public void Create(double m11 = 0, double m12 = 0, double m13 = 0,
                    double m21 = 0, double m22 = 0, double m23 = 0,
                    double m31 = 0, double m32 = 0, double m33 = 0)
        {
            this.M11 = m11; this.M12 = m12; this.M13 = m13;
            this.M21 = m21; this.M22 = m22; this.M23 = m23;
            this.M31 = m31; this.M32 = m32; this.M33 = m33;
        }

        /// <summary>
        /// Создание пустой 2D матрицы (3х3) типа double[,]
        /// </summary>
        /// <returns>Новая матрица</returns>
        public static double[,] Create()
        {
            return new double[3, 3] {
                { 0,   0,   0 },
                { 0,   0,   0 },
                { 0,   0,   0 }
            };
        }

        /// <summary>
        /// Конвертация в двумерный масиив double[3,3]
        /// </summary>
        /// <returns>Двумерный массив double[3,3]</returns>
        public double[,] ToArray()
        {
            return new double[3, 3] {
                { M11,   M12,   M13 },
                { M21,   M22,   M23 },
                { M31,   M32,   M33 }
            };
        }

        /// <summary>
        /// Текстовый вид матрицы
        /// </summary>
        /// <returns>Строка</returns>
        public new string ToString()
        {
            return $"[ {M11}, {M12}, {M13} \n" +
                    $" {M21}, {M22}, {M23} \n" +
                    $" {M31}, {M32}, {M33} ]";
        }


        /// <summary>
        /// Матрица 2D трансформации: rotation + scale + offset
        /// </summary>
        /// <param name="angle">Угол поворота в градусах</param>
        /// <param name="scaleX">Растяжение по X</param>
        /// <param name="scaleY">Растяжение по Y</param>
        /// <param name="offsX">Смещение по Х</param>
        /// <param name="offsY">Смещение по Y</param>
        /// <returns>Результирующая матрица</returns>
        public double[,] FullTransform(double angle, double scaleX, double scaleY, double offsX, double offsY)
        {
            /* 
                Матрица поворота:
                cosQ    sinQ    0
                -sinQ   cosQ    0
                offX    offY    1
            */

            double angleRadian = angle * Math.PI / 180; //переводим угол в радианты

            double ma = Math.Cos(angleRadian) + scaleX;
            double mb = Math.Sin(angleRadian);
            double mc = -Math.Sin(angleRadian);
            double md = Math.Cos(angleRadian) + scaleY;

            return new double[3, 3] {
                { ma,   mb,     0 },
                { mc,   md,     0 },
                { offsX, offsY, 1 }
            };
        }
        /// <summary>
        /// Матрица 2D трансформации: rotation + scale + offset
        /// </summary>
        /// <param name="angle">Угол поворота в градусах</param>
        /// <param name="scaleX">Растяжение по X</param>
        /// <param name="scaleY">Растяжение по Y</param>
        /// <param name="offsX">Смещение по Х</param>
        /// <param name="offsY">Смещение по Y</param>
        /// <returns>Результирующая матрица</returns>
        public static double[,] CreateFullTransform(double angle, double scaleX, double scaleY, double offsX, double offsY)
        {
            /* 
                Матрица поворота:
                cosQ    sinQ    0
                -sinQ   cosQ    0
                offX    offY    1
            */

            double angleRadian = angle * Math.PI / 180; //переводим угол в радианы

            double ma = Math.Cos(angleRadian) + scaleX;
            double mb = Math.Sin(angleRadian);
            double mc = -Math.Sin(angleRadian);
            double md = Math.Cos(angleRadian) + scaleY;

            return new double[3, 3] {
                { ma,   mb,     0 },
                { mc,   md,     0 },
                { offsX, offsY, 1 }
            };
        }

        /// <summary>
        /// Вращение матрицы 2D
        /// </summary>
        /// <param name="angle">Угол поворота в градусах</param>
        public void Rotate(double angle)
        {
            /* 
                Матрица поворота:
                cosQ    sinQ    0
                -sinQ   cosQ    0
                0       0       1
            */

            double radians = angle * Math.PI / 180; //переводим угол в радианы
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            M11 = cos; M12 = sin; M13 = 0;
            M21 = -sin; M22 = cos; M23 = 0;
            M31 = 0; M32 = 0; M33 = 1;
        }
        /// <summary>
        /// Вращение матрицы 2D
        /// </summary>
        /// <param name="angle">Угол поворота в градусах</param>
        /// <returns>Новая матрица поворота</returns>
        public static double[,] CreateRotation(double angle)
        {
            /* 
                Матрица поворота:
                cosQ    sinQ    0
                -sinQ   cosQ    0
                0       0       1
            */

            double radians = angle * Math.PI / 180; //переводим угол в радианты
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            return new double[3, 3] {
                    { cos,  sin, 0 },
                    { -sin, cos, 0 },
                    { 0,    0,   1 }
                };
        }

        /// <summary>
        /// Матрица растяжения 2D
        /// </summary>
        /// <param name="Sx">X</param>
        /// <param name="Sy">Y</param>
        public void Scale(double Sx, double Sy)
        {
            /* 
                Матрица поворота:
                Sx   0    0
                0    Sy   0
                0    0    1
            */

            M11 = Sx; M12 = 0; M13 = 0;
            M21 = 0; M22 = Sy; M23 = 0;
            M31 = 0; M32 = 0; M33 = 1;
        }
        /// <summary>
        /// Матрица растяжения 2D
        /// </summary>
        /// <param name="Sx">X</param>
        /// <param name="Sy">Y</param>
        /// <returns></returns>
        public static double[,] CreateScale(double Sx, double Sy)
        {
            /* 
                Матрица поворота:
                Sx   0    0
                0    Sy   0
                0    0    1
            */

            return new double[3, 3] {
                { Sx,   0,  0 },
                { 0,    Sy, 0 },
                { 0,    0,  1 }
            };
        }

        /// <summary>
        /// Матрица перемещения 2D
        /// </summary>
        /// <param name="Tx">X</param>
        /// <param name="Ty">Y</param>
        /// <returns></returns>
        public void Translate(double Tx, double Ty)
        {
            /* 
                Матрица поворота:
                1   0   0
                0   1   0
                Tx  Ty  1
            */

            M11 = 1; M12 = 0; M13 = 0;
            M21 = 0; M22 = 1; M23 = 0;
            M31 = Tx; M32 = Ty; M33 = 1;
        }
        /// <summary>
        /// Матрица перемещения 2D
        /// </summary>
        /// <param name="Tx">X</param>
        /// <param name="Ty">Y</param>
        /// <returns></returns>
        public static double[,] CreateTranslation(double Tx, double Ty)
        {
            /* 
                Матрица поворота:
                1   0   0
                0   1   0
                Tx  Ty  1
            */

            return new double[3, 3] {
                { 1,    0,   0 },
                { 0,    1,   0 },
                { Tx,   Ty,  1 }
            };
        }


        // ПЕРЕГРУЗКА ОПЕРАТОРОВ

        public static Matrix2D operator *(Matrix2D a, Matrix2D b)
        {
            var m11 = a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31;
            var m12 = a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32;
            var m13 = a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33;

            var m21 = a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31;
            var m22 = a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32;
            var m23 = a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33;

            var m31 = a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31;
            var m32 = a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32;
            var m33 = a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33;

            return new Matrix2D()
            {
                M11 = m11,
                M12 = m12,
                M13 = m13,
                M21 = m21,
                M22 = m22,
                M23 = m23,
                M31 = m31,
                M32 = m32,
                M33 = m33
            };
        }
    }
}
