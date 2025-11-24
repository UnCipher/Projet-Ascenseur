using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(Collider))]
public class PaintableBuilding : MonoBehaviour
{
    [Header("Peinture")]
    public Material paintableMaterial;
    public Material paintBrushMaterial;
    public int textureSize = 1024;

    [Header("Peinture dynamique")]
    public float defaultBrushSize = 0.05f;
    public float defaultBrushStrength = 1f;

    [HideInInspector]
    public RenderTexture paintMask;

    void Start()
    {
        InitializeMask();
    }

    void InitializeMask()
    {
        paintMask = new RenderTexture(textureSize, textureSize, 0, RenderTextureFormat.ARGB32);
        paintMask.enableRandomWrite = true;
        paintMask.Create();

        if (paintableMaterial != null)
            paintableMaterial.SetTexture("_PaintMask", paintMask);

        ClearMask();
    }

    public void ClearMask()
    {
        RenderTexture active = RenderTexture.active;
        RenderTexture.active = paintMask;
        GL.Clear(true, true, Color.black);
        RenderTexture.active = active;
    }

    /// <summary>
    /// Appliquer la peinture sur le masque
    /// </summary>
    public void Paint(Vector2 uv, float brushSize = -1f, float strength = -1f)
    {
        if (paintBrushMaterial == null || paintMask == null)
            return;

        if (brushSize < 0) brushSize = defaultBrushSize;
        if (strength < 0) strength = defaultBrushStrength;

        paintBrushMaterial.SetVector("_UVPosition", new Vector4(uv.x, uv.y, 0, 0));
        paintBrushMaterial.SetFloat("_BrushSize", brushSize);
        paintBrushMaterial.SetFloat("_Strength", strength);

        RenderTexture temp = RenderTexture.GetTemporary(paintMask.width, paintMask.height, 0, paintMask.format);
        Graphics.Blit(paintMask, temp);
        Graphics.Blit(temp, paintMask, paintBrushMaterial);
        RenderTexture.ReleaseTemporary(temp);
    }
}