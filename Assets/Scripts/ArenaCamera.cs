using UnityEngine;

// Keeps the entire clearing visible at different window sizes.
[ExecuteAlways, RequireComponent(typeof(Camera))]
public class ArenaCamera : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Vector2 arenaSize = new Vector2(30f, 18f);
    Camera arenaCamera;

    void OnEnable()
    {
        arenaCamera = GetComponent<Camera>();
        FitArena();
    }

    void LateUpdate()
    {
        FitArena();
    }

    void FitArena()
    {
        if (arenaCamera == null) return;
        arenaCamera.orthographic = true;
        float verticalSize = arenaSize.y / 2f;
        float horizontalSize = arenaSize.x / (2f * Mathf.Max(0.1f, arenaCamera.aspect));
        arenaCamera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
    }
}
