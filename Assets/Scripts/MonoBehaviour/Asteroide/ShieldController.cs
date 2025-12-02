using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [Header("Micro Activation Settings")]
    [Range(0,1f)]
    [SerializeField] float volumeThreshold = 0.4f;
    [SerializeField] float cooldown = 2f;
    [SerializeField] float shieldDuration = 1.5f;

    [Header("References")]
    [SerializeField] GameObject shieldVisual;

    bool shieldActive = false;
    bool canActivate = true;

    public static bool IsProtected;

    void Start()
    {
        if (shieldVisual != null)
            shieldVisual.SetActive(false);

        IsProtected = false;
    }

    void FixedUpdate()
    {
        float micVolume = LevelManager.GetMicrophoneInfo().average;

        if (canActivate && micVolume >= volumeThreshold)
        {
            ActivateShield();
        }
    }

    void ActivateShield()
    {
        canActivate = false;
        shieldActive = true;
        IsProtected = true;

        if (shieldVisual != null)
            shieldVisual.SetActive(true);

        Invoke(nameof(DeactivateShield), shieldDuration);

        Invoke(nameof(ResetActivation), cooldown);
    }

    void DeactivateShield()
    {
        shieldActive = false;
        IsProtected = false;

        if (shieldVisual != null)
            shieldVisual.SetActive(false);
    }

    void ResetActivation()
    {
        canActivate = true;
    }
}
