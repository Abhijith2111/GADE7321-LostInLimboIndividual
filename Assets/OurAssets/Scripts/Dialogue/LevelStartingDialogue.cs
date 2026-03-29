using UnityEngine;

[RequireComponent(typeof(DialogueHolder))]
public class LevelStartingDialogue : MonoBehaviour
{
    // Singleton just to make sure only one starting dialogue. Doesn't need to be public because
    // doesn't need to be accessed by other classes
    static LevelStartingDialogue Instance { get; set; }

    void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start() => GetComponent<DialogueHolder>().StartDialogue();
}
