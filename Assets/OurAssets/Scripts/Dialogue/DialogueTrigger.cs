using UnityEngine;

[RequireComponent(typeof(Collider), typeof(DialogueHolder))]
public class DialogueTrigger : MonoBehaviour
{
    protected virtual void OnValidate() => GetComponent<Collider>().isTrigger = true; // Ensure collider is a trigger

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) GetComponent<DialogueHolder>().StartDialogue();
    }
}
