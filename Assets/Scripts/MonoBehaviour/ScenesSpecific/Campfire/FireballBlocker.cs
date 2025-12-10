using UnityEngine;

public class FireballBlocker : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private LayerMask fireballLayer;
    [SerializeField] private float fireballDestroyDelay = 0.1f;
    [SerializeField] private GameObject blockEffectPrefab;

    [Header("Main Settings")]
    [SerializeField] private float smoothSpeed = 0.12f;

    private Vector2 smoothedUV;

    void FixedUpdate()
    {
        Player[] players = LevelManager.GetActivePlayers();
        if (players.Length == 0) return;

        for (int i = 0; i < players.Length; i++)
        {
            Wall.WallInfo left = players[i].GetLeftWallInfo();
            Wall.WallInfo right = players[i].GetRightWallInfo();

            // Les deux mains au centre = on vise
            if (left.selectedWall == Wall.SelectedWall.Center &&
                right.selectedWall == Wall.SelectedWall.Center)
            {
                Vector2 avg = (left.uv + right.uv) * 0.5f;
                smoothedUV = Vector2.Lerp(smoothedUV, avg, smoothSpeed);

                Vector3 screenPos = new Vector3(
                    smoothedUV.x * Screen.width,
                    smoothedUV.y * Screen.height,
                    10f
                );

                Ray ray = LevelManager.instance.centerCamera.ScreenPointToRay(screenPos);

                if (Physics.Raycast(ray, out RaycastHit hit, 200f, fireballLayer))
                {
                    BlockFireball(hit);
                }
            }
        }
    }

    private void BlockFireball(RaycastHit hit)
    {
        GameObject fireball = hit.collider.gameObject;

        // Effet d’impact optionnel
        if (blockEffectPrefab != null)
        {
            GameObject fx = Instantiate(blockEffectPrefab, hit.point, Quaternion.identity);
            Destroy(fx, 2f);
        }

        // Détruire la fireball
        Destroy(fireball, fireballDestroyDelay);
    }
}