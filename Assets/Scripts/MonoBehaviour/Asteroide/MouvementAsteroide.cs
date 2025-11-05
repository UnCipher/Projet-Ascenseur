using UnityEngine;

public class MouvementAsteroide : MonoBehaviour
{

    [SerializeField] private GestionnaireCompteur gestionnaireCompteur;
    [SerializeField] private InfoAsteroide infoAsteroide;
    
    [SerializeField] private float vitesse = 5f;
    public Vector3 directionAsteroides = new Vector3(0, 0, -1);

    void Start()
    {
        gestionnaireCompteur = GestionnaireCompteur.instance;
    }

    void Update()
    {
        transform.position += directionAsteroides * vitesse * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        gestionnaireCompteur.AsteroideCompteur(infoAsteroide.nbAsteroide);
        Invoke("DestroyAsteroid", 3f);
    }

    private void DestroyAsteroid()
    {
        Destroy(gameObject);
    }
}
