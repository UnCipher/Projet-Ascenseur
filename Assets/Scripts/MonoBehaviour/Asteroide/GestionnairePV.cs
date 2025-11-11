using UnityEngine;
using TMPro;

public class GestionnairePV : MonoBehaviour
{

    [SerializeField] private InfoCompteur so_infoCompteur;
    [SerializeField] private TMP_Text champPV;

    void Start()
    {
        so_infoCompteur.nbVie = 3;
        champPV.text = "Points de vie : " + so_infoCompteur.nbVie;
    }

    void Update()
    {
        if (so_infoCompteur.nbVie == 0)
            LevelManager.instance.OnElevator();
    }
}
