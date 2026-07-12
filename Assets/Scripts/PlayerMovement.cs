using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float rotSensitivity = 1.0f;
    private float xRot = 0.0f;
    InputAction moveAction;
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        if (moveAction != null) moveAction.Enable();
        else Debug.LogError("Action Move could not be found");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rotate();
    }
    void Rotate()
    {
        xRot -= moveAction.ReadValue<Vector2>().x * rotSensitivity;
        transform.localRotation = Quaternion.Euler(0f, 0f, xRot);
    }
}
