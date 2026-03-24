using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

// Determining the last used input device was so painful to figure out
// because we can't use PlayerInput because that should be attached to
// the player and also that isn't supposed to be used with the
// project-wide input actions. So there was no really easy way to detect
// what the last used input device was
public class InputManager : MonoBehaviour
{
    static InputManager _instance;
    public static InputManager Instance
    {
        get
        {
            // Lazy instantiation
            if (!_instance)
            {
                GameObject go = new GameObject("InputManager");
                _instance = go.AddComponent<InputManager>();
                DontDestroyOnLoad(_instance.gameObject);
                // Add function to event
                InputState.onChange += _instance.OnInputStateChange;
                InputSystem.onDeviceChange += _instance.OnDeviceChange;
            }
            return _instance;
        }
    }

    // Last used device for input
    InputDevice _lastUsedDevice;
    public InputDevice LastUsedDevice
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
    InputDevice DefaultInputDevice => Gamepad.all.Count > 0 ? (Gamepad.current ?? Gamepad.all[0]) : (InputSystem.devices.Count > 0 ? InputSystem.devices[0] : null);

    void Awake()
    {
        if (_instance && _instance != this) Destroy(gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            // Add function to event
            InputState.onChange += OnInputStateChange;
            InputSystem.onDeviceChange += OnDeviceChange;
        }
    }

    void OnDestroy()
    {
        // Remove function from event
        if (_instance != this) return;
        InputState.onChange -= OnInputStateChange;
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void Start() => _lastUsedDevice ??= DefaultInputDevice;

    // Change the last used device to the most recently used device. Don't do it when
    // the last used device is not set (this happens at the start when all connected
    // devices get added and produce a state change which we don't want). Also don't
    // do it when eventPtr is null, this happens constantly for all connected devices
    // when they aren't used, so we don't want that to affect the last used device
    void OnInputStateChange(InputDevice device, InputEventPtr eventPtr) => _lastUsedDevice = _lastUsedDevice != null && eventPtr != null ? device : _lastUsedDevice;

    void OnDeviceChange(InputDevice device, InputDeviceChange deviceChange)
    {
        // Don't want to do anything if the last device is null. This can happen at
        // the start which in that case we would want to use the default device. Also
        // if we remove a device we need to test if the device id matches which won't
        // work if last device is null
        if (_lastUsedDevice == null) return;
        if (deviceChange == InputDeviceChange.Added || deviceChange == InputDeviceChange.Reconnected)
            _lastUsedDevice = device; // A device was added or reconnected
        else if ((deviceChange == InputDeviceChange.Removed || deviceChange == InputDeviceChange.Disconnected || deviceChange == InputDeviceChange.Disabled) && _lastUsedDevice.deviceId == device.deviceId)
            _lastUsedDevice = DefaultInputDevice; // The last used device was removed/disconnected/disabled
    }
}
