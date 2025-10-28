using UnityEngine;

public class MouvementAsteroide : MonoBehaviour
{
    [SerializeField] private float vitesse = 5f;
    [SerializeField] private Vector3 direction;
        void Update()
        {
            transform.position += direction * vitesse * Time.deltaTime;
        }
}
