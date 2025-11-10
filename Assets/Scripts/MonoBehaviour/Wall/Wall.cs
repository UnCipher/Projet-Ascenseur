using UnityEngine;

public class Wall : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    public Material material;
    [Header("Wall Properties")]
    [SerializeField] SelectedWall selectedWall;
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

    public enum SelectedWall
    {
        None,
        Right,
        Center,
        Left,
    }
    
    [System.Serializable]
    public class WallCoordinates
    {
        public Vector3 minPoint;
        public Vector3 maxPoint;
    }

    // ---------------------------
    // Functions
    // ---------------------------

    public Vector3 GetWallPoint(Vector3 handPos)
    {
        // Set Values
        Vector2 pointPos = Vector2.down;
        Vector2 point = Vector2.zero;

        float minX;
        float maxX;

        float minY;
        float maxY;

        // Get Correct Axis
        if (vector2Axis == Vector2Axis.XY)
        {
            pointPos = new Vector2(handPos.x, handPos.y);

            minX = wallCoordinates.minPoint.x;
            minY = wallCoordinates.minPoint.y;

            maxX = wallCoordinates.maxPoint.x;
            maxY = wallCoordinates.maxPoint.y;
        }

        else
        {
            pointPos = new Vector2(handPos.z, handPos.y);

            minX = wallCoordinates.minPoint.z;
            minY = wallCoordinates.minPoint.y;

            maxX = wallCoordinates.maxPoint.z;
            maxY = wallCoordinates.maxPoint.y;
        }

        // Normalize Value
        point.x = (pointPos.x - minX) / (maxX - minX);
        point.y = (pointPos.y - minY) / (maxY - minY);

        // Clamp Value
        point.x = ClampValue(point.x, 0, 1);
        point.y = ClampValue(point.y, 0, 1);

        // Reverse if Necessary
        if (reverseAxis)
            point.x = -point.x;

        return point;
    }

    float ClampValue(float value, float minValue, float maxValue)
    {
        // Set Values
        float newValue = value;

        // If Bigger than Max Value, or Smaller than Min Value
        if (value > maxValue)
            newValue = maxValue;

        if (value < minValue)
            newValue = minValue;

        // Return Value
        return newValue;
    }

    // Get Functions
    // ---------------------------

    public SelectedWall GetSelectedWall()
    {
        // Return This Wall ID
        return selectedWall;
    }
}