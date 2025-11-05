using UnityEngine;

[RequireComponent(typeof(LevelManager))]
public class MultiDisplay : MonoBehaviour
{
    // ---------------------------
    // Functions
    // ---------------------------

    void Awake()
    {
        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }
}
