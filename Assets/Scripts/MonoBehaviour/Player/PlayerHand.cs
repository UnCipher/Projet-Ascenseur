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

    [Header("Interaction")]
    [SerializeField] float firstWaitTime;
    [SerializeField] float waitTime;
    float hoverTime;

    HandInteractable lastHoveredInteracble;
    bool firstTimeInteracted;

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
        // Set Values
        if (GetCurrentWall() && player.GetPlayerActive())
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

        LookForInteraction();
    }

    Wall GetCurrentWall()
    {
        // Clear Null Walls
        for (int i = 0; i < selectedWalls.Count; i++)
            if (selectedWalls[i] == null)
                selectedWalls.Remove(selectedWalls[i]);

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
    
    void LookForInteraction()
    {
        if (player.GetPlayerActive() && currentWall != Wall.SelectedWall.None)
        {
            // Set Values
            Camera camera = LevelManager.instance.GetWallCamera(currentWall);
            Ray ray = camera.ViewportPointToRay(wallPoint);

            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                if (hit.transform.GetComponent<HandInteractable>() != null)
                {
                    HandInteractable interactable = hit.transform.GetComponent<HandInteractable>();

                    if (interactable != lastHoveredInteracble)
                    {
                        lastHoveredInteracble = interactable;
                        firstTimeInteracted = false;
                        hoverTime = 0;
                    }

                    else
                    {
                        float currentWaitTime = firstWaitTime;

                        if (firstTimeInteracted)
                            currentWaitTime = waitTime;

                        if(hoverTime >= currentWaitTime)
                        {
                            interactable.OnHandHover();
                            hoverTime -= currentWaitTime;
                            firstTimeInteracted = true;
                        }
                    }

                    hoverTime += Time.fixedDeltaTime;
                    return;
                }
            }
        }

        // Else
        // Reset Values
        lastHoveredInteracble = null;
        firstTimeInteracted = false;
        hoverTime = 0;
    }

    // Collision Detections
    // ---------------------------

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Wall>())
        {
            // Set Values
            Wall collidingWall = other.GetComponent<Wall>();
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