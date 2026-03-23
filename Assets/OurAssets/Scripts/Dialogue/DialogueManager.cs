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

    public DialogueItem CurrentDialogueItem { get; private set; }
    public Sprite CurrentDialogueIcon { get; private set; }

    void Awake()
    {
        if (_instance && _instance != this) Destroy(gameObject);
        else _instance = this;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null) return;
        System.Array.ForEach(dialogue.dialogueItems, (item) => dialogueQueue.Enqueue(item));
        if (!dialogueQueue.IsEmpty) LoadNextItem(); // Load the first item
    }

    public void LoadNextItem()
    {
        if (CurrentDialogueIcon) Resources.UnloadAsset(CurrentDialogueIcon);
        CurrentDialogueItem = dialogueQueue.Dequeue();
        CurrentDialogueIcon = DialogueLoader.Instance.LoadIcon(CurrentDialogueItem);
    }
}
