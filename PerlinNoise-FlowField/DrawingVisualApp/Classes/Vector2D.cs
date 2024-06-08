using System;


namespace DrawingVisualApp
{
    internal class Vector2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2D(double x = 0, double y = 0)
        {
            X = x;
            Y = y;
        }

        public Vector2D CopyToVector2D() => new Vector2D(X, Y);

        /// <summary>
        /// Копирование вектора в массив
        /// </summary>
        /// <returns>Новый массив [x, y]</returns>
        public double[] CopyToArray() => new double[2] { X, Y };

        /// <summary>
        /// Присвоение скалярных значений вектору
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Set(double x, double y)
        {
            X = x;
            Y = y;
        }
        /// <summary>
        /// Текстовое представление вектора 
        /// </summary>
        /// <returns>Строка вида "[x, y]"</returns>
        public override string ToString() => $"Vector2D Object: [{X}, {Y}]";

        /// <summary>
        /// Вывод значений вектора на консоль вида "[x, y]"
        /// </summary>
        public void ToConsole() => Console.WriteLine(ToString());

        /*
         *  СЛОЖЕНИЕ
         */
        /// <summary>
        /// Сложение вектора со скалярами
        /// </summary>
        /// <param name="Vector2D"></param>
        /// <returns>Возвращает новый вектор и изменяет текущий</returns>
        public Vector2D Add(double x, double y)
        {
            X += x;
            Y += y;
            return CopyToVector2D();
        }

        /// <summary>
        /// Сложение вектора со скаляром
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Возвращает новый вектор и изменяет текущий</returns>
        public Vector2D Add(double value)
        {
            X += value;
            Y += value;
            return CopyToVector2D();
        }

        /// <summary>
        /// Сложение двух векторов
        /// </summary>
        /// <param name="v"></param>
        /// <returns>Возвращает новый вектор и изменяет текущий</returns>
        public Vector2D Add(Vector2D v)
        {
            X += v.X;
            Y += v.Y;
            return CopyToVector2D();
        }

        /// <summary>
        /// Сложение двух векторов
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>Возвращает новый вектор</returns>
        public static Vector2D Add(Vector2D v1, Vector2D v2) => new Vector2D(v1.X + v2.X, v1.Y + v2.Y);

        /*
         *  ВЫЧИТАНИЕ
         */
        /// <summary>
        /// Вычитание из вектора скаляров
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Возвращает новый вектор и изменяет текущий</returns>
        public Vector2D Sub(double x, double y)
        {
            X -= x;
            Y -= y;
            return CopyToVector2D();
        }

        /// <summary>
        /// Вычитание из вектора другого вектора
        /// </summary>
        /// <param name="v"></param>
        /// <returns>Возвращает новый вектор и изменяет текущий</returns>
        public Vector2D Sub(Vector2D v)
        {
            X -= v.X;
            Y -= v.Y;
            return CopyToVector2D();
        }

        /// <summary>
        /// Вычитание двух векторов
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>Возвращает новый вектор</returns>
        public static Vector2D Sub(Vector2D v1, Vector2D v2) => new Vector2D(v1.X - v2.X, v1.Y - v2.Y);

        /*
         *  ДЕЛЕНИЕ
         */
        /// <summary>
        /// Деление вектора на скаляр
        /// </summary>
        /// <param name="n"></param>
        /// <returns>Возвращает новый вектор и изменяет текущий</returns>
        public Vector2D Div(double n)
        {
            X /= n;
            Y /= n;
            return CopyToVector2D();
        }

        /// <summary>
        /// Деление вектора на другой вектор
        /// </summary>
        /// <param name="v"></param>
        /// <returns>Возвращает новый вектор и изменяет текущий</returns>
        public Vector2D Div(Vector2D v)
        {
            X /= v.X;
            Y /= v.Y;
            return CopyToVector2D();
        }

        /// <summary>
        /// Divide (деление) двух векторов
        /// </summary>
        /// <param name="val"></param>
        /// <returns>Возвращает новый вектор</returns>
        public static Vector2D Div(Vector2D v1, Vector2D v2) => new Vector2D(v1.X / v2.X, v1.Y / v2.Y);

