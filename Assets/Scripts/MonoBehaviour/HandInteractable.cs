using UnityEngine.Events;
using UnityEngine;

public class HandInteractable : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    [Header("Interaction")]
    [SerializeField] UnityEvent interaction;

    // ---------------------------
    // Functions
    // ---------------------------

    public void OnHandHover()
    {
        // Call Interaction
        interaction?.Invoke();
    }
}
