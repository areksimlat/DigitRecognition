using NeuralNetLibrary.components;
using NeuralNetLibrary.components.learning;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DigitRecognition
{
    public abstract class DataHolder
    {
        public int ImageHeight { get; }
        public int ImageWidth { get; }
        public List<DataItem> Items { get; }

        public DataHolder(int imageHeight, int imageWidth)
        {
            ImageHeight = imageHeight;
            ImageWidth = imageWidth;
            Items = new List<DataItem>();
        }

        public abstract void Load();        

        public void Shuffle()
        {
            Random rand = new Random();

            for (int i = 0; i < Items.Count; i++)
            {
                int randIndex = rand.Next(Items.Count);

                DataItem temp = Items[i];
                Items[i] = Items[randIndex];
                Items[randIndex] = temp;
            }
        }

        public void Separate(int learningPercent, int validatePercent, int testingPercent)
        {
            int allCount = Items.Count;
            int learningCount = (int)Math.Round((learningPercent / 100.0) * allCount);
            int validateCount = (int)Math.Round((validatePercent / 100.0) * allCount);
            int testingCount = (int)Math.Round((testingPercent / 100.0) * allCount);

            int i = 0;

            for (; i < learningCount; i++)
                Items[i].dataType = DataItem.DataTypes.Learning;

            for (; i < learningCount + validateCount; i++)
                Items[i].dataType = DataItem.DataTypes.Validate;

            for (; i < learningCount + validateCount + testingCount; i++)
                Items[i].dataType = DataItem.DataTypes.Testing;

            for (; i < allCount; i++)
                Items[i].dataType = DataItem.DataTypes.None;
        }

        public List<BitArray> GetDistinctOutputs()
        {
            List<BitArray> distinctOutputs = new List<BitArray>();
            BitArrayComparer comparer = new BitArrayComparer();

            foreach (DataItem item in Items)
            {
                bool itemExsist = false;

                foreach (BitArray distinctItem in distinctOutputs)
                {
                    if (comparer.Equals(item.Outputs, distinctItem))
                    {
                        itemExsist = true;
                        break; 
                    } 
                }

                if (!itemExsist)
                    distinctOutputs.Add(item.Outputs);
            }

            return distinctOutputs;
        }

        public Dictionary<int, BitArray> GetTranslateOutputs(List<BitArray> list)
        {
            Dictionary<int, BitArray> dictonary = new Dictionary<int, BitArray>();

            foreach (BitArray bitArray in list)
            {
                for (int i = 0; i < bitArray.Length; i++)
                {
                    if (bitArray[i])
                    {
                        int value = i;
                        dictonary.Add(value, bitArray);
                        break;
                    }
                }

            }

            return dictonary;
        }
    }
}
