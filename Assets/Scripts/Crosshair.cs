using UnityEngine;
using UnityEngine.InputSystem;

// follows the mouse inside the game view
public class Crosshair : MonoBehaviour
{
    public RectTransform indicator;
    public PlayerHealth playerHealth;

    void LateUpdate()
    {
        Mouse mouse = Mouse.current;
        bool visible = mouse != null && Application.isFocused && !playerHealth.IsDead && Time.timeScale > 0f;
        Vector2 position = mouse != null ? mouse.position.ReadValue() : Vector2.zero;
        visible = visible && Camera.main.pixelRect.Contains(position);
        indicator.gameObject.SetActive(visible);
        Cursor.visible = !visible;
        if (!visible) return;

        RectTransform canvas = (RectTransform)indicator.parent;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, position, null, out Vector2 localPosition);
        indicator.anchoredPosition = localPosition;
    }

    void OnApplicationFocus(bool focused)
    {
        if (!focused) Cursor.visible = true;
    }

    void OnDisable()
    {
        Cursor.visible = true;
        if (indicator != null) indicator.gameObject.SetActive(false);
    }
}