        /// <summary>
        /// Деление вектора на скаляр
        /// </summary>
        /// <param name="v">Вектор</param>
        /// <param name="n">Скаляр</param>
        /// <returns>Возвращает новый вектор</returns>
        public static Vector2D Div(Vector2D v, double n) => new Vector2D(v.X / n, v.Y / n);

        /*
         *  УМНОЖЕНИЕ НА СКАЛЯР
         */
        /// <summary>
        /// Multiply (умножение) вектора на число
        /// </summary>
        /// <param name="n"></param>
        /// <returns>Возвращает новый вектор и изменяет текущий</returns>
        public Vector2D Mult(double n)
        {
            X *= n;
            Y *= n;
            return CopyToVector2D();
        }

        /// <summary>
        /// Multiply (умножение) вектора на число
        /// </summary>
        /// <param name="v"></param>
        /// <param name="n"></param>
        /// <returns>Возвращает новый вектор</returns>
        public static Vector2D Mult(Vector2D v, double n) => new Vector2D(v.X * n, v.Y * n);

        public static Vector2D Mult(double[,] matrix2D, Vector2D v)
        {
            Vector2D result = new Vector2D();
            result.X = matrix2D[0, 0] * v.X + matrix2D[0, 1] * v.Y;
            result.Y = matrix2D[1, 0] * v.X + matrix2D[1, 1] * v.Y;
            return result;
        }

        /*
         *  СКАЛЯРНОЕ (Dot) УМНОЖЕНИЕ ВЕКТОРОВ
         */
        /// <summary>
        /// Скалярное (Dot) умножение векторов
        /// </summary>
        /// <param name="v"></param>
        /// <returns>Скаляр</returns>
        public double Dot(Vector2D v) => X * v.X + Y * v.Y;
        /// <summary>
        /// Скалярное (Dot) умножение векторов
        /// </summary>
        /// <param name="v1">Вектор 1</param>
        /// <param name="v2">Вектор 2</param>
        /// <returns>Скаляр двоичное число</returns>
        public static double Dot(Vector2D v1, Vector2D v2) => v1.X * v2.X + v1.Y * v2.Y;

        /*
         *  ВЕКТОРНОЕ (Cross) УМНОЖЕНИЕ ВЕКТОРОВ
         */
        /// <summary>
        /// Векторное произведение векторов. Для 2D результат всегда скаляр, т.к. z = 0
        /// </summary>
        /// <param name="v"></param>
        /// <returns>Скаляр</returns>
        public double Cross(Vector2D v) => X * v.Y - Y * v.X;
        public static double Cross(Vector2D v1, Vector2D v2) => v1.X * v2.Y - v1.Y * v2.X;
        /*
         * ИНТЕРПОЛЯЦИЯ
         */
        /// <summary>
        /// Интерполяция вектора к другому вектору
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="amt">Value between 0.0 (old Vector2D) and 1.0 (new Vector2D). 0.9 is very near the new Vector2D. 0.5 is halfway in between</param>
        /// <returns>Возвращает новый вектор</returns>
        public Vector2D Lerp(double x, double y, double amt)
        {
            return new Vector2D(
                X + (x - X) * amt,
                Y + (y - Y) * amt
            );
        }
        /// <summary>
        /// Интерполяция вектора к другому вектору
        /// </summary>
        /// <param name="v"></param>
        /// <param name="amt">Value between 0.0 (old Vector2D) and 1.0 (new Vector2D). 0.9 is very near the new Vector2D. 0.5 is halfway in between</param>
        /// <returns>Возвращает новый вектор</returns>
        public Vector2D Lerp(Vector2D v, double amt)
        {
            return new Vector2D(
                X + (v.X - X) * amt,
                Y + (v.Y - Y) * amt
            );
        }
        /// <summary>
        /// Интерполяция вектора к другому вектору
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="amt">Value between 0.0 (old Vector2D) and 1.0 (new Vector2D). 0.9 is very near the new Vector2D. 0.5 is halfway in between</param>
        /// <returns>Возвращает новый вектор</returns>
        public static Vector2D Lerp(Vector2D v1, Vector2D v2, double amt)
        {
            Vector2D tmp = new Vector2D();
            tmp = v1.CopyToVector2D();
            return tmp.Lerp(v2, amt);
        }

