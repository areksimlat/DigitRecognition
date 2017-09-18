using System;

namespace NeuralNetLibrary.components.activators
{
    [Serializable]
    public class ThresholdActivation : ActivationFunction
    {
        private double threshold;

        public ThresholdActivation(double threshold)
        {
            this.threshold = threshold;
        }

        public double Equation(double weight)
        {
            if (weight > threshold)
                return 1;

            return 0;

        }

        public double Derivative(double weight)
        {
            return 0;
        }

        public string GetName()
        {
            return "ThresholdActivation";
        }
    }
}
