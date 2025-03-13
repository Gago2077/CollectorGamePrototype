using System.Diagnostics.Contracts;
using UnityEngine;

public class ShoppingCartController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _holdPosition;
    [SerializeField] private float _grabDistance = 2f;
    [SerializeField] private LayerMask _cartLayer;

    private PlayerMovement _playerMovement;
    [Header("Physics Settings")]
    [SerializeField] private float _positionSpring = 500f;
    [SerializeField] private float _maxSpeed = 4f;
    [SerializeField] private float _rotationSpeed = 5f;

    private FixedJoint _fixedJoint;
    private Rigidbody _cartRb;
    public bool IsRiding;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (IsRiding) ReleaseCart();
            else TryGrabCart();
        }
    }

    private void TryGrabCart()
    {
        if (Camera.main == null ) return;

        // Raycast from screen center
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (!_playerMovement.isCrouching && Physics.Raycast(ray, out RaycastHit hit, _grabDistance, _cartLayer) &&
            hit.collider.CompareTag("Cart"))
        {
            _cartRb = hit.rigidbody;
            if (_cartRb == null) return;

            // Snap to position/rotation immediately
            _cartRb.transform.position = _holdPosition.position;
            _cartRb.transform.rotation = _holdPosition.rotation;

            AttachFixedJoint();
            IsRiding = true;
        }
    }

    private void AttachFixedJoint()
    {
        if (!TryGetComponent<Rigidbody>(out var playerRb))
        {
            playerRb = gameObject.AddComponent<Rigidbody>();
            playerRb.isKinematic = true;
        }

        _fixedJoint = gameObject.AddComponent<FixedJoint>();
        _fixedJoint.connectedBody = _cartRb;
        _fixedJoint.enableCollision = false;
    }

    private void ReleaseCart()
    {
        IsRiding = false;
        if (_fixedJoint != null) Destroy(_fixedJoint);
        _cartRb = null;
    }

    private void FixedUpdate()
    {
        if (!IsRiding || _cartRb == null) return;

        //// Maintain position
        //Vector3 positionError = holdPosition.position - cartRb.position;
        //cartRb.AddForce(positionError * positionSpring);

        // Maintain rotation
        Quaternion rotationError = _holdPosition.rotation * Quaternion.Inverse(_cartRb.rotation);
        rotationError.ToAngleAxis(out float angle, out Vector3 axis);
        _cartRb.AddTorque(axis * (angle * Mathf.Deg2Rad * _rotationSpeed));

        //// Limit velocity
        //if (cartRb.velocity.magnitude > maxSpeed)
        //{
        //    cartRb.velocity = cartRb.velocity.normalized * maxSpeed;
        //}
    }
}