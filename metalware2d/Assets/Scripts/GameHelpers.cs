using UnityEngine;

public static class GameHelpers
{
    public static void ClearEnemiesAndBullets()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++) Object.Destroy(enemies[i]);
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        for (int i = 0; i < bullets.Length; i++) Object.Destroy(bullets[i]);
    }

    public static void ResetPlayerToOrigin()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p == null) return;
        p.transform.position = Vector3.zero;
        PlayerController pc = p.GetComponent<PlayerController>();
        if (pc != null) pc.ResetPlayer();
    }
}
