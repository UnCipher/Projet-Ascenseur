using UnityEngine;
using UnityEngine.InputSystem;

public class CurseurRaycast : MonoBehaviour
{    
    [SerializeField] private GestionnaireCompteur gestionnaireCompteur;
    [SerializeField] private InfoCompteur so_infoCompteur;
    [SerializeField] private InfoAsteroide infoAsteroide;
    [SerializeField] private GameObject pistolet;
    [SerializeField] private float distancePistolet = 10f;
    [SerializeField] private Animator pistoletAnimator;
    [SerializeField] private Animator pistoletAnimator2;

    [Header("Fracture")]
    [SerializeField] private GameObject[] fractureAsteroidPrefabs;
    [SerializeField] private float fractureAsteroidLifetime = 5f;
    [SerializeField] private int nombreDeFractures = 2;

    [Header("Effet visuel")]
    [SerializeField] private GameObject effetExplosionPrefab;
    [SerializeField] private float effetExplosionLifetime = 3f;

    [SerializeField] Transform sym;

    // Contrôle souris (debug)

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 mousePass = context.ReadValue<Vector2>();

        Ray ray = LevelManager.instance.centerCamera.ScreenPointToRay(mousePass);
        RaycastHit hit;

        Vector3 mouseWorldPosition = LevelManager.instance.centerCamera.ScreenToWorldPoint(
            new Vector3(mousePass.x, mousePass.y, LevelManager.instance.centerCamera.nearClipPlane)
        );

        mouseWorldPosition.z = distancePistolet;
        pistolet.transform.position = mouseWorldPosition;

        Debug.Log(mousePass);

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.transform.name);
            GérerImpact(hit);
        }
    }
    
    
    void Start()
    {
        LevelManager.instance.transform.eulerAngles = new Vector3(0, 180, 0);
    }

    // Contrôle Kinect Azure 
    void FixedUpdate()
    {
        // Set Values
        Player[] players = LevelManager.GetActivePlayers();

        for(int i = 0;i<players.Length;i++)
        {
            // Set Values
            Wall.WallInfo leftWallInfo = players[i].GetLeftWallInfo();
            Wall.WallInfo rightWallInfo = players[i].GetRightWallInfo();

            // Check Left
            if (leftWallInfo.selectedWall == Wall.SelectedWall.Center)
            {
                Vector2 screenPos = new Vector3(leftWallInfo.uv.x * Screen.width, leftWallInfo.uv.y * Screen.height, distancePistolet);
                Vector3 worldPos = LevelManager.instance.centerCamera.ScreenToWorldPoint(screenPos);

                Ray ray = LevelManager.instance.centerCamera.ScreenPointToRay(screenPos);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GérerImpact(hit);
                    Debug.Log("do sum 1");
                }

                Debug.Log("left lele / " + screenPos);
            }

            // Check Right
            if (rightWallInfo.selectedWall == Wall.SelectedWall.Center)
            {
                Vector3 screenPos = new Vector3(rightWallInfo.uv.x * Screen.width, rightWallInfo.uv.y * Screen.height, distancePistolet);
                Vector3 worldPos = LevelManager.instance.centerCamera.ScreenToWorldPoint(screenPos);
                Debug.Log("right Lel / " + screenPos);

                sym.localPosition = screenPos;

                Ray ray = LevelManager.instance.centerCamera.ScreenPointToRay(screenPos);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.Log("do sum 1");
                    GérerImpact(hit);
                }
            }
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
        
        pistoletAnimator.SetTrigger("Fire");
        pistoletAnimator2.SetTrigger("Fire");
        Debug.Log(pistoletAnimator);

        gestionnaireCompteur.AsteroideCompteur(infoAsteroide.nbAsteroide);

        if (so_infoCompteur.compteur == 0)
            LevelManager.instance.OnElevator();
    }
}
