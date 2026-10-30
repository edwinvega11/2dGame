using UnityEngine;

// follow the player and take damage
[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Transform player;
    public float moveSpeed = 2.6f;
    public float stoppingDistance = 0.7f;
    public int health = 3;
    public Sprite[] idleFrames;
    public Sprite[] runFrames;
    public float framesPerSecond = 8f;
    public SoulEffect soulPrefab;
    [HideInInspector] public WaveManager waveManager;

    SpriteRenderer spriteRenderer;
    float animationTime;
    float flashEndTime;
    PlayerHealth playerHealth;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // find the player when spawning from a prefab
        if (player == null)
        {
            PlayerMovement movement = FindAnyObjectByType<PlayerMovement>();
            if (movement != null) player = movement.transform;
        }
        if (player != null) playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (health <= 0 || Time.timeScale == 0f || (playerHealth != null && playerHealth.IsDead)) return;
        bool isMoving = false;
        if (player != null)
        {
            Vector2 direction = player.position - transform.position;
            float distance = direction.magnitude;
            if (distance <= stoppingDistance + 0.05f && playerHealth != null)
                playerHealth.TakeDamage(1);
            if (distance > stoppingDistance)
            {
                // stop beside the player
                float step = Mathf.Min(moveSpeed * Time.deltaTime, distance - stoppingDistance);
                transform.position += (Vector3)(direction.normalized * step);
                isMoving = true;
            }
            if (Mathf.Abs(direction.x) > 0.01f) spriteRenderer.flipX = direction.x < 0f;
        }

        Sprite[] frames = isMoving ? runFrames : idleFrames;
        if (frames != null && frames.Length > 0)
        {
            animationTime += Time.deltaTime;
            int frame = Mathf.FloorToInt(animationTime * framesPerSecond) % frames.Length;
            spriteRenderer.sprite = frames[frame];
        }
        spriteRenderer.color = Time.time < flashEndTime ? Color.red : Color.white;
    }

    public void TakeDamage(int damage)
    {
        if (health <= 0) return;
        health -= damage;
        flashEndTime = Time.time + 0.12f;
        if (health <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GameAudio.Instance?.Soul();
            if (soulPrefab != null)
                Instantiate(soulPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            if (waveManager != null) waveManager.EnemyDefeated();
            Destroy(gameObject);
        }
        else GameAudio.Instance?.HitCreep();
    }
}
