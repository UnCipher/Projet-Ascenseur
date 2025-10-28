using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OSC))]
public class LevelManager : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    [HideInInspector]
    public static LevelManager instance;
    public MicrophoneInfo microphone = new MicrophoneInfo();
    public List<Player> players = new List<Player>();

    [Header("OSC Messages")]
    OSC osc;

    // Classes
    // ---------------------------

    [System.Serializable]
    public class MicrophoneInfo
    {
        public float average;
        [Space(5)]

        public float lowpass;
        public float medium;
        public float highpass;
    }

    public enum MicrophoneAudioType
    {
        Average,
        Lowpass,
        Medium,
        Highpass,
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
        // Call Players' Functions
        for (int i = 0; i < players.Count; i++)
            players[i].ChangeHandsPositions();
    }

    // OSC Functions
    // ---------------------------

    void InitiateOSCMessages()
    {
        // Receive Messages
        osc.SetAddressHandler("/p1-active", P1Active);
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

    // Audio
    // ---------------------------

    void MicAverageVolume(OscMessage osc)
    {
        microphone.average = osc.GetFloat(0);
    }

    void MicLowpassVolume(OscMessage osc)
    {
        microphone.lowpass = osc.GetFloat(0);
    }

    void MicMediumVolume(OscMessage osc)
    {
        microphone.medium = osc.GetFloat(0);
    }

    void MicHighpassVolume(OscMessage osc)
    {
        microphone.highpass = osc.GetFloat(0);
    }


    // Player 1
    // ---------------------------

    void P1Active(OscMessage osc)
    {
        if(players[0])
        {
            int value = osc.GetInt(0);
            if (value == 1)
                players[0].ActivatePlayer();

            else
                players[0].DesactivatePlayer();
        }
    }
    
    void P1LHandX(OscMessage osc)
    {
        if(players[0])
        players[0].handsInfo.leftHandPosX = osc.GetFloat(0);
    }

    void P1LHandY(OscMessage osc)
    {
        if(players[0])
        players[0].handsInfo.leftHandPosY = osc.GetFloat(0);
    }

    void P1LHandZ(OscMessage osc)
    {
        if(players[0])
        players[0].handsInfo.leftHandPosZ = osc.GetFloat(0);
    }

    void P1RHandX(OscMessage osc)
    {
        if(players[0])
        players[0].handsInfo.rightHandPosX = osc.GetFloat(0);
    }

    void P1RHandY(OscMessage osc)
    {
        if(players[0])
        players[0].handsInfo.rightHandPosY = osc.GetFloat(0);
    }
    
    void P1RHandZ(OscMessage osc)
    {
        if(players[0])
        players[0].handsInfo.rightHandPosZ = osc.GetFloat(0);
    }

}
