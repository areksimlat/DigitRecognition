using NeuralNetLibrary.components.activators;
using System;

namespace NeuralNetLibrary.components
{
    [Serializable]
    public class Neuron
    {
        public ActivationFunction Activator { get; }
        private double weightedSum;
        public double[] InputWeights { get; set; }
        public bool IsBias { get; }              
        public double Output { get; set; }
        public double Derivative { get; private set; }
        public double OutputError { get; set; }


        private Neuron(ActivationFunction activator, bool isBias)
        {
            Activator = activator;
            IsBias = isBias;
        }

        public static Neuron CreateBias(ActivationFunction activator)
        {
            return new Neuron(activator, true)
            {
                Output = 1
            };
        }

        public static Neuron CreateNeuron(ActivationFunction activator, int numberOfInputs)
        {
            double[] inputWeights = null;

            if (numberOfInputs > 0)
            {
                Random rand = new Random();
                inputWeights = new double[numberOfInputs];

                for (int i = 0; i < numberOfInputs; i++)
                    inputWeights[i] = rand.NextDouble() - 0.5;
            }

            return new Neuron(activator, false)
            {
                Output = 0,
                InputWeights = inputWeights
            };
        }

        private void calculateWeightsSum(double[] sourceOutputs)
        {
            weightedSum = 0;

            for (int i = 0; i < InputWeights.Length; i++)
                weightedSum += InputWeights[i] * sourceOutputs[i];
        }

        public void Activate(double[] sourceOutputs)
        {
            calculateWeightsSum(sourceOutputs);
            Output = Activator.Equation(weightedSum);
            Derivative = Activator.Derivative(Output);
        }
    }
}
