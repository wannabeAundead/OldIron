using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int maxHealth = 100;
    public int currentHealth;
    public float invincibilityTime = 0.5f;
    public bool IsDead;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private HitFlash hitFlash;
    private float invincibleUntil;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }
        hitFlash = GetComponent<HitFlash>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        bool playing = (GameManager.Instance == null) || (GameManager.Instance.State == GameState.Playing);
        if (IsDead || !playing) { moveInput = Vector2.zero; return; }
        float x = 0f;
        float y = 0f;
        if (Input.GetKey(KeyCode.A)) x -= 1f;
        if (Input.GetKey(KeyCode.D)) x += 1f;
        if (Input.GetKey(KeyCode.W)) y += 1f;
        if (Input.GetKey(KeyCode.S)) y -= 1f;
        moveInput = new Vector2(x, y).normalized;
    }

    void FixedUpdate()
    {
        if (rb != null) rb.linearVelocity = moveInput * moveSpeed;
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;
        if (Time.time < invincibleUntil) return;
        invincibleUntil = Time.time + invincibilityTime;

        currentHealth -= amount;
        if (hitFlash != null) hitFlash.Flash();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            IsDead = true;
            if (rb != null) rb.linearVelocity = Vector2.zero;
            if (GameManager.Instance != null) GameManager.Instance.RequestGameOver();
        }
    }

    public void ResetPlayer()
    {
        currentHealth = maxHealth;
        IsDead = false;
        invincibleUntil = 0f;
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    void OnCollisionStay2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Enemy")) TakeDamage(10);
    }
}
