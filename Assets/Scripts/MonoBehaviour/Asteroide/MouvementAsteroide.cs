using UnityEngine;

public class MouvementAsteroide : MonoBehaviour
{
    [SerializeField] private InfoAsteroide infoAsteroide;
    
    [SerializeField] private float vitesse = 5f;
    public Vector3 directionAsteroides = new Vector3(0, 0, -1); 

    void Update()
    {
        transform.position += directionAsteroides * vitesse * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Invoke("DestroyAsteroid", 3f);
    }

    private void DestroyAsteroid()
    {
        Destroy(gameObject);
    }
}
