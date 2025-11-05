using UnityEngine;
using TMPro;

public class RetirerPV : MonoBehaviour
{
    [SerializeField] private InfoCompteur so_infoCompteur;
    [SerializeField] private TMP_Text champPV;
    private void OnTriggerEnter(Collider other)
    {
        so_infoCompteur.nbVie -= 1;
        champPV.text = "Points de vie : " + so_infoCompteur.nbVie;
    }
}
