using UnityEngine;

public class MouvementAsteroide : MonoBehaviour
{

    [SerializeField] private GestionnaireScore gestionnaireScore;
    [SerializeField] private InfoAsteroide infoAsteroide;
    
    [SerializeField] private float vitesse = 5f;
    [SerializeField] private Vector3 direction;

    void Start()
    {
        gestionnaireScore = GestionnaireScore.instance;
    }

    void Update()
    {
        transform.position += direction * vitesse * Time.deltaTime;
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
