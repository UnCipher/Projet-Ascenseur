using System;
using UnityEngine;

[RequireComponent(typeof(OSC))]
public class LevelManager : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    [HideInInspector]
    public static LevelManager instance;
    public PlayerHandInfo player1 = new PlayerHandInfo();
    public PlayerHandInfo player2 = new PlayerHandInfo();
    public PlayerHandInfo player3 = new PlayerHandInfo();
    public PlayerHandInfo player4 = new PlayerHandInfo();

    [Header("References")]
    OSC osc;
    public float test;
    // Events
    // ---------------------------

    public EventHandler<PlayerHandInfoArgs> player1HandInfo;

    public class PlayerHandInfoArgs : EventArgs
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

    // Classes
    // ---------------------------

    [Serializable]
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

    void Awake()
    {
        // Set Values
        if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
        instance = this;
    }

    void Start()
    {
        // Set Values
        osc = GetComponent<OSC>();

        // Call Functions
        InitiateOSCMessages();
    }

    void FixedUpdate()
    {
        // Call Events
        player1HandInfo?.Invoke(this, new PlayerHandInfoArgs
        {
            leftHandPosX = player1.leftHandPosX,
            leftHandPosY = player1.leftHandPosY,
            leftHandPosZ = player1.leftHandPosZ,

            rightHandPosX = player1.rightHandPosX,
            rightHandPosY = player1.rightHandPosY,
            rightHandPosZ = player1.rightHandPosZ,
        });
    }

    // OSC Functions
    // ---------------------------

    void InitiateOSCMessages()
    {
        // Receive Messages
        // P1
        osc.SetAddressHandler("/p1-lHx", P1LHandX);
        osc.SetAddressHandler("/p1-lHy", P1LHandY);
        osc.SetAddressHandler("/p1-lHz", P1LHandZ);

        osc.SetAddressHandler("/p1-rHx", P1RHandX);
        osc.SetAddressHandler("/p1-rHy", P1RHandY);
        osc.SetAddressHandler("/p1-rHz", P1RHandZ);

        // P2
        osc.SetAddressHandler("/p2-lHx", P2LHandX);
        osc.SetAddressHandler("/p2-lHy", P2LHandY);
        osc.SetAddressHandler("/p2-lHz", P2LHandZ);

        osc.SetAddressHandler("/p2-rHx", P2RHandX);
        osc.SetAddressHandler("/p2-rHy", P2RHandY);
        osc.SetAddressHandler("/p2-rHz", P2RHandZ);

        // P3
        osc.SetAddressHandler("/p3-lHx", P3LHandX);
        osc.SetAddressHandler("/p3-lHy", P3LHandY);
        osc.SetAddressHandler("/p3-lHz", P3LHandZ);

        osc.SetAddressHandler("/p3-rHx", P3RHandX);
        osc.SetAddressHandler("/p3-rHy", P3RHandY);
        osc.SetAddressHandler("/p3-rHz", P3RHandZ);

        // P4
        osc.SetAddressHandler("/p4-lHx", P4LHandX);
        osc.SetAddressHandler("/p4-lHy", P4LHandY);
        osc.SetAddressHandler("/p4-lHz", P4LHandZ);

        osc.SetAddressHandler("/p4-rHx", P4RHandX);
        osc.SetAddressHandler("/p4-rHy", P4RHandY);
        osc.SetAddressHandler("/p4-rHz", P4RHandZ);
    }

    // Player 1
    // ---------------------------

    void P1LHandX(OscMessage osc)
    {
        player1.leftHandPosX = osc.GetFloat(0);
    }

    void P1LHandY(OscMessage osc)
    {
        player1.leftHandPosY = osc.GetFloat(0);
    }

    void P1LHandZ(OscMessage osc)
    {
        player1.leftHandPosZ = osc.GetFloat(0);
    }

    void P1RHandX(OscMessage osc)
    {
        player1.rightHandPosX = osc.GetFloat(0);
    }

    void P1RHandY(OscMessage osc)
    {
        player1.rightHandPosY = osc.GetFloat(0);
    }

    void P1RHandZ(OscMessage osc)
    {
        player1.rightHandPosZ = osc.GetFloat(0);
    }


    // Player 2
    // ---------------------------

    void P2LHandX(OscMessage osc)
    {
        player2.leftHandPosX = osc.GetFloat(0);
    }

    void P2LHandY(OscMessage osc)
    {
        player2.leftHandPosY = osc.GetFloat(0);
    }

    void P2LHandZ(OscMessage osc)
    {
        player2.leftHandPosZ = osc.GetFloat(0);
    }

    void P2RHandX(OscMessage osc)
    {
        player2.rightHandPosX = osc.GetFloat(0);
    }

    void P2RHandY(OscMessage osc)
    {
        player2.rightHandPosY = osc.GetFloat(0);
    }

    void P2RHandZ(OscMessage osc)
    {
        player2.rightHandPosZ = osc.GetFloat(0);
    }


     // Player 3
    // ---------------------------

    void P3LHandX(OscMessage osc)
    {
        player3.leftHandPosX = osc.GetFloat(0);
    }

    void P3LHandY(OscMessage osc)
    {
        player3.leftHandPosY = osc.GetFloat(0);
    }

    void P3LHandZ(OscMessage osc)
    {
        player3.leftHandPosZ = osc.GetFloat(0);
    }

    void P3RHandX(OscMessage osc)
    {
        player3.rightHandPosX = osc.GetFloat(0);
    }

    void P3RHandY(OscMessage osc)
    {
        player3.rightHandPosY = osc.GetFloat(0);
    }

    void P3RHandZ(OscMessage osc)
    {
        player3.rightHandPosZ = osc.GetFloat(0);
    }
    
    // Player 4
    // ---------------------------

    void P4LHandX(OscMessage osc)
    {
        player4.leftHandPosX = osc.GetFloat(0);
    }

    void P4LHandY(OscMessage osc)
    {
        player4.leftHandPosY = osc.GetFloat(0);
    }

    void P4LHandZ(OscMessage osc)
    {
        player4.leftHandPosZ = osc.GetFloat(0);
    }

    void P4RHandX(OscMessage osc)
    {
        player4.rightHandPosX = osc.GetFloat(0);
    }

    void P4RHandY(OscMessage osc)
    {
        player4.rightHandPosY = osc.GetFloat(0);
    }
    
    void P4RHandZ(OscMessage osc)
    {
        player4.rightHandPosZ = osc.GetFloat(0);
    }


}
