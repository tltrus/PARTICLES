using System;
using System.Runtime.InteropServices;

namespace DrawingVisualApp
{
    public unsafe class Fluid : IDisposable
    {
        float dt;
        float diff;
        float visc;
        float[] s;
        float[] density;
        float[] vx;
        float[] vy;
        float[] vx0;
        float[] vy0;
        int N;
        int iter;
        int SCALE;

        // Unsafe указатели
        float* p_s, p_density, p_vx, p_vy, p_vx0, p_vy0;
        GCHandle[] handles;

        // Кэш для граничных условий
        int[] boundaryCache;
        float[,] coefficientCache;

        public Fluid(float diffusion, float viscosity, float dt)
        {
            N = MainWindow.N;
            iter = MainWindow.iter;
            SCALE = MainWindow.SCALE;

            this.dt = dt;
            diff = diffusion;
            visc = viscosity;

            // Инициализация массивов
            s = new float[N * N];
            density = new float[N * N];
            vx = new float[N * N];
            vy = new float[N * N];
            vx0 = new float[N * N];
            vy0 = new float[N * N];

            // Закрепляем массивы в памяти
            handles = new GCHandle[6];
            handles[0] = GCHandle.Alloc(s, GCHandleType.Pinned);
            handles[1] = GCHandle.Alloc(density, GCHandleType.Pinned);
            handles[2] = GCHandle.Alloc(vx, GCHandleType.Pinned);
            handles[3] = GCHandle.Alloc(vy, GCHandleType.Pinned);
            handles[4] = GCHandle.Alloc(vx0, GCHandleType.Pinned);
            handles[5] = GCHandle.Alloc(vy0, GCHandleType.Pinned);

            p_s = (float*)handles[0].AddrOfPinnedObject();
            p_density = (float*)handles[1].AddrOfPinnedObject();
            p_vx = (float*)handles[2].AddrOfPinnedObject();
            p_vy = (float*)handles[3].AddrOfPinnedObject();
            p_vx0 = (float*)handles[4].AddrOfPinnedObject();
            p_vy0 = (float*)handles[5].AddrOfPinnedObject();

            InitializeCache();
        }

        private void InitializeCache()
        {
            boundaryCache = new int[N * N];
            coefficientCache = new float[iter, 4];

            // Предварительные вычисления
            for (int i = 0; i < iter; i++)
            {
                coefficientCache[i, 0] = 1.0f / (1 + 4 * 0.1f * diff * (N - 2) * (N - 2));
                coefficientCache[i, 1] = 0.1f * diff * (N - 2) * (N - 2);
            }
        }

        public void Dispose()
        {
            foreach (var handle in handles)
            {
                handle.Free();
            }
        }

        // Инлайним критический метод
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        int IX(int x, int y)
        {
            if (x < 0) x = 0;
            if (x > N - 1) x = N - 1;
            if (y < 0) y = 0;
            if (y > N - 1) y = N - 1;
            return x + y * N;
        }

        public float GetDensity(int x, int y)
        {
            return p_density[IX(x, y)];
        }

        // Быстрый доступ ко всему массиву плотности
        public float[] GetDensityArray()
        {
            return density;
        }

        public void Step()
        {
            float visc = this.visc;
            float diff = this.diff;
            float dt = this.dt;

            diffuse(1, vx0, vx, visc, dt);
            diffuse(2, vy0, vy, visc, dt);

            project(vx0, vy0, vx, vy);

            advect(1, vx, vx0, vx0, vy0, dt);
            advect(2, vy, vy0, vx0, vy0, dt);

            project(vx, vy, vx0, vy0);

            diffuse(0, s, density, diff, dt);
            advect(0, density, s, vx, vy, dt);
        }

        public void addDensity(int x, int y, float amount)
        {
            p_density[IX(x, y)] += amount;
        }

        // Пакетное добавление плотности
        public void addDensityBatch(int[] xs, int[] ys, float[] amounts, int count)
        {
            for (int i = 0; i < count; i++)
            {
                p_density[IX(xs[i], ys[i])] += amounts[i];
            }
        }

        public void addVelocity(int x, int y, float amountX, float amountY)
        {
            int index = IX(x, y);
            p_vx[index] += amountX;
            p_vy[index] += amountY;
        }

        public void diffuse(int b, float[] x, float[] x0, float diff, float dt)
        {
            float a = dt * diff * (N - 2) * (N - 2);
            lin_solve(b, x, x0, a, 1 + 4 * a);
        }

