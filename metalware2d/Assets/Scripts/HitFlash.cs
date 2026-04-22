using UnityEngine;
using System.Collections;

public class HitFlash : MonoBehaviour
{
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Material meshMaterialInstance;
    private Color originalColor;
    private bool canTint;
    private string colorProp;
    private Coroutine running;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            canTint = true;
            return;
        }

        MeshRenderer mr = GetComponentInChildren<MeshRenderer>();
        if (mr != null)
        {
            meshMaterialInstance = mr.material;
            if (meshMaterialInstance.HasProperty("_BaseColor"))
            {
                colorProp = "_BaseColor";
                originalColor = meshMaterialInstance.GetColor(colorProp);
                canTint = true;
            }
            else if (meshMaterialInstance.HasProperty("_Color"))
            {
                colorProp = "_Color";
                originalColor = meshMaterialInstance.GetColor(colorProp);
                canTint = true;
            }
        }
    }

    public void Flash()
    {
        if (!canTint) return;
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        SetColor(flashColor);
        yield return new WaitForSeconds(flashDuration);
        SetColor(originalColor);
        running = null;
    }

    void SetColor(Color c)
    {
        if (spriteRenderer != null) spriteRenderer.color = c;
        else if (meshMaterialInstance != null) meshMaterialInstance.SetColor(colorProp, c);
    }
}
