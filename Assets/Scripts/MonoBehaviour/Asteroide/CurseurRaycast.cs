using UnityEngine;
using UnityEngine.InputSystem;

public class CurseurRaycast : MonoBehaviour
{    
    [SerializeField] private GestionnaireCompteur gestionnaireCompteur;
    [SerializeField] private InfoCompteur so_infoCompteur;
    [SerializeField] private GestionnaireScene gestionnaireScene;
    [SerializeField] private InfoAsteroide infoAsteroide;
    [SerializeField] private GameObject pistolet;
    [SerializeField] private float distancePistolet = 10f;

    [Header("Fracture")]
    [SerializeField] private GameObject[] fractureAsteroidPrefabs;
    [SerializeField] private float fractureAsteroidLifetime = 10f;
    [SerializeField] private int nombreDeFractures = 2;

    [Header("Effet visuel")]
    [SerializeField] private GameObject effetExplosionPrefab;
    [SerializeField] private float effetExplosionLifetime = 3f;

    public Camera mainCamera;
    public Player player;

    // Contrôle souris (debug)
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 mousePass = context.ReadValue<Vector2>();
        
        Ray ray = Camera.main.ScreenPointToRay(mousePass);
        RaycastHit hit;

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(mousePass.x, mousePass.y, mainCamera.nearClipPlane)
        );

        mouseWorldPosition.z = distancePistolet;
        pistolet.transform.position = mouseWorldPosition;

        if (Physics.Raycast(ray, out hit))
        {
            GérerImpact(hit);
        }
    }

    // Contrôle Kinect Azure 
    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("No Player reference assigned in CurseurRaycast!");
            return;
        }

        float x = player.handsInfo.rightHandPosX;
        float y = player.handsInfo.rightHandPosY;
        float z = player.handsInfo.rightHandPosZ;

        Vector3 screenPos = new Vector3(x * Screen.width, y * Screen.height, distancePistolet);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);

        pistolet.transform.position = Vector3.Lerp(
            pistolet.transform.position,
            worldPos,
            Time.deltaTime * 10f
        );

        Ray ray = mainCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GérerImpact(hit);
        }
    }

    // Fonction commune d'impact (Kinect & souris)
    private void GérerImpact(RaycastHit hit)
    {
        Destroy(hit.transform.gameObject);

        if (effetExplosionPrefab != null)
        {
            GameObject explosion = Instantiate(effetExplosionPrefab, hit.point, Quaternion.identity);
            explosion.transform.forward = hit.normal;
            Destroy(explosion, effetExplosionLifetime);
        }

        for (int i = 0; i < nombreDeFractures; i++)
        {
            int randomIndex = Random.Range(0, fractureAsteroidPrefabs.Length);
            GameObject fractureAsteroidPrefab = fractureAsteroidPrefabs[randomIndex];

            Vector3 spawnPosition = hit.point + Random.insideUnitSphere * 0.3f;

            GameObject instantiated = Instantiate(
                fractureAsteroidPrefab,
                spawnPosition,
                Random.rotation
            );

            MouvementAsteroide[] mouvements = instantiated.GetComponentsInChildren<MouvementAsteroide>();
            foreach (MouvementAsteroide mouvement in mouvements)
            {
                Vector3 randomDirection = (instantiated.transform.position - hit.point).normalized + Random.insideUnitSphere * 0.4f;
                mouvement.directionAsteroides = randomDirection.normalized;
            }

            Destroy(instantiated, fractureAsteroidLifetime);
        }

        gestionnaireCompteur.AsteroideCompteur(infoAsteroide.nbAsteroide);

        if (so_infoCompteur.compteur == 0)
        {
            gestionnaireScene.ChangeScene("Elevator");
        }
    }
}
