using UnityEngine;
using UnityEngine.InputSystem;

public class CurseurRaycast : MonoBehaviour
{    
    [SerializeField] private GestionnaireScore gestionnaireScore;
    [SerializeField] private InfoAsteroide infoAsteroide;
    [SerializeField] private GameObject pistolet;
    [SerializeField] private float distancePistolet;

    public Camera mainCamera;

    public void OnLook(InputAction.CallbackContext context)
    {

        Vector2 mousePass = context.ReadValue<Vector2>();
        // Debug.Log(mousePass);
        
        Ray ray = Camera.main.ScreenPointToRay(mousePass);
        RaycastHit hit;

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePass.x, mousePass.y, mainCamera.nearClipPlane));

        mouseWorldPosition.z = distancePistolet;

        pistolet.transform.position = mouseWorldPosition;

        if (Physics.Raycast(ray, out hit))
        {
            // Debug.Log(hit.transform.name);
            Destroy(hit.transform.gameObject);
            gestionnaireScore.AsteroideScore(infoAsteroide.scoreAsteroide);
        }
    }
}
