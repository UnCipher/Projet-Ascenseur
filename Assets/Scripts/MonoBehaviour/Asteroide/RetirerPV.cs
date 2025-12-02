using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RetirerPV : MonoBehaviour
{
    [SerializeField] private InfoCompteur so_infoCompteur;
    [SerializeField] private TMP_Text champPV;
    [SerializeField] private GameObject vitreBrise1;
    [SerializeField] private GameObject vitreBrise2;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip vitreBrise3_son;

    [Header("GlobalVolume")]
    [SerializeField] private Volume globalVolume;
    private DepthOfField depthOfField;
    private Vignette vignette;

    [Header("Overrides Smooth")]
    [SerializeField] private float focalTarget = 0f;
    [SerializeField] private float vignetteTarget = 0f;
    [SerializeField] private float smoothSpeed = 2f;

    private bool sonJoue = false;

    void Start()
    {
        so_infoCompteur.nbVie = 3;
        champPV.text = "Points de vie : " + so_infoCompteur.nbVie;

        if (!globalVolume.profile.TryGet<DepthOfField>(out depthOfField))
            Debug.LogError("DepthOfField non trouvé !");
        if (!globalVolume.profile.TryGet<Vignette>(out vignette))
            Debug.LogError("Vignette non trouvée !");

        depthOfField.focalLength.value = 0f;
        vignette.intensity.value = 0f;
    }

    void Update(){
        if (depthOfField != null && vignette != null && focalTarget != 0 && vignetteTarget != 0)
        {
            depthOfField.focalLength.value = Mathf.Lerp(
                depthOfField.focalLength.value,
                focalTarget,
                Time.deltaTime * smoothSpeed
            );

            vignette.intensity.value = Mathf.Lerp(
                vignette.intensity.value,
                vignetteTarget,
                Time.deltaTime * smoothSpeed
            );
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Asteroide"))
        {
            if (ShieldController.IsProtected)
            {
                return;
            }
            
            so_infoCompteur.nbVie -= 1;
            champPV.text = "Points de vie : " + so_infoCompteur.nbVie;

            VisuelPertePV();
        }
    }

    private void VisuelPertePV(){
        if (so_infoCompteur.nbVie == 2) vitreBrise1.SetActive(true);
        if (so_infoCompteur.nbVie == 1) vitreBrise2.SetActive(true);

        if (so_infoCompteur.nbVie == 0)
        {
            vitreBrise1.SetActive(false);
            vitreBrise2.SetActive(false);

            focalTarget = 300f;
            vignetteTarget = 1f;
            Debug.Log(vignette);

            if(!sonJoue && audioSource != null && vitreBrise3_son != null)
            {
                audioSource.PlayOneShot(vitreBrise3_son);
                sonJoue = true;
            }

            Invoke("DelayElevator", 1f);
        }
    }

    private void DelayElevator(){
        LevelManager.instance.OnElevator();
    }
}
