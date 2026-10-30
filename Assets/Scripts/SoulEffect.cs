using UnityEngine;

// a small soul floats up and fades after a kill
public class SoulEffect : MonoBehaviour
{
    public SpriteRenderer core;
    public SpriteRenderer glow;
    public float lifetime = 0.65f;

    float age;
    Color coreColor;
    Color glowColor;
    Vector3 glowScale;

    void Awake()
    {
        coreColor = core.color;
        glowColor = glow.color;
        glowScale = glow.transform.localScale;
    }

    void Update()
    {
        // keep the last soul moving while the round pauses
        age += Time.unscaledDeltaTime;
        float progress = Mathf.Clamp01(age / lifetime);
        transform.position += Vector3.up * 0.9f * Time.unscaledDeltaTime;
        core.color = new Color(coreColor.r, coreColor.g, coreColor.b, 1f - progress);
        glow.color = new Color(glowColor.r, glowColor.g, glowColor.b, glowColor.a * (1f - progress));
        glow.transform.localScale = glowScale * (1f + progress * 0.6f);
        if (age >= lifetime) Destroy(gameObject);
    }
}
