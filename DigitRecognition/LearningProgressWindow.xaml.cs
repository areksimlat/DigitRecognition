using NeuralNetLibrary.algorithms;
using NeuralNetLibrary.components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace DigitRecognition
{
    /// <summary>
    /// Logika interakcji dla klasy LearningProgressWindow.xaml
    /// </summary>
    public partial class LearningProgressWindow : Window
    {
        public List<DataItem> DataItems { get; set; }
        public NeuralNetwork NeuralNet { get; set; }
        public double LearningRate { get; set; }
        public double Momentum { get; set; }
        public double ErrorThreshold { get; set; }
        public bool Multithreading { get; set; }
        private BackpropagationAlgorithm backpropagationAlgorithm;
        private Thread thread;
        private bool stopSignal;
        private bool refreshProgress;

        public LearningProgressWindow()
        {
            InitializeComponent();
        }

        private void initChart()
        {
            chart.ChartAreas["chartArea"].AxisX.Title = "Epoka";
            chart.ChartAreas["chartArea"].AxisY.Title = "Błąd";
            chart.ChartAreas["chartArea"].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chart.ChartAreas["chartArea"].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;

            chart.Series["series"].Points.Clear();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            refreshProgress = true;
            initChart();
            startTraining();
        }

        private void startTraining()
        {
            stopSignal = false;

            if (Multithreading)
            {                
                backpropagationAlgorithm = new ParallelBackpropagation(
                   NeuralNet, DataItems, LearningRate, Momentum, ErrorThreshold, Environment.ProcessorCount);
            }
            else
            {
                backpropagationAlgorithm = new Backpropagation(
                     NeuralNet, DataItems, LearningRate, Momentum, ErrorThreshold);
            }

            thread = new Thread(new ThreadStart(trainNetwork));
            thread.Start();
        }

        private void trainNetwork()
        {
            bool trainComplete = false;
            int currentEpoch;
            double currentError;
            var watch = Stopwatch.StartNew();

            do
            {
                trainComplete = backpropagationAlgorithm.Train();

                if (refreshProgress)
                {
                    Dispatcher.Invoke(() =>
                    {
                        currentEpoch = backpropagationAlgorithm.GetCurrentEpoch();
                        currentError = backpropagationAlgorithm.GetCurrentError();

                        epochLabel.Content = currentEpoch.ToString();
                        errorLabel.Content = String.Format("{0:0.00000}", currentError);
                        learningProgressBar.Value = (int)((ErrorThreshold / currentError * 100));

                        chart.Series["series"].Points.AddXY(currentEpoch, currentError);
                    });
                }
            }
            while (!trainComplete && !stopSignal);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            
            Dispatcher.Invoke(() =>
            {
                abortButton.IsEnabled = false;
            });

            if (stopSignal)
            {
                backpropagationAlgorithm.Abort();
                MessageBox.Show("Przerwano uczenie sieci (Czas: " + elapsedMs + " ms)");
            }
            else
            {
                MessageBox.Show("Sieć została nauczona (Czas: " + elapsedMs + " ms)"); ;
            }            
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            refreshProgress = false;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            refreshProgress = true;
        }

        private void Abort_Button_Click(object sender, RoutedEventArgs e)
        {
            stopSignal = true;
            abortButton.IsEnabled = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            stopSignal = true;
        }
    }
}
