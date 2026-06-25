using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed    = 5f;
    [SerializeField] private float sprintSpeed  = 9f;
    [SerializeField] private float crouchSpeed  = 2.5f;
    [SerializeField] private float jumpForce    = 5f;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private float touchSensitivity = 0.08f;
    [SerializeField] private float verticalClamp    = 80f;

    [Header("Crouch")]
    [SerializeField] private float standHeight  = 2f;
    [SerializeField] private float crouchHeight = 1f;
      [SerializeField] private float standCamHeight  = 1.8f;  // camera Y when standing
    [SerializeField] private float crouchCamHeight = 0.8f;  // camera Y when crouching

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float     groundRadius = 0.2f;
    [SerializeField] private LayerMask groundMask;

    private PlayerInputActions inputActions;
  

    // ── Components ───────────────────────────────────────────────────
    private Rigidbody        rb;
    private CapsuleCollider  col;
    private Transform        camTransform;

    // ── Input values ─────────────────────────────────────────────────
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool    jumpRequested;
    private bool    isSprinting;
    private bool    isCrouching;
    private bool    attackRequested;
    private bool    interactRequested;
    private int     cycleDirection;   // –1 = Previous, +1 = Next

    // ── State ────────────────────────────────────────────────────────
    private bool  isGrounded;
    private bool  isMobile;
    private float cameraPitch;

    // ════════════════════════════════════════════════════════════════
    private void Awake()
    {
        rb  = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        rb.freezeRotation = true;
        inputActions = new PlayerInputActions();

        camTransform = GetComponentInChildren<Camera>().transform;

        isMobile = Application.isMobilePlatform;

        if (!isMobile)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;
        }
    }

    // ── Input System callbacks ───────────────────────────────────────
    // Method names MUST match your Action names exactly.

    public void OnMove(InputValue v)     => moveInput  = v.Get<Vector2>();
    public void OnLook(InputValue v)     => lookInput  = v.Get<Vector2>();
    public void OnSprint(InputValue v)
    {
        isSprinting = v.isPressed;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Sprint.started  += _ => isSprinting = true;
        inputActions.Player.Sprint.canceled += _ => isSprinting = false;  // guaranteed on release
    }

    private void OnDisable()
    {
        inputActions.Player.Sprint.started  -= _ => isSprinting = true;
        inputActions.Player.Sprint.canceled -= _ => isSprinting = false;
        inputActions.Player.Disable();
    }

    public void OnJump(InputValue v)
    {
        if (v.isPressed && isGrounded) jumpRequested = true;
    }

    public void OnCrouch(InputValue v)
{
    isCrouching = v.isPressed; // this part is fine

    if (col != null)
    {
        if (isCrouching)
        {
            col.height = crouchHeight;
            // Move collider center down so feet stay on ground
            col.center = new Vector3(0f, crouchHeight / 2f, 0f);
            // Also lower the camera
            camTransform.localPosition = new Vector3(0f, crouchHeight - 0.2f, 0f);
        }
        else
        {
            col.height = standHeight;
            // Restore collider center
            col.center = new Vector3(0f, standHeight / 2f, 0f);
            // Restore camera height
            camTransform.localPosition = new Vector3(0f, standHeight - 0.2f, 0f);
        }
    }
}

    public void OnAttack(InputValue v)
    {
        if (v.isPressed) attackRequested = true;
    }
    

    public void OnInteract(InputValue v)
    {
        // This is where you would handle the interaction logic, such as raycasting to detect interactable objects.
        if (v.isPressed) interactRequested = true;
        Ray ray = new Ray(camTransform.position, camTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 2.5f))
        {
            // Try calling OnInteract on whatever we're looking at
            hit.collider.SendMessageUpwards("OnInteract", SendMessageOptions.DontRequireReceiver);
        }

    }

    // Previous / Next — useful for weapon or item cycling
    public void OnPrevious(InputValue v) { if (v.isPressed) cycleDirection = -1; }
    public void OnNext(InputValue v)     { if (v.isPressed) cycleDirection =  1; }

    // ── Update: look, cursor, gameplay actions ───────────────────────
    private void Update()
    {
        HandleLook();
        HandleCursorUnlock();
        HandleAttack();
        HandleInteract();
        HandleCycle();
    }

    // ── FixedUpdate: physics ─────────────────────────────────────────
    private void FixedUpdate()
    {
        CheckGround();
        HandleMovement();
        HandleJump();
    }

    // ════════════════════════════════════════════════════════════════
    private void HandleLook()
    {
        float sens = isMobile ? touchSensitivity : mouseSensitivity;

        // Yaw — rotate player body left/right
        transform.Rotate(Vector3.up, lookInput.x * sens);

        // Pitch — tilt camera up/down, clamped
        cameraPitch -= lookInput.y * sens;
        cameraPitch  = Mathf.Clamp(cameraPitch, -verticalClamp, verticalClamp);
        camTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    private void HandleMovement()
    {
        float speed = isCrouching ? crouchSpeed
                    : isSprinting ? sprintSpeed
                    : walkSpeed;

        Vector3 dir = transform.forward * moveInput.y
                    + transform.right   * moveInput.x;
        dir = Vector3.ClampMagnitude(dir, 1f);

        rb.linearVelocity = new Vector3(
            dir.x * speed,
            rb.linearVelocity.y,   // preserve gravity / jump
            dir.z * speed
        );
    }

    private void HandleJump()
    {
        if (!jumpRequested) return;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpRequested = false;
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position, groundRadius, groundMask);
    }

    private void HandleAttack()
    {
        if (!attackRequested) return;
        Debug.Log("Attack! — connect your weapon/animation logic here.");
        attackRequested = false;
    }

    private void HandleInteract()
    {
        if (!interactRequested) return;
        Debug.Log("Interact! — raycast for interactable objects here.");
        interactRequested = false;
    }

    private void HandleCycle()
    {
        if (cycleDirection == 0) return;
        Debug.Log($"Cycle {(cycleDirection > 0 ? "Next" : "Previous")} — swap weapon/item here.");
        cycleDirection = 0;
    }

    private void HandleCursorUnlock()
    {
        if (!isMobile && Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible   = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}