using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    AudioSource source;

    // ---------------------------
    // Functions
    // ---------------------------

    public void InitiateSound(SoundProfile profile)
    {
        // Set Values
        source = gameObject.AddComponent<AudioSource>();
        int soundIndex = GetRandomIndex(0, profile.sounds.Length);
        
        // Start Sound
        source.clip = profile.sounds[soundIndex].clip;
        source.volume = profile.sounds[soundIndex].volume;
        source.Play();

        // Destroy After Sound Completion
        Destroy(gameObject, source.clip.length);
    }

    int GetRandomIndex(int minInt, int maxInt)
    {
        // Set Value
        int randomIndex = Random.Range(minInt, maxInt);

        // Return Value
        return randomIndex;
    }

    // Static Functions
    // ---------------------------

    public static void CreateSoundPlayer(SoundProfile profile)
    {
        if(profile.sounds.Length > 0)
        {
            // Set Values
            var soundObject = new GameObject();
            SoundPlayer player = soundObject.AddComponent<SoundPlayer>();
            soundObject.name = "New Sound";

            // Call Functions
            player.InitiateSound(profile);
        }

        else
        Debug.Log("No Sound Inside Sound Profile");
    }

    public static void CreateSoundPlayer(SoundProfile profile, Vector3 position)
    {
        if(profile.sounds.Length > 0)
        {
            // Set Values
            var soundObject = new GameObject();
            SoundPlayer player = soundObject.AddComponent<SoundPlayer>();
            soundObject.name = "New Sound";

            // Set Position
            soundObject.transform.position = position;

            // Call Functions
            player.InitiateSound(profile);
        }

        else
        Debug.Log("No Sound Inside Sound Profile");
    }

    public static void CreateSoundPlayer(SoundProfile profile, Transform parent)
    {
        if(profile.sounds.Length > 0)
        {
            // Set Values
            var soundObject = new GameObject();
            SoundPlayer player = soundObject.AddComponent<SoundPlayer>();
            soundObject.name = "New Sound";

            // Set Transform
            soundObject.transform.SetParent(parent, false);

            // Call Functions
            player.InitiateSound(profile);
        }

        else
        Debug.Log("No Sound Inside Sound Profile");
    }
}
