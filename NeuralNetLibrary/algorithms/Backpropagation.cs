using NeuralNetLibrary.components;
using NeuralNetLibrary.components.learning;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NeuralNetLibrary.algorithms
{
    public class Backpropagation : BackpropagationAlgorithm
    {
        private NeuralNetwork neuralNetwork;
        private List<DataItem> dataItems;
        private double learningRate;
        private double momentum;
        private double errorThreshold;
        private RandomOrder randomLearningOrder;
        private RandomOrder randomValidateOrder;
        private RandomOrder randomCurrentOrder;
        public double CurrentError { get; private set; }
        public int CurrentEpoch { get; private set; }
        public int ValidatePeriod { get; set; }
        public int RemainEpochToValidate { get; private set; }        

        public Backpropagation(NeuralNetwork neuralNetwork, List<DataItem> dataItems, 
            double learningRate, double momentum, double errorThreshold)
        {
            this.neuralNetwork = neuralNetwork;
            this.dataItems = dataItems;
            this.learningRate = learningRate;
            this.momentum = momentum;
            this.errorThreshold = errorThreshold;

            CurrentError = double.MaxValue;
            CurrentEpoch = 0;
            ValidatePeriod = 50;
            RemainEpochToValidate = ValidatePeriod;

            randomLearningOrder = new RandomOrder(dataItems, DataItem.DataTypes.Learning);
            randomValidateOrder = new RandomOrder(dataItems, DataItem.DataTypes.Validate);
            randomCurrentOrder = randomLearningOrder;
        }

        public int GetCurrentEpoch()
        {
            return CurrentEpoch;
        }

        public double GetCurrentError()
        {
            return CurrentError;
        }

        public bool Train()
        {
            if (CurrentError > errorThreshold)
            {
                if (RemainEpochToValidate == 0)
                {
                    RemainEpochToValidate = ValidatePeriod;
                    randomCurrentOrder = randomValidateOrder;
                }
                else
                {
                    randomCurrentOrder = randomLearningOrder;
                }

                CurrentEpoch++;
                RemainEpochToValidate--;

                List<int> indexOrder = randomCurrentOrder.GetShuffledOrder();
                CurrentError = backpropagate(indexOrder);
            }

            return (CurrentError < errorThreshold);
        }

        public void Abort()
        {

        }

        private double backpropagate(List<int> indexOrder)
        {
            Dictionary<Neuron, double[]> weightsDictionary = new Dictionary<Neuron, double[]>();
            double error = 0;
            
            foreach (int index in indexOrder)
            {
                DataItem dataItem = dataItems[index];

                neuralNetwork.SetInputs(dataItem.Inputs);
                neuralNetwork.Propagate();
                double[] netOutputs = neuralNetwork.GetOutputs();
                double[] expectedOutputs = dataItem.GetDoubleOutputs();

                List<Layer> layers = neuralNetwork.Layers;
                List<Neuron> outputNeurons = layers[layers.Count - 1].Neurons;

                // STEP 1: 
                // Calculate error
                for (int i = 0; i < outputNeurons.Count; i++)
                    outputNeurons[i].OutputError = netOutputs[i] - expectedOutputs[i];

                for (int i = layers.Count - 1; i > 1; i--)
                {
                    List<Neuron> prevNeurons = layers[i - 1].Neurons;
                    List<Neuron> currNeurons = layers[i].Neurons;

                    for (int j = 0; j < prevNeurons.Count; j++)
                    {
                        double neuronError = 0;

                        foreach (Neuron currNeuron in currNeurons)
                            if (!currNeuron.IsBias)
                                neuronError += currNeuron.OutputError * currNeuron.InputWeights[j];

                        prevNeurons[j].OutputError = neuronError;
                    }
                }

                // STEP 2: 
                // Correrct weights
                for (int i = 1; i < layers.Count; i++)
                {
                    List<Neuron> prevNeurons = layers[i - 1].Neurons;

                    foreach (Neuron currNeuron in layers[i].Neurons)
                    {
                        if (!currNeuron.IsBias)
                        {
                            double[] prevWeights = null;
                            double[] newWeights = new double[prevNeurons.Count];

                            weightsDictionary.TryGetValue(currNeuron, out prevWeights);

                            for (int j = 0; j < prevNeurons.Count; j++)
                            {
                                double delta = learningRate * currNeuron.OutputError * currNeuron.Derivative * prevNeurons[j].Output;
                                double prevDelta = prevWeights == null ? 0 : prevWeights[j] - currNeuron.InputWeights[j];
                                double momentumDelta = prevDelta * momentum;
                                newWeights[j] = currNeuron.InputWeights[j] - (delta + momentumDelta);
                            }

                            if (prevWeights == null)
                                weightsDictionary.Add(currNeuron, currNeuron.InputWeights);
                            else
                                weightsDictionary[currNeuron] = currNeuron.InputWeights;

                            currNeuron.InputWeights = newWeights;
                        }
                    }
                }

                neuralNetwork.Propagate();
                netOutputs = neuralNetwork.GetOutputs();
                error += calculateError(netOutputs, expectedOutputs);
            }

            return error;
        }

        private double calculateError(double[] currOutput, double[] expectOutput)
        {
            double sum = 0;

            for (int i = 0; i < currOutput.Length; i++)
                sum += (expectOutput[i] - currOutput[i]) * (expectOutput[i] - currOutput[i]);

            return sum / 2;
        }
    }
}
