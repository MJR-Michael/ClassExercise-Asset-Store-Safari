using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveForce = 1500f;
    [SerializeField] private float turnTorque = 300f;
    [SerializeField] private float maxSlopeAngle = 45f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.5f;
    [SerializeField] private LayerMask groundMask;

    private InputSystem_Actions controls;
    private Vector3 moveInput;
    private bool brakeInput;
    private bool reverseInput;
    private Rigidbody rb;
    private bool isGrounded;

    private void Awake()
    {
        controls = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0); // stability
    }

    private void OnEnable()
    {
        controls.Vehicle.Enable();

        // Read as Vector3 because your action uses a Vector3Composite
        controls.Vehicle.Move.performed += ctx => moveInput = ctx.ReadValue<Vector3>();
        controls.Vehicle.Move.canceled += ctx => moveInput = Vector3.zero;

        controls.Vehicle.Brake.performed += ctx => brakeInput = true;
        controls.Vehicle.Brake.canceled += ctx => brakeInput = false;

        controls.Vehicle.Reverse.performed += ctx => reverseInput = true;
        controls.Vehicle.Reverse.canceled += ctx => reverseInput = false;
    }

    private void OnDisable()
    {
        controls.Vehicle.Disable();
    }

    private void FixedUpdate()
    {
        CheckGround();

        if (isGrounded)
        {
            // Forward/backward input
            float forward = moveInput.z;
            if (reverseInput) forward = -1f;
            if (brakeInput) forward = 0f;

            // Apply forward/backward force
            Vector3 force = transform.forward * forward * moveForce;
            rb.AddForce(force, ForceMode.Acceleration);

            // Steering
            float turn = moveInput.x * turnTorque;
            rb.AddTorque(Vector3.up * turn, ForceMode.Acceleration);
        }
        else
        {
            // Extra downward force when airborne
            rb.AddForce(Vector3.down * 50f, ForceMode.Acceleration);
        }
    }

    private void CheckGround()
    {
        isGrounded = false;

        if (groundCheck == null) return;

        Vector3 origin = groundCheck.position + Vector3.up * 0.05f; // start slightly above to avoid starting inside
        float checkRadius = 0.15f; // tweak: radius near wheel/underside
        float checkDistance = groundCheckDistance + 0.05f;

        // visualize
        Debug.DrawRay(origin, Vector3.down * checkDistance, Color.cyan);
        Debug.DrawLine(origin + Vector3.left * checkRadius, origin + Vector3.right * checkRadius); // rough visual aid

        // SphereCast to be robust against small penetrations
        if (Physics.SphereCast(origin, checkRadius, Vector3.down, out RaycastHit hit, checkDistance, groundMask, QueryTriggerInteraction.Ignore))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle <= maxSlopeAngle)
            {
                isGrounded = true;
                // Optional: cache hit.point or hit.normal for e.g. aligning vehicle to ground
            }
            else
            {
                // hit but too steep
                // Debug.Log($"Too steep: {slopeAngle} > {maxSlopeAngle}");
            }
        }
        else
        {
            // fallback: if spherecast misses, optionally try OverlapSphere (works if small penetration)
            Collider[] cols = Physics.OverlapSphere(origin + Vector3.down * checkDistance * 0.5f, checkRadius, groundMask, QueryTriggerInteraction.Ignore);
            if (cols.Length > 0)
            {
                // We found colliders within the sphere -> grounded
                isGrounded = true;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }
    }
}
