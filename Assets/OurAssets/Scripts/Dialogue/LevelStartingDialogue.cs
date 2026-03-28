using UnityEngine;

[RequireComponent(typeof(DialogueHolder))]
public class LevelStartingDialogue : MonoBehaviour
{
    // Singleton just to make sure only one starting dialogue. Doesn't need to be public because
    // doesn't need to be accessed by other classes
    static LevelStartingDialogue Instance { get; set; }

    DialogueHolder holder;

    void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            holder = GetComponent<DialogueHolder>();
        }
    }

    void Start() => holder.StartDialogue();
}
