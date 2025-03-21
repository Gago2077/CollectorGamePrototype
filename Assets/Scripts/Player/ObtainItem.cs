using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ObtainItem : MonoBehaviour
{
    public float holdDistance = 2f;
    public float throwForce = 10f;
    public float rotateSpeed = 100f;
    public float viewDistance = 5f;
    private Rigidbody heldItemRb;
    public LayerMask pickupLayerMask;
    private bool isHoldingItem = false;
    private Quaternion itemRotation;
    public ShoppingCartController cartController;
    private int originalLayer; // Store original layer as int

    private void Update()
    {
        HandlePickupInput();

        // Rotate the held item every frame for smoother rotation
        if (isHoldingItem)
        {
            RotateItem();
        }
    }

    private void FixedUpdate()
    {
        if (isHoldingItem)
        {
            MoveHeldItem();
        }
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * viewDistance, Color.red);
        //DebugLookDirection();
    }

    private void HandlePickupInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isHoldingItem && !cartController.IsRiding)
        {
            TryPickupItem();

            if (heldItemRb != null) // Ensure we have a valid item before changing layers
            {
                originalLayer = heldItemRb.gameObject.layer; // Store original layer as int
                heldItemRb.gameObject.layer = LayerMask.NameToLayer("NoCollision");
            }
        }

        if (Input.GetKeyUp(KeyCode.E) && isHoldingItem)
        {
            DropItem();

            if (heldItemRb != null) // Ensure the item exists before restoring
            {
                heldItemRb.gameObject.layer = originalLayer; // Restore original layer
            }
        }
    }

    private void TryPickupItem()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, viewDistance, pickupLayerMask))
        {
            if (hit.collider.CompareTag("Obtainable Item") || hit.collider.CompareTag("OrderItem"))
            {
                heldItemRb = hit.collider.GetComponent<Rigidbody>();
                if (heldItemRb != null)
                {
                    isHoldingItem = true;
                    heldItemRb.useGravity = false;
                    heldItemRb.isKinematic = true;
                    itemRotation = heldItemRb.rotation;
                    heldItemRb.interpolation = RigidbodyInterpolation.Interpolate;
                }
            }
        }
    }

    private void MoveHeldItem()
    {
        Vector3 targetPosition = Camera.main.transform.position + Camera.main.transform.forward * holdDistance;
        heldItemRb.MovePosition(targetPosition);
    }

    private void DropItem()
    {
        if (heldItemRb == null) return; // Prevent errors

        heldItemRb.useGravity = true;
        heldItemRb.isKinematic = false;
        heldItemRb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);

        // Restore layer before setting `heldItemRb` to null
        heldItemRb.gameObject.layer = originalLayer;

        isHoldingItem = false;
        heldItemRb = null;

        // Reset movement and look flags when dropping the item
        PlayerMovement.canMove = true;
        MouseMovement.canLook = true;
    }

    private void RotateItem()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            // Disable player controls during rotation
            PlayerMovement.canMove = false;
            MouseMovement.canLook = false;

            // Use Time.deltaTime from Update for a smoother frame-dependent rotation
            float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

            // Update the rotation quaternion based on mouse movement
            itemRotation *= Quaternion.Euler(Vector3.up * mouseX);
            itemRotation *= Quaternion.Euler(Vector3.right * -mouseY);

            // Apply the new rotation directly to the held item's transform
            heldItemRb.transform.rotation = itemRotation;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            // Re-enable controls when the mouse button is released
            PlayerMovement.canMove = true;
            MouseMovement.canLook = true;
        }
    }

    private void DebugLookDirection()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, viewDistance))
        {
            Debug.Log("Looking at: " + hit.collider.name);
            Debug.DrawLine(Camera.main.transform.position, hit.point, Color.green);
        }
    }
}
