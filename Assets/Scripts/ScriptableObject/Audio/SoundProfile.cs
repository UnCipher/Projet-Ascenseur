using UnityEngine;

[CreateAssetMenu(fileName = "SoundProfile_0000", menuName = "ScriptableObject/Sound Profile")]
public class SoundProfile : ScriptableObject
{
    // ---------------------------
    // Values
    // ---------------------------

    public Sound[] sounds;

    // Classes
    // ---------------------------

    [System.Serializable]
    public class Sound
    {
        public AudioClip clip;
        [Range(0, 1)] public float volume;
    }
}
