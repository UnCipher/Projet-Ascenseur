using UnityEngine;

public class Painter : MonoBehaviour
{
    public Material paintBrushMaterial;
    public float brushSize = 0.1f;

    public void PaintAt(PaintableBuilding building, Vector3 worldPos)
    {
        // Convertir position world → UV sur le mesh
        if (!TryGetUV(building, worldPos, out Vector2 uv)) return;

        // Peinture
        paintBrushMaterial.SetFloat("_BrushSize", brushSize);
        paintBrushMaterial.SetVector("_BrushCenter", new Vector4(uv.x, uv.y, 0, 0));

        Graphics.Blit(null, building.paintMask, paintBrushMaterial);
    }

    bool TryGetUV(PaintableBuilding building, Vector3 worldPos, out Vector2 uv)
    {
        uv = Vector2.zero;

        Ray ray = new Ray(worldPos + Vector3.back * 5f, Vector3.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            if (hit.transform == building.transform)
            {
                uv = hit.textureCoord;
                return true;
            }
        }
        return false;
    }
}