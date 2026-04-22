using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image fillImage;
    public PlayerController player;

    void Update()
    {
        if (player == null || fillImage == null) return;
        fillImage.fillAmount = Mathf.Clamp01((float)player.currentHealth / Mathf.Max(1, player.maxHealth));
    }
}
