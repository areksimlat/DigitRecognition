using NeuralNetLibrary.algorithms;
using NeuralNetLibrary.components;
using NeuralNetLibrary.components.learning;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DigitRecognition
{
    /// <summary>
    /// Logika interakcji dla klasy TestNeuralNetWindow.xaml
    /// </summary>
    public partial class TestNeuralNetWindow : Window
    {
        public NeuralNetwork NeuralNet { get; set; }
        public List<DataItem> DataItems { get; set; }
        public DataItem.DataTypes DataType { get; set; }
        public OutputConverter OutputConvert { get; set; }        
        private List<int> values;
        private List<Label> correctLabels;
        private List<Label> errorLabels;
        private List<Label> countLabels;
        private List<Label> percentLabels;
        private Tester tester;
        private bool stopSignal;
        private int allSamplesCount;

        public TestNeuralNetWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataItems == null || OutputConvert == null)
                return;

            init();
            initAllSamplesCount();
            startTest();
        }

        private void init()
        {
            correctLabels = new List<Label>();
            errorLabels = new List<Label>();
            countLabels = new List<Label>();
            percentLabels = new List<Label>();

            values = OutputConvert.GetValues();

            for (int i = 0; i < values.Count; i++)
            {
                Label titleLabel = new Label()
                {
                    Content = values[i].ToString(),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                Label correctLabel = new Label()
                {
                    Content = "0",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                Label errorLabel = new Label()
                {
                    Content = "0",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                Label countLabel = new Label()
                {
                    Content = "0",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                Label percentLabel = new Label()
                {
                    Content = "0",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                Grid.SetColumn(titleLabel, 0);
                Grid.SetRow(titleLabel, i + 1);

                Grid.SetColumn(correctLabel, 1);
                Grid.SetRow(correctLabel, i + 1);

                Grid.SetColumn(errorLabel, 2);
                Grid.SetRow(errorLabel, i + 1);

                Grid.SetColumn(countLabel, 3);
                Grid.SetRow(countLabel, i + 1);

                Grid.SetColumn(percentLabel, 4);
                Grid.SetRow(percentLabel, i + 1);

                RowDefinition rowDefinition = new RowDefinition();
                SampleGrid.RowDefinitions.Add(rowDefinition);

                SampleGrid.Children.Add(titleLabel);
                SampleGrid.Children.Add(correctLabel);
                SampleGrid.Children.Add(errorLabel);
                SampleGrid.Children.Add(countLabel);
                SampleGrid.Children.Add(percentLabel);

                correctLabels.Add(correctLabel);
                errorLabels.Add(errorLabel);
                countLabels.Add(countLabel);
                percentLabels.Add(percentLabel);
            }
        }

        private void initAllSamplesCount()
        {
            allSamplesCount = 0;

            for (int i = 0; i < DataItems.Count; i++)
                if (DataItems[i].dataType == DataType)
                    allSamplesCount++;

            allSamplesLabel.Content = allSamplesCount.ToString();
        }

        private void updateInfo()
        {
            int itemIndex = values.IndexOf((int)tester.ExpectedValue);

            if (itemIndex >= 0)
            {
                Label correctLabel = correctLabels[itemIndex];
                Label errorLabel = errorLabels[itemIndex];
                Label countLabel = countLabels[itemIndex];
                Label percentageLabel = percentLabels[itemIndex];

                int correctValue = int.Parse((String)correctLabel.Content);
                int errorValue = int.Parse((String)errorLabel.Content);
                int countValue = int.Parse((String)countLabel.Content);
                double percentageValue = double.Parse((String)percentageLabel.Content);
                int currentSample = int.Parse((String)currentSampleLabel.Content);
                double probability = OutputConvert.GetProbability(tester.NetworkOutputs, tester.PatternOutputs);

                if (tester.ReceivedValue == tester.ExpectedValue)
                    correctValue++;
                else
                    errorValue++;

                countValue++;
                currentSample++;
                percentageValue = (correctValue * 100) / countValue;

                currentSampleLabel.Content = currentSample.ToString();
                expectedValueLabel.Content = tester.ExpectedValue.ToString();
                receivedValueLabel.Content = tester.ReceivedValue.ToString();
                probabilityLabel.Content = String.Format("{0:0.00} %", probability);

                correctLabel.Content = correctValue.ToString();
                errorLabel.Content = errorValue.ToString();
                countLabel.Content = countValue.ToString();
                percentageLabel.Content = String.Format("{0:0.00}", percentageValue);

                testProgressBar.Value = (int)(currentSample * 100 / allSamplesCount);
            }
        }

        private void testNetwork()
        {
            for (int i = 0; i < DataItems.Count && !stopSignal; i++)
            {
                DataItem dataItem = DataItems[i];

                if (dataItem.dataType == DataType)
                {
                    tester.Test(dataItem);

                    Dispatcher.Invoke(() => { updateInfo(); });
                }
            }

            if (stopSignal)
            {
                MessageBox.Show("Przerwano testowanie sieci");
            }
            else
            {
                MessageBox.Show("Zakończono testowanie sieci");
                Dispatcher.Invoke(() => { abortButton.IsEnabled = false; });
            }
                
        }

        private void startTest()
        {
            stopSignal = false;
            tester = new Tester(NeuralNet, OutputConvert);

            new Thread(new ThreadStart(testNetwork)).Start();
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
