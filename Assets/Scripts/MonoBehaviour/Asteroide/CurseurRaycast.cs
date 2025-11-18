using UnityEngine;
using UnityEngine.InputSystem;

public class CurseurRaycast : MonoBehaviour
{    
    [SerializeField] private GestionnaireCompteur gestionnaireCompteur;
    [SerializeField] private InfoCompteur so_infoCompteur;
    [SerializeField] private InfoAsteroide infoAsteroide;
    [SerializeField] private GameObject pistolet;
    [SerializeField] private GameObject pistolet2;
    [SerializeField] private float distancePistolet = 10f;

    [Header("Fracture")]
    [SerializeField] private GameObject[] fractureAsteroidPrefabs;
    [SerializeField] private float fractureAsteroidLifetime = 5f;
    [SerializeField] private int nombreDeFractures = 2;

    [Header("Effet visuel")]
    [SerializeField] private GameObject effetExplosionPrefab;
    [SerializeField] private float effetExplosionLifetime = 3f;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserDuration = 0.1f;

    [Header("Sons")]
    [SerializeField] private AudioClip laserClip;
    private AudioSource audioSource;

    [Header("Animations fusil")] 
    [SerializeField] private Animator pistoletAnimator;
    [SerializeField] private Animator pistoletAnimator2;

    private string currentCorner = "";

    [Header("Lissage du mouvement des mains")]
    [SerializeField] [Range(0.01f, 1f)] private float smoothSpeed = 0.15f;

    // stockage interne de la position lissée
    private Vector2 smoothedUV = Vector2.zero;


    // Contrôle souris (debug)
    
    /* public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Ray ray = LevelManager.instance.centerCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        Debug.Log(ray);

        if (Physics.Raycast(ray, out hit))
        {
            GérerImpact(hit);
        }
        else
        {
            Debug.Log("Aucun objet touché par le raycast !");
        }
    } */
    
    
    void Start()
    {
        LevelManager.instance.transform.eulerAngles = new Vector3(0, 180, 0);
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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

            DetectHandCorner(leftWallInfo, rightWallInfo);

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

                Ray ray = LevelManager.instance.centerCamera.ScreenPointToRay(screenPos);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.Log("do sum 1");
                    GérerImpact(hit);
                }
            }
        }
    }

    private void DetectHandCorner(Wall.WallInfo left, Wall.WallInfo right)
    {
        // moyenne des positions UV des mains
        Vector2 avg = (left.uv + right.uv) * 0.5f;

        smoothedUV = Vector2.Lerp(smoothedUV, avg, smoothSpeed);

        float x = smoothedUV.x - 0.5f;
        float y = smoothedUV.y - 0.5f;

        string nextCorner = "";

        if (x < 0 && y > 0) nextCorner = "Armature|UpLeft";
        else if (x > 0 && y > 0) nextCorner = "Armature|UpRight";
        else if (x < 0 && y < 0) nextCorner = "Armature|DownLeft";
        else if (x > 0 && y < 0) nextCorner = "Armature|DownRight";

        if (nextCorner == "" || nextCorner == currentCorner)
            return;

        // reset triggers
        pistoletAnimator.ResetTrigger("Armature|UpLeft");
        pistoletAnimator.ResetTrigger("Armature|UpRight");
        pistoletAnimator.ResetTrigger("Armature|DownLeft");
        pistoletAnimator.ResetTrigger("Armature|DownRight");

        pistoletAnimator2.ResetTrigger("Armature|UpLeft");
        pistoletAnimator2.ResetTrigger("Armature|UpRight");
        pistoletAnimator2.ResetTrigger("Armature|DownLeft");
        pistoletAnimator2.ResetTrigger("Armature|DownRight");

        // Active nouveau coin
        pistoletAnimator.SetTrigger(nextCorner);
        pistoletAnimator2.SetTrigger(nextCorner);

        currentCorner = nextCorner;
    }

    // Fonction commune d'impact (Kinect & souris)
    private void GérerImpact(RaycastHit hit)
    {
        if (hit.transform.gameObject.GetComponent<MouvementAsteroide>())
        {
            TirerLaser(pistolet.transform.position, hit.point);
            TirerLaser(pistolet2.transform.position, hit.point);

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

            audioSource.PlayOneShot(laserClip);

            if (so_infoCompteur.compteur == 0)
                LevelManager.instance.OnElevator();

            
        }
    }
        
    private void TirerLaser(Vector3 start, Vector3 end)
    {
        if (laserPrefab == null) return;

        GameObject laser = Instantiate(laserPrefab);
        LineRenderer lr = laser.GetComponent<LineRenderer>();
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        Destroy(laser, laserDuration);
    }
}
