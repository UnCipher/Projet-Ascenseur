using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    [SerializeField] List<Wall> selectedWalls = new List<Wall>();
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
        if (selectedWalls.Count > 0)
        {
            // Set Values
            Wall wall = selectedWalls[0];
            float shortestDistance = Vector3.Distance(transform.position, selectedWalls[0].transform.position);

            for (int i = 0; i < selectedWalls.Count; i++)
                if (Vector3.Distance(transform.position, selectedWalls[i].transform.position) < shortestDistance)
                    wall = selectedWalls[i];

            // Return Value
            return wall;
        }

        // Else
        return null;
    }
    
    public Wall.SelectedWall GetSelectedWall()
    {
        // Return Value
        return currentWall;
    }

    // Collision Detections
    // ---------------------------

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Wall>())
        {
            // Set Values
            Wall collidingWall = other.GetComponent<Wall>();
            Debug.Log("works?");
            selectedWalls.Add(collidingWall);
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