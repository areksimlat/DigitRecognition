using NeuralNetLibrary.components.activators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NeuralNetLibrary.components
{
    [Serializable]
    public class NeuralNetwork
    {
        public List<Layer> Layers { get; private set; }


        public NeuralNetwork()
        {
            Layers = new List<Layer>();
        }

        public void CreateLayer(ActivationFunction activator, int numberOfNeurons, bool createBias)
        {
            Layer previousLayer = Layers.Count == 0 ? null : Layers[Layers.Count - 1];
            Layers.Add(new Layer(previousLayer, activator, numberOfNeurons, createBias));
        }

        public void SetInputs(BitArray inputValues)
        {
            Layer inputLayer = Layers[0];
            List<Neuron> inputNeurons = inputLayer.Neurons;
            int numberOfInputs = inputLayer.GetNeuronsCountWithoutBias();

            if (inputValues.Length != numberOfInputs)
                throw new ArgumentOutOfRangeException();

            for (int i = 0; i < numberOfInputs; i++)
                inputNeurons[i].Output = inputValues.Get(i) ? 1 : 0;
        }

        public void Propagate()
        {
            for (int i = 1; i < Layers.Count; i++)
                Layers[i].Propagate(Layers[i - 1]);
        }

        public double[] GetOutputs()
        {
            return Layers[Layers.Count - 1].GetNeuronOutputs();
        }

        public static bool Save(NeuralNetwork neuralNetwork, string filename)
        {
            if (neuralNetwork == null)
                return false;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, neuralNetwork);
                memoryStream.Flush();
                memoryStream.Position = 0;
                File.WriteAllBytes(filename, memoryStream.ToArray());
            }

            return true;
        }        

        public static NeuralNetwork Load(string filename)
        {
            byte[] bytes = File.ReadAllBytes(filename);

            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (NeuralNetwork)binaryFormatter.Deserialize(memoryStream);
            }
        }
    }
}
