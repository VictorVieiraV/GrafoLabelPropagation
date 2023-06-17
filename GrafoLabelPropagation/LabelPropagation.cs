using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace GrafoLabelPropagation
{
    public class LabelPropagation
    {
        public static void RodarODiacho()
        {
            int n_labeled = 10;
            double alpha = 0.99;
            double sigma = 0.1;


            int n = 20;
            bool shuffle = true;
            double noise = 0.1;
            int? randomState = null;

            MoonGenerator moonGenerator = new MoonGenerator();
            MakeMoonsResult result = moonGenerator.MakeMoons(n, shuffle, noise, randomState);

            Matrix<double> X = result.X;
            double[] Y = result.Y;


            // Importar make_moons da biblioteca sklearn.datasets
            //Tuple<Matrix<double>, double[]> data = MakeMoons(n, 0.1);
            //Matrix<double> X = data.Item1;
            //double[] Y = data.Item2;

            string[] colors = new string[Y.Length];
            for (int i = 0; i < Y.Length; i++)
            {
                colors[i] = (Y[i] == 0) ? "red" : "blue";
            }

            // Criar matriz Y_input
            Matrix<double> Y_input = ConcatenateMatrix(DenseMatrix.OfColumnArrays(Y.Take(n_labeled).Select(y => new[] { (y == 0) ? 1.0 : 0.0 }).ToArray()),
                                                       DenseMatrix.Create(n - n_labeled, 2, 0.0));

            // Calcular matriz de distância dm
            Matrix<double> dm = CalculateDistanceMatrix(X);

            // Função RBF
            Func<double, double, double> rbf = (x, s) => Math.Exp((-x) / (2 * Math.Pow(s, 2)));

            // Aplicar RBF aos elementos da matriz dm
            Matrix<double> W = Matrix<double>.Build.Dense(n, n);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    double distance = dm[i, j];
                    W[i, j] = rbf(distance, sigma);
                }
            }

            // Preencher diagonal de W com zeros
            for (int i = 0; i < n; i++)
            {
                W[i, i] = 0.0;
            }

            // Calcular soma das linhas de W
            Vector<double> sumLines = W.RowSums();

            // Construir matriz diagonal D
            Matrix<double> D = Matrix<double>.Build.Dense(n, n, 0.0);

            for (int i = 0; i < n; i++)
            {
                D[i, i] = sumLines[i];
            }

            // Calcular matriz S
            Matrix<double> DsqrtInv = D.Power((int)-0.5);
            Matrix<double> S = DsqrtInv.Multiply(W).Multiply(DsqrtInv);

            int n_iter = 400;

            // Propagação de rótulos
            Matrix<double> F = S.Multiply(Y_input).Multiply(alpha).Add(Y_input.Multiply(1 - alpha));
            for (int t = 0; t < n_iter; t++)
            {
                F = S.Multiply(F).Multiply(alpha).Add(Y_input.Multiply(1 - alpha));
            }

            // Criar matriz Y_result
            Matrix<double> Y_result = Matrix<double>.Build.Dense(F.RowCount, F.ColumnCount);
            for (int i = 0; i < Y_result.RowCount; i++)
            {
                int argMaxIndex = F.Row(i).MaximumIndex();
                Y_result[i, argMaxIndex] = 1.0;
            }

            double[] Y_v = Y_result.Column(0).ToArray().Select(x => (x == 0) ? 1.0 : 0.0).ToArray();

            // Criar arquivo CSV com os dados de dispersão
            CreateScatterPlotCSV(X.Column(0).ToArray(), X.Column(1).ToArray(), Y_v.Select(x => (x == 0) ? "red" : "blue").ToArray(), "scatter_plot.csv");
        }

        //static Tuple<Matrix<double>, double[]> MakeMoons(int n, double noise)
        //{
        //    double[] X = new double[2 * n];
        //    double[] Y = new double[n];
        //    Random random = new Random();
        //    for (int i = 0; i < n; i++)
        //    {
        //        double r = random.NextDouble();
        //        double theta = Math.PI * random.NextDouble();
        //        double x = 0.5 * r * Math.Cos(theta) + 0.5;
        //        double y = 0.5 * r * Math.Sin(theta) + 0.25;
        //        double nx = x + noise * random.NextDouble();
        //        double ny = y + noise * random.NextDouble();
        //        X[2 * i] = nx;
        //        X[2 * i + 1] = ny;
        //        Y[i] = (y > 0.5) ? 1 : 0;
        //    }

        //    return Tuple.Create(Matrix<double>.Build.DenseOfColumnMajor(n, 2, X), Y);
        //}

        static Matrix<double> CalculateDistanceMatrix(Matrix<double> X)
        {
            int n = X.RowCount;
            Matrix<double> dm = DenseMatrix.Create(n, n, 0.0);
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    double dist = Math.Sqrt(Math.Pow(X[i, 0] - X[j, 0], 2) + Math.Pow(X[i, 1] - X[j, 1], 2));
                    dm[i, j] = dist;
                    dm[j, i] = dist;
                }
            }
            return dm;
        }

        static Matrix<double> ConcatenateMatrix(Matrix<double> m1, Matrix<double> m2)
        {
            int rows1 = m1.RowCount;
            int cols1 = m1.ColumnCount;
            int rows2 = m2.RowCount;
            int cols2 = m2.ColumnCount;

            if (rows1 != rows2)
                throw new ArgumentException("As matrizes devem ter o mesmo número de linhas.");

            Matrix<double> result = Matrix<double>.Build.Dense(rows1, cols1 + cols2);

            for (int i = 0; i < rows1; i++)
            {
                for (int j = 0; j < cols1; j++)
                {
                    result[i, j] = m1[i, j];
                }

                for (int j = 0; j < cols2; j++)
                {
                    result[i, cols1 + j] = m2[i, j];
                }
            }

            return result;
        }

        static void CreateScatterPlotCSV(double[] x, double[] y, string[] colors, string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("x,y,color");
                for (int i = 0; i < x.Length; i++)
                {
                    writer.WriteLine($"{x[i]},{y[i]},{colors[i]}");
                }
            }
        }
    }

    public class MakeMoonsResult
    {
        public Matrix<double> X { get; set; }
        public double[] Y { get; set; }
    }

    public class MoonGenerator
    {
        private Random random;

        public MakeMoonsResult MakeMoons(int n, bool shuffle = true, double noise = 0.1, int? randomState = null)
        {
            random = randomState.HasValue ? new Random(randomState.Value) : new Random();

            double[] X = new double[2 * n];
            double[] Y = new double[n];

            for (int i = 0; i < n; i++)
            {
                double r = random.NextDouble();
                double theta = Math.PI * random.NextDouble();
                double x = 0.5 * r * Math.Cos(theta) + 0.5;
                double y = 0.5 * r * Math.Sin(theta) + 0.25;
                double nx = x + noise * random.NextDouble();
                double ny = y + noise * random.NextDouble();
                X[2 * i] = nx;
                X[2 * i + 1] = ny;
                Y[i] = (y > 0.5) ? 1 : 0;
            }

            if (shuffle)
            {
                ShuffleData(X, Y);
            }

            var result = new MakeMoonsResult
            {
                X = Matrix<double>.Build.DenseOfColumnMajor(n, 2, X),
                Y = Y
            };

            return result;
        }

        private void ShuffleData(double[] X, double[] Y)
        {
            int n = X.Length / 2;

            for (int i = n - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);

                Swap(ref X[2 * i], ref X[2 * j]);
                Swap(ref X[2 * i + 1], ref X[2 * j + 1]);
                Swap(ref Y[i], ref Y[j]);
            }
        }

        private void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }
}