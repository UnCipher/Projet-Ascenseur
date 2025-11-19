using UnityEngine;

public class PipeGameController : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    [Header("Pipe Controller Properties")]

    [Header("References")]
    Pipe[] pipes;

    // ---------------------------
    // Functions
    // ---------------------------

    public void UpdatePipes(Pipe priority)
    {

    }

    public void UpdatePipes()
    {
        
    }

    // Get Functions
    // ---------------------------

    public Pipe GetPipeAt(Vector2Int coordinate)
    {
        // Set Value
        Pipe pipe = null;

        // Find Pipe
        for(int i = 0;i<pipes.Length;i++)
        if(pipes[i].GetTilePosition() == coordinate)
        pipe = pipes[i];

        // Return Value
        return pipe;
    }
}
