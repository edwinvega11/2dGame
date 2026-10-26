using UnityEngine;
using UnityEngine.InputSystem;

// Moves the dwarf with WASD or arrow keys.
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Set in Inspector")]
    [Min(0f)] public float moveSpeed = 5f;
    public Vector2 minimumPosition = new Vector2(-12.5f, -6.7f);
    public Vector2 maximumPosition = new Vector2(12.5f, 6f);

    [HideInInspector] public Vector2 moveDirection;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        Vector2 input = Vector2.zero;

        if (keyboard != null && Application.isFocused)
        {
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) input.x -= 1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) input.x += 1f;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) input.y -= 1f;
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) input.y += 1f;
        }

        // Clamping the input prevents diagonal movement from being faster.
        moveDirection = Vector2.ClampMagnitude(input, 1f);

        Vector3 pos = transform.position;
        pos.x += moveDirection.x * moveSpeed * Time.deltaTime;
        pos.y += moveDirection.y * moveSpeed * Time.deltaTime;

        // Keep the player inside the clearing.
        pos.x = Mathf.Clamp(pos.x, minimumPosition.x, maximumPosition.x);
        pos.y = Mathf.Clamp(pos.y, minimumPosition.y, maximumPosition.y);
        transform.position = pos;

        if (moveDirection.x != 0f)
            spriteRenderer.flipX = moveDirection.x < 0f;
    }
}
