using System.Windows;
using System.Windows.Media;

namespace DrawingVisualApp
{
    internal class Map
    {
        public int rows;
        public int cols;
        int cellsize;
        public int[,] map;
        int[,] next_map;

        public Map(int rows = 40, int cols = 40, int cellsize = 10) 
        { 
            this.rows = rows;
            this.cols = cols;
            this.cellsize = cellsize;

            map = new int[rows, cols];
            next_map = new int[rows, cols];

            // Set walls and floor
            for (int i = 0; i < rows; ++i)
            {
                map[i, cols - 1] = -2;    // right wall = -2
                map[i, 0] = -2;           // right wall = -2
            }
            for (int j = 0; j < cols; ++j)
            {
                map[rows - 1, j] = -2;    // floor = -2
            }
        }

        public int GetCellindex(Point p)
        {
            int X = (int)p.X / cellsize;
            int Y = (int)p.Y / cellsize;

            return (Y * cols) + X;
        }

        public void SetWall(Point p)
        {
            int X = (int)p.X / cellsize;
            int Y = (int)p.Y / cellsize;

            if ((X >= cols) || (Y >= rows)) return; // if click out of field

            map[Y, X] = (map[Y, X] == -2) ? 0 : -2; // trigger set\reset cell wall
        }

        public void SetTo(int row, int col, int value) => map[row, col] = value;

        private int SearchEmptyCell(int row_index, int start_index)
        {
            int counter1 = start_index;
            int counter2 = start_index;

            bool flag = true;
            counter1--;
            counter2++;

            while (flag)
            {
                if (counter1 >= 0)
                {
                    if (map[row_index, counter1] == 1)
                    {
                        counter1--;
                    }
                    else if (map[row_index, counter1] == 0)
                    {
                        return counter1;
                    }
                }

                if (counter2 < cols)
                {
                    if (map[row_index, counter2] == 1)
                    {
                        counter2++;
                    }
                    else if (map[row_index, counter2] == 0)
                    {
                        return counter2;
                    }
                }

                if (map[row_index, counter1] == -2 && map[row_index, counter2] == -2) flag = false;
            }

            return -5;
        }

        public void Draw(DrawingContext dc)
        {
            next_map = new int[rows, cols];

            for (int i = 1; i < rows - 1; i++)
            {
                for (int j = 1; j < cols - 1; j++)
                {

                    if (map[i, j] == -2)
                    {
                        next_map[i, j] = map[i, j];
                    }

                    Brush brush = Brushes.White;
                    if (map[i, j] == 0)
                    {
                        brush = Brushes.White;
                    }
                    else
                    if (map[i, j] == 1)
                    {
                        brush = Brushes.Blue;
                    }
                    else
                    if (map[i, j] == -2)
                    {
                        brush = Brushes.Red;
                    }

                    Rect rect = new Rect()
                    {
                        X = j * cellsize,
                        Y = i * cellsize,
                        Width = cellsize,
                        Height = cellsize
                    };
                    dc.DrawRectangle(brush, new Pen(Brushes.Black, 0.1), rect);

                    //      |_|
                    //       0
                    if (map[i, j] == 1 && map[i + 1, j] == 0)
                    {
                        next_map[i + 1, j] = 1;
                    }
                    else
                    //      |_|
                    //     0   0
                    if (map[i, j] == 1 && map[i + 1, j + 1] == 0 && map[i + 1, j - 1] == 0)
                    {
                        if (MainWindow.rnd.NextDouble() < 0.5)
                            next_map[i + 1, j + 1] = 1;
                        else
                            next_map[i + 1, j - 1] = 1;
                    }
                    else
                    //      |_|
                    //     0   !0
                    if (map[i, j] == 1 && map[i + 1, j - 1] == 0 && map[i + 1, j + 1] != 0)
                    {
                        next_map[i + 1, j - 1] = 1;
                    }
                    else
                    //      |_|
                    //    !0   0
                    if (map[i, j] == 1 && map[i + 1, j + 1] == 0 && map[i + 1, j - 1] != 0)
                    {
                        next_map[i + 1, j + 1] = 1;
                    }
                    else
                    //      |_|
                    //     1   1
                    if (map[i, j] == 1 && map[i + 1, j + 1] == 1 && map[i + 1, j - 1] == 1)
                    {
                        int calc_index = SearchEmptyCell(i + 1, j);

                        if (calc_index != -5)
                            map[i + 1, calc_index] = 1;
                        else
                            next_map[i, j] = map[i, j];
                    }
                    else
                    if (map[i, j] == 1)
                    {
                        next_map[i, j] = map[i, j];
                    }
                }
            }

            for (int i = 1; i < rows - 1; i++)
                for (int j = 1; j < cols - 1; j++)
                {
                    map[i, j] = next_map[i, j];
                }
        }
    }
}
