using NeuralNetLibrary.components;
using NeuralNetLibrary.components.learning;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetLibrary.algorithms
{
    public class Tester
    {
        private NeuralNetwork neuralNetwork;
        private OutputConverter outputConverter;
        public double ExpectedValue { get; private set; }
        public double ReceivedValue { get; private set; }
        public double Probability { get; private set; }
        public double[] NetworkOutputs { get; private set; }
        public BitArray PatternOutputs { get; private set; }

        public Tester(NeuralNetwork neuralNetwork, OutputConverter outputConverter)
        {
            this.neuralNetwork = neuralNetwork;
            this.outputConverter = outputConverter;
        }

        public void Test(DataItem dataItem)
        {
            PatternOutputs = dataItem.Outputs;

            neuralNetwork.SetInputs(dataItem.Inputs);
            neuralNetwork.Propagate();
            NetworkOutputs = neuralNetwork.GetOutputs();

            ExpectedValue = outputConverter.Convert(PatternOutputs);
            ReceivedValue = outputConverter.GetMostProbablyValue(NetworkOutputs);
            Probability = outputConverter.GetProbability(NetworkOutputs, PatternOutputs);
        }
    }
}
