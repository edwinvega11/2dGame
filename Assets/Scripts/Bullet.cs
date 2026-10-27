using UnityEngine;

// Travels in the direction the gun was pointing when this bullet was fired.
public class Bullet : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float speed = 15f;
    public float lifetime = 1.5f;

    void Start()
    {
        // Remove old bullets so they do not accumulate outside the arena.
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
