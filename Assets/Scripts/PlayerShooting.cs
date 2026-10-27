using UnityEngine;
using UnityEngine.InputSystem;

// Aims the gun at the mouse and fires at a steady rate.
public class PlayerShooting : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Bullet bulletPrefab;
    public Transform muzzle;
    public SpriteRenderer gunRenderer;
    public SpriteRenderer playerRenderer;
    [Min(0.05f)] public float secondsBetweenShots = 0.3f;

    Camera gameCamera;
    Vector3 muzzleOffset;
    float nextShotTime;

    void Start()
    {
        gameCamera = Camera.main;
        muzzleOffset = muzzle.localPosition;
    }

    void LateUpdate()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null || gameCamera == null || !Application.isFocused) return;

        Vector2 mousePosition = mouse.position.ReadValue();
        // Stop firing while the pointer is outside the Game view.
        if (!gameCamera.pixelRect.Contains(mousePosition)) return;

        Vector3 screenPosition = new Vector3(mousePosition.x, mousePosition.y,
            transform.position.z - gameCamera.transform.position.z);
        Vector3 target = gameCamera.ScreenToWorldPoint(screenPosition);
        Vector2 direction = target - transform.position;
        if (direction.sqrMagnitude < 0.01f) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Flip the gun vertically on the left so it never looks upside down.
        bool aimingLeft = direction.x < 0f;
        gunRenderer.flipY = aimingLeft;
        playerRenderer.flipX = aimingLeft;
        gunRenderer.sortingOrder = direction.y > 0f ? 9 : 11;
        muzzle.localPosition = new Vector3(muzzleOffset.x,
            aimingLeft ? -muzzleOffset.y : muzzleOffset.y, 0f);

        if (Time.time >= nextShotTime)
        {
            Instantiate(bulletPrefab, muzzle.position, transform.rotation);
            nextShotTime = Time.time + secondsBetweenShots;
        }
    }
}
