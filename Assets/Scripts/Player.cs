using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float rotSensitivity = 1.0f;
    private float xRot = 0.0f;
    InputAction moveAction;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    InputAction fireAction;
    public float firingPeriod = 1.0f;
    private float elapsedTime = 0.0f;
    public Transform projectileSpawner;
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        if (moveAction != null) moveAction.Enable();
        else Debug.LogError("Action Move could not be found");

        fireAction = InputSystem.actions.FindAction("Attack");
        if (fireAction != null) fireAction.Enable();
        else Debug.LogError("Action Attack could not be found");
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
        xRot -= moveAction.ReadValue<Vector2>().x * rotSensitivity;
        transform.localRotation = Quaternion.Euler(0f, 0f, xRot);
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
}
