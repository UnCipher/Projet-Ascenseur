using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    [Header("References")]
    [SerializeField] int playerNumber;
    [SerializeField] bool isActive;
    [SerializeField] float desactivateTime;
    [SerializeField] bool desactivating;
    [Space(5)]

    [SerializeField] private PlayerHand leftHand;
    Wall.SelectedWall leftSelectedWall;
    [SerializeField] Vector2 leftUVOnWall;
    [Space(5)]

    [SerializeField] private PlayerHand rightHand;
    Wall.SelectedWall rightSelectedWall;
    [SerializeField] Vector2 rightUVOnWall;
    [Space(10)]
    
    public PlayerHandInfo handsInfo;

    // Classes
    // ---------------------------

    [System.Serializable]
    public class PlayerHandInfo
    {
        public Vector3 leftHandPos;
        public Vector3 rightHandPos;
    }

    // ---------------------------
    // Functions
    // ---------------------------

    void FixedUpdate()
    {
        // Call Functions
        ChangeHandsPositions();
    }
    
    public void ChangeHandsPositions()
    {
        if (isActive)
        {
            // Change Hands Positions
            leftHand.transform.localPosition = new Vector3(-handsInfo.leftHandPos.x, handsInfo.leftHandPos.y, handsInfo.leftHandPos.z);
            rightHand.transform.localPosition = new Vector3(-handsInfo.rightHandPos.x, handsInfo.rightHandPos.y, handsInfo.rightHandPos.z);
        }
    }

    public void RequestDesactivatePlayer()
    {
        if(!desactivating && isActive)
        StartCoroutine("DesactivatePlayer");
    }

    IEnumerator DesactivatePlayer()
    {
        // Set Values
        desactivating = true;

        // Wait For ?? Seconds
        yield return new WaitForSeconds(desactivateTime);

        // Desactivate
        isActive = false;
    }
    
    public void ActivatePlayer()
    {
        // Set Values
        isActive = true;

        // Cancel Desactivation
        if(desactivating)
        {
            StopAllCoroutines();
            desactivating = false;
        }
    }

    // Player Informations Functions
    // ---------------------------

    public void SetPlayerNumber(int value)
    {
        // Set Value
        playerNumber = value;
    }

    public int GetPlayerNumber()
    {
        // Return Value
        return playerNumber;
    }

    public PlayerHand GetLeftHand()
    {
        // Return Value
        return leftHand;
    }

    public PlayerHand GetRightHand()
    {
        // Return Value
        return rightHand;
    }
    
    public void SetPlayerActive(bool value)
    {
        // Set Value
        isActive = value;
    }

    public bool GetPlayerActive()
    {
        // Return Value
        return isActive;
    }

    public void SetLeftWallInfo(Vector2 uv, Wall.SelectedWall selected)
    {
        // Set Values
        leftUVOnWall = uv;
        leftSelectedWall = selected;
    }

    public Wall.WallInfo GetLeftWallInfo()
    {
        // Set Value
        Wall.WallInfo wallInfo = new Wall.WallInfo();

        wallInfo.uv = leftUVOnWall;
        wallInfo.selectedWall = leftSelectedWall;

        // Return Value
        return wallInfo;
    }
    
    public Wall.WallInfo GetRightWallInfo()
    {
        // Set Value
        Wall.WallInfo wallInfo = new Wall.WallInfo();

        wallInfo.uv = rightUVOnWall;
        wallInfo.selectedWall = rightSelectedWall;

        // Return Value
        return wallInfo;
    }
    
    public void SetRightWallInfo(Vector2 uv, Wall.SelectedWall selected)
    {
        // Set Values
        rightUVOnWall = uv;
        rightSelectedWall = selected;
    }
}