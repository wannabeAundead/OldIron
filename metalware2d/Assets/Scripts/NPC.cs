using UnityEngine;

public class NPC : MonoBehaviour
{
    public WeaponDatabase weaponDatabase;
    public Sprite[] npcSprites;
    public string npcName = "";
    public Sprite CurrentSprite;
    public Weapon OfferedWeapon;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnStateChanged += HandleStateChanged;
    }

    void OnDisable()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnStateChanged -= HandleStateChanged;
    }

    public void PickNewOffer()
    {
        if (npcSprites != null && npcSprites.Length > 0)
        {
            int idx = Random.Range(0, npcSprites.Length);
            CurrentSprite = npcSprites[idx];
            if (CurrentSprite != null) npcName = CurrentSprite.name;
            if (spriteRenderer != null && CurrentSprite != null)
            {
                spriteRenderer.sprite = CurrentSprite;
                spriteRenderer.color = Color.white;
            }
        }
        if (weaponDatabase != null)
        {
            OfferedWeapon = weaponDatabase.GetRandom();
        }
    }

    void HandleStateChanged(GameState s)
    {
        if (s == GameState.LevelUp)
        {
            if (OfferedWeapon == null)
            {
                if (npcSprites != null && npcSprites.Length > 0)
                {
                    int idx = Random.Range(0, npcSprites.Length);
                    CurrentSprite = npcSprites[idx];
                    if (CurrentSprite != null) npcName = CurrentSprite.name;
                    if (spriteRenderer != null && CurrentSprite != null)
                    {
                        spriteRenderer.sprite = CurrentSprite;
                        spriteRenderer.color = Color.white;
                    }
                }
                if (weaponDatabase != null)
                {
                    OfferedWeapon = weaponDatabase.GetRandom();
                }
            }
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector3 offset = new Vector3(2.5f, 0f, 0f);
                transform.position = player.transform.position + offset;
            }
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            OfferedWeapon = null;
        }
    }
}
