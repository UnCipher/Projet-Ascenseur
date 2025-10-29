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

    // OSC Functions
    // ---------------------------

    void InitiateOSCMessages()
    {
        // Receive Messages

        // Microphone
        osc.SetAddressHandler("/micAverage", MicAverageVolume);
        osc.SetAddressHandler("/micLowpass", MicLowpassVolume);
        osc.SetAddressHandler("/micMedium", MicMediumVolume);
        osc.SetAddressHandler("/micHighpass", MicHighpassVolume);

        // Players

        // Player 1 
        osc.SetAddressHandler("/p1-active", P1Active);
        osc.SetAddressHandler("/p1-lHx", P1LHandX);
        osc.SetAddressHandler("/p1-lHy", P1LHandY);
        osc.SetAddressHandler("/p1-lHz", P1LHandZ);

        osc.SetAddressHandler("/p1-rHx", P1RHandX);
        osc.SetAddressHandler("/p1-rHy", P1RHandY);
        osc.SetAddressHandler("/p1-rHz", P1RHandZ);

        // Player 2
        osc.SetAddressHandler("/p2-active", P2Active);
        osc.SetAddressHandler("/p2-lHx", P2LHandX);
        osc.SetAddressHandler("/p2-lHy", P2LHandY);
        osc.SetAddressHandler("/p2-lHz", P2LHandZ);

        osc.SetAddressHandler("/p2-rHx", P2RHandX);
        osc.SetAddressHandler("/p2-rHy", P2RHandY);
        osc.SetAddressHandler("/p2-rHz", P2RHandZ);

        // Player 3
        osc.SetAddressHandler("/p3-active", P3Active);
        osc.SetAddressHandler("/p3-lHx", P3LHandX);
        osc.SetAddressHandler("/p3-lHy", P3LHandY);
        osc.SetAddressHandler("/p3-lHz", P3LHandZ);

        osc.SetAddressHandler("/p3-rHx", P3RHandX);
        osc.SetAddressHandler("/p3-rHy", P3RHandY);
        osc.SetAddressHandler("/p3-rHz", P3RHandZ);

        // Player 4
        osc.SetAddressHandler("/p4-active", P4Active);
        osc.SetAddressHandler("/p4-lHx", P4LHandX);
        osc.SetAddressHandler("/p4-lHy", P4LHandY);
        osc.SetAddressHandler("/p4-lHz", P4LHandZ);

        osc.SetAddressHandler("/p4-rHx", P4RHandX);
        osc.SetAddressHandler("/p4-rHy", P4RHandY);
        osc.SetAddressHandler("/p4-rHz", P4RHandZ);
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
        if (players[0])
            players[0].handsInfo.rightHandPosZ = osc.GetFloat(0);
    }
    
    // Player 2
    // ---------------------------

    void P2Active(OscMessage osc)
    {
        if(players[1])
        {
            int value = osc.GetInt(0);
            if (value == 1)
                players[1].ActivatePlayer();

            else
                players[1].DesactivatePlayer();
        }
    }
    
    void P2LHandX(OscMessage osc)
    {
        if(players[1])
        players[1].handsInfo.leftHandPosX = osc.GetFloat(0);
    }

    void P2LHandY(OscMessage osc)
    {
        if(players[1])
        players[1].handsInfo.leftHandPosY = osc.GetFloat(0);
    }

    void P2LHandZ(OscMessage osc)
    {
        if(players[1])
        players[1].handsInfo.leftHandPosZ = osc.GetFloat(0);
    }

    void P2RHandX(OscMessage osc)
    {
        if(players[1])
        players[1].handsInfo.rightHandPosX = osc.GetFloat(0);
    }

    void P2RHandY(OscMessage osc)
    {
        if(players[1])
        players[1].handsInfo.rightHandPosY = osc.GetFloat(0);
    }

    void P2RHandZ(OscMessage osc)
    {
        if (players[1])
            players[1].handsInfo.rightHandPosZ = osc.GetFloat(0);
    }
    
    // Player 3
    // ---------------------------

    void P3Active(OscMessage osc)
    {
        if(players[2])
        {
            int value = osc.GetInt(0);
            if (value == 1)
                players[2].ActivatePlayer();

            else
                players[2].DesactivatePlayer();
        }
    }
    
    void P3LHandX(OscMessage osc)
    {
        if(players[2])
        players[2].handsInfo.leftHandPosX = osc.GetFloat(0);
    }

    void P3LHandY(OscMessage osc)
    {
        if(players[2])
        players[2].handsInfo.leftHandPosY = osc.GetFloat(0);
    }

    void P3LHandZ(OscMessage osc)
    {
        if(players[2])
        players[2].handsInfo.leftHandPosZ = osc.GetFloat(0);
    }

    void P3RHandX(OscMessage osc)
    {
        if(players[2])
        players[2].handsInfo.rightHandPosX = osc.GetFloat(0);
    }

    void P3RHandY(OscMessage osc)
    {
        if(players[2])
        players[2].handsInfo.rightHandPosY = osc.GetFloat(0);
    }

    void P3RHandZ(OscMessage osc)
    {
        if (players[2])
            players[2].handsInfo.rightHandPosZ = osc.GetFloat(0);
    }
    
    // Player 4
    // ---------------------------

    void P4Active(OscMessage osc)
    {
        if(players[3])
        {
            int value = osc.GetInt(0);
            if (value == 1)
                players[3].ActivatePlayer();

            else
                players[3].DesactivatePlayer();
        }
    }
    
    void P4LHandX(OscMessage osc)
    {
        if(players[3])
        players[3].handsInfo.leftHandPosX = osc.GetFloat(0);
    }

    void P4LHandY(OscMessage osc)
    {
        if(players[3])
        players[3].handsInfo.leftHandPosY = osc.GetFloat(0);
    }

    void P4LHandZ(OscMessage osc)
    {
        if(players[3])
        players[3].handsInfo.leftHandPosZ = osc.GetFloat(0);
    }

    void P4RHandX(OscMessage osc)
    {
        if(players[3])
        players[3].handsInfo.rightHandPosX = osc.GetFloat(0);
    }

    void P4RHandY(OscMessage osc)
    {
        if(players[3])
        players[3].handsInfo.rightHandPosY = osc.GetFloat(0);
    }
    
    void P4RHandZ(OscMessage osc)
    {
        if(players[3])
        players[3].handsInfo.rightHandPosZ = osc.GetFloat(0);
    }
}
