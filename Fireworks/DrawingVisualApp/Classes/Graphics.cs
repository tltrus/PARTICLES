using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows;

namespace DrawingVisualApp
{
    public enum Shape2DType
    {
        ROMB,
        TRIANGLE,
        ARROW
    }

    public class Shape2D
    {
        Vector2D centralpoint;
        Shape2DType type;
        int size = 5;
        List<Vector2D> points;
        int sign; // знак в какую сторону будет вращение
        SolidColorBrush brush;

        public Shape2D(double x, double y, Shape2DType shapetype)
        {
            centralpoint = new Vector2D(x, y);
            type = shapetype;
            points = new List<Vector2D>();
            sign = Randoms.rnd.Next(-1, 2);
            brush = Brushes.White;

            CreateShape();
        }

        public void SetSize(int value)
        {
            size = value;
            CreateShape();
        }

        public void SetBrush(Color color) => brush = new SolidColorBrush(color);
        public void SetBrush(SolidColorBrush brush) => this.brush = (SolidColorBrush)brush;
        public void SetOpacity(double opacity)
        {
            var color = brush.Color;
            color.A = (byte)opacity;
            brush = new SolidColorBrush(color);
        }

        private void CreateRomb()
        {
            var x1 = centralpoint.X - size;
            var y1 = centralpoint.Y;
            points.Add(new Vector2D(x1, y1)); // low left point
            var x2 = centralpoint.X;
            var y2 = centralpoint.Y - size;
            points.Add(new Vector2D(x2, y2)); // up middle point
            var x3 = centralpoint.X + size;
            var y3 = centralpoint.Y;
            points.Add(new Vector2D(x3, y3)); // low right point
            var x4 = centralpoint.X;
            var y4 = centralpoint.Y + size;
            points.Add(new Vector2D(x4, y4)); // low middle point
        }
        private void CreateTriangle()
        {
            var x1 = centralpoint.X - size;
            var y1 = centralpoint.Y + size;
            points.Add(new Vector2D(x1, y1)); // low left point
            var x2 = centralpoint.X;
            var y2 = centralpoint.Y - size;
            points.Add(new Vector2D(x2, y2)); // up middle point
            var x3 = centralpoint.X + size;
            var y3 = centralpoint.Y + size;
            points.Add(new Vector2D(x3, y3)); // low right point
        }
        private void CreateArrow()
        {
            var x1 = centralpoint.X - size;
            var y1 = centralpoint.Y + size * 2;
            points.Add(new Vector2D(x1, y1)); // low left point
            var x2 = centralpoint.X;
            var y2 = centralpoint.Y - size;
            points.Add(new Vector2D(x2, y2)); // up middle point
            var x3 = centralpoint.X + size;
            var y3 = centralpoint.Y + size * 2;
            points.Add(new Vector2D(x3, y3)); // low right point
            var x4 = centralpoint.X;
            var y4 = centralpoint.Y + size;
            points.Add(new Vector2D(x4, y4)); // middle point
        }
        private void CreateShape()
        {
            points.Clear();
            switch (type)
            {
                case Shape2DType.ROMB:
                    CreateRomb();
                    break;
                case Shape2DType.TRIANGLE:
                    CreateTriangle();
                    break;
                case Shape2DType.ARROW:
                    CreateArrow();
                    break;
            }
        }

        public List<Vector2D> GetPoints() => points;
        public Vector2D GetCentralPoint() => centralpoint.CopyToVector();
        public Vector2D GetDirection()
        {
            var dir = Vector2D.Sub(points[1], centralpoint);
            dir.Normalize();
            return dir.CopyToVector();
        }


        /// <summary>
        /// Сложение вектора ко всем точкам фигуры
        /// </summary>
        /// <param name="v">вектор</param>
        public void Addition(Vector2D v)
        {
            foreach (var p in points)
                p.Add(v);
            centralpoint.Add(v);
        }

        public void Addition(double x, double y)
        {
            Addition(new Vector2D(x, y));
        }

        private void Rotate(double angle, double x, double y)
        {
            double[,] m_before = ListToArr(points);
            double[,] matrixTrans;

            Matrix2D mTranslNeg = new Matrix2D();
            mTranslNeg.Translate(-x, -y);

            Matrix2D mRot = new Matrix2D();
            mRot.Rotate(angle);

            Matrix2D mTransl = new Matrix2D();
            mTransl.Translate(x, y);

            Matrix2D mRes = mTranslNeg * mRot * mTransl; // Перемножение матриц
            matrixTrans = mRes.ToArray();

            double[,] m_after = Matrix2D.Mult(m_before, matrixTrans);

            points = ArrToList(m_after);
        }

        public void RotateAroundCenter(double angle) => Rotate(angle * sign, centralpoint.X, centralpoint.Y);

        private double[,] ListToArr(List<Vector2D> list)
        {
            int rows = list.Count;
            int cols = 3;
            double[,] matrix = new double[rows, cols];

            for (int i = 0; i < list.Count; ++i)
            {
                matrix[i, 0] = list[i].X;
                matrix[i, 1] = list[i].Y;
                matrix[i, 2] = 1;
            }
            return matrix;
        }

        private List<Vector2D> ArrToList(double[,] arr)
        {
            List<Vector2D> list = new List<Vector2D>();

            for (int i = 0; i < arr.GetUpperBound(0) + 1; ++i)
            {
                var x = arr[i, 0];
                var y = arr[i, 1];
                var w = arr[i, 2];
                list.Add(new Vector2D(x, y));
            }
            return list;
        }

        public void Show(DrawingContext dc)
        {
            PointCollection dc_points = new PointCollection();
            for (int i = 1; i < points.Count; ++i)
            {
                var x = points[i].X;
                var y = points[i].Y;
                var point = new Point(x, y);
                dc_points.Add(point);
            }

            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                var beginPoint = new Point(points[0].X, points[0].Y);
                geometryContext.BeginFigure(beginPoint, true, true);
                geometryContext.PolyLineTo(dc_points, true, true);
            }

            dc.DrawGeometry(brush, null, streamGeometry);
        }
    }
}
