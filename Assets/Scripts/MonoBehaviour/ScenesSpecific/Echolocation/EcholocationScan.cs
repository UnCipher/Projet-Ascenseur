using UnityEngine;

public class EcholocationScan : MonoBehaviour
{
    // ---------------------------
    // Values
    // ---------------------------

    public static GameObject particlePrefab;

    // ---------------------------
    // Functions
    // ---------------------------

    public static void Instantiate(Vector3 position, float size, float duration)
    {
        // Set Values
        GameObject particleObject = Instantiate(particlePrefab, position, Quaternion.Euler(Vector3.zero));
        ParticleSystem particle = particleObject.transform.GetChild(0).GetComponent<ParticleSystem>();

        if (particle != null)
        {
            var main = particle.main;

            main.startLifetime = duration;
            main.startSize = size;
        }

        Destroy(particleObject, duration + .5f);
    }
}
