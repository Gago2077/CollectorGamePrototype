using System.Diagnostics.Contracts;
using UnityEngine;

public class ShoppingCartController : MonoBehaviour
{
    [Header("References")]
    public Transform holdPosition;
    public float grabDistance = 2f;
    public LayerMask cartLayer;

    private PlayerMovement playerMovement;
    [Header("Physics Settings")]
    public float positionSpring = 500f;
    public float maxSpeed = 4f;
    public float rotationSpeed = 5f;

    private FixedJoint fixedJoint;
    private Rigidbody cartRb;
    public bool isRiding;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isRiding) ReleaseCart();
            else TryGrabCart();
        }
    }

    private void TryGrabCart()
    {
        if (Camera.main == null ) return;

        // Raycast from screen center
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (!playerMovement.isCrouching && Physics.Raycast(ray, out RaycastHit hit, grabDistance, cartLayer) &&
            hit.collider.CompareTag("Cart"))
        {
            cartRb = hit.rigidbody;
            if (cartRb == null) return;

            // Snap to position/rotation immediately
            cartRb.transform.position = holdPosition.position;
            cartRb.transform.rotation = holdPosition.rotation;

            AttachFixedJoint();
            isRiding = true;
        }
    }

    private void AttachFixedJoint()
    {
        if (!TryGetComponent<Rigidbody>(out var playerRb))
        {
            playerRb = gameObject.AddComponent<Rigidbody>();
            playerRb.isKinematic = true;
        }

        fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = cartRb;
        fixedJoint.enableCollision = false;
    }

    private void ReleaseCart()
    {
        isRiding = false;
        if (fixedJoint != null) Destroy(fixedJoint);
        cartRb = null;
    }

    private void FixedUpdate()
    {
        if (!isRiding || cartRb == null) return;

        //// Maintain position
        //Vector3 positionError = holdPosition.position - cartRb.position;
        //cartRb.AddForce(positionError * positionSpring);

        // Maintain rotation
        Quaternion rotationError = holdPosition.rotation * Quaternion.Inverse(cartRb.rotation);
        rotationError.ToAngleAxis(out float angle, out Vector3 axis);
        cartRb.AddTorque(axis * (angle * Mathf.Deg2Rad * rotationSpeed));

        //// Limit velocity
        //if (cartRb.velocity.magnitude > maxSpeed)
        //{
        //    cartRb.velocity = cartRb.velocity.normalized * maxSpeed;
        //}
    }
}