using UnityEngine;
using System;

public class DragonController : MonoBehaviour
{
    public static Action OnDragonDeath;

    [Header("Références")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject dragonModel;
    [SerializeField] private GameObject spawnFireball;

    private void OnEnable()
    {
        OnDragonDeath += KillDragon;
    }

    private void OnDisable()
    {
        OnDragonDeath -= KillDragon;
    }

    private void KillDragon()
    {
        Debug.Log("Le dragon est mort !");

        if (animator != null)
            animator.SetTrigger("Die");

        Invoke(nameof(HideDragon), 2f);
    }

    private void HideDragon()
    {
        if (dragonModel != null)
        {
             dragonModel.SetActive(false);
            spawnFireball.SetActive(false);

            Invoke("DelayElevator", 1f);
        }
    }

    private void DelayElevator(){
        LevelManager.instance.OnContinue();
    }
}