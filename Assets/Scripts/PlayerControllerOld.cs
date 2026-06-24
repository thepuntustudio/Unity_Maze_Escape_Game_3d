using UnityEngine;

public class PlayerControllerOld: MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 100f;

    private Rigidbody rb;

    private float moveHorizontal;
    private float moveVertical;
    private float rotateHorizontal;
    private bool jumpRequested;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Read input here
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
        rotateHorizontal = Input.GetAxis("Mouse X");

        if (Input.GetButtonDown("Jump"))
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        // Rotation
        float rotation = rotateHorizontal * rotationSpeed * Time.fixedDeltaTime;
        Quaternion newRotation = Quaternion.Euler(0f, rotation, 0f);
        rb.MoveRotation(rb.rotation * newRotation);

        // Build movement direction and normalize it
        Vector3 moveDir = transform.forward * moveVertical + transform.right * moveHorizontal;
        moveDir = Vector3.ClampMagnitude(moveDir, 1f);

        // Preserve Y velocity so jump doesn't get cancelled
        rb.linearVelocity = new Vector3(
            moveDir.x * speed,
            rb.linearVelocity.y,
            moveDir.z * speed
        );

        // Jump
        if (jumpRequested)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequested = false;
        }
    }
}