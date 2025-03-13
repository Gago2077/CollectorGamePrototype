using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float normalSpeed = 12f;
    public float cartSpeed = 8f;
    public float jumpHeight = 3.0f;
    public float gravity = -9.81f * 2f;
    public static bool canMove = true;
    public bool isCrouching = false;
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("References")]
    public ShoppingCartController cartController;

    [Header("Debug")]
    [SerializeField] private float currentSpeed;

    private Rigidbody rb;
    private bool isGrounded;
    private float horizontalInput;
    private float verticalInput;
    private bool jumpRequested;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        currentSpeed = normalSpeed;
    }

    private void Update()
    {
        if (!canMove) return;

        // Get input in Update
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
        }

        if (Input.GetKeyDown(KeyCode.C) && !isCrouching)
        {
            Crouch();
            isCrouching = true;
        }
        else if(Input.GetKeyDown(KeyCode.C) && isCrouching)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isCrouching = false;
        }

    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        HandleGroundCheck();
        HandleMovement();
        HandleJump();
        ApplyGravity();
    }

    private void Crouch()
    {
        transform.localScale = new Vector3(1, 0.5f, 1);
    }
    private void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Reset vertical velocity when grounded
        if (isGrounded && rb.velocity.y < 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, -2f, rb.velocity.z);
        }
    }

    private void HandleMovement()
    {
        // Smooth speed adjustment
        float targetSpeed = cartController != null && cartController.IsRiding
            ? cartSpeed
            : normalSpeed;

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.fixedDeltaTime * 5f);

        // Calculate movement direction
        Vector3 move = (transform.right * horizontalInput + transform.forward * verticalInput).normalized;
        Vector3 velocity = move * currentSpeed;

        // Preserve vertical velocity while applying horizontal movement
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    private void HandleJump()
    {
        if (jumpRequested && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Sqrt(jumpHeight * -2f * gravity), rb.velocity.z);
            jumpRequested = false;
        }
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            rb.velocity += Vector3.up * gravity * Time.fixedDeltaTime;
        }
    }
}