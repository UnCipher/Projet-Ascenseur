using UnityEngine;

public class MouvementBouleDeFeu : MonoBehaviour
{
    [Header("Réglages")]
    [SerializeField] private float vitesse = 8f;

    [Header("Cible")]
    [SerializeField] private Vector3 targetPosition;

    private Vector3 direction;

    void Start()
    {
        direction = (targetPosition - transform.position).normalized;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    void Update()
    {
        transform.position += direction * vitesse * Time.deltaTime;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        Invoke(nameof(DestroyFireball), 3f);
    }

    private void DestroyFireball()
    {
        Destroy(gameObject);
    }
}