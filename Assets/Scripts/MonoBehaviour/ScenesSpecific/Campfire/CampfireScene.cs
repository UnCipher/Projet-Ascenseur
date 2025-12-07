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

    [SerializeField] int randomMinTick;
    [SerializeField] int randomMaxTick;
    [SerializeField] int selectedRandomTick;
    [SerializeField] int randomCurrentTick;
    [SerializeField] UnityEvent randomEvent;
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
        SetRandomTick();
    }

    void FixedUpdate()
    {
        // Scene Events
        if (currentEventIndex < campfireEvents.Length && canBeActivated)
        {
            // Call Functions
            SetHighestVolume(campfireEvents[currentEventIndex].audioType);

            if (currentTick >= tickBetweenCheck)
                CheckForSceneEvent();

            else
                currentTick++;
        }

        // Random Events
        if (randomCurrentTick >= selectedRandomTick)
            CallRandomEvents();

        randomCurrentTick++;

        // Temporary
        Debug.Log("Ticks : " + currentTick + "/" + tickBetweenCheck + ", CurrentEvent : " + currentEventIndex);
    }

    void SetRandomTick()
    {
        // Set Values
        selectedRandomTick = Random.Range(randomMinTick, randomMaxTick);
        randomCurrentTick = 0;
    }
    
    void CallRandomEvents()
    {
        // Call Functions
        SetRandomTick();

        if (canBeActivated)
            randomEvent?.Invoke();
    }

    void SetHighestVolume(LevelManager.MicrophoneAudioType type)
    {
        // Set Values
        float newVolume;

        // Set Correct Volume Type
        if (type == LevelManager.MicrophoneAudioType.Average)
            newVolume = LevelManager.GetMicrophoneInfo().average;

        else if (type == LevelManager.MicrophoneAudioType.Lowpass)
            newVolume = LevelManager.GetMicrophoneInfo().lowpass;

        else if (type == LevelManager.MicrophoneAudioType.Medium)
            newVolume = LevelManager.GetMicrophoneInfo().medium;

        else
            newVolume = LevelManager.GetMicrophoneInfo().average;

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
        if (currentEventIndex >= campfireEvents.Length)
            LevelManager.instance.OnContinue();

        else
        {
            canBeActivated = true;
            Debug.Log("Interaction Reactivated for Event #" + currentEventIndex);
        }
    }

    // Get Functions
    // ---------------------------
}
