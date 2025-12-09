using UnityEngine;
using System.Collections;

public class ShieldController : MonoBehaviour
{
    [Header("Micro Activation Settings")]
    [Range(0, 1f)]
    [SerializeField] float volumeThreshold = 0.4f;
    [SerializeField] float cooldown = 2f;
    [SerializeField] float shieldDuration = 1.5f;

    [Header("References")]
    [SerializeField] GameObject shieldVisual;

    [Header("Shield Animation")]
    [SerializeField] private float activationScaleTime = 0.25f;
    [SerializeField] private float deactivationScaleTime = 0.20f;

    private Vector3 finalScale;

    bool shieldActive = false;
    bool canActivate = true;

    public static bool IsProtected;

    void Start()
    {
        if (shieldVisual != null)
        {
            finalScale = shieldVisual.transform.localScale;

            shieldVisual.transform.localScale = Vector3.zero;
            shieldVisual.SetActive(false);
        }

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

        StartCoroutine(ScaleUpShield());

        Invoke(nameof(DeactivateShield), shieldDuration);

        Invoke(nameof(ResetActivation), cooldown);
    }

    void DeactivateShield()
    {
        shieldActive = false;
        IsProtected = false;

        StartCoroutine(ScaleDownShield());
    }

    IEnumerator ScaleUpShield()
    {
        if (shieldVisual == null)
            yield break;

        shieldVisual.SetActive(true);
        shieldVisual.transform.localScale = Vector3.zero;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / activationScaleTime;
            shieldVisual.transform.localScale =
                Vector3.Lerp(Vector3.zero, finalScale, t);
            yield return null;
        }

        shieldVisual.transform.localScale = finalScale;
    }

    IEnumerator ScaleDownShield()
    {
        if (shieldVisual == null)
            yield break;

        float t = 0f;
        Vector3 startScale = shieldVisual.transform.localScale;

        while (t < 1f)
        {
            t += Time.deltaTime / deactivationScaleTime;
            shieldVisual.transform.localScale =
                Vector3.Lerp(startScale, Vector3.zero, t);
            yield return null;
        }

        shieldVisual.transform.localScale = Vector3.zero;
        shieldVisual.SetActive(false);
    }

    void ResetActivation()
    {
        canActivate = true;
    }
}