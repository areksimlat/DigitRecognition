namespace NeuralNetLibrary.algorithms
{
    public interface BackpropagationAlgorithm
    {
        int GetCurrentEpoch();
        double GetCurrentError();
        bool Train();
        void Abort();        
    }
}
