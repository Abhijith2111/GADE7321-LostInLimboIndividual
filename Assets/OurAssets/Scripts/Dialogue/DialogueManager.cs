using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    static DialogueManager _instance;
    public static DialogueManager Instance
    {
        get
        {
            // Lazy instantiation
            if (!_instance)
            {
                GameObject go = new GameObject("DialogueManager");
                _instance = go.AddComponent<DialogueManager>();
            }
            return _instance;
        }
    }

    readonly QueueADT<DialogueItem> dialogueQueue = new QueueADT<DialogueItem>();

    Dialogue currentDialogue;
    public DialogueItem CurrentDialogueItem { get; private set; }

    void Awake()
    {
        if (_instance && _instance != this) Destroy(gameObject);
        else _instance = this;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null) return;
        Clear();
        currentDialogue = dialogue;
        System.Array.ForEach(currentDialogue.DialogueItems.ToArray(), (item) => dialogueQueue.Enqueue(item));
        if (!dialogueQueue.IsEmpty) LoadNextItem(); // Load the first item
    }

    public void LoadNextItem()
    {
        if (dialogueQueue.IsEmpty)
        {
            Clear();
            return;
        }
        CurrentDialogueItem = dialogueQueue.Dequeue();
    }

    public void Clear()
    {
        if (currentDialogue == null) return;
        currentDialogue.UnloadAllSprites();
		dialogueQueue.Clear();
        currentDialogue = null;
		CurrentDialogueItem = null;
	}
}
