using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    [HideInInspector]
    public static LevelManager instance;
    
    public HandsOnWall handsOnWall;
    public MicrophoneInfo microphone = new MicrophoneInfo();
    public List<Player> players = new List<Player>();

    [Space(10)]
    public ElevatorState elevatorState;
    [SerializeField] bool operationnal = true;

    [Header("Scene Manager")]
    [SerializeField] float sceneChangeStartup;
    [SerializeField] float sceneChangeCooldown;
    [Space(5)]

    [SerializeField] string animationEnterTrigger;
    [SerializeField] string animationLeaveTrigger;

    public GameScenes scenes;
    string currentScene;
    bool changingScene;

    [Header("References")]
    [SerializeField] Animator animator;

    public Camera leftCamera;
    public Camera centerCamera;
    public Camera rightCamera;
    OSC osc;

    // Classes
    // ---------------------------

    [System.Serializable]
    public class GameScenes
    {
        public SceneSettings elevator;
        [Space(5)]

        public SceneSettings campfire;
        [Space(5)]

        public SceneSettings asteroid;
        [Space(5)]

        public SceneSettings echolocation;
    }
    
    [System.Serializable]
    public class SceneSettings
    {
        public string name;
        public float duration;
    }

    [System.Serializable]
    public class MicrophoneInfo
    {
        public float average;
        [Space(5)]

        public float lowpass;
        public float medium;
        public float highpass;
    }

    [System.Serializable]
    public class HandsOnWall
    {
        public List<PlayerHand> leftWall;
        public List<PlayerHand> centerWall;
        public List<PlayerHand> rightWall;
    }

    public enum ElevatorState
    {
        Fine,
        Damaged,
        Broken,
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

        else
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
    }

    void Start()
    {
        // Set Values
        osc = gameObject.AddComponent<OSC>();

        if (!animator)
            animator = GetComponent<Animator>();

        // Call Functions
        InitiateOSCMessages();
    }

    void FixedUpdate()
    {
        // Send Wall Messages
        SendPlayersWallInfo();
    }

    // Scene Functions
    // ---------------------------

    IEnumerator ChangeScene(SceneSettings scene)
    {
        if (GetCurrentSceneName() != scene.name && !changingScene && operationnal)
        {
            // Set Values
            currentScene = scene.name;
            changingScene = true;

            // Cancel Invoke
            CancelInvoke("OnSceneCompleted");

            // Change Elevator State
            if(scene.name == scenes.elevator.name)
            {
                if(elevatorState == ElevatorState.Damaged)
                {
                    InitiateEndGame();
                }

                else
                {
                    elevatorState = ElevatorState.Damaged;
                }
            }

            // Play Animation
            if (GetCurrentSceneName() != scenes.elevator.name)
            {
                animator.SetTrigger(animationLeaveTrigger);
                yield return new WaitForSeconds(sceneChangeStartup);
            }

            // Clear Parent
            transform.SetParent(null, false);
            transform.eulerAngles = Vector3.zero;

            // Load Scene
            SceneManager.LoadScene(scene.name);

            // Start CountDown to Elevator
            if (scene.duration > sceneChangeStartup)
                Invoke("OnSceneCompleted", scene.duration - sceneChangeStartup);

            // Play Animation
            if (scene.name != scenes.elevator.name)
            {
                animator.SetTrigger(animationEnterTrigger);
                yield return new WaitForSeconds(sceneChangeCooldown);
            }

            changingScene = false;
        }
    }

    void StartElevator()
    {
        if(GetCurrentSceneName() == scenes.elevator.name)
        {
            // Set Values
            operationnal = true;
            elevatorState = ElevatorState.Fine;

            // Start Animation
        }
    }

    void LeaveElevator()
    {
        if (GetCurrentSceneName() == scenes.elevator.name)
        {
            operationnal = false;
        }
    }
    
    void InitiateEndGame()
    {
        operationnal = false;
        Debug.Log("lel start MiniGame and Cutscene");
    }
    
    public void OnSceneCompleted()
    {
        // Call Elevator Scene
        StartCoroutine(ChangeScene(scenes.elevator));
    }

    public void OnElevator()
    {
        // Call Elevator Scene
        StartCoroutine(ChangeScene(scenes.elevator));
    }

    public void OnAsteroid()
    {
        // Call Asteroid Scene
        if(GetCurrentSceneName() == scenes.elevator.name)
        StartCoroutine(ChangeScene(scenes.asteroid));
    }

    public void OnCampfire()
    {
        // Call Campfire Scene
        if(GetCurrentSceneName() == scenes.elevator.name)
        StartCoroutine(ChangeScene(scenes.campfire));
    }

    public void OnEcholocation()
    {
        // Call Echolocation Scene
        if(GetCurrentSceneName() == scenes.elevator.name)
        StartCoroutine(ChangeScene(scenes.echolocation));
    }

    public void OnElevatorLeave()
    {
        if(GetCurrentSceneName() == "Elevator")
        {
            LeaveElevator();
        }
    }
    
    public void OnDebugRestart()
    {
        StartElevator();
    }

    public void OnDebugleave()
    {
        Application.Quit();
    }

    // OSC Functions
    // ---------------------------

    void InitiateOSCMessages()
    {
        // Receive Messages
        
        // Microphone
        osc.SetAddressHandler("/micVolume", MicVolume);

        // Players

        // Player 1 
        osc.SetAddressHandler("/p1Left", P1Left);
        osc.SetAddressHandler("/p1Right", P1Right);
        osc.SetAddressHandler("/p1Active", P1Active);

        // Player 2
        osc.SetAddressHandler("/p2Left", P2Left);
        osc.SetAddressHandler("/p2Right", P2Right);
        osc.SetAddressHandler("/p2Active", P2Active);

        // Player 3
        osc.SetAddressHandler("/p3Left", P3Left);
        osc.SetAddressHandler("/p3Right", P3Right);
        osc.SetAddressHandler("/p3Active", P3Active);

        // Player 4
        osc.SetAddressHandler("/p4Left", P4Left);
        osc.SetAddressHandler("/p4Right", P4Right);
        osc.SetAddressHandler("/p4Active", P4Active);
    }

    public void SendOSCMessage(string name, int value)
    {
        // Set Values
        OscMessage message = new OscMessage();

        message.address = "/" + name;
        message.values.Add(value);

        // Send Message
        osc.Send(message);
    }

    public void SendOSCMessage(string name, float value)
    {
        // Set Values
        OscMessage message = new OscMessage();

        message.address = "/" + name;
        message.values.Add(value);

        // Send Message
        osc.Send(message);
    }

    public void SendOSCMessage(string name, Vector2 value)
    {
        // Set Values
        OscMessage message = new OscMessage();

        message.address = "/" + name;
        message.values.Add(value);

        // Send Message
        osc.Send(message);
    }

    public void SendOSCMessage(string name, Vector3 value)
    {
        // Set Values
        OscMessage message = new OscMessage();

        message.address = "/" + name;
        message.values.Add(value);

        // Send Message
        osc.Send(message);
    }

    // Audio
    // ---------------------------

    void MicVolume(OscMessage osc)
    {
        microphone.average = osc.GetFloat(0);
        microphone.lowpass = osc.GetFloat(1);
        microphone.medium = osc.GetFloat(2);
        microphone.highpass = osc.GetFloat(3);
    }

    // Players
    // ---------------------------

    void SendPlayersWallInfo()
    {
        // Loop through all Players
        for(int i = 0;i<players.Count;i++)
        {
            // Set Values
            int number = i + 1;

            string leftAddress = "p" + number + "-lh-info";
            string rightAddress = "p" + number + "-rh-info";

            Wall.WallInfo leftWall = players[i].GetLeftWallInfo();
            Wall.WallInfo rightWall = players[i].GetRightWallInfo();

            // Send Messages
            SendOSCMessage(leftAddress + "-x", leftWall.uv.x);
            SendOSCMessage(leftAddress + "-y", leftWall.uv.y);
            SendOSCMessage(leftAddress, (int)leftWall.selectedWall);

            SendOSCMessage(rightAddress + "-x", rightWall.uv.x);
            SendOSCMessage(rightAddress + "-y", rightWall.uv.y);
            SendOSCMessage(rightAddress, (int)rightWall.selectedWall);
        }
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
                players[0].RequestDesactivatePlayer();
        }
    }

    void P1Left(OscMessage osc)
    {
        if (players[0])
        {
            Vector3 handPos = new Vector3(osc.GetFloat(0), osc.GetFloat(1), osc.GetFloat(2));
            Vector3 fingerPos = new Vector3(osc.GetFloat(3), osc.GetFloat(4), osc.GetFloat(5));

            players[0].handsInfo.leftHandPos = handPos;
            players[0].handsInfo.leftHandPos = fingerPos;
        }
    }
    
    void P1Right(OscMessage osc)
    {
        if(players[0])
        {
            Vector3 handPos = new Vector3(osc.GetFloat(0), osc.GetFloat(1), osc.GetFloat(2));
            Vector3 fingerPos = new Vector3(osc.GetFloat(3), osc.GetFloat(4), osc.GetFloat(5));

            players[0].handsInfo.rightHandPos = handPos;
            players[0].handsInfo.rightHandPos = fingerPos;
        }
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
                players[1].RequestDesactivatePlayer();
        }
    }

    void P2Left(OscMessage osc)
    {
        if (players[1])
        {
            Vector3 handPos = new Vector3(osc.GetFloat(0), osc.GetFloat(1), osc.GetFloat(2));
            Vector3 fingerPos = new Vector3(osc.GetFloat(3), osc.GetFloat(4), osc.GetFloat(5));

            players[1].handsInfo.leftHandPos = handPos;
            players[1].handsInfo.leftHandPos = fingerPos;
        }
    }
    
    void P2Right(OscMessage osc)
    {
        if(players[1])
        {
            Vector3 handPos = new Vector3(osc.GetFloat(0), osc.GetFloat(1), osc.GetFloat(2));
            Vector3 fingerPos = new Vector3(osc.GetFloat(3), osc.GetFloat(4), osc.GetFloat(5));

            players[1].handsInfo.rightHandPos = handPos;
            players[1].handsInfo.rightHandPos = fingerPos;
        }
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
                players[2].RequestDesactivatePlayer();
        }
    }

    void P3Left(OscMessage osc)
    {
        if (players[2])
        {
            Vector3 handPos = new Vector3(osc.GetFloat(0), osc.GetFloat(1), osc.GetFloat(2));
            Vector3 fingerPos = new Vector3(osc.GetFloat(3), osc.GetFloat(4), osc.GetFloat(5));

            players[2].handsInfo.leftHandPos = handPos;
            players[2].handsInfo.leftHandPos = fingerPos;
        }
    }
    
    void P3Right(OscMessage osc)
    {
        if(players[2])
        {
            Vector3 handPos = new Vector3(osc.GetFloat(0), osc.GetFloat(1), osc.GetFloat(2));
            Vector3 fingerPos = new Vector3(osc.GetFloat(3), osc.GetFloat(4), osc.GetFloat(5));

            players[2].handsInfo.rightHandPos = handPos;
            players[2].handsInfo.rightHandPos = fingerPos;
        }
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
                players[3].RequestDesactivatePlayer();
        }
    }

    void P4Left(OscMessage osc)
    {
        if (players[3])
        {
            Vector3 handPos = new Vector3(osc.GetFloat(0), osc.GetFloat(1), osc.GetFloat(2));
            Vector3 fingerPos = new Vector3(osc.GetFloat(3), osc.GetFloat(4), osc.GetFloat(5));

            players[3].handsInfo.leftHandPos = handPos;
            players[3].handsInfo.leftHandPos = fingerPos;
        }
    }
    
    void P4Right(OscMessage osc)
    {
        if(players[3])
        {
            Vector3 handPos = new Vector3(osc.GetFloat(0), osc.GetFloat(1), osc.GetFloat(2));
            Vector3 fingerPos = new Vector3(osc.GetFloat(3), osc.GetFloat(4), osc.GetFloat(5));

            players[3].handsInfo.rightHandPos = handPos;
            players[3].handsInfo.rightHandPos = fingerPos;
        }
    }

    // Get Functions
    // ---------------------------

    public static MicrophoneInfo GetMicrophoneInfo()
    {
        // Set Values
        MicrophoneInfo microphoneInfo = instance.microphone;

        // Return Value
        return microphoneInfo;
    }

    public static int GetActivePlayersNumber()
    {
        // Set Value
        int number = 0;

        // Incremate if Player is Active
        for (int i = 0; i < instance.players.Count; i++)
            if (instance.players[i].GetPlayerActive())
                number++;

        // Return the number calculated
        return number;
    }

    public static PlayerHand[] GetAllHandOnWall(Wall.SelectedWall wall)
    {
        // Set Values
        List<PlayerHand> hands = new List<PlayerHand>();
        Player[] activePlayers = GetActivePlayers();

        for(int i = 0;i<activePlayers.Length;i++)
        {
            if (activePlayers[i].GetLeftHand().GetSelectedWall() == wall)
                hands.Add(activePlayers[i].GetLeftHand());

            if (activePlayers[i].GetRightHand().GetSelectedWall() == wall)
                hands.Add(activePlayers[i].GetRightHand());
        }

        return hands.ToArray();
    }

    public static Player[] GetActivePlayers()
    {
        // Set Value
        List<Player> activePlayers = new List<Player>();

        // Incremate if Player is Active
        for (int i = 0; i < instance.players.Count; i++)
            if (instance.players[i].GetPlayerActive())
                activePlayers.Add(instance.players[i]);

        // Return Active Players
        return activePlayers.ToArray();
    }

    public int GetCurrentSceneIndex()
    {
        // Set Values
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Return String
        return sceneIndex;
    }
    
    public string GetCurrentSceneName()
    {
        // Set Values
        string sceneName = SceneManager.GetActiveScene().name;

        // Return String
        return sceneName;
    }
}