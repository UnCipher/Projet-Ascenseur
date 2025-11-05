using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionnaireScene : MonoBehaviour
{
   public void ChangeScene(string nomScene){
        SceneManager.LoadScene(nomScene);
   }
}
