using NeuralNetLibrary.components;
using NeuralNetLibrary.components.activators;
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
using System.Windows.Shapes;

namespace DigitRecognition
{
    /// <summary>
    /// Logika interakcji dla klasy CreateNeuralNetWindow.xaml
    /// </summary>
    public partial class CreateNeuralNetWindow : Window
    {
        private int inputNeurons;
        private int outputNeurons;
        private int hiddenLayersCount;
        private List<Label> hiddenNeuronLabels;
        private List<TextBox> hiddenNeuronTextBoxes;
        private NeuralNetwork neuralNetwork;

        public CreateNeuralNetWindow(int inputNeurons, int outputNeurons)
        {
            InitializeComponent();

            this.inputNeurons = inputNeurons;
            this.outputNeurons = outputNeurons;

            init();
        }

        private void init()
        {
            inputNeuronTextBox.Text = inputNeurons.ToString();
            outputNeuronTextBox.Text = outputNeurons.ToString();
            hiddenNeuronTextBoxes = new List<TextBox>();
            hiddenNeuronLabels = new List<Label>();

            hiddenLayersCount = 0;

            addLayerButton_Click(null, null);
            hiddenNeuronTextBoxes[hiddenLayersCount - 1].Text = (inputNeurons / 2).ToString();
        }

        private void addLayerButton_Click(object sender, RoutedEventArgs e)
        {
            Label label = new Label()
            {
                Content = "Ukryta " + (hiddenLayersCount + 1),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            TextBox textBox = new TextBox()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(5),
            };
            textBox.LostFocus += hiddenNeuronTextBox_LostFocus;

            Grid.SetRow(label, hiddenLayersCount);
            Grid.SetColumn(label, 0);

            Grid.SetRow(textBox, hiddenLayersCount);
            Grid.SetColumn(textBox, 1);

            RowDefinition rowDefinition = new RowDefinition()
            {
                Height = new GridLength(30)
            };

            HiddenLayersGrid.RowDefinitions.Add(rowDefinition);
            HiddenLayersGrid.Children.Add(label);
            HiddenLayersGrid.Children.Add(textBox);

            hiddenNeuronTextBoxes.Add(textBox);
            hiddenNeuronLabels.Add(label);
            hiddenLayersCount += 1;

            removeLayerButton.IsEnabled = true;
        }

        private void removeLayerButton_Click(object sender, RoutedEventArgs e)
        {
            Label label = hiddenNeuronLabels[hiddenLayersCount - 1];
            TextBox textBox = hiddenNeuronTextBoxes[hiddenLayersCount - 1];

            HiddenLayersGrid.Children.Remove(label);
            HiddenLayersGrid.Children.Remove(textBox);
            HiddenLayersGrid.RowDefinitions.RemoveAt(hiddenLayersCount - 1);

            hiddenNeuronLabels.Remove(label);
            hiddenNeuronTextBoxes.Remove(textBox);
            hiddenLayersCount -= 1;

            if (hiddenLayersCount == 0)
                removeLayerButton.IsEnabled = false;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            ActivationFunction activator = new SigmoidActivation();

            neuralNetwork = new NeuralNetwork();
            neuralNetwork.CreateLayer(activator, inputNeurons, true);

            foreach (TextBox textBox in hiddenNeuronTextBoxes)
            {
                int numberOfNeurons;

                if (Int32.TryParse(textBox.Text, out numberOfNeurons))
                    neuralNetwork.CreateLayer(activator, numberOfNeurons, true);
            }

            neuralNetwork.CreateLayer(activator, outputNeurons, false);

            Close();
        }

        private void hiddenNeuronTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int value;

            if (Int32.TryParse(textBox.Text, out value) && value > 0)
                textBox.Text = value.ToString();
            else
                textBox.Text = "1";
        }

        public NeuralNetwork GetNeuralNetwork()
        {
            return neuralNetwork;
        }
    }
}
