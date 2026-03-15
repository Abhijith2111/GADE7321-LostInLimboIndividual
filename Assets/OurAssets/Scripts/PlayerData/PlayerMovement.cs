using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // movement setup
    private Rigidbody rb;
    public float MoveSpeed = 5f;
    private float moveHorizontal;
    private float moveForward;

    //jumping setup
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float ascendMultiplier = 2f;
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    private float groundCheckDelay = 0.3f;
    private float playerHeight;
    private float raycastDist;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Raycast (Scanner for game logic) set underneath players feet.
        playerHeight = GetComponent<CapsuleCollider>().height * transform.localScale.y;
        raycastDist = (playerHeight / 2) * 0.2f;

        //akes the mouse dissappear and useless.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveForward = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if(!isGrounded && groundCheckTimer <= 0)
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
            isGrounded = Physics.Raycast(rayOrigin, Vector3.down, raycastDist, groundLayer);
        }
        else
        {
            groundCheckTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
        ApplyJumpPhysics();
    }

    void MovePlayer()
    {
        Vector3 movement = (transform.right * moveHorizontal + transform.forward * moveForward).normalized;
        Vector3 targetVelocity = movement * MoveSpeed;

        Vector3 velocity = rb.angularVelocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.angularVelocity = velocity;

        if(isGrounded && moveHorizontal ==0 && moveForward == 0)
        {
            rb.angularVelocity = new Vector3(0, rb.angularVelocity.y, 0);
        }
    }

    void Jump()
    {
        isGrounded = false;
        groundCheckTimer = groundCheckDelay;
        rb.angularVelocity = new Vector3(rb.angularVelocity.x, jumpForce, rb.angularVelocity.z);

    }
    void ApplyJumpPhysics()
    {
        if(rb.angularVelocity.y < 0)
        {
            rb.angularVelocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        }
        else if(rb.angularVelocity.y > 0)
        {
            rb.angularVelocity += Vector3.up * Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime;
        }
    }
}
