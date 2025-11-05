using UnityEngine;
using TMPro;

public class GestionnaireCompteur : MonoBehaviour
{
    [SerializeField]
    private InfoCompteur so_infoCompteur;

    [SerializeField]
    private TMP_Text champCompteur;

    public static GestionnaireCompteur instance;

    void Start()
    {
        ResetCompteur();
        UpdateText();
    }

    void Update()
    {
        if (so_infoCompteur.compteur == 0)
        {
            // Lancer scène Ascenseur
        }
    }
    
    void Awake()
    {
        instance = this;
    }

    public void ResetCompteur(){
        so_infoCompteur.compteur = 40;
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
