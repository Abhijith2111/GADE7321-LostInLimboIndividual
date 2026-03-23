using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;

// Determining the last used input device was so painful to figure out
// because we can't use PlayerInput because that should be attached to
// the player and also that isn't supposed to be used with the
// project-wide input actions. So there was no really easy way to detect
// what the last used input device was
public class ControlsTutorialDialogueTrigger : DialogueTrigger
{
    [SerializeField, Min(0)]
    int keyboardAndMouseDialogueIndex = 0;
    [SerializeField, Min(0)]
    int xInputDialogueIndex = 1;
    [SerializeField, Min(0)]
    int dualShockDialogueIndex = 2;
    [SerializeField, Min(0)]
    int switchDialogueIndex = 3;
    [SerializeField, Min(0)]
    int genericControllerDialogueIndex = 4;

    // Last used device for input
    InputDevice _lastUsedDevice;
    InputDevice LastUsedDevice
    {
        get
        {
            // Lazy instantiation
            _lastUsedDevice ??= DefaultInputDevice;
            return _lastUsedDevice;
        }
    }

    // If a gamepad is connected then default to displaying controls for the gamepad
    // otherwise use the first device connected
    InputDevice DefaultInputDevice => Gamepad.all.Count > 0 ? Gamepad.all[0] : InputSystem.devices[0];

    protected override void Awake()
    {
        base.Awake();
        // Add function to event
        InputState.onChange += OnInputStateChange;
    }

    // Set the last used device to the default device if it hasn't been set yet
    void Start() => _lastUsedDevice ??= DefaultInputDevice;

    // Remove function from event
    void OnDestroy() => InputState.onChange -= OnInputStateChange;

    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        int dialogueIndex;
        if (LastUsedDevice is Keyboard || LastUsedDevice is Mouse) dialogueIndex = keyboardAndMouseDialogueIndex;
        else if (LastUsedDevice is XInputController) dialogueIndex = xInputDialogueIndex;
        else if (LastUsedDevice is DualShockGamepad) dialogueIndex = dualShockDialogueIndex;
        else if (LastUsedDevice is SwitchProControllerHID) dialogueIndex = switchDialogueIndex;
        else dialogueIndex = genericControllerDialogueIndex;
        holder.StartDialogue(dialogueIndex);
    }

    // Change the last used device to the most recently used device. Don't do it when
    // the last used device is not set (this happens at the start when all connected
    // devices get added and produce a state change which we don't want). Also don't
    // do it when eventPtr is null, this happens constantly for all connected devices
    // when they aren't used, so we don't want that to affect the last used device
    void OnInputStateChange(InputDevice device, InputEventPtr eventPtr) => _lastUsedDevice = _lastUsedDevice != null && eventPtr != null ? device : _lastUsedDevice;
}
