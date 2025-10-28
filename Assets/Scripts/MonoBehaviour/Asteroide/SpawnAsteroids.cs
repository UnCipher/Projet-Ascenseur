using UnityEngine;

public class SpawnAsteroids : MonoBehaviour
{

    [SerializeField]
    private GameObject asteroidPrefab;

    [SerializeField]
    private Vector3 zoneSize;

    [SerializeField] private float repeatTime = 0.5f;
    
    void Start()
    {
        InvokeRepeating("AddGameObject", 0f, repeatTime);
    }

    void AddGameObject()
    {
        GameObject instantiated = Instantiate(asteroidPrefab);

        instantiated.transform.position = new Vector3(
            Random.Range(transform.position.x - zoneSize.x / 2, transform.position.x + zoneSize.x / 2),
            Random.Range(transform.position.y - zoneSize.y / 2, transform.position.y + zoneSize.y / 2),
            Random.Range(transform.position.z - zoneSize.z / 2, transform.position.z + zoneSize.z / 2)
        );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, zoneSize);
    }
}
