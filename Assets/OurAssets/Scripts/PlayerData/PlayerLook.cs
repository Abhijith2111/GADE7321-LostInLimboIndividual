using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField]
    Transform cameraTarget;
    [SerializeField, Range(0.01f, 0.5f)]
    float deadZone = 0.05f;
    [SerializeField]
    [Min(0f)]
    float mouseHorizontalSensitivity = 0.2f;
    [SerializeField]
    [Min(0f)]
    float mouseVerticalSensitivity = 0.225f;
    [SerializeField]
    [Min(0f)]
    float controllerHorizontalSensitivity = 180f;
    [SerializeField]
    [Min(0f)]
    float controllerVerticalSensitivity = 202.5f;
    [SerializeField]
    [Range(-90f, 0f)]
    float minVerticalAngle = -80f;
    [SerializeField]
    [Range(0f, 90f)]
    float maxVerticalAngle = 80f;

    float pitch = 0f;
    float yaw = 0f;

    Vector2 LookInput => InputSystem.actions.FindAction("Look").ReadValue<Vector2>();

    // Last used device for looking
    InputDevice LookDevice => InputSystem.actions.FindAction("Look").activeControl?.device;

    void Awake()
    {
        // This doesn't seem to be doing anything...
        // Did Unity change this?
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mX = LookInput.x * LookInput.x < deadZone * deadZone ? 0f : LookInput.x;
        float mY = LookInput.y * LookInput.y < deadZone * deadZone ? 0f : LookInput.y;
        float hSens = LookDevice is Gamepad ? controllerHorizontalSensitivity * Time.deltaTime : mouseHorizontalSensitivity;
        float vSens = LookDevice is Gamepad ? controllerVerticalSensitivity * Time.deltaTime : mouseVerticalSensitivity;
        yaw += mX * hSens;
        pitch = Mathf.Clamp(pitch - mY * vSens, minVerticalAngle, maxVerticalAngle);
        if (cameraTarget != null) cameraTarget.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
