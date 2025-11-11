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
        public bool leftHandClosed;
        public bool leftHandClosedOnLastFrame;
        [Space(5)]

        public Vector3 leftHandPos;
        public Vector3 leftFingerPos;
        public float leftDistanceToBeClosed;
        [Space(10)]

        public bool rightHandClosed;
        public bool rightHandClosedOnLastFrame;
        [Space(5)]

        public Vector3 rightHandPos;
        public Vector3 rightFingerPos;
        public float rightDistanceToBeClosed;
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
            leftHand.transform.localPosition = handsInfo.leftHandPos;
            rightHand.transform.localPosition = handsInfo.rightHandPos;

            // Change Closed State
            // Left
            if (handsInfo.leftHandClosed)
                handsInfo.leftHandClosedOnLastFrame = true;

            else
                handsInfo.leftHandClosedOnLastFrame = false;

            handsInfo.leftHandClosed = Vector3.Distance(handsInfo.leftFingerPos, handsInfo.leftHandPos) <= handsInfo.leftDistanceToBeClosed;

            // Right
            if (handsInfo.rightHandClosed)
                handsInfo.rightHandClosedOnLastFrame = true;

            else
                handsInfo.rightHandClosedOnLastFrame = false;

            handsInfo.rightHandClosed = Vector3.Distance(handsInfo.rightFingerPos, handsInfo.rightHandPos) <= handsInfo.rightDistanceToBeClosed;
        }
    }

    public void RequestDesactivatePlayer()
    {
        if(!desactivating && isActive)
        StartCoroutine("DesactivatePlayer");
    }

    public IEnumerator DesactivatePlayer()
    {
        // Set Values
        desactivating = true;
        Debug.Log("Desactivating Player " + playerNumber);

        // Wait For ?? Seconds
        yield return new WaitForSeconds(desactivateTime);

        // Desactivate
        Debug.Log("Desactivated Player " + playerNumber);
        isActive = false;
    }
    
    public void ActivatePlayer()
    {
        // Set Values
        isActive = true;
        Debug.Log("Activated Player " + playerNumber);

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

        //  Debug.Log("Player " + playerNumber + "'s left Hand is on the " + leftSelectedWall + " which coordinates are " + leftUVOnWall);
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

       // Debug.Log("Player " + playerNumber + "'s right Hand is on the " + rightSelectedWall + " which coordinates are " + rightUVOnWall);
    }
}