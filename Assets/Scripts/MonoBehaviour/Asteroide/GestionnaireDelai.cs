using UnityEngine;
using TMPro;
using System.Collections;

public class GestionnaireDelai : MonoBehaviour
{
    [SerializeField] private SpawnAsteroids spawnAsteroids;
    [SerializeField] private TMP_Text champDelai;

    void Start()
    {
        StartCoroutine(CompteurVisuel());
    }

    private IEnumerator CompteurVisuel()
    {
        float delaiVisuelLocal = spawnAsteroids.startDelay;

        while (delaiVisuelLocal > 0)
        {
            champDelai.text = delaiVisuelLocal + " secondes restantes";
            yield return new WaitForSeconds(1f);
            delaiVisuelLocal -= 1f;
        }

        champDelai.text = "Astéroïdes en approche !";
      
        yield return new WaitForSeconds(3f);

        champDelai.text = "";
    }
}
