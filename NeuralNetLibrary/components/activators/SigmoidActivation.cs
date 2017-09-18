using System;

namespace NeuralNetLibrary.components.activators
{
    [Serializable]
    public class SigmoidActivation : ActivationFunction
    {
        public double Equation(double weight)
        {
            return 1 / (1 + Math.Exp(-weight));
        }

        public double Derivative(double weight)
        {
            return weight * (1 - weight);
        }

        public string GetName()
        {
            return "SigmoidActivation";
        }
    }
}
