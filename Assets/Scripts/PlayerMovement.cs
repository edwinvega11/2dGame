using UnityEngine;
using UnityEngine.InputSystem;

// move with wasd or arrow keys
public class PlayerMovement : MonoBehaviour
{
    [Header("Set in Inspector")]
    [Min(0f)] public float moveSpeed = 5f;
    public Vector2 minimumPosition = new Vector2(-12.5f, -6.7f);
    public Vector2 maximumPosition = new Vector2(12.5f, 6f);

    [HideInInspector] public Vector2 moveDirection;
    void Update()
    {
        if (Time.timeScale == 0f) { moveDirection = Vector2.zero; return; }
        Keyboard keyboard = Keyboard.current;
        Vector2 input = Vector2.zero;

        if (keyboard != null && Application.isFocused)
        {
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) input.x -= 1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) input.x += 1f;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) input.y -= 1f;
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) input.y += 1f;
        }

        // keep diagonal movement at the same speed
        moveDirection = Vector2.ClampMagnitude(input, 1f);

        Vector3 pos = transform.position;
        pos.x += moveDirection.x * moveSpeed * Time.deltaTime;
        pos.y += moveDirection.y * moveSpeed * Time.deltaTime;

        // stay inside the clearing
        pos.x = Mathf.Clamp(pos.x, minimumPosition.x, maximumPosition.x);
        pos.y = Mathf.Clamp(pos.y, minimumPosition.y, maximumPosition.y);
        transform.position = pos;
    }
}
