using UnityEngine;

[CreateAssetMenu(fileName ="PipeMiniGameProfile_00", menuName ="ScriptableObject/Pipe Mini Game Profile")]
public class PipeMiniGameProfile : ScriptableObject
{
    // ---------------------------
    // Values
    // ---------------------------

    [Header("Pipes' Profile")]
    public float gameDuration;
    public PipeProperties[] pipes;

    // Classes
    // ---------------------------

    public enum SelectedWall
    {
        Left,
        Center,
        Right,
    }

    [System.Serializable]
    public class PipeProperties
    {
        public Pipe.PipeType pipeType;
        public SelectedWall wall;
        [Space(5)]

        public Vector2Int tilePosition;
        public Vector2Int visualTilePosition;
        public Pipe.PipeRotation pipeRotation;
        [Space(5)]

        public bool isAlwaysActive;
        public bool required;
    }

}
