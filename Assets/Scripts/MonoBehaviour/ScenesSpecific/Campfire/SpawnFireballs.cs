using UnityEngine;

public class SpawnFireballs : MonoBehaviour
{
    [Header("Fireball Settings")]
    [SerializeField] private GameObject[] fireballPrefabs;
    [SerializeField] private bool rotateTowardsPlayer = true;
    [SerializeField] private float fireballLifetime = 8f;

    [Header("Spawn Area")]
    [SerializeField] private Vector3 zoneSize = new Vector3(10, 10, 10);

    [Header("Timing")]
    [SerializeField] private float startDelay = 5f;
    [SerializeField] private float repeatTime = 0.5f;

    [Header("Dragon Animator")]
    [SerializeField] private Animator dragonAnimator;

    private Transform playerTarget;

    void Start()
    {
        playerTarget = LevelManager.instance.centerCamera.transform;

        dragonAnimator.SetTrigger("Fireball");
        InvokeRepeating(nameof(SpawnFireball), startDelay, repeatTime);
    }

    private void SpawnFireball()
    {
        if (fireballPrefabs.Length == 0)
            return;

        int randomIndex = Random.Range(0, fireballPrefabs.Length);
        GameObject prefab = fireballPrefabs[randomIndex];

        GameObject fireball = Instantiate(prefab);

        fireball.transform.position = new Vector3(
            Random.Range(transform.position.x - zoneSize.x / 2, transform.position.x + zoneSize.x / 2),
            Random.Range(transform.position.y - zoneSize.y / 2, transform.position.y + zoneSize.y / 2),
            Random.Range(transform.position.z - zoneSize.z / 2, transform.position.z + zoneSize.z / 2)
        );

        if (rotateTowardsPlayer && playerTarget != null)
        {
            Vector3 lookDir = (playerTarget.position - fireball.transform.position).normalized;
            fireball.transform.rotation = Quaternion.LookRotation(lookDir);
        }

        Destroy(fireball, fireballLifetime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, zoneSize);
    }
}