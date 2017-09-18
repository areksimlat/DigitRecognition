using NeuralNetLibrary.components;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace DigitRecognition
{
    class KaggleDataHolder : DataHolder
    {
        private string path = "data/kaggle.csv";

        public KaggleDataHolder() : base(28, 28)
        {

        }

        public override void Load()
        {
            Items.Clear();

            using (StreamReader reader = new StreamReader(path))
            {
                reader.ReadLine();
                String line;

                while ((line = reader.ReadLine()) != null)
                {
                    String[] parts = line.Split(',');

                    BitArray inputs = new BitArray(784);
                    BitArray outputs = new BitArray(10);

                    outputs.Set(Byte.Parse(parts[0]), true);

                    for (int i = 0; i < 784; i++)
                        inputs[i] = (Byte.Parse(parts[i + 1]) > 127);

                    Items.Add(new DataItem(inputs, outputs));
                }
            }
        }

        private BitArray getDownscaledImage(byte[] srcBytes)
        {
            Bitmap srcBitmap = new Bitmap(28, 28, PixelFormat.Format8bppIndexed);
            BitmapData srcBitmapData = srcBitmap.LockBits(
                new Rectangle(0, 0, srcBitmap.Width, srcBitmap.Height),
                ImageLockMode.ReadWrite,
                srcBitmap.PixelFormat
                );
            Marshal.Copy(srcBytes, 0, srcBitmapData.Scan0, srcBytes.Length);
            srcBitmap.UnlockBits(srcBitmapData);

            Bitmap dstBitmap = new Bitmap(srcBitmap, new Size(16, 16));
            BitmapData dstBitmapData = dstBitmap.LockBits(
                new Rectangle(0, 0, dstBitmap.Width, dstBitmap.Height),
                ImageLockMode.ReadWrite,
                srcBitmap.PixelFormat
                );
            byte[] dstBytes = new byte[dstBitmapData.Height * dstBitmapData.Width];
            Marshal.Copy(dstBitmapData.Scan0, dstBytes, 0, dstBytes.Length);
            dstBitmap.UnlockBits(dstBitmapData);

            BitArray dstBitArray = new BitArray(dstBytes.Length);
            
            for (int i = 0; i < dstBytes.Length; i++)
                dstBitArray[i] = (dstBytes[i] > 127);

            return dstBitArray;
        }
    }
}