        /*
         * ПОЛУЧЕНИЕ УГЛА ВЕКТОРА
         */
        /// <summary>
        /// Возвращает угол вектора (only 2D Vector2Ds)
        /// </summary>
        /// <returns>Значение угла в радианах</returns>
        public double HeadingRad() => Math.Atan2(Y, X);

        /// <summary>
        /// Возвращает угол вектора (only 2D Vector2Ds)
        /// </summary>
        /// <returns>Значение угла в градусах [от 0 до 359]</returns>
        public double HeadingDeg()
        {
            var h = HeadingRad();
            return h >= 0 ? h * 180 / Math.PI : (2 * Math.PI + h) * 360 / (2 * Math.PI);
        }

        /*
         * ПОЛУЧЕНИЕ УГЛА МЕЖДУ ДВУХ ВЕКТОРОВ
         */
        /// <summary>
        /// Вычисление угла между двумя векторами
        /// </summary>
        /// <param name="v"></param>
        /// <returns>Возвращает угол в радианах</returns>
        public double angleBetween(Vector2D v)
        {
            // В основе вычислений формула: a * b = |a| * |b| * cos θ, где
            // a * b - скалярное (dot) произведение
            // |a| и |b| - длины (mag) векторов

            var dotmagmag = Dot(v) / (Mag() * v.Mag());

            // Из содержания библиотеки p5j:
            // Mathematically speaking: the dotmagmag variable will be between -1 and 1
            // inclusive. Practically though it could be slightly outside this range due
            // to floating-point rounding issues. This can make Math.acos return NaN.
            //
            // Solution: we'll clamp the value to the -1,1 range
            var angle = Math.Acos(Math.Min(1, Math.Max(-1, dotmagmag)));
            return angle;
        }

        /*
         * ВРАЩЕНИЕ ВЕКТОРА
         */
        /// <summary>
        /// Поворот вектора на заданный угол (only 2D Vector2Ds)
        /// </summary>
        /// <param name="a">Угол в радианах</param>
        /// <returns>Возвращает новый вектор и изменяет текущий</returns>
        public Vector2D Rotate(double a)
        {
            var newHeading = HeadingRad() + a;
            var mag = Mag();
            X = Math.Cos(newHeading) * mag;
            Y = Math.Sin(newHeading) * mag;
            return CopyToVector2D();
        }

        /*
         *
         */

        /// <summary>
        /// Получение квадрата длины вектора
        /// </summary>
        /// <returns>Скаляр</returns>
        public double MagSq() => X * X + Y * Y;

        /// <summary>
        /// Вычисление длины вектора
        /// </summary>
        /// <returns>Скаляр</returns>
        public double Mag() => Math.Sqrt(MagSq());

        /// <summary>
        /// Задание длины вектора
        /// </summary>
        /// <param name="n">вещественная длина</param>
        /// <returns></returns>
        public void SetMag(double n)
        {
            Vector2D v = Normalize();
            v.Mult(n);
            X = v.X;
            Y = v.Y;
        }

        /// <summary>
        /// Задание длины вектора
        /// </summary>
        /// <param name="n">целочисленная длина</param>
        /// <returns></returns>
        public void SetMag(int n)
        {
            Vector2D v = Normalize();
            v.Mult(n);
            X = v.X;
            Y = v.Y;
        }


        /// <summary>
        /// Нормализация вектора
        /// </summary>
        /// <returns>Возвращает новый вектор и изменяет текущий</returns>
        public Vector2D Normalize()
        {
            var len = Mag();
            if (len == 0)
                return new Vector2D(0, 0);
            else
            {
                Mult(1 / len);
                return CopyToVector2D();
            }
        }
        public static Vector2D Normalize(Vector2D v)
        {
            Vector2D tmp = v.CopyToVector2D();
            tmp.Normalize();
            return tmp;
        }

