using UnityEngine;
using System.Collections.Generic;

public class MeleeHitbox : MonoBehaviour
{
    public int damage = 50;

    private HashSet<Enemy> alreadyHit = new HashSet<Enemy>();

    void OnEnable()
    {
        alreadyHit.Clear();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        Enemy e = other.GetComponent<Enemy>();
        if (e == null) return;
        if (alreadyHit.Contains(e)) return;
        alreadyHit.Add(e);
        e.TakeDamage(damage);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        Enemy e = other.GetComponent<Enemy>();
        if (e == null) return;
        if (alreadyHit.Contains(e)) return;
        alreadyHit.Add(e);
        e.TakeDamage(damage);
    }
}
