using UnityEngine;
using UnityEngine.InputSystem;

public class FlyCamera : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float sprintMultiplier = 3f;
    public float mouseSensitivity = 0.12f;

    [Header("Movement keys")]
    public Key forwardKey = Key.W;
    public Key backwardKey = Key.S;
    public Key leftKey = Key.A;
    public Key rightKey = Key.D;
    public Key upKey = Key.Space;
    public Key downKey = Key.LeftCtrl;
    public Key sprintKey = Key.LeftShift;

    private float pitch;

    void Start()
    {
        pitch = transform.eulerAngles.x;
        if (pitch > 180f) pitch -= 360f;
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        Mouse mouse = Mouse.current;
        bool looking = mouse != null && mouse.rightButton.isPressed;
        Cursor.lockState = looking ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !looking;

        if (looking)
        {
            Vector2 delta = mouse.delta.ReadValue();
            transform.Rotate(Vector3.up, delta.x * mouseSensitivity, Space.World);
            pitch = Mathf.Clamp(pitch - delta.y * mouseSensitivity, -89f, 89f);
            transform.rotation = Quaternion.Euler(pitch, transform.eulerAngles.y, 0f);
        }

        if (keyboard == null) return;
        float forward = (keyboard[forwardKey].isPressed ? 1f : 0f) - (keyboard[backwardKey].isPressed ? 1f : 0f);
        float sideways = (keyboard[rightKey].isPressed ? 1f : 0f) - (keyboard[leftKey].isPressed ? 1f : 0f);
        float vertical = (keyboard[upKey].isPressed ? 1f : 0f) - (keyboard[downKey].isPressed ? 1f : 0f);
        Vector3 direction = transform.forward * forward + transform.right * sideways + Vector3.up * vertical;
        float speed = moveSpeed * (keyboard[sprintKey].isPressed ? sprintMultiplier : 1f);
        transform.position += direction.normalized * speed * Time.deltaTime;
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
