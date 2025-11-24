using UnityEngine;
using System.Collections;

public class RaycastSplato : MonoBehaviour
{
    [Header("Références")]
    public float smoothSpeed = 0.15f;

    [Header("Peinture")]
    public float brushSize = 0.05f;
    public float brushStrength = 1f;

    [Header("Splash")]
    public GameObject splashPrefab;
    public float splashLifetime = 1.5f;

    private Vector2 smoothedUV = Vector2.zero;
    private Camera cam;

    void Start()
    {
        // On démarre une coroutine pour attendre que LevelManager soit instancié
        StartCoroutine(WaitForLevelManager());
    }

    IEnumerator WaitForLevelManager()
    {
        while (LevelManager.instance == null || LevelManager.instance.centerCamera == null)
        {
            yield return null; // attend la frame suivante
        }

        cam = LevelManager.instance.centerCamera;
    }

    void FixedUpdate()
    {
        if (cam == null) return; // Caméra non encore assignée

        // Récupération des joueurs actifs
        Player[] players = LevelManager.GetActivePlayers();
        if (players.Length == 0) return;

        for (int i = 0; i < players.Length; i++)
        {
            var left = players[i].GetLeftWallInfo();
            var right = players[i].GetRightWallInfo();

            // Vérifie si les mains sont sur le mur central
            if (left.selectedWall != Wall.SelectedWall.Center &&
                right.selectedWall != Wall.SelectedWall.Center)
                continue;

            // Moyenne des UV pour les deux mains
            Vector2 avgUV = (left.uv + right.uv) * 0.5f;
            smoothedUV = Vector2.Lerp(smoothedUV, avgUV, smoothSpeed);

            // Raycast depuis la caméra
            Ray ray = cam.ViewportPointToRay(smoothedUV);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                PaintableBuilding building = hit.collider.GetComponent<PaintableBuilding>();
                if (building != null)
                {
                    building.Paint(hit.textureCoord, brushSize, brushStrength);
                    SpawnSplash(hit.point, hit.normal);
                }
            }
        }
    }

    private void SpawnSplash(Vector3 position, Vector3 normal)
    {
        if (splashPrefab == null) return;

        GameObject splash = Instantiate(splashPrefab, position, Quaternion.LookRotation(normal));
        float randomScale = Random.Range(0.5f, 1.5f);
        splash.transform.localScale = Vector3.one * randomScale;
        Destroy(splash, splashLifetime);
    }
}