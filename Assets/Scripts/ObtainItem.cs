using UnityEngine;

public class ObtainItem : MonoBehaviour
{
    public LayerMask itemMask;
    public float holdDistance = 2f;
    public float throwForce = 10f;
    public float rotateSpeed = 100f; // Increased rotation speed for better control

    private Rigidbody heldItemRb;
    private bool isHoldingItem = false;
    private Quaternion itemRotation; // Store current item rotation

    private void Update()
    {
        HandlePickupInput();
    }

    private void FixedUpdate()
    {
        if (isHoldingItem)
        {
            MoveHeldItem();
            RotateItem();
        }
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 5f, Color.red);
    }

    private void HandlePickupInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isHoldingItem)
        {
            TryPickupItem();
        }

        if (Input.GetKeyUp(KeyCode.E) && isHoldingItem)
        {
            DropItem();
        }
    }

    private void TryPickupItem()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5.0f, itemMask))
        {
            heldItemRb = hit.collider.GetComponent<Rigidbody>();
            if (heldItemRb != null)
            {
                isHoldingItem = true;
                heldItemRb.useGravity = false;
                heldItemRb.isKinematic = true;
                itemRotation = heldItemRb.rotation; // Save initial rotation
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
        heldItemRb.useGravity = true;
        heldItemRb.isKinematic = false;
        heldItemRb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
        isHoldingItem = false;
        heldItemRb = null;
    }

    private void RotateItem()
    {
        if (Input.GetKey(KeyCode.J))
        {
            itemRotation *= Quaternion.Euler(Vector3.up * rotateSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.L))
        {
            itemRotation *= Quaternion.Euler(Vector3.down * rotateSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.I))
        {
            itemRotation *= Quaternion.Euler(Vector3.right * rotateSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.K))
        {
            itemRotation *= Quaternion.Euler(Vector3.left * rotateSpeed * Time.deltaTime);
        }
        heldItemRb.MoveRotation(itemRotation);
    }
}
