using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputScript : MonoBehaviour
{
    [SerializeField]
    InputActionReference moveAction;
    [SerializeField]
    InputActionReference lookAction;
    [SerializeField]
    InputActionReference jumpAction;
    [SerializeField]
    InputActionReference runAction;
    [SerializeField]
    InputActionReference crouchAction;

    public Vector2 MoveInput => moveAction.action.ReadValue<Vector2>();
    public Vector2 LookInput => lookAction.action.ReadValue<Vector2>();
    public InputDevice LookDevice { get; private set; }
    public void AddJumpAction(System.Action<InputAction.CallbackContext> action) => jumpAction.action.started += action;
    public void RemoveJumpAction(System.Action<InputAction.CallbackContext> action) => jumpAction.action.started -= action;
    public void AddRunActionStart(System.Action<InputAction.CallbackContext> action) => runAction.action.started += action;
    public void RemoveRunActionStart(System.Action<InputAction.CallbackContext> action) => runAction.action.started -= action;
    public void AddRunActionEnd(System.Action<InputAction.CallbackContext> action) => runAction.action.canceled += action;
    public void RemoveRunActionEnd(System.Action<InputAction.CallbackContext> action) => runAction.action.canceled -= action;
    public void AddCrouchActionStart(System.Action<InputAction.CallbackContext> action) => crouchAction.action.started += action;
    public void RemoveCrouchActionStart(System.Action<InputAction.CallbackContext> action) => crouchAction.action.started -= action;
    public void AddCrouchActionEnd(System.Action<InputAction.CallbackContext> action) => crouchAction.action.canceled += action;
    public void RemoveCrouchActionEnd(System.Action<InputAction.CallbackContext> action) => crouchAction.action.canceled -= action;

    void Awake()
    {
        lookAction.action.performed += DetectLookInput;
        lookAction.action.Enable();
    }

    void OnDestroy() => lookAction.action.performed -= DetectLookInput;

    void DetectLookInput(InputAction.CallbackContext ctx) => LookDevice = ctx.control.device;
}
