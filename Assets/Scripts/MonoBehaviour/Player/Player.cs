using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    [Header("References")]
    [SerializeField] private PlayerHand leftHand;
    [SerializeField] private PlayerHand rightHand;

    // ---------------------------
    // Functions
    // ---------------------------

    void Start()
    {
        // Call Functions
        LevelManager.instance.player1HandInfo += ChangeHandsPositions;
    }

    void ChangeHandsPositions(object sender, LevelManager.PlayerHandInfoArgs args)
    {
        // Set Values
        Vector3 leftPos = new Vector3(args.leftHandPosX, args.leftHandPosY, args.leftHandPosZ);
        Vector3 rightPos = new Vector3(args.rightHandPosX, args.rightHandPosY, args.rightHandPosZ);

/*
        leftHand.transform.localPosition = leftPos;
        rightHand.transform.localPosition = rightPos;
        */
    }
}
