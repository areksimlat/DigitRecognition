using System.Collections;
using System.Collections.Generic;

namespace NeuralNetLibrary.components.learning
{
    public class BitArrayComparer : IEqualityComparer<BitArray>
    {
        public bool Equals(BitArray x, BitArray y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }

            for (int i = 0; i < x.Length; i++)
                if (x[i] != y[i])
                    return false;

            return true;
        }

        public int GetHashCode(BitArray obj)
        {
            int result = 17;

            for (int i = 0; i < obj.Length; i++)
            {
                unchecked
                {
                    int objValue = obj[i] ? 1 : 0;
                    result = (int)(result * 23 + objValue);
                }
            }
            return result;
        }
    }
}
