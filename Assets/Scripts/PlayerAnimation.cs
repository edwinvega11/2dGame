using UnityEngine;

// Switches between the dwarf's idle and walking frames.
[RequireComponent(typeof(PlayerMovement), typeof(SpriteRenderer))]
public class PlayerAnimation : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Sprite[] idleFrames;
    public Sprite[] runFrames;
    [Min(1f)] public float framesPerSecond = 8f;

    PlayerMovement movement;
    SpriteRenderer spriteRenderer;
    float animationTime;
    bool wasMoving;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        bool isMoving = movement.moveDirection.sqrMagnitude > 0f;
        if (isMoving != wasMoving) animationTime = 0f;
        wasMoving = isMoving;

        Sprite[] frames = idleFrames;
        if (isMoving) frames = runFrames;
        if (frames == null || frames.Length == 0) return;

        animationTime += Time.deltaTime;
        // The remainder (%) wraps the frame number back to the start.
        int frame = Mathf.FloorToInt(animationTime * framesPerSecond) % frames.Length;
        spriteRenderer.sprite = frames[frame];
    }
}
