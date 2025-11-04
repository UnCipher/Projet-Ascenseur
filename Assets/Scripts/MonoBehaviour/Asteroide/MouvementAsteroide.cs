using UnityEngine;

public class MouvementAsteroide : MonoBehaviour
{

    [SerializeField] private GestionnaireScore gestionnaireScore;
    [SerializeField] private InfoAsteroide infoAsteroide;
    
    [SerializeField] private float vitesse = 5f;
    public Vector3 directionAsteroides = new Vector3(0, 0, -1);

    void Start()
    {
        gestionnaireScore = GestionnaireScore.instance;
    }

    void Update()
    {
        transform.position += directionAsteroides * vitesse * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        gestionnaireScore.AsteroideScore(infoAsteroide.scoreAsteroide);
        Invoke("DestroyAsteroid", 3f);
    }

    private void DestroyAsteroid()
    {
        Destroy(gameObject);
    }
}
