using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement & Rotation Tweaks")]
    public float rotSensitivity = 100.0f;
    private float xRot = 0.0f;
    private InputAction moveAction;

    [Header("Projectile Tweaks")]
    public GameObject projectilePrefab;
    private InputAction fireAction;
    public float firingPeriod = 0.2f;
    private float elapsedTime = 0.0f;
    public Transform projectileSpawner;

    private HealthSystem healthSystem;
    public HeartDisplay heartDisplay;

    void Start()
    {
        // Reset time scale to normal speed when the game starts/restarts
        Time.timeScale = 1.0f;

        healthSystem = new HealthSystem(3);

        if (heartDisplay != null)
        {
            heartDisplay.UpdateHearts(healthSystem.GetHealth());
        }

        moveAction = InputSystem.actions.FindAction("Move");
        if (moveAction != null) moveAction.Enable();

        fireAction = InputSystem.actions.FindAction("Attack");
        if (fireAction != null) fireAction.Enable();
    }

    void FixedUpdate()
    {
        RotatePlayer();
    }

    private void Update()
    {
        SpawnProjectile();
    }

    void RotatePlayer()
    {
        if (moveAction != null)
        {
            xRot -= moveAction.ReadValue<Vector2>().x * rotSensitivity * Time.fixedDeltaTime;
            transform.localRotation = Quaternion.Euler(0f, 0f, xRot);
        }
    }

    void SpawnProjectile()
    {
        elapsedTime += Time.deltaTime;

        if (fireAction != null && fireAction.IsInProgress())
        {
            if (elapsedTime >= firingPeriod)
            {
                Transform spawnPoint = projectileSpawner != null ? projectileSpawner : transform;
                Instantiate(projectilePrefab, spawnPoint.position, transform.rotation);
                elapsedTime = 0.0f;
            }
        }
    }

    public HealthSystem GetHealthSystem() => healthSystem;

    public void TakeDamage(int damage)
    {
        if (healthSystem == null) return;

        healthSystem.TakeDamage(damage);

        if (heartDisplay != null)
        {
            heartDisplay.UpdateHearts(healthSystem.GetHealth());
        }

        if (!healthSystem.IsAlive())
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");

        // PAUSE THE GAME TIME
        Time.timeScale = 0.0f;

        // Hide the player object
        gameObject.SetActive(false);
    }
}