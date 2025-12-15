using UnityEngine;
using System.Collections;
using TMPro;

public class VentAttackController : MonoBehaviour
{
    [Header("Micro Activation Settings")]
    [Range(0, 1f)]
    [SerializeField] float volumeThreshold = 0.5f;
    [SerializeField] float cooldown = 1.2f;
    [SerializeField] float ventDuration = 0.6f;

    [Header("Dragon Data (ScriptableObject)")]
    [SerializeField] private InfoDragon infoDragon;

    [Header("Visual Effect")]
    [SerializeField] private GameObject ventVFX;

    [Header("TMPro")]
    [SerializeField] private TMP_Text champPVDragon;

    private bool canAttack = true;
    private bool isVentActive = false;

    void Start()
    {
        if (ventVFX != null)
            ventVFX.SetActive(false);

        infoDragon.nbVie = 3;
        champPVDragon.text = "PV Dragon : " + infoDragon.nbVie;
    }

    void FixedUpdate()
    {
        float micVolume = LevelManager.GetMicrophoneInfo().average;

        if (canAttack && micVolume >= volumeThreshold)
        {
            TriggerVentAttack();
        }
    }

    private void TriggerVentAttack()
    {
        canAttack = false;
        isVentActive = true;

        if (ventVFX != null)
            StartCoroutine(VentEffectRoutine());

        infoDragon.nbVie -= 1;
        Debug.Log("PV Dragon : " + infoDragon.nbVie);
        champPVDragon.text = "PV Dragon : " + infoDragon.nbVie;

        if (infoDragon.nbVie <= 0)
        {
            DragonController.OnDragonDeath?.Invoke();
        }

        Invoke(nameof(ResetAttack), cooldown);
    }

    IEnumerator VentEffectRoutine()
    {
        ventVFX.SetActive(true);
        yield return new WaitForSeconds(ventDuration);
        ventVFX.SetActive(false);
        isVentActive = false;
    }

    private void ResetAttack()
    {
        canAttack = true;
    }
}