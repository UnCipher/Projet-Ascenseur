using UnityEngine;
using UnityEngine.InputSystem;

public class CurseurRaycast : MonoBehaviour
{    
    [SerializeField] private GestionnaireScore gestionnaireScore;
    [SerializeField] private InfoAsteroide infoAsteroide;
    [SerializeField] private GameObject pistolet;
    [SerializeField] private float distancePistolet;

    public Camera mainCamera;

    public Player player;

    // Code pour Debug avec la souris

    // public void OnLook(InputAction.CallbackContext context)
    // {

    //     Vector2 mousePass = context.ReadValue<Vector2>();
    //     // Debug.Log(mousePass);
        
    //     Ray ray = Camera.main.ScreenPointToRay(mousePass);
    //     RaycastHit hit;

    //     Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePass.x, mousePass.y, mainCamera.nearClipPlane));

    //     mouseWorldPosition.z = distancePistolet;

    //     pistolet.transform.position = mouseWorldPosition;

    //     if (Physics.Raycast(ray, out hit))
    //     {
    //         // Debug.Log(hit.transform.name);
    //         Destroy(hit.transform.gameObject);
    //         gestionnaireScore.AsteroideScore(infoAsteroide.scoreAsteroide);
    //     }
    // }
    
    // Code pour la Kinect Azure

    void Update()
    {

        if (player == null)
        {
            Debug.LogWarning("No Player reference assigned in CurseurRaycast!");
            return;
        }

        // Read right hand position floats from the Player script
        float x = player.handsInfo.rightHandPosX;
        float y = player.handsInfo.rightHandPosY;
        float z = player.handsInfo.rightHandPosZ;

        // Construct a vector from the floats
        Vector3 handPos = new Vector3(x * Screen.width, y * Screen.height, distancePistolet);

        // Move your gun smoothly to that position
        Vector3 screenPos = new Vector3(
            x * Screen.width,
            y * Screen.height,
            distancePistolet
        );

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);

        pistolet.transform.position = Vector3.Lerp(
            pistolet.transform.position,
            worldPos,
            Time.deltaTime * 10f
        );

        // Raycast forward from the camera through the hand position
        Ray ray = mainCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Destroy(hit.transform.gameObject);
            gestionnaireScore.AsteroideScore(infoAsteroide.scoreAsteroide);
        }
    }
}
