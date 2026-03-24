using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;

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

    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        int dialogueIndex;
        InputDevice lastUsedDevice = InputManager.Instance.LastUsedDevice;
        if (lastUsedDevice is Keyboard || lastUsedDevice is Mouse) dialogueIndex = keyboardAndMouseDialogueIndex;
        else if (lastUsedDevice is XInputController) dialogueIndex = xInputDialogueIndex;
        else if (lastUsedDevice is DualShockGamepad) dialogueIndex = dualShockDialogueIndex;
        else if (lastUsedDevice is SwitchProControllerHID) dialogueIndex = switchDialogueIndex;
        else dialogueIndex = genericControllerDialogueIndex;
        holder.StartDialogue(dialogueIndex);
    }
}
