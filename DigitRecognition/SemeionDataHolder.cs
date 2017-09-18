using NeuralNetLibrary.components;
using System;
using System.Collections;
using System.IO;

namespace DigitRecognition
{
    class SemeionDataHolder : DataHolder
    {
        private string path = "data/semeion.data";

        public SemeionDataHolder() : base(16, 16)
        {

        }        

        public override void Load()
        {
            Items.Clear();

            using (StreamReader reader = new StreamReader(path))            
            {
                String line;

                while ((line = reader.ReadLine()) != null)
                {
                    String[] parts = line.Split(' ');

                    BitArray inputs = new BitArray(256);
                    BitArray outputs = new BitArray(10);

                    for (int i = 0; i < 256; i++)
                        inputs[i] = (Double.Parse(parts[i].Replace('.', ',')) == 1);

                    for (int i = 0; i < 10; i++)
                        outputs[i] = (Double.Parse(parts[256 + i].Replace('.', ',')) == 1);

                    Items.Add(new DataItem(inputs, outputs));
                }
            }
        }
    }
}
