using System;
using System.Collections;
using System.Collections.Generic;

namespace NeuralNetLibrary.components.learning
{
    public class OutputConverter
    {
        private Dictionary<BitArray, int> dictionary;

        public OutputConverter()
        {
            dictionary = new Dictionary<BitArray, int>(new BitArrayComparer());
        }

        public void AddItem(BitArray output, int value)
        {
            dictionary.Add(output, value);
        }

        public double Convert(BitArray output)
        {
            int value;

            if (dictionary.TryGetValue(output, out value))
                return value;

            return double.NaN;
        }

        public double GetMostProbablyValue(double[] output)
        {
            double error;
            double smallestError = double.MaxValue;
            double bestValue = double.NaN;

            foreach (KeyValuePair<BitArray, int> entry in dictionary)
            {
                BitArray patternOutputs = entry.Key;
                error = 0;

                for (int i = 0; i < patternOutputs.Length; i++)
                {
                    if (patternOutputs[i])
                        error += 1.0 - output[i];
                    else
                        error += output[i];
                }                    

                if (error < smallestError)
                {
                    smallestError = error;
                    bestValue = entry.Value;
                }
            }

            return bestValue;
        }

        public double GetProbability(double[] output, BitArray pattern)
        {
            double totalError = 0;

            for (int i = 0; i < output.Length; i++)
            {
                if (pattern[i])
                    totalError += 1.0 - output[i];
                else
                    totalError += output[i];
            }

            double probability = 100 - (totalError * output.Length);
            return probability;
        }

        public BitArray GetOutputs(double value)
        {
            foreach (KeyValuePair<BitArray, int> entry in dictionary)
                if (entry.Value == value)
                    return entry.Key;

            return null;
        }

        public List<int> GetValues()
        {
            List<int> values = new List<int>();

            foreach (KeyValuePair<BitArray, int> entry in dictionary)
                values.Add(entry.Value);

            return values;
        }
    }
}
