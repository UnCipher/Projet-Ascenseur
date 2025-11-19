using UnityEngine;

public class Pipe : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    [Header("Pipe Properties")]
    public PipeType pipeType;
    [Space(5)]

    [SerializeField] Vector2Int tilePosition;
    [SerializeField] PipeRotation pipeRotation;
    [Space(5)]

    [SerializeField] ActiveDirection activeDirection;
    [Space(5)]

    [SerializeField] bool isActive;
    [SerializeField] bool isAlwaysActive;
    [SerializeField] bool required;

    // Classes
    // ---------------------------

    public enum PipeType
    {
        OneWay,
        TwoWay,
        ThreeWay,
        FourWay,
    }

    public enum PipeRotation
    {
        Up,
        Right,
        Down,
        Left,
    }

    [System.Serializable]
    public class ActiveDirection
    {
        public bool upActive;
        public bool rightActive;
        public bool downActive;
        public bool leftActive;
    }

    // ---------------------------
    // Functions
    // ---------------------------

    public void CheckForConnections()
    {

    }

    public void RotatePipe()
    {

    }

    // Get Functions
    // ---------------------------

    public Vector2Int GetTilePosition()
    {
        // Return Value
        return tilePosition;
    }
}
