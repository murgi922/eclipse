using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float rotSensitivity = 5f;
    [SerializeField] private float rotSnappiness = 10f;
    private float xRot = 0.0f;
    InputAction moveAction;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    InputAction fireAction;
    public float firingPeriod = 1.0f;
    private float elapsedTime = 0.0f;
    public Transform projectileSpawner;

    
    private HealthSystem healthSystem;
    private PolygonCollider2D collider2D;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        if (moveAction != null) moveAction.Enable();
        else Debug.LogError("Action Move could not be found");

        fireAction = InputSystem.actions.FindAction("Attack");
        if (fireAction != null) fireAction.Enable();
        else Debug.LogError("Action Attack could not be found");

        healthSystem = new HealthSystem(100);
        collider2D = this.GetComponent<PolygonCollider2D>();
    }
    void FixedUpdate()
    {
        RotatePlayer();
    }
    private void Update()
    {
        SpawnProjectile();
        if (!healthSystem.IsAlive()) Destroy(GameObject.FindWithTag("PlayerChild"));
    }
    void RotatePlayer()
    {
        xRot -= moveAction.ReadValue<Vector2>().x * rotSensitivity;
        Quaternion targetRot = Quaternion.Euler(0f, 0f, xRot);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.fixedDeltaTime * rotSnappiness);
    }
    void SpawnProjectile()
    {
        if (elapsedTime <= firingPeriod) elapsedTime += Time.deltaTime;
        if (fireAction.IsInProgress())
        {
            if (elapsedTime > firingPeriod)
            {
                Instantiate(projectilePrefab, projectileSpawner.position, Quaternion.identity);
                elapsedTime = 0.0f;
            }
        }
    }
    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
    public void TakeDamage(int damage)
    {
        healthSystem.TakeDamage(damage);
    }
}
