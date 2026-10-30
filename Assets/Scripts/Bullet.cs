using UnityEngine;

// move toward the aim point
public class Bullet : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float speed = 15f;
    public float lifetime = 1.5f;
    public int damage = 1;
    public LayerMask enemyLayers;

    void Start()
    {
        // clean up missed shots
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        float distance = speed * Time.deltaTime;
        // check between frames so fast bullets do not skip enemies
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.08f, transform.right, distance, enemyLayers);
        if (hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null) enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
        transform.position += transform.right * distance;
    }
}
