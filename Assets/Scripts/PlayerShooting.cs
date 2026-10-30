using UnityEngine;
using UnityEngine.InputSystem;

// aim at the mouse and fire automatically
public class PlayerShooting : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Bullet bulletPrefab;
    public Transform muzzle;
    public SpriteRenderer gunRenderer;
    public SpriteRenderer playerRenderer;
    [Min(0.05f)] public float secondsBetweenShots = 0.3f;
    public int damage = 1;

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
        if (Time.timeScale == 0f) return;
        Mouse mouse = Mouse.current;
        if (mouse == null || gameCamera == null || !Application.isFocused) return;

        Vector2 mousePosition = mouse.position.ReadValue();
        // only shoot inside the game view
        if (!gameCamera.pixelRect.Contains(mousePosition)) return;

        Vector3 screenPosition = new Vector3(mousePosition.x, mousePosition.y,
            transform.position.z - gameCamera.transform.position.z);
        Vector3 target = gameCamera.ScreenToWorldPoint(screenPosition);
        Vector2 direction = target - transform.position;
        if (direction.sqrMagnitude < 0.01f) return;

        //convert direction to angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // keep the gun upright when aiming left
        bool aimingLeft = direction.x < 0f;
        gunRenderer.flipY = aimingLeft;
        playerRenderer.flipX = aimingLeft;
        gunRenderer.sortingOrder = direction.y > 0f ? 9 : 11;
        muzzle.localPosition = new Vector3(muzzleOffset.x,
            aimingLeft ? -muzzleOffset.y : muzzleOffset.y, 0f);

        if (Time.time >= nextShotTime)
        {
            // send the bullet from the barrel toward the crosshair
            Vector2 shotDirection = target - muzzle.position;
            float shotAngle = Mathf.Atan2(shotDirection.y, shotDirection.x) * Mathf.Rad2Deg;
            Bullet bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.Euler(0f, 0f, shotAngle));
            bullet.damage = damage;
            GameAudio.Instance?.Shoot();
            nextShotTime = Time.time + secondsBetweenShots;
        }
    }
}
