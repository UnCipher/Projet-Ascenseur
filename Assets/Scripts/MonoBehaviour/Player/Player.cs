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
    Vector2 leftUVOnWall;
    [Space(5)]

    [SerializeField] private PlayerHand rightHand;
    Wall.SelectedWall rightSelectedWall;
    Vector2 rightUVOnWall;
    [Space(10)]

    public PlayerHandInfo handsInfo;

    // Classes
    // ---------------------------

    [System.Serializable]
    public class PlayerHandInfo
    {
        public float leftHandPosX;
        public float leftHandPosY;
        public float leftHandPosZ;
        public bool leftHandClosed;

        public float rightHandPosX;
        public float rightHandPosY;
        public float rightHandPosZ;
        public bool rightHandClosed;
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
            // Set Values
            Vector3 leftPos = new Vector3(handsInfo.leftHandPosX, handsInfo.leftHandPosY, handsInfo.leftHandPosZ);
            Vector3 rightPos = new Vector3(handsInfo.rightHandPosX, handsInfo.rightHandPosY, handsInfo.rightHandPosZ);

            leftHand.transform.localPosition = leftPos;
            rightHand.transform.localPosition = rightPos;
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

        Debug.Log("Player " + playerNumber + "'s left Hand is on the " + leftSelectedWall + " which coordinates are " + leftUVOnWall);
    }
    
    public void SetRightWallInfo(Vector2 uv, Wall.SelectedWall selected)
    {
        // Set Values
        rightUVOnWall = uv;
        rightSelectedWall = selected;

        Debug.Log("Player " + playerNumber + "'s right Hand is on the " + rightSelectedWall + " which coordinates are " + rightUVOnWall);
    }
}