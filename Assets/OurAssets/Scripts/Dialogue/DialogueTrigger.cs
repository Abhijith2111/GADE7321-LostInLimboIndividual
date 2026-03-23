using UnityEngine;

[RequireComponent(typeof(Collider), typeof(DialogueHolder))]
public class DialogueTrigger : MonoBehaviour
{
    protected Collider _collider;
    protected DialogueHolder holder;

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider>();
        if (!_collider.isTrigger) _collider.isTrigger = true; // Make sure collider is a trigger
        holder = GetComponent<DialogueHolder>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) holder.StartDialogue();
    }
}
