using UnityEngine;
using TMPro;

public class GestionnairePV : MonoBehaviour
{

    [SerializeField] private InfoCompteur so_infoCompteur;
    [SerializeField] private TMP_Text champPV;
    [SerializeField] private GameObject vitreBrise1;
    [SerializeField] private GameObject vitreBrise2;
    [SerializeField] private GameObject vitreBrise3;

    void Start()
    {
        so_infoCompteur.nbVie = 3;
        champPV.text = "Points de vie : " + so_infoCompteur.nbVie;
    }

    void Update()
    {


        if(so_infoCompteur.nbVie == 2)
            vitreBrise1.SetActive(true);


        if(so_infoCompteur.nbVie == 1)
            vitreBrise2.SetActive(true);


        if (so_infoCompteur.nbVie == 0){
            vitreBrise3.SetActive(true);
            LevelManager.instance.OnElevator();
        }   
      
    }
}