        public unsafe void lin_solve(int b, float[] x, float[] x0, float a, float c)
        {
            float cRecip = 1f / c;
            int size = N;

            fixed (float* px = x, px0 = x0)
            {
                for (int k = 0; k < iter; k++)
                {
                    // Оптимизированные циклы с меньшим количеством умножений
                    for (int j = 1; j < size - 1; j++)
                    {
                        int rowOffset = j * size;
                        int prevRowOffset = (j - 1) * size;
                        int nextRowOffset = (j + 1) * size;

                        for (int i = 1; i < size - 1; i++)
                        {
                            int idx = rowOffset + i;
                            px[idx] = (px0[idx] + a * (px[idx + 1] + px[idx - 1] +
                                     px[nextRowOffset + i] + px[prevRowOffset + i])) * cRecip;
                        }
                    }
                    set_bnd(b, x);
                }
            }
        }

        public unsafe void project(float[] velocX, float[] velocY, float[] p, float[] div)
        {
            int size = N;
            float scale = 0.5f / N;

            fixed (float* pvelocX = velocX, pvelocY = velocY, pp = p, pdiv = div)
            {
                for (int j = 1; j < size - 1; j++)
                {
                    int rowOffset = j * size;
                    int prevRowOffset = (j - 1) * size;
                    int nextRowOffset = (j + 1) * size;

                    for (int i = 1; i < size - 1; i++)
                    {
                        int idx = rowOffset + i;
                        pdiv[idx] = -scale * (pvelocX[idx + 1] - pvelocX[idx - 1] +
                                              pvelocY[nextRowOffset + i] - pvelocY[prevRowOffset + i]);
                        pp[idx] = 0;
                    }
                }
            }

            set_bnd(0, div);
            set_bnd(0, p);
            lin_solve(0, p, div, 1, 4);

            fixed (float* pvelocX = velocX, pvelocY = velocY, pp = p)
            {
                for (int j = 1; j < size - 1; j++)
                {
                    int rowOffset = j * size;
                    int nextRowOffset = (j + 1) * size;
                    int prevRowOffset = (j - 1) * size;

                    for (int i = 1; i < size - 1; i++)
                    {
                        int idx = rowOffset + i;
                        pvelocX[idx] -= scale * (pp[idx + 1] - pp[idx - 1]) * N * N;
                        pvelocY[idx] -= scale * (pp[nextRowOffset + i] - pp[prevRowOffset + i]) * N * N;
                    }
                }
            }

            set_bnd(1, velocX);
            set_bnd(2, velocY);
        }

        public unsafe void advect(int b, float[] d, float[] d0, float[] velocX, float[] velocY, float dt)
        {
            int size = N;
            float dtx = dt * (size - 2);
            float dty = dt * (size - 2);

            fixed (float* pd = d, pd0 = d0, pvelocX = velocX, pvelocY = velocY)
            {
                for (int j = 1; j < size - 1; j++)
                {
                    float jfloat = j;
                    int rowOffset = j * size;

                    for (int i = 1; i < size - 1; i++)
                    {
                        int idx = rowOffset + i;
                        float ifloat = i;

                        float x = ifloat - dtx * pvelocX[idx];
                        float y = jfloat - dty * pvelocY[idx];

                        x = Math.Max(0.5f, Math.Min(size + 0.5f, x));
                        y = Math.Max(0.5f, Math.Min(size + 0.5f, y));

                        int i0 = (int)x;
                        int i1 = i0 + 1;
                        int j0 = (int)y;
                        int j1 = j0 + 1;

                        float s1 = x - i0;
                        float s0 = 1.0f - s1;
                        float t1 = y - j0;
                        float t0 = 1.0f - t1;

                        pd[idx] = s0 * (t0 * pd0[i0 + j0 * size] + t1 * pd0[i0 + j1 * size]) +
                                  s1 * (t0 * pd0[i1 + j0 * size] + t1 * pd0[i1 + j1 * size]);
                    }
                }
            }

            set_bnd(b, d);
        }

        public unsafe void set_bnd(int b, float[] x)
        {
            int size = N;

            fixed (float* px = x)
            {
                // Граничные условия для стен
                for (int i = 1; i < size - 1; i++)
                {
                    px[i] = (b == 1) ? -px[size + i] : px[size + i];                    // Левая граница
                    px[(size - 1) + i * size] = (b == 1) ? -px[(size - 2) + i * size] : px[(size - 2) + i * size]; // Правая граница
                }

                for (int i = 1; i < size - 1; i++)
                {
                    px[i * size] = (b == 2) ? -px[1 + i * size] : px[1 + i * size];     // Верхняя граница
                    px[(size - 1) * size + i] = (b == 2) ? -px[(size - 2) * size + i] : px[(size - 2) * size + i]; // Нижняя граница
                }

                // Углы
                px[0] = (px[1] + px[size]) * 0.5f;
                px[size - 1] = (px[size - 2] + px[2 * size - 1]) * 0.5f;
                px[(size - 1) * size] = (px[(size - 2) * size + 1] + px[(size - 1) * size + 1]) * 0.5f;
                px[size * size - 1] = (px[size * size - 2] + px[(size - 1) * size - 1]) * 0.5f;
            }
        }
    }
}