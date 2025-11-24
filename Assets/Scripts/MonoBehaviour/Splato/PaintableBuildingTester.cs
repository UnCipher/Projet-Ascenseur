using UnityEngine;

public class PaintableBuildingTester : MonoBehaviour
{
    private PaintableBuilding building;

    void Start()
    {
        building = GetComponent<PaintableBuilding>();
    }

    void Update()
    {
        if (building == null) return;

        // Test : peinture au centre du mesh avec la touche P
        if (Input.GetKeyDown(KeyCode.P))
        {
            // UV au centre
            Vector2 uvCenter = new Vector2(0.5f, 0.5f);
            building.Paint(uvCenter);
            Debug.Log("Peinture appliquée au centre !");
        }

        // Test : effacer le masque avec la touche C
        if (Input.GetKeyDown(KeyCode.C))
        {
            building.ClearMask();
            Debug.Log("Masque effacé !");
        }
    }
}
