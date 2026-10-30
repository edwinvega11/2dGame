using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// handles damage and restarting the round
public class PlayerHealth : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int maxHealth = 3;
    public float hurtCooldown = 1f;
    public Image healthFill;
    public GameObject gameOverPanel;

    public int CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    SpriteRenderer spriteRenderer;
    float nextHitTime;
    float flashEndTime;
    Camera gameCamera;

    void Awake()
    {
        Time.timeScale = 1f;
        CurrentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameCamera = Camera.main;
        gameOverPanel.SetActive(false);
        UpdateHealthDisplay();
    }

    void Update()
    {
        if (!IsDead) spriteRenderer.color = Time.time < flashEndTime ? Color.red : Color.white;
    }

    void LateUpdate()
    {
        // keep the bar just below the player's feet
        RectTransform bar = healthFill.rectTransform;
        Vector3 screenPosition = gameCamera.WorldToScreenPoint(transform.position + Vector3.down * 0.25f);
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)bar.parent,
            screenPosition, null, out Vector2 position);
        bar.anchoredPosition = position - Vector2.right * bar.sizeDelta.x / 2f;
    }

    public void HealFully()
    {
        if (IsDead) return;
        CurrentHealth = maxHealth;
        UpdateHealthDisplay();
    }

    public void TakeDamage(int damage)
    {
        // give the player a moment to escape after a hit
        if (IsDead || Time.time < nextHitTime || damage <= 0) return;
        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
        nextHitTime = Time.time + hurtCooldown;
        flashEndTime = Time.time + 0.2f;
        UpdateHealthDisplay();

        if (CurrentHealth == 0)
        {
            IsDead = true;
            GameAudio.Instance?.Lose();
            spriteRenderer.color = Color.gray;
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else GameAudio.Instance?.HurtPlayer();
    }

    void UpdateHealthDisplay()
    {
        healthFill.rectTransform.localScale = new Vector3((float)CurrentHealth / maxHealth, 1f, 1f);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
