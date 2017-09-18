using System;

namespace NeuralNetLibrary.components.activators
{
    [Serializable]
    public class LinearActivation : ActivationFunction
    {
        public double Equation(double weight)
        {
            return weight;
        }

        public double Derivative(double weight)
        {
            return 1;
        }

        public string GetName()
        {
            return "LinearActivation";
        }
    }
}
