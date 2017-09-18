using NeuralNetLibrary.components;
using NeuralNetLibrary.components.activators;
using System;
using System.Collections.Generic;

namespace NeuralNetLibrary
{
    [Serializable]
    public class Layer
    {
        public List<Neuron> Neurons { get; private set; }


        public Layer(Layer previousLayer, ActivationFunction activator, int numberOfNeurons, bool createBias)
        {
            Neurons = new List<Neuron>();

            int numberOfInputs = previousLayer == null ? 0 : previousLayer.Neurons.Count;

            for (int i = 0; i < numberOfNeurons; i++)
                Neurons.Add(Neuron.CreateNeuron(activator, numberOfInputs));

            if (createBias)
                Neurons.Add(Neuron.CreateBias(activator));
        }

        public int GetNeuronsCountWithoutBias()
        {
            if (Neurons[Neurons.Count - 1].IsBias)
                return Neurons.Count - 1;

            return Neurons.Count;
        }

        public double[] GetNeuronOutputs()
        {
            double[] neuronOutputs = new double[Neurons.Count];

            for (int i = 0; i < Neurons.Count; i++)
                neuronOutputs[i] = Neurons[i].Output;

            return neuronOutputs;
        }

        public void Propagate(Layer previousLayer)
        {
            double[] sourceOutputs = previousLayer.GetNeuronOutputs();
            int numberOfNeurons = GetNeuronsCountWithoutBias();

            for (int i = 0; i < numberOfNeurons; i++)
                Neurons[i].Activate(sourceOutputs);
        }
    }
}
