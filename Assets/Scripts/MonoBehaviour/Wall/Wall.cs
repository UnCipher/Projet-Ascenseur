using UnityEngine;

public class Wall : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    public Material material;
    [Header("Wall Properties")]
    [SerializeField] Vector2Axis vector2Axis;
    [SerializeField] bool reverseAxis;
    [Space(5)]

    [SerializeField] WallCoordinates wallCoordinates = new WallCoordinates();

    public enum Vector2Axis
    {
        XY,
        ZY,
    }

    // Classes
    // ---------------------------

    [System.Serializable]
    public class WallCoordinates
    {
        public Vector3 minPoint;
        public Vector3 maxPoint;
    }

    // ---------------------------
    // Functions
    // ---------------------------

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public Vector3 GetWallPoint(Vector3 handPos)
    {
        // Set Values
        Vector2 point = Vector2.zero;

        // Get Correct Axis
        if (vector2Axis == Vector2Axis.XY)
            point = new Vector2(handPos.x, handPos.y);

        else
            point = new Vector2(handPos.x, handPos.y);

        // Reverse if Necessary
        if (reverseAxis)
            point.x = -point.x;

        // Normalize
        Vector2 normalizedPoint = point.normalized;

        Debug.Log("Default : " + point + " / Normalized : " + normalizedPoint);

        return normalizedPoint;
    }
}