        /// <summary>
        /// Ограничение (Limit) длины вектора до max значения
        /// </summary>
        /// <param name="max">Требуемая максимальная длина вектора</param>
        /// <returns>Вектор с лимитированной длиной</returns>
        public Vector2D Limit(double max)
        {
            var mSq = MagSq(); // получение квадрата длины текущего вектора
            if (mSq > max * max)
            {
                Div(Math.Sqrt(mSq)) //normalize it
                  .Mult(max); // домнажаем на максимальную длину
            }
            //Normalize().Mult(max);
            return CopyToVector2D();
        }

        /*
         * СОЗДАНИЕ ВЕКТОРА
         */
        /// <summary>
        /// Создание вектора по углу
        /// </summary>
        /// <param name="angle">Угол в радианах</param>
        /// <param name="len">Длина вектора. По умолчанию длина = 1</param>
        /// <returns>Возвращает новый вектор</returns>
        public static Vector2D FromAngle(double angle, double len = 1) => new Vector2D(len * Math.Cos(angle), len * Math.Sin(angle));

        /// <summary>
        /// Создание единичного 2D вектора по случайному углу 2PI
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns>Возвращает новый вектор</returns>
        public static Vector2D Random2D(Random rnd) => FromAngle(rnd.NextDouble() * Math.PI * 2);
        /// <summary>
        /// Вычисление расстояния между векторами
        /// </summary>
        /// <param name="v"></param>
        /// <returns>Cкаляр</returns>
        public double Dist(Vector2D v) => Sub(this, v).Mag();
        /// <summary>
        /// Вычисление расстояния между двумя векторами
        /// </summary>
        /// <param name="v1">Вектор 1</param>
        /// <param name="v2">Вектор 2</param>
        /// <returns>Cкаляр</returns>
        public static double Dist(Vector2D v1, Vector2D v2) => v1.Dist(v2);

        /// <summary>
        /// Вычисление перпендикулярного вектора к вектору a
        /// </summary>
        /// <param name="v">Вектор, к которому строится нормаль</param>
        /// <returns>Новый перпендикулярный вектор</returns>
        public static Vector2D NormalVector2D(Vector2D a)
        {
            /*
             * Исходная формула скалярного произведения: ax * bx + ay * by = 0
             * Отсюда bx = -ay * by / ax, при ax != 0
             * Отсюда by = -ax * bx / ay, при ay != 0
             */

            double bx = 0, by = 0;

            if (a.X != 0)
            {
                by = 1;
                bx = -a.Y * by / a.X;
            }
            else
            if (a.Y != 0)
            {
                bx = 1;
                by = -a.X * bx / a.Y;
            }

            return new Vector2D(bx, by);
        }

        public static Vector2D operator +(Vector2D left, Vector2D right) => Add(left, right);

        public static Vector2D operator -(Vector2D v) => v.Mult(-1);

        public static Vector2D operator -(Vector2D left, Vector2D right) => Sub(left, right);

        public static Vector2D operator *(float left, Vector2D right) => Mult(right, left);

        public static Vector2D operator *(Vector2D left, Vector2D right) => new Vector2D(left.X * right.X, left.Y * right.Y);

        public static Vector2D operator *(Vector2D left, float right) => Mult(left, right);
        public static Vector2D operator *(Vector2D left, double right) => Mult(left, right);

        public static Vector2D operator /(Vector2D left, Vector2D right) => Div(left, right);

        public static Vector2D operator /(Vector2D value1, float value2) => Div(value1, value2);

        public static bool operator ==(Vector2D left, Vector2D right)
        {
            if (left.X == right.X && left.Y == right.Y)
                return true;
            else
                return false;
        }

        public static bool operator !=(Vector2D left, Vector2D right)
        {
            if (left.X != right.X || left.Y != right.Y)
                return true;
            else
                return false;
        }
    }

}
