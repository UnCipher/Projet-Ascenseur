using UnityEngine;
using UnityEngine.Events;

public class CampfireScene : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    [Header("Audio Settings")]
    [SerializeField] int tickBetweenCheck;
    [SerializeField] float highestVolumeBetweenTick;
    [SerializeField] int currentTick;
    [Space(5)]

    [SerializeField] int currentEventIndex;
    [SerializeField] CampfireMicEvents[] campfireEvents;
    [SerializeField] bool canBeActivated = true;

    // Classes
    // ---------------------------

    [System.Serializable]
    public class CampfireMicEvents
    {
        public float minVolumeToActivate;
        public LevelManager.MicrophoneAudioType audioType;
        [Space(5)]

        public UnityEvent events;
        public float timeBeforeNextEvent;
    }

    // ---------------------------
    // Functions
    // ---------------------------

    void Start()
    {
        // IDK just got here
    }

    void FixedUpdate()
    {
        if (currentEventIndex < campfireEvents.Length && canBeActivated)
        {
            // Call Functions
            SetHighestVolume(campfireEvents[currentEventIndex].audioType);

            if (currentTick >= tickBetweenCheck)
                CheckForSceneEvent();

            else
                currentTick++;
        }
        
        // Temporary
        Debug.Log("Ticks : " + currentTick + "/" + tickBetweenCheck + ", CurrentEvent : " + currentEventIndex);
    }

    void SetHighestVolume(LevelManager.MicrophoneAudioType type)
    {
        // Set Values
        float newVolume;

        // Set Correct Volume Type
        if (type == LevelManager.MicrophoneAudioType.Average)
            newVolume = GetMicrophoneInfo().average;

        else if (type == LevelManager.MicrophoneAudioType.Lowpass)
            newVolume = GetMicrophoneInfo().lowpass;

        else if (type == LevelManager.MicrophoneAudioType.Medium)
            newVolume = GetMicrophoneInfo().medium;

        else
            newVolume = GetMicrophoneInfo().average;

        // Check if Value is Bigger than current Highest Volume
        if (newVolume > highestVolumeBetweenTick)
            highestVolumeBetweenTick = newVolume;
    }

    void CheckForSceneEvent()
    {
        // Check if Event can be Played
        if (highestVolumeBetweenTick >= campfireEvents[currentEventIndex].minVolumeToActivate)
        {
            campfireEvents[currentEventIndex].events.Invoke();

            // Call Functions
            Invoke("ReactivateEvent", campfireEvents[currentEventIndex].timeBeforeNextEvent);
            Debug.Log("Interaction desactivated for : " + campfireEvents[currentEventIndex].timeBeforeNextEvent + " Seconds");

            // Set Values
            canBeActivated = false;
            currentEventIndex++;
        }

        // Reset Values
        highestVolumeBetweenTick = 0;
        currentTick = 0;
    }

    void ReactivateEvent()
    {
        // Set Values
        canBeActivated = true;
        Debug.Log("Interaction Reactivated for Event #" + currentEventIndex);
    }
    
    public void Test1()
    {
        Debug.Log("lele, it works #1");
    }
    public void Test2()
    {
        Debug.Log("lele, it works #2");
    }
    public void Test3()
    {
        Debug.Log("lele, it works #3");
    }
    public void Test4()
    {
        Debug.Log("lele, it works #4");
    }
    
    // Get Functions
    // ---------------------------

    LevelManager.MicrophoneInfo GetMicrophoneInfo()
    {
        // Set Values
        LevelManager.MicrophoneInfo microphoneInfo = LevelManager.instance.microphone;

        // Return Value
        return microphoneInfo;
    }
}
