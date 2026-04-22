using UnityEngine;
using System.Collections;

public class PlayerMeleeAttack : MonoBehaviour
{
    public GameObject slashHitbox;
    public Transform slashPivot;

    public float activeTime = 0.18f;
    public float cooldown = 0.5f;
    public float swingStartOffset = 70f;
    public float swingEndOffset = -70f;

    // Distance from player center to center of hitbox, in world units
    public float hitboxWorldDistance = 2f;

    private float nextAttackTime;
    private Renderer visualRenderer;
    private SpriteRenderer visualSprite;
    private Material visualMat;
    private string colorProp;
    private Color visualBaseColor;
    private PlayerController controller;
    private PlayerShooter shooter;

    // Cached original local position of hitbox (used to preserve its "distance along +X")
    private Vector3 hitboxBaseLocalPos;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        shooter = GetComponent<PlayerShooter>();
        if (slashHitbox != null)
        {
            hitboxBaseLocalPos = slashHitbox.transform.localPosition;
            slashHitbox.SetActive(false);
        }

        if (slashPivot != null)
        {
            slashPivot.gameObject.SetActive(false);
            visualSprite = slashPivot.GetComponentInChildren<SpriteRenderer>(true);
            if (visualSprite != null)
            {
                visualBaseColor = visualSprite.color;
            }
            else
            {
                visualRenderer = slashPivot.GetComponentInChildren<MeshRenderer>(true);
                if (visualRenderer != null)
                {
                    visualMat = visualRenderer.material;
                    if (visualMat.HasProperty("_BaseColor")) colorProp = "_BaseColor";
                    else if (visualMat.HasProperty("_Color")) colorProp = "_Color";
                    if (colorProp != null)
                    {
                        visualBaseColor = visualMat.GetColor(colorProp);
                        if (visualMat.HasProperty("_Surface")) visualMat.SetFloat("_Surface", 1f);
                        if (visualMat.HasProperty("_SrcBlend")) visualMat.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        if (visualMat.HasProperty("_DstBlend")) visualMat.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        if (visualMat.HasProperty("_ZWrite")) visualMat.SetFloat("_ZWrite", 0f);
                        visualMat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                        visualMat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    }
                }
            }
        }
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing) return;
        if (controller != null && controller.IsDead) return;
        if (Input.GetMouseButtonDown(1) && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + cooldown;
            StartCoroutine(DoSlash());
        }
    }

    IEnumerator DoSlash()
    {
        Vector2 aim = (shooter != null) ? shooter.AimDirection : Vector2.right;
        if (aim.sqrMagnitude < 0.0001f) aim = Vector2.right;
        float baseAngle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;

        // Place hitbox at player position + aim*distance, in WORLD space.
        if (slashHitbox != null)
        {
            slashHitbox.SetActive(true);
            // Use world-space position so hitbox is always <distance> units from player along aim direction.
            slashHitbox.transform.position = transform.position + (Vector3)(aim * hitboxWorldDistance);
            slashHitbox.transform.rotation = Quaternion.Euler(0f, 0f, baseAngle);
        }
        if (slashPivot != null) slashPivot.gameObject.SetActive(true);

        float elapsed = 0f;
        while (elapsed < activeTime)
        {
            float t = elapsed / activeTime;

            // Keep hitbox following player (in case player moves mid-swing)
            if (slashHitbox != null)
            {
                slashHitbox.transform.position = transform.position + (Vector3)(aim * hitboxWorldDistance);
            }

            if (slashPivot != null)
            {
                float swing = Mathf.Lerp(swingStartOffset, swingEndOffset, t);
                slashPivot.localRotation = Quaternion.Euler(0f, 0f, baseAngle + swing);
            }

            float alpha = 1f - t;
            if (visualSprite != null)
            {
                Color c = visualBaseColor; c.a = alpha;
                visualSprite.color = c;
            }
            else if (visualMat != null && colorProp != null)
            {
                Color c = visualBaseColor; c.a = alpha;
                visualMat.SetColor(colorProp, c);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (slashHitbox != null)
        {
            slashHitbox.SetActive(false);
            // Restore its local position so it doesn't wander forever
            slashHitbox.transform.localPosition = hitboxBaseLocalPos;
            slashHitbox.transform.localRotation = Quaternion.identity;
        }
        if (slashPivot != null) slashPivot.gameObject.SetActive(false);
        if (visualSprite != null) visualSprite.color = visualBaseColor;
        else if (visualMat != null && colorProp != null) visualMat.SetColor(colorProp, visualBaseColor);
    }
}
