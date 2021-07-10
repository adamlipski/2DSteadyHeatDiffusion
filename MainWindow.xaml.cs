using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2DSteadyHeatDiffusion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializePlot(CalculateValues());
        }

        private double[,] CalculateValues()
        {
            // Number of grid points
            int N = 51;

            // Domain size, squre shape a = b = L
            int L = 1;

            // Corresponding grid spacing 
            double h = Double.Parse(L.ToString()) / (Double.Parse(N.ToString()) - 1);

            // Thermal conductivity 
            double k = 0.1;

            // Cross-section area
            double A = 0.001;

            //Iterations
            int iterations = 0;

            // Create array of T values
            double[,] T = new double[N, N];


            //Boundary conditions - Top boundary (row) is 1
            //            T[0,:] = 1.

            for (int i = 0; i < N; i++) T[0, i] = 1.0;



            // Initializing iterated temperature
            double[,] T_new = new double[N, N];
            for (int i = 0; i < N; i++) T_new[0, i] = 1.0;

            // Error related variables
            //double epsilon = 1.0E-8;
            double epsilon = 0.00000008;
            double numerical_error = 1.0;

            // Plot for numerical error
            //            plt.figure(10)

            bool error = numerical_error > epsilon;
            // Check the error tolerance
            while (error)
            {   // Computing for all interior points
                foreach (int i in Enumerable.Range(1, N - 2))
                {
                    foreach (int j in Enumerable.Range(1, N - 2))
                    {
                        double a_E = (k * A / h);

                        double a_W = (k * A / h);

                        double a_N = (k * A / h);

                        double a_S = (k * A / h);

                        double a_P = a_E + a_W + a_N + a_S;

                        T_new[i, j] = (a_E * T[i, j + 1] + a_W * T[i, j - 1] + a_N * T[i - 1, j] + a_S * T[i + 1, j]) / a_P;

                    }
                }

                // Resetting the numerical eroor and recalculate
                numerical_error = 0;
                foreach (int i in Enumerable.Range(1, N - 2))
                {
                    foreach (int j in Enumerable.Range(1, N - 2))
                    {
                        numerical_error += Math.Abs(T[i, j] - T_new[i, j]);
                    }

                }
                //MessageBox.Show(numerical_error.ToString());
                // Iteration advancement and reassignment
                iterations = iterations + 1;
                T = T_new;
                error = numerical_error > epsilon;

            }
            MessageBox.Show(iterations.ToString());
            return T;

        }
        void InitializePlot(double[,] data2D)
        {
            // var plt = new ScottPlot.Plot(600, 400);
            var plt = WpfPlot1.Plot;
            var rand = new Random(0);
            //double[,] data2D = DataGen.Random2D(rand, 5, 4);

            var hm = plt.AddHeatmap(data2D, lockScales: false);
            hm.Smooth = true;

            plt.SetAxisLimits(0, 51, 49, 51);
        }
    }
}
