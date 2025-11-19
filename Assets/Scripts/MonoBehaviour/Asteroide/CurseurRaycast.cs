using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using System.Collections;

public class CurseurRaycast : MonoBehaviour
{    
    [SerializeField] private GestionnaireCompteur gestionnaireCompteur;
    [SerializeField] private InfoCompteur so_infoCompteur;
    [SerializeField] private InfoAsteroide infoAsteroide;
    [SerializeField] private GameObject pistolet;
    [SerializeField] private GameObject pistolet2;
    [SerializeField] private float fusilRotationSpeed = 8f;
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
    [SerializeField] SoundProfile laserProfile;
    [SerializeField] private AudioClip laserClip;
    private AudioSource audioSource;

    [Header("Animations fusil")] 
    [SerializeField] private Animator pistoletAnimator;
    [SerializeField] private Animator pistoletAnimator2;

    [Header("Lissage du mouvement des mains")]
    [SerializeField] [Range(0.01f, 1f)] private float smoothSpeed = 0.15f;

    private Vector2 smoothedUV = Vector2.zero;

    [SerializeField] private Vector3 fusilDirectionOffset = new Vector3(0, 180, 0);
    [SerializeField] private Vector3 fusilPositionOffset;

    public VisualEffect warpSpeedVFX;
    private float rate = 0.02f;

    private bool warpActive;
    [SerializeField] private GameObject spawnAsteroids;

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

    void Start(){
        warpActive = false;
        warpSpeedVFX.Stop();
        warpSpeedVFX.SetFloat("WarpAmount", 0);
    }

    // Contrôle Kinect Azure 
     void FixedUpdate()
    {
        Player[] players = LevelManager.GetActivePlayers();
        if (players.Length == 0) return;

        for (int i = 0; i < players.Length; i++)
        {
            Wall.WallInfo leftWall = players[i].GetLeftWallInfo();
            Wall.WallInfo rightWall = players[i].GetRightWallInfo();

            if (leftWall.selectedWall == Wall.SelectedWall.Center &&
                rightWall.selectedWall == Wall.SelectedWall.Center)
            {
                // Moyenne des mains
                Vector2 avg = (leftWall.uv + rightWall.uv) * 0.5f;

                // Lissage
                smoothedUV = Vector2.Lerp(smoothedUV, avg, smoothSpeed);

                // Conversion caméra
                Vector3 screenPos = new Vector3(
                    smoothedUV.x * Screen.width,
                    smoothedUV.y * Screen.height,
                    10f // profondeur
                );

                Vector3 worldPos = LevelManager.instance.centerCamera.ScreenToWorldPoint(screenPos);

                // Déplacer les fusils vers la main
                OrienterFusilsVers(worldPos);

                // Raycast
                Ray ray = LevelManager.instance.centerCamera.ScreenPointToRay(screenPos);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform.GetComponent<MouvementAsteroide>())
                        GérerImpact(hit);
                }
            }
        }
    }

    private void OrienterFusilsVers(Vector3 target)
    {
        Vector3 targetOffset = target + fusilPositionOffset;

        Quaternion rot1 = Quaternion.LookRotation(targetOffset - pistolet.transform.position);
        Quaternion rot2 = Quaternion.LookRotation(targetOffset - pistolet2.transform.position);

        rot1 *= Quaternion.Euler(fusilDirectionOffset);
        rot2 *= Quaternion.Euler(fusilDirectionOffset);

        pistolet.transform.rotation = Quaternion.Lerp(
            pistolet.transform.rotation,
            rot1,
            Time.deltaTime * fusilRotationSpeed
        );

        pistolet2.transform.rotation = Quaternion.Lerp(
            pistolet2.transform.rotation,
            rot2,
            Time.deltaTime * fusilRotationSpeed
        );
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

            SoundPlayer.CreateSoundPlayer(laserProfile);

            if (so_infoCompteur.compteur == 0){
                    warpActive = true;
                    StartCoroutine(ActivateParticles());
                    spawnAsteroids.SetActive(false);

                   Invoke("DelayElevator", 10f);
            }
        }
    }

    private void DelayElevator(){
        LevelManager.instance.OnElevator();
    }

    private IEnumerator ActivateParticles()
    {
        if(warpActive)
        {
            warpSpeedVFX.Play();

            float amount = warpSpeedVFX.GetFloat("WarpAmount");
            while(amount < 1 && warpActive)
            {
                amount += rate;
                warpSpeedVFX.SetFloat("WarpAmount", amount);
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            float amount = warpSpeedVFX.GetFloat("WarpAmount");
            while(amount > 0 && !warpActive)
            {
                amount -= rate;
                warpSpeedVFX.SetFloat("WarpAmount", amount);
                yield return new WaitForSeconds(0.1f);

                if(amount <= 0+rate)
                {
                    amount = 0;
                    warpSpeedVFX.SetFloat("WarpAmount", amount);
                    warpSpeedVFX.Stop();
                }
            }

           
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
