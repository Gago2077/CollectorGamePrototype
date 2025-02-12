using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 12.0f;
    public float jumpHeight = 3.0f;
    public float gravity = -9.81f * 2f;
    static public bool canMove { get; set; } = true;
    [Header("Ground Check")]
    public Transform groundCheck; // Create an empty GameObject at the player's feet and assign it here.
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Components")]
    public CharacterController controller;

    private Vector3 velocity;

    private void Start()
    {
        // Get the CharacterController if not manually assigned.
        if (controller == null)
            controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(!canMove)
            return;
        // Check if the player is on the ground
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            // A small downward force keeps the player grounded.
            velocity.y = -2f;
        }

        // Get input for horizontal movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Move relative to the player’s local axes
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity over time
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
