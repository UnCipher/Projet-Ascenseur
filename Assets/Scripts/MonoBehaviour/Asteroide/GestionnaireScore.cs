using UnityEngine;
using TMPro;

public class GestionnaireScore : MonoBehaviour
{
    [SerializeField]
    private InfoScore so_infoScore;

    [SerializeField]
    private TMP_Text champScore;

    void Start(){
        UpdateText();
    }

    public void ResetScore(){
        so_infoScore.score = 0;
    }

    public void EnemyScore(int nombreScore)
    {
        so_infoScore.score += nombreScore;
        UpdateText();
    }

    public void UpdateText()
    {
        champScore.text = "Score : " + so_infoScore.score;
    }
}
