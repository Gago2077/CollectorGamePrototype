using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 500.0f;
    static public bool canLook { get; set; } = true;
    [Header("Player Reference")]
    // Assign the Player GameObject (the one with the CharacterController)
    public Transform playerBody;

    private float xRotation = 0.0f;

    private void Start()
    {
        // Hide and lock the cursor for a first-person experience.
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Get mouse movement inputs
        if(canLook)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Rotate the player horizontally (around Y-axis)
            playerBody.Rotate(Vector3.up * mouseX);

            // Rotate the camera vertically, clamping so it can't flip
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}
