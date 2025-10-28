using UnityEngine;
using UnityEngine.InputSystem;

public class CurseurRaycast : MonoBehaviour
{    
    [SerializeField] private GestionnaireScore gestionnaireScore;
    [SerializeField] private InfoAsteroide infoAsteroide;

    void Update()
    {
        
    }

    public void OnLook(InputAction.CallbackContext context)
    {

        Vector2 mousePass = context.ReadValue<Vector2>();
        Debug.Log(mousePass);
        
        Ray ray = Camera.main.ScreenPointToRay(mousePass);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.transform.name);
            Destroy(hit.transform.gameObject);
            gestionnaireScore.AsteroideScore(infoAsteroide.scoreAsteroide);
        }
    }
}
