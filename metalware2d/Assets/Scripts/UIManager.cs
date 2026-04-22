using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject hudPanel;
    public GameObject pausePanel;
    public GameObject levelUpPanel;
    public GameObject gameOverPanel;

    public TMP_Text levelText;
    public TMP_Text timerText;
    public TMP_Text npcNameText;
    public TMP_Text offeredWeaponText;
    public TMP_Text currentWeaponText;
    public Image npcPortraitImage;

    public NPC npc;
    private PlayerInventory inventory;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) inventory = p.GetComponent<PlayerInventory>();
        if (GameManager.Instance != null) GameManager.Instance.OnStateChanged += OnStateChanged;
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (hudPanel != null) hudPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (levelUpPanel != null) levelUpPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnStateChanged -= OnStateChanged;
    }

    void OnStateChanged(GameState s)
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(s == GameState.MainMenu);
        if (hudPanel != null) hudPanel.SetActive(s == GameState.Playing || s == GameState.Paused);
        if (pausePanel != null) pausePanel.SetActive(s == GameState.Paused);
        if (levelUpPanel != null) levelUpPanel.SetActive(s == GameState.LevelUp);
        if (gameOverPanel != null) gameOverPanel.SetActive(s == GameState.GameOver);

        if (s == GameState.LevelUp && npc != null && inventory != null)
        {
            npc.PickNewOffer();
            Weapon offered = npc.OfferedWeapon;

            if (npcPortraitImage != null)
            {
                npcPortraitImage.sprite = npc.CurrentSprite;
                npcPortraitImage.enabled = (npc.CurrentSprite != null);
                npcPortraitImage.preserveAspect = true;
            }

            if (npcNameText != null)
            {
                npcNameText.text = (offered != null)
                    ? npc.npcName + " offers you a " + offered.displayName
                    : npc.npcName + " has nothing for you.";
            }

            if (offeredWeaponText != null)
            {
                offeredWeaponText.text = (offered != null)
                    ? "OFFERED: " + offered.displayName + "\n" + offered.description +
                      "\nDMG " + offered.bulletDamage + " | RATE " + offered.fireRate.ToString("F2") + "s"
                    : "OFFERED:\n(no weapon available)";
            }

            if (currentWeaponText != null && inventory.equippedWeapon != null)
            {
                Weapon c = inventory.equippedWeapon;
                currentWeaponText.text = "CURRENT: " + c.displayName + "\n" + c.description +
                    "\nDMG " + c.bulletDamage + " | RATE " + c.fireRate.ToString("F2") + "s";
            }
        }
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.State == GameState.Playing)
        {
            if (levelText != null) levelText.text = "LEVEL " + GameManager.Instance.currentLevel;
            if (timerText != null) timerText.text = Mathf.CeilToInt(GameManager.Instance.levelTimeRemaining).ToString() + "s";
        }

        if (GameManager.Instance.State == GameState.LevelUp)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (inventory != null && npc != null && npc.OfferedWeapon != null)
                {
                    inventory.Equip(npc.OfferedWeapon);
                }
                GameManager.Instance.RequestAdvanceLevel();
                return;
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                GameManager.Instance.RequestAdvanceLevel();
                return;
            }
        }
    }
}
