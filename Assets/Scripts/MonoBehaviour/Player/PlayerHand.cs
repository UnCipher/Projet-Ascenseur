using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    List<Wall> selectedWalls = new List<Wall>();
    Wall currentWall;

    [Header("References")]
    [SerializeField] MeshRenderer mesh;
    [SerializeField] Material defaultMat;

    // ---------------------------
    // Functions
    // ---------------------------

    void FixedUpdate()
    {
        Vector2 t;
        if(currentWall)
        {
            t = currentWall.GetWallPoint(transform.position);

            LevelManager.instance.SendMessage("handPosX", t.x);
            LevelManager.instance.SendMessage("handPosY", t.y);
        }
    }

    void UpdateWallChange()
    {
        if(selectedWalls.Count > 0)
        {
            currentWall = selectedWalls[0];
            mesh.material = currentWall.material;
            
        }

        else
        {
            currentWall = null;
            mesh.material = defaultMat;
        }
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

            // Call Function
            UpdateWallChange();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Wall>())
        {
            // Set Values
            Wall collidingWall = other.GetComponent<Wall>();
            selectedWalls.Remove(collidingWall);

            // Call Function
            UpdateWallChange();
        }
    }
}
