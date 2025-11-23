using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    [SerializeField] private AudioSource warpSpeed_source;
    [SerializeField] private AudioClip warpSpeed_clip;

    [Header("Animations fusil")] 
    [SerializeField] private Animator pistoletAnimator;
    [SerializeField] private Animator pistoletAnimator2;

    [Header("Lissage du mouvement des mains")]
    [SerializeField] [Range(0.01f, 1f)] private float smoothSpeed = 0.15f;

    private Vector2 smoothedUV = Vector2.zero;

    [SerializeField] private Vector3 fusilDirectionOffset = new Vector3(0, 180, 0);
    [SerializeField] private Vector3 fusilPositionOffset;

    public VisualEffect warpSpeedVFX;
    public MeshRenderer warpSpeedShader;
    [SerializeField] private float rate = 0.02f;
    [SerializeField ]private float delai = 2.5f;

    private bool warpActive;
    [SerializeField] private GameObject spawnAsteroids;

    [Header("GlobalVolume")]
    [SerializeField] private Volume globalVolume;
    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;
    private ColorAdjustments colorAdjustments;

    [Header("Overrides Smooth")]
    [SerializeField] private float chromaticAberrationTarget = 0f;
    [SerializeField] private float lensDistortionTarget = 0f;
    [SerializeField] private float colorAdjustmentsTarget = 0f;
    [SerializeField] private float smoothSpeedPostProcess = 2f;

    [Header("Planète")]
    [SerializeField] private GameObject planete;
    [SerializeField] private float distancePlanete = -2000f;



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
        warpSpeedShader.material.SetFloat("Active_", 0);

    if (globalVolume.profile.TryGet<ChromaticAberration>(out var ca))
        chromaticAberration = ca;

    if (globalVolume.profile.TryGet<LensDistortion>(out var ld))
        lensDistortion = ld;

    if (globalVolume.profile.TryGet<ColorAdjustments>(out var col))
        colorAdjustments = col;

        chromaticAberration.intensity.value = 0f;
        lensDistortion.intensity.value = 0f;
        colorAdjustments.postExposure.value = 0f;
    }

    void Update(){
        if (chromaticAberration != null && lensDistortion != null && colorAdjustments != null)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(
                chromaticAberration.intensity.value,
                chromaticAberrationTarget,
                Time.deltaTime * smoothSpeedPostProcess
            );

            lensDistortion.intensity.value = Mathf.Lerp(
                lensDistortion.intensity.value,
                lensDistortionTarget,
                Time.deltaTime * smoothSpeedPostProcess
            );

            colorAdjustments.postExposure.value = Mathf.Lerp(
                colorAdjustments.postExposure.value,
                colorAdjustmentsTarget,
                Time.deltaTime * smoothSpeedPostProcess
            );

            Vector3 pos = planete.transform.localPosition;

            float newZ = Mathf.Lerp(
                pos.z,
                distancePlanete,
                Time.deltaTime * smoothSpeedPostProcess
            );

            planete.transform.localPosition = new Vector3(pos.x, pos.y, newZ);
        }
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
                    GérerImpact(hit);
                }
            }

            // Check Right
            if (rightWallInfo.selectedWall == Wall.SelectedWall.Center)
            {
                Vector3 screenPos = new Vector3(rightWallInfo.uv.x * Screen.width, rightWallInfo.uv.y * Screen.height, distancePistolet);
                Vector3 worldPos = LevelManager.instance.centerCamera.ScreenToWorldPoint(screenPos);

                sym.localPosition = screenPos;

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

            if (so_infoCompteur.compteur == 0)
            {
                StartCoroutine(WarpSequence());
                Invoke("DelayElevator", 10f);
            }

        }
    }

    private void DelayElevator(){
        LevelManager.instance.OnElevator();
    }

    private IEnumerator WarpSequence()
    {
        yield return new WaitForSeconds(2f);

        warpActive = true;

        StartCoroutine(ActivateParticles());
        StartCoroutine(ActivateShader());
        Destroy(spawnAsteroids);

        yield return new WaitForSeconds(1.5f);

        warpSpeed_source.PlayOneShot(warpSpeed_clip);

        chromaticAberrationTarget = 1f;
        lensDistortionTarget = -0.7f;
        colorAdjustmentsTarget = 3f;

        yield return new WaitForSeconds(3f);

        warpActive = false;

        StartCoroutine(ActivateParticles());
        StartCoroutine(ActivateShader());

        yield return new WaitForSeconds(2f);

        chromaticAberrationTarget = 0f;
        lensDistortionTarget = 0f;
        colorAdjustmentsTarget = 0f;
        distancePlanete = -300f;
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

    private IEnumerator ActivateShader()
    {
        
        if(warpActive)
        {
            yield return new WaitForSeconds(delai);
            float amount = warpSpeedShader.material.GetFloat("Active_");
            while(amount < 1 && warpActive)
            {
                amount += rate;
                warpSpeedShader.material.SetFloat("Active_", amount);
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            float amount = warpSpeedShader.material.GetFloat("Active_");
            while(amount > 0 && !warpActive)
            {
                amount -= rate;
                warpSpeedShader.material.SetFloat("Active_", amount);
                yield return new WaitForSeconds(0.1f);

                if(amount <= 0+rate)
                {
                    amount = 0;
                    warpSpeedShader.material.SetFloat("Active_", amount);
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
