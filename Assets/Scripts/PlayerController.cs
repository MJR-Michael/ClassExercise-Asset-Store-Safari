using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveForce = 20f;     // forward/backward speed
    [SerializeField] private float turnSpeed = 120f;    // turning speed (degrees/sec)

    private InputSystem_Actions controls;
    private Vector3 moveInput;
    private bool brakeInput;
    private bool reverseInput;
    private Rigidbody rb;

    private void Awake()
    {
        controls = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();

        // Keep car upright, stable
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    private void OnEnable()
    {
        controls.Vehicle.Enable();

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
        // --- MOVEMENT ---
        float forward = moveInput.z;
        if (reverseInput) forward = -1f;
        if (brakeInput) forward = 0f;

        Vector3 moveDir = transform.forward * forward * moveForce;
        rb.AddForce(moveDir, ForceMode.Acceleration);

        // --- TURNING ---
        float turn = moveInput.x;
        if (Mathf.Abs(turn) > 0.01f)
        {
            float turnAmount = turn * turnSpeed * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }
}
