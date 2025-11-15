using UnityEngine;

public class SpawnAsteroids : MonoBehaviour
{

    [SerializeField] private GameObject[] asteroidPrefabs;

    [SerializeField]
    private Vector3 zoneSize;

    [SerializeField] private float repeatTime = 0.5f;

    [SerializeField] private float asteroidLifetime = 10f;

    public float startDelay = 15f;
    
    void Start()
    {
        InvokeRepeating("AddGameObject", startDelay, repeatTime);
    }

    private void AddGameObject()
    {

        int randomIndex = Random.Range(0, asteroidPrefabs.Length);
        GameObject asteroidPrefab = asteroidPrefabs[randomIndex];

        GameObject instantiated = Instantiate(asteroidPrefab);

        instantiated.transform.position = new Vector3(
            Random.Range(transform.position.x - zoneSize.x / 2, transform.position.x + zoneSize.x / 2),
            Random.Range(transform.position.y - zoneSize.y / 2, transform.position.y + zoneSize.y / 2),
            Random.Range(transform.position.z - zoneSize.z / 2, transform.position.z + zoneSize.z / 2)
        );

        Destroy(instantiated, asteroidLifetime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, zoneSize);
    }
}
