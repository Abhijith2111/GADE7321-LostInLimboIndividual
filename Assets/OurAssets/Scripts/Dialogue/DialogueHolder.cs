using UnityEngine;

public class DialogueHolder : MonoBehaviour
{
    [SerializeField]
    TextAsset[] dialogueFiles;
    [SerializeField]
    bool canPlayMultipleTimes = false;

    bool hasBeenPlayed = false;

    public void StartDialogue(int dialogueNumber = 0)
    {
        if (hasBeenPlayed && !canPlayMultipleTimes || dialogueNumber > dialogueFiles.Length) return;
        hasBeenPlayed = true;
        Dialogue dialogue = JsonUtility.FromJson<Dialogue>(dialogueFiles[dialogueNumber].text);
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
