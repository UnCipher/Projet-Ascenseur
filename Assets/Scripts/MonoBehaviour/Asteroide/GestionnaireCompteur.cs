using UnityEngine;
using TMPro;

public class GestionnaireCompteur : MonoBehaviour
{
    [SerializeField]
    private InfoCompteur so_infoCompteur;

    [SerializeField]
    private TMP_Text champCompteur;

    [SerializeField] private int nbAsteroides = 2;

    public static GestionnaireCompteur instance;

    void Start()
    {
        ResetCompteur();
        UpdateText();
    }
    
    void Awake()
    {
        instance = this;
    }

    public void ResetCompteur(){
        so_infoCompteur.compteur = nbAsteroides;
    }

    public void AsteroideCompteur(int nombreCompteur)
    {
        so_infoCompteur.compteur += nombreCompteur;
        UpdateText();
    }

    public void UpdateText()
    {
        champCompteur.text = "Astéroïdes restants : " + so_infoCompteur.compteur;
    }
}
