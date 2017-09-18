namespace NeuralNetLibrary.components.activators
{
    public interface ActivationFunction
    {
        double Equation(double weight);
        double Derivative(double weight);
        string GetName();
    }
}
