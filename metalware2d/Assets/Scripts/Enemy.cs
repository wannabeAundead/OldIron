using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int maxHealth = 50;

    private int currentHealth;
    private Rigidbody2D rb;
    private Transform target;
    private HitFlash hitFlash;

    void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }
        hitFlash = GetComponent<HitFlash>();
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) target = player.transform;
    }

    void FixedUpdate()
    {
        if (target == null || rb == null) return;
        Vector2 dir = ((Vector2)(target.position - transform.position)).normalized;
        rb.linearVelocity = dir * moveSpeed;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (hitFlash != null) hitFlash.Flash();
        if (currentHealth <= 0) Destroy(gameObject);
    }
}
