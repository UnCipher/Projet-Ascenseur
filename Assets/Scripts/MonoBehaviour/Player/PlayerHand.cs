using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    List<Wall> selectedWalls = new List<Wall>();
    Wall.SelectedWall currentWall;
    Vector2 wallPoint;

    [Header("References")]
    [SerializeField] Player player;
    [SerializeField] Hand hand;
    [Space(5)]

    [SerializeField] MeshRenderer mesh;
    [SerializeField] Material defaultMat;

    // Classes
    // ---------------------------

    public enum Hand
    {
        IDK,
        Left,
        Right,
    }

    // ---------------------------
    // Functions
    // ---------------------------

    void Start()
    {
        if (!player)
            player = transform.parent.GetComponent<Player>();
    }

    void FixedUpdate()
    {
        if (player.GetPlayerActive())
        {
            // Set Values
            if (GetCurrentWall())
            {
                Wall wall = GetCurrentWall();
                wallPoint = wall.GetWallPoint(transform.position);
                currentWall = wall.GetSelectedWall();
            }

            else
                currentWall = Wall.SelectedWall.None;

            // Call Function
            if (hand == Hand.Left)
                player.SetLeftWallInfo(wallPoint, currentWall);

            else
                player.SetRightWallInfo(wallPoint, currentWall);
        }
    }
    
    Wall GetCurrentWall()
    {
        // Set Values
        Wall wall = null;
        float shortestDistance = 0;

        for (int i = 0; i < selectedWalls.Count; i++)
            if (Vector3.Distance(transform.position, selectedWalls[i].transform.position) < shortestDistance)
                wall = selectedWalls[i];

        // Return Value
        return wall;
    }

    // Collision Detections
    // ---------------------------

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Wall>())
        {
            // Set Values
            Wall collidingWall = other.GetComponent<Wall>();
            selectedWalls.Insert(0, collidingWall);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Wall>())
        {
            // Set Values
            Wall collidingWall = other.GetComponent<Wall>();
            selectedWalls.Remove(collidingWall);
        }
    }
}
