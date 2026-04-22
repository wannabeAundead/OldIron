using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public GameObject bulletPrefab;

    private float nextFireTime;
    private Camera cam;
    private PlayerController controller;
    private PlayerInventory inventory;

    public Vector2 AimDirection { get; private set; } = Vector2.right;

    void Awake()
    {
        cam = Camera.main;
        controller = GetComponent<PlayerController>();
        inventory = GetComponent<PlayerInventory>();
    }

    void Update()
    {
        if (cam == null) return;
        if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing) return;
        if (controller != null && controller.IsDead) return;

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        AimDirection = ((Vector2)(mouseWorld - transform.position)).normalized;

        Weapon w = (inventory != null) ? inventory.equippedWeapon : null;
        if (w == null) return;

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + w.fireRate;
            if (bulletPrefab != null)
            {
                GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Bullet bullet = b.GetComponent<Bullet>();
                if (bullet != null) bullet.Launch(AimDirection, w.bulletSpeed, w.bulletDamage);
                Renderer r = b.GetComponentInChildren<Renderer>();
                if (r != null && r.material != null)
                {
                    if (r.material.HasProperty("_BaseColor")) r.material.SetColor("_BaseColor", w.bulletColor);
                    else if (r.material.HasProperty("_Color")) r.material.SetColor("_Color", w.bulletColor);
                }
            }
        }
    }
}
