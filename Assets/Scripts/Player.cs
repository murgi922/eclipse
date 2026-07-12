using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float rotSensitivity = 1.0f;
    private float xRot = 0.0f;
    InputAction moveAction;
    [Header("Projectile")]
    public float projectileSpeed = 10.0f;
    public GameObject projectilePrefab;
    InputAction fireAction;
    private float elapsedTime = 0.0f;
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        if (moveAction != null) moveAction.Enable();
        else Debug.LogError("Action Move could not be found");

        fireAction = InputSystem.actions.FindAction("Attack");
        if (fireAction != null) fireAction.Enable();
        else Debug.LogError("Action Attack could not be found");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RotatePlayer();
    }
    void RotatePlayer()
    {
        xRot -= moveAction.ReadValue<Vector2>().x * rotSensitivity;
        transform.localRotation = Quaternion.Euler(0f, 0f, xRot);
    }
    void SpawnProjectile()
    {
        if (fireAction.IsInProgress())
        {

        }
    }
}
