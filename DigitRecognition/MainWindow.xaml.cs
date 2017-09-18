using Microsoft.Win32;
using NeuralNetLibrary.components;
using NeuralNetLibrary.components.learning;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace DigitRecognition
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageViewer imageViewer;
        private DataHolder dataHolder;
        private NeuralNetwork neuralNetwork;
        private OutputConverter outputConverter;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void validateDataSeparation(Slider changedSlider, Slider firstSlider, Slider secondSlider)
        {
            if (changedSlider != null && firstSlider != null && secondSlider != null)
            {
                int changedValue = (int)changedSlider.Value;
                int firstValue = (int)firstSlider.Value;
                int secondValue = (int)secondSlider.Value;

                int sum = changedValue + firstValue + secondValue;

                if (sum > 100)
                {
                    int overflow = sum - 100;

                    if (overflow - (int)firstSlider.Value > 0)
                    {
                        overflow -= (int)firstSlider.Value;

                        firstSlider.Value = 0;
                        secondSlider.Value -= overflow;
                    }
                    else
                    {
                        firstSlider.Value -= overflow;
                    }
                }
            }
        }

        private void learningDataSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.learningDataTextBox.Text = ((int)e.NewValue).ToString();
            validateDataSeparation(learningDataSlider, validateDataSlider, testingDataSlider);            
        }

        private void validateDataSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.validateDataTextBox.Text = ((int)e.NewValue).ToString();
            validateDataSeparation(validateDataSlider, testingDataSlider, learningDataSlider);
        }

        private void testingDataSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.testingDataTextBox.Text = ((int)e.NewValue).ToString();
            validateDataSeparation(testingDataSlider, validateDataSlider, learningDataSlider);
        }

        private void learningDataTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int newValue;

            if (Int32.TryParse(learningDataTextBox.Text, out newValue) &&
                    newValue >= 0 && newValue <= 100)
                learningDataSlider.Value = newValue;
            else
                learningDataTextBox.Text = learningDataSlider.Value.ToString();
        }

        private void validateDataTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int newValue;

            if (Int32.TryParse(validateDataTextBox.Text, out newValue) &&
                    newValue >= 0 && newValue <= 100)
                validateDataSlider.Value = newValue;
            else
                validateDataTextBox.Text = validateDataSlider.Value.ToString();
        }

        private void testingDataTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int newValue;

            if (Int32.TryParse(testingDataTextBox.Text, out newValue) &&
                    newValue >= 0 && newValue <= 100)
                testingDataSlider.Value = newValue;
            else
                testingDataTextBox.Text = testingDataSlider.Value.ToString();
        }

        private void showDataComboBox_Initialized(object sender, EventArgs e)
        {            
            showDataComboBox.Items.Add("Uczące");
            showDataComboBox.Items.Add("Walidujące");
            showDataComboBox.Items.Add("Testowe");
            showDataComboBox.Items.Add("Pozostałe");

            showDataComboBox.SelectedIndex = 0;
        }

        private void showDataComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (showItemComboBox != null)
            {
                String selectedValue = (String)showDataComboBox.SelectedValue;
                DataItem.DataTypes selectedDataType = DataItem.DataTypes.None;

                switch (selectedValue)
                {
                    case "Uczące":
                        selectedDataType = DataItem.DataTypes.Learning;
                        break;

                    case "Walidujące":
                        selectedDataType = DataItem.DataTypes.Validate;
                        break;

                    case "Testowe":
                        selectedDataType = DataItem.DataTypes.Testing;
                        break;
                }

                showItemComboBox.Items.Clear();

                for (int i = 0; i < dataHolder.Items.Count; i++)
                    if (dataHolder.Items[i].dataType == selectedDataType)
                        showItemComboBox.Items.Add(
                            new ComboBoxItem()
                            {
                                Content = outputConverter.Convert(dataHolder.Items[i].Outputs).ToString(),
                                Tag = i
                            }
                        );                    

                showItemComboBox.SelectedIndex = 0;
            }
        }

        private void showItemComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)showItemComboBox.SelectedItem;
            
            if (selectedItem != null)
            {
                int itemIndex = (int)selectedItem.Tag;
                DataItem dataItem = dataHolder.Items[itemIndex];
                imageViewer.Draw(dataItem.Inputs);

                if (neuralNetwork != null)
                {
                    neuralNetwork.SetInputs(dataItem.Inputs);
                    neuralNetwork.Propagate();
                    double[] neuralNetOutput = neuralNetwork.GetOutputs();
                    
                    double expectedOutput = outputConverter.Convert(dataItem.Outputs);
                    double networkOutput = outputConverter.GetMostProbablyValue(neuralNetOutput);
                    double outputProbability = outputConverter.GetProbability(neuralNetOutput, dataItem.Outputs);

                    expectedOutputLabel.Content = expectedOutput.ToString();
                    neuralNetOutputLabel.Content = networkOutput.ToString();
                    outputProbabilityLabel.Content = String.Format("{0:0.00} %", outputProbability);
                }
            }
        }

        private void learningRateTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double newValue;

            if (Double.TryParse(learningRateTextBox.Text.Replace('.', ','), out newValue) &&
                    newValue >= 0 && newValue <= 1)
                learningRateTextBox.Text = newValue.ToString();
            else
                learningRateTextBox.Text = "0,1";
        }

        private void momentumTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double newValue;

            if (Double.TryParse(momentumTextBox.Text.Replace('.', ','), out newValue) &&
                    newValue >= 0 && newValue <= 1)
                momentumTextBox.Text = newValue.ToString();
            else
                momentumTextBox.Text = "0,9";
        }

        private void errorThresholdTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double newValue;

            if (Double.TryParse(errorThresholdTextBox.Text.Replace('.', ','), out newValue) &&
                    newValue >= 0 && newValue <= 1)
                errorThresholdTextBox.Text = newValue.ToString();
            else
                errorThresholdTextBox.Text = "0,1";
        }

        private void confirmDataButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)swapOrderCheckBox.IsChecked)
                dataHolder.Shuffle();

            int learningPercent = (int)learningDataSlider.Value;
            int validatePercent = (int)validateDataSlider.Value;
            int testingPercent = (int)testingDataSlider.Value;

            MyCustomMessage customMessage = new MyCustomMessage()
            {
                Owner = this,
                Message = "Trwa dzielenie danych..."
            };
            customMessage.Show();
            dataHolder.Load();

            dataHolder.Separate(learningPercent, validatePercent, testingPercent);

            customMessage.Close();
            MessageBox.Show(this, "Dane zostały podzielone");

            ShowDataGroupBox.IsEnabled = true;
            showDataComboBox_SelectionChanged(null, null);
        }

        private void CreateNeuralNetwork_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (dataHolder != null)
            {
                CreateNeuralNetWindow createNeuralNetWindow = new CreateNeuralNetWindow(
                dataHolder.Items[0].Inputs.Length, dataHolder.Items[0].Outputs.Length);

                createNeuralNetWindow.Owner = this;
                createNeuralNetWindow.ShowDialog();

                neuralNetwork = createNeuralNetWindow.GetNeuralNetwork();

                if (neuralNetwork != null)
                {
                    LearningSettingsGroupBox.IsEnabled = true;
                    neuralNetStatus.Content = "Warstw: " + neuralNetwork.Layers.Count;
                    MessageBox.Show(this, "Sieć została utworzona");
                }
                else
                {
                    neuralNetStatus.Content = "Brak";
                }
            }
            else
            {
                MessageBox.Show(this, "Najpierw wczytaj dane");
            }
        }

        private void TrainNeuralNetwork_Button_Click(object sender, RoutedEventArgs e)
        {
            double learningRate, momentum, errorThreshold;

            if (Double.TryParse(learningRateTextBox.Text, out learningRate) &&
                    Double.TryParse(momentumTextBox.Text, out momentum) &&
                    Double.TryParse(errorThresholdTextBox.Text, out errorThreshold))
            {
                LearningProgressWindow learningProgressWindow = new LearningProgressWindow()
                {
                    DataItems = dataHolder.Items,
                    NeuralNet = neuralNetwork,
                    LearningRate = learningRate,
                    Momentum = momentum,
                    ErrorThreshold = errorThreshold,
                    Multithreading = (bool)multithreadingComboBox.IsChecked                    
                };

                learningProgressWindow.Owner = this;
                learningProgressWindow.ShowDialog();

                TestingSettingsGroupBox.IsEnabled = true;         
            }
            else
            {
                MessageBox.Show(this, "Wystąpił nieoczekiwany błąd. Sprawdź wartości parametrów uczenia");
            }
        }

        private void ShowGrid_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            imageViewer.ShowGrid();
        }

        private void ShowGrid_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            imageViewer.HideGrid();
        }

        private void EnableEdit_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            userCorrectImageValue.IsEnabled = true;
            checkButton.IsEnabled = true;
            clearButton.IsEnabled = true;

            imageViewer.EnableEdit();
        }

        private void EnableEdit_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            userCorrectImageValue.IsEnabled = false;
            checkButton.IsEnabled = false;
            clearButton.IsEnabled = false;

            imageViewer.DisableEdit();
        }

        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            expectedOutputLabel.Content = "";
            neuralNetOutputLabel.Content = "";
            outputProbabilityLabel.Content = "";

            imageViewer.Clear();
        }

        private void Check_Button_Click(object sender, RoutedEventArgs e)
        {
            if (neuralNetwork != null)
            {
                BitArray imageInputs = imageViewer.GetCurrentImage();
                neuralNetwork.SetInputs(imageInputs);
                neuralNetwork.Propagate();
                double[] imageOutputs = neuralNetwork.GetOutputs();

                double networkOutput = outputConverter.GetMostProbablyValue(imageOutputs);
                neuralNetOutputLabel.Content = networkOutput.ToString();

                if (userCorrectImageValue.Text.Length > 0)
                {
                    int value;

                    if (Int32.TryParse(userCorrectImageValue.Text, out value))
                    {
                        BitArray expectedOutput = outputConverter.GetOutputs(value);
                        double outputProbability = outputConverter.GetProbability(imageOutputs, expectedOutput);

                        expectedOutputLabel.Content = value.ToString();
                        outputProbabilityLabel.Content = String.Format("{0:0.00} %", outputProbability);
                        return;
                    }                  
                }

                expectedOutputLabel.Content = "";
                outputProbabilityLabel.Content = "";
            }
        }

        private void userCorrectImageValue_LostFocus(object sender, RoutedEventArgs e)
        {
            int value;

            if (Int32.TryParse(userCorrectImageValue.Text, out value) && value >= 0 && value <= 9)
                userCorrectImageValue.Text = value.ToString();
            else
                userCorrectImageValue.Text = "";
        }

        private void testDataTypeComboBox_Initialized(object sender, EventArgs e)
        {
            testDataTypeComboBox.Items.Add("Uczące");
            testDataTypeComboBox.Items.Add("Walidujące");
            testDataTypeComboBox.Items.Add("Testowe");
            testDataTypeComboBox.Items.Add("Pozostałe");

            testDataTypeComboBox.SelectedIndex = 0;
        }

        private void runTestButton_Click(object sender, RoutedEventArgs e)
        {
            String selectedValue = (String)testDataTypeComboBox.SelectedValue;
            DataItem.DataTypes selectedDataType = DataItem.DataTypes.None;

            switch (selectedValue)
            {
                case "Uczące":
                    selectedDataType = DataItem.DataTypes.Learning;
                    break;

                case "Walidujące":
                    selectedDataType = DataItem.DataTypes.Validate;
                    break;

                case "Testowe":
                    selectedDataType = DataItem.DataTypes.Testing;
                    break;
            }

            TestNeuralNetWindow testNeuralNetWindow = new TestNeuralNetWindow()
            {
                NeuralNet = neuralNetwork,
                DataItems = dataHolder.Items,
                DataType = selectedDataType,
                OutputConvert = outputConverter
            };

            testNeuralNetWindow.Owner = this;
            testNeuralNetWindow.ShowDialog();
        }

        private void LoadNetworkFromFile_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".nnf";
            openFileDialog.Filter = "Neural Network Files (*.nnf)|*.nnf";

            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string filename = openFileDialog.FileName;
                neuralNetwork = NeuralNetwork.Load(filename);

                if (neuralNetwork != null)
                {
                    LearningSettingsGroupBox.IsEnabled = true;
                    neuralNetStatus.Content = "Warstw: " + neuralNetwork.Layers.Count;
                    MessageBox.Show(this, "Sieć została wczytana");
                }
                else
                {
                    MessageBox.Show(this, "Nie można zwczytać sieci");
                }
            }
        }

        private void SaveNetworkFromFile_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (neuralNetwork != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "NeuralNetwork";
                saveFileDialog.Filter = "Neural Network Files (*.nnf)|*.nnf";
                Nullable<bool> result = saveFileDialog.ShowDialog();
                
                if (result == true)
                {
                    string filename = saveFileDialog.FileName;
                    if (NeuralNetwork.Save(neuralNetwork, filename))
                        MessageBox.Show(this, "Sieć została zapisana");
                    else
                        MessageBox.Show(this, "Nie można zapisać sieci");
                }
            }
        }

        private void initData()
        {
            neuralNetwork = null;
            neuralNetStatus.Content = "Brak";
            LearningSettingsGroupBox.IsEnabled = false;
            ShowDataGroupBox.IsEnabled = false;

            expectedOutputLabel.Content = "";
            neuralNetOutputLabel.Content = "";
            outputProbabilityLabel.Content = "";

            MyCustomMessage customMessage = new MyCustomMessage()
            {
                Owner = this,
                Message = "Trwa ładowanie danych..."
            };
            customMessage.Show();

            Thread thread = new Thread(() => { dataHolder.Load(); });
            thread.Start();
            thread.Join();

            List<BitArray> distinctOutputs = dataHolder.GetDistinctOutputs();
            Dictionary<int, BitArray> translatedOutputs = dataHolder.GetTranslateOutputs(distinctOutputs);

            outputConverter = new OutputConverter();

            foreach (KeyValuePair<int, BitArray> entry in translatedOutputs)
                outputConverter.AddItem(entry.Value, entry.Key);

            imageViewer = new ImageViewer(ImageGrid, dataHolder.ImageHeight, dataHolder.ImageWidth);

            DataSettingsGroupBox.IsEnabled = true;
            dataStatus.Content = "Rekordów: " + dataHolder.Items.Count;

            customMessage.Close();
            MessageBox.Show(this, "Wczytano " + dataHolder.Items.Count + " rekordów");
        }

        private void SemeionLoad_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            dataHolder = new SemeionDataHolder();
            initData();
        }

        private void KaggleLoad_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            dataHolder = new KaggleDataHolder();
            initData();
        }
    }
}
