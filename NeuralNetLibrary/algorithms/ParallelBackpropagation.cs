using NeuralNetLibrary.components;
using NeuralNetLibrary.components.learning;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NeuralNetLibrary.algorithms
{
    public class ParallelBackpropagation : BackpropagationAlgorithm
    {
        private NeuralNetwork neuralNetwork;
        private List<DataItem> dataItems;
        private double learningRate;
        private double momentum;
        private double errorThreshold;
        private RandomOrder randomLearningOrder;
        private RandomOrder randomValidateOrder;
        private RandomOrder randomCurrentOrder;
        private Task[] threads;
        private Barrier barrier;
        private SemaphoreSlim semaphore;
        private ManualResetEvent[] threadEvents;
        private bool stopThreads;
        private int numberOfThreads;
        public double CurrentError { get; private set; }
        public int CurrentEpoch { get; private set; }
        public int ValidatePeriod { get; set; }
        public int RemainEpochToValidate { get; private set; }

        public ParallelBackpropagation(NeuralNetwork neuralNetwork, List<DataItem> dataItems,
            double learningRate, double momentum, double errorThreshold, int numberOfThreads)
        {
            this.neuralNetwork = neuralNetwork;
            this.dataItems = dataItems;
            this.learningRate = learningRate;
            this.momentum = momentum;
            this.errorThreshold = errorThreshold;

            CurrentError = double.MaxValue;
            CurrentEpoch = 0;
            ValidatePeriod = 20;
            RemainEpochToValidate = ValidatePeriod;

            randomLearningOrder = new RandomOrder(dataItems, DataItem.DataTypes.Learning);
            randomValidateOrder = new RandomOrder(dataItems, DataItem.DataTypes.Validate);
            randomCurrentOrder = randomLearningOrder;

            this.numberOfThreads = numberOfThreads;
            barrier = new Barrier(numberOfThreads);
            semaphore = new SemaphoreSlim(0);
            threads = new Task[numberOfThreads];
            threadEvents = new ManualResetEvent[numberOfThreads];

            stopThreads = false;

            for (int i = 0; i < numberOfThreads; i++)
            {
                int threadId = i;
                threads[i] = new Task(() => { backpropagateThread(threadId); });
                threads[i].Start();

                threadEvents[i] = new ManualResetEvent(false);
            }
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
            stopThreads = true;
            semaphore.Release();
        }

        private double calculateError(double[] currOutput, double[] expectOutput)
        {
            double sum = 0;

            for (int i = 0; i < currOutput.Length; i++)
                sum += (expectOutput[i] - currOutput[i]) * (expectOutput[i] - currOutput[i]);

            return sum / 2;
        }

        private double backpropagate(List<int> indexOrder)
        {
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

                for (int i = 0; i < outputNeurons.Count; i++)
                    outputNeurons[i].OutputError = expectedOutputs[i] - netOutputs[i];

                semaphore.Release(numberOfThreads);
                WaitHandle.WaitAll(threadEvents);

                neuralNetwork.Propagate();
                netOutputs = neuralNetwork.GetOutputs();
                error += calculateError(netOutputs, expectedOutputs);                  
            }

            if (error < errorThreshold)
            {
                stopThreads = true;
                semaphore.Release(numberOfThreads);
            }

            return error;
        }            

        private void backpropagateThread(int threadId)
        {
            List<Layer> layers = neuralNetwork.Layers;

            while (true)
            {
                semaphore.Wait();

                if (stopThreads)
                    break;

                for (int i = layers.Count - 1; i > 1; i--)
                {
                    List<Neuron> prevNeurons = layers[i - 1].Neurons;
                    List<Neuron> currNeurons = layers[i].Neurons;

                    int partSize = prevNeurons.Count / numberOfThreads;
                    int startIndex =  partSize * threadId;
                    int endIndex = (threadId != (numberOfThreads - 1)) ? (startIndex + partSize) : layers.Count;

                    for (int j = startIndex; j < endIndex; j++)
                    {
                        double neuronError = 0;

                        foreach (Neuron currNeuron in currNeurons)
                            if (!currNeuron.IsBias)
                                neuronError += currNeuron.OutputError * currNeuron.InputWeights[j];

                        prevNeurons[j].OutputError = neuronError;
                    }

                    barrier.SignalAndWait();
                }

                Dictionary<Neuron, double[]> weightsDictionary = new Dictionary<Neuron, double[]>();

                // STEP 2: 
                // Correrct weights
                for (int i = 1; i < layers.Count; i++)
                {
                    List<Neuron> prevNeurons = layers[i - 1].Neurons;

                    int partSize = layers[i].Neurons.Count / numberOfThreads;
                    int startIndex = partSize * threadId;
                    int endIndex = (threadId != (numberOfThreads - 1)) ? (startIndex + partSize) : layers[i].Neurons.Count;

                    for (int j = startIndex; j < endIndex; j++)
                    {
                        Neuron currNeuron = layers[i].Neurons[j];

                        if (!currNeuron.IsBias)
                        {
                            double[] prevWeights = null;
                            weightsDictionary.TryGetValue(currNeuron, out prevWeights);

                            for (int k = 0; k < prevNeurons.Count; k++)
                            {
                                double delta = learningRate * currNeuron.OutputError * currNeuron.Derivative * prevNeurons[k].Output;
                                double prevDelta = prevWeights == null ? 0 : currNeuron.InputWeights[k] - prevWeights[k];
                                double momentumDelta = prevDelta * momentum;
                                double weight = currNeuron.InputWeights[k] + delta + momentumDelta;

                                currNeuron.InputWeights[k] = weight;
                            }

                            weightsDictionary.Add(currNeuron, currNeuron.InputWeights);
                        }
                    }

                    barrier.SignalAndWait();
                }

                threadEvents[threadId].Set();
            }
        }        
    }
}
