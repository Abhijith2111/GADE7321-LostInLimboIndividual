using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInputScript))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpHeight = 1f; // David - Changed to set jump height instead of velocity
    public float gravity = 10f;
    public float defaultHeight = 1.6f;
    public float crouchHeight = 0.8f;
    public float crouchSpeed = 3f;
    [SerializeField, Min(0f)]
    float groundDistance = 0.1f; // Added by David
    [SerializeField]
    LayerMask groundMask; // Added by David
    [SerializeField]
    Transform cameraTarget; // Added by David
    [SerializeField, Range(0f, 0.1f)]
    float deadZone = 0.05f; // Added by David
    [SerializeField, Min(0f)]
    float turnSpeed = 6f; // Added by David
    [SerializeField, Min(0f)]
    float runTurnSpeed = 12f; // Added by David

    PlayerInputScript pIS; // Added by David
    bool isGrounded; // Added by David

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;

    private bool canMove = true; // David - What is this for Abhi?
    bool isRunning = false; // David - Made this a member instead of local variable to work with new input system
    bool isCrouching = false; // Added by David

    void Awake()
    {
        // David - Moved get component to awake because it's better to do here
        // compared to start
        characterController = GetComponent<CharacterController>();
        pIS = GetComponent<PlayerInputScript>();
        pIS.AddJumpAction(Jump);
        pIS.AddRunActionStart(RunStart);
        pIS.AddRunActionEnd(RunEnd);
        pIS.AddCrouchActionStart(CrouchStart);
        pIS.AddCrouchActionEnd(CrouchEnd);
    }

    void OnDestroy()
    {
        pIS.RemoveJumpAction(Jump);
        pIS.RemoveRunActionStart(RunStart);
        pIS.RemoveRunActionEnd(RunEnd);
        pIS.RemoveCrouchActionStart(CrouchStart);
        pIS.RemoveCrouchActionEnd(CrouchEnd);
    }

    void Update()
    {
        // David - character controller is grounded is very buggy I've found it
        // only updates every other frame, so I am using sphere cast as it is more
        // reliable. Also it's been a while since I first wrote this, but I think
        // I do sphere cast instead of check sphere because ig check sphere would
        // have a chance to leave a gap between it and the player especially if the
        // ground distance is large and sphere cast fixes that, idk if this actually
        // matters too much or not, but it's such an edge case that testing is hard
        isGrounded = Physics.SphereCast(
            origin: transform.position + Vector3.up * characterController.radius,
            radius: characterController.radius,
            direction: Vector3.down,
            hitInfo: out RaycastHit hit,
            maxDistance: groundDistance,
            layerMask: groundMask);

        // David - I think I added this to my previous character controllers to reset
        // velocity being too negative from gravity but also to make sure the character
        // controller gets pushed into the ground
        if (isGrounded && moveDirection.y < 0f) moveDirection.y = -1f;

        // David - Use the camera target forward and right instead of player for movement
        Vector3 forward = new Vector3(cameraTarget.forward.x, 0f, cameraTarget.forward.z).normalized;
        Vector3 right = new Vector3(cameraTarget.right.x, 0f, cameraTarget.right.z).normalized;

        // David - Only move horizontally if move input magnitude is above dead zone
        float xIn = pIS.MoveInput.x;
        float zIn = pIS.MoveInput.y;
        Vector3 movementDirectionXZ = Vector3.ClampMagnitude(forward * zIn + right * xIn, 1f);
        // David - merged curSpeedX and curSpeedZ into curSpeedXZ
        Vector3 curSpeedXZ = Vector3.zero;
        if (movementDirectionXZ.sqrMagnitude >= deadZone * deadZone)
        {
            // David - Rotate character to movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movementDirectionXZ);
            float currentSpeedMultiplier = isCrouching ? crouchSpeed : (isRunning ? runSpeed : walkSpeed); // David - This is so we don't hard code the values when we stop crouching
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Mathf.Clamp01((isRunning ? runTurnSpeed : turnSpeed) * Time.deltaTime));
            curSpeedXZ = canMove ? movementDirectionXZ * currentSpeedMultiplier : Vector3.zero;
        }

        // David - Got rid of movementDirectionY because we can just use
        // (Vector3.up * moveDirection.y) to do the exact same thing
        moveDirection = curSpeedXZ + (Vector3.up * moveDirection.y);

        if (!isGrounded)
        {
            // David - Changed to moveDirection.y because movementDirectionY was local so
            // changing it here did nothing since it was only applied to moveDirection.y
            // before here
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // David - Shorter version of what Abhi wrote that also adjusts the centre since
        // player position is now at y = 0 and the speed is already calculated
        characterController.height = isCrouching && canMove ? crouchHeight : defaultHeight;
        characterController.center = new Vector3(characterController.center.x, characterController.height / 2f, characterController.center.z);

        characterController.Move(moveDirection * Time.deltaTime);
    }

    void Jump(InputAction.CallbackContext ctx)
    {
        // David - Jump to specific height
        if (canMove && isGrounded) moveDirection.y = Mathf.Sqrt(2f * gravity * jumpHeight);
    }

    void RunStart(InputAction.CallbackContext ctx) => isRunning = true;

    void RunEnd(InputAction.CallbackContext ctx) => isRunning = false;

    void CrouchStart(InputAction.CallbackContext ctx) => isCrouching = true;

    void CrouchEnd(InputAction.CallbackContext ctx) => isCrouching = false;

    // got rid of floating issue.
}
