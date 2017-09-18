using System.Collections;

namespace NeuralNetLibrary.components
{
    public class DataItem
    {
        public enum DataTypes
        {
            None,
            Learning,
            Validate,
            Testing
        }

        public BitArray Inputs { get; }
        public BitArray Outputs { get; }
        public DataTypes dataType { get; set; }

        public DataItem(BitArray inputs, BitArray outputs)
        {
            Inputs = inputs;
            Outputs = outputs;
        }

        public double[] GetDoubleOutputs()
        {
            double[] doubleOutputs = new double[Outputs.Length];

            for (int i = 0; i < Outputs.Length; i++)
                doubleOutputs[i] = Outputs.Get(i) ? 1 : 0;

            return doubleOutputs;
        }
    }
}
