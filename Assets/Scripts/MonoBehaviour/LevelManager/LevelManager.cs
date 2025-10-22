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

    [Header("OSC Messages")]
    OSC osc;

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
        player1HandInfo?.Invoke(this, new PlayerHandInfoArgs{
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
        osc.SetAddressHandler("/p1-lHx", P1LHandX);
        osc.SetAddressHandler("/p1-lHy", P1LHandY);
        osc.SetAddressHandler("/p1-lHz", P1LHandZ);

        osc.SetAddressHandler("/p1-rHx", P1RHandX);
        osc.SetAddressHandler("/p1-rHy", P1RHandY);
        osc.SetAddressHandler("/p1-rHz", P1RHandZ);
    }

    public void SendMessage(string address, float value)
    {
        // Set Values
        OscMessage message = new OscMessage();

        message.address = "/" + address;
        message.values.Add(value);

        // Send Message
        osc.Send(message);
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

}
