using UnityEngine;

public class EcholocationController : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    [Header("Audio Settings")]
    [SerializeField] int tickBetweenCheck;
    [SerializeField] int currentTick;

    [Header("Movement Properties")]
    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;

    [Header("Scan Properties")]
    [SerializeField] float scanCooldown;
    [Range(0, 1)][SerializeField] float minToActivateScan;
    bool canBeActivated = true;
    [Space(5)]

    [SerializeField] LevelManager.MicrophoneAudioType scanSizeAudioType;
    [SerializeField] float highestSizeVolumeBetweenTick;

    [Range(0, 1)][SerializeField] float minSizeVolumeIn;
    [Range(0, 1)][SerializeField] float maxSizeVolumeIn;
    [Space(5)]

    [SerializeField] float scanMinSize;
    [SerializeField] float scanMaxSize;
    [Space(10)]

    [SerializeField] LevelManager.MicrophoneAudioType scanDurationAudioType;
    [SerializeField] float highestDurationVolumeBetweenTick;

    [Range(0, 1)][SerializeField] float minDurationVolumeIn;
    [Range(0, 1)][SerializeField] float maxDurationVolumeIn;

    [Space(5)]

    [SerializeField] float scanMinDuration;
    [SerializeField] float scanMaxDuration;

    [Header("References")]
    [SerializeField] GameObject particleScanPrefab;
    [SerializeField] Rigidbody rb;

    // ---------------------------
    // Functions
    // ---------------------------

    void Start()
    {
        // Set Values
        if(!rb)
        rb = GetComponent<Rigidbody>();

        // Set LevelManager as Child
        LevelManager.instance.transform.SetParent(this.transform, false);
        LevelManager.instance.transform.localPosition = Vector3.zero;
    }

    void FixedUpdate()
    {
        // Mouvement
        if (LevelManager.GetActivePlayersNumber() > 0)
            Movevement();

        // Echo Location
        if (canBeActivated)
        {
            // Call Functions
            highestDurationVolumeBetweenTick = SetHighestVolume(scanDurationAudioType, highestDurationVolumeBetweenTick);
            highestSizeVolumeBetweenTick = SetHighestVolume(scanSizeAudioType, highestSizeVolumeBetweenTick);

            if (currentTick >= tickBetweenCheck)
                CheckForScan();

            else
                currentTick++;
        }
    }
    
    void Movevement()
    {
        // Set Values
        float leftForce = LevelManager.GetAllHandOnWall(Wall.SelectedWall.Left).Length / (LevelManager.GetActivePlayersNumber() * 2);
        float centerForce = LevelManager.GetAllHandOnWall(Wall.SelectedWall.Center).Length / (LevelManager.GetActivePlayersNumber() * 2);
        float rightForce = LevelManager.GetAllHandOnWall(Wall.SelectedWall.Right).Length / (LevelManager.GetActivePlayersNumber() * 2);

        Debug.Log(leftForce + " / " + centerForce + " / " + rightForce);

        // Move Left
        transform.Rotate(Vector3.up * Time.fixedDeltaTime * leftForce * -turnSpeed);

        // Move Forward
        rb.AddForce(-transform.forward * moveSpeed * centerForce);

        // Move Right
        transform.Rotate(Vector3.up * Time.fixedDeltaTime * rightForce * turnSpeed);
    }

    float SetHighestVolume(LevelManager.MicrophoneAudioType type, float highestVolume)
    {
        // Set Values
        float newVolume;
        float returnVolume = highestVolume;

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
        if (newVolume > highestVolume)
            returnVolume = newVolume;

        // Return Value
        return returnVolume;
    }

    void CheckForScan()
    {
        if (LevelManager.GetMicrophoneInfo().average >= minToActivateScan)
        {
            // Set Values
            canBeActivated = false;

            float sizeVolumeRatio = GetAudioRatio(minSizeVolumeIn, maxSizeVolumeIn, highestSizeVolumeBetweenTick);
            float durationVolumeRatio = GetAudioRatio(minDurationVolumeIn, maxDurationVolumeIn, highestDurationVolumeBetweenTick);

            float scanSize = GetInBetweenValue(scanMinSize, scanMaxSize, sizeVolumeRatio);
            float scanDuration = GetInBetweenValue(scanMinDuration, scanMaxDuration, durationVolumeRatio);

            // Call Invoke
            Invoke("ReactivateScan", scanCooldown);

            // Create Scan
            InstantiateScan(transform.position, scanSize, scanDuration);

            Debug.Log("Scan size : " + scanSize + " / Scan duration : " + scanDuration);
        }

        // Reset Values
        currentTick = 0;
        highestDurationVolumeBetweenTick = 0;
        highestSizeVolumeBetweenTick = 0;
    }
    
    void ReactivateScan()
    {
        // Set Value
        canBeActivated = true;
    }

    float GetAudioRatio(float minValue, float maxValue, float value)
    {
        // Set Value
        float ratio = (value - minValue) / (maxValue - minValue);

        // Clamp Value
        if (ratio < 0)
            ratio = 0;

        if (ratio > 1)
            ratio = 1;

        // Return Value
        return ratio;
    }
    
    float GetInBetweenValue(float minValue, float maxValue, float ratio)
    {
        // Set Value
        float inBetweenValue = ((maxValue - minValue) * ratio) + minValue;

        // Clamp Value
        if (inBetweenValue < minValue)
            inBetweenValue = minValue;

        if (inBetweenValue > maxValue)
            inBetweenValue = maxValue;

        // Return Value
        return inBetweenValue;
    }

    void InstantiateScan(Vector3 position, float size, float duration)
    {
        // Set Values
        GameObject particleObject = Instantiate(particleScanPrefab, position, Quaternion.Euler(Vector3.zero));
        ParticleSystem particle = particleObject.transform.GetChild(0).GetComponent<ParticleSystem>();

        if (particle != null)
        {
            var main = particle.main;

            main.startLifetime = duration;
            main.startSize = size;
        }

        Destroy(particleObject, duration + .5f);
    }
}