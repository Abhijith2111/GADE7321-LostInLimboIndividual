using UnityEngine;

public class DialogueHolder : MonoBehaviour
{
    [SerializeField]
    TextAsset[] dialogueFiles;
    [SerializeField]
    bool canPlayMultipleTimes = false;
    [SerializeField]
    DialogueDisplayer displayer;

    bool hasBeenPlayed = false;

    public void StartDialogue(int dialogueNumber = 0, System.Action callbackFunction = null)
    {
        if (hasBeenPlayed && !canPlayMultipleTimes || dialogueNumber >= dialogueFiles.Length || dialogueFiles.Length == 0) return;
        hasBeenPlayed = true;
        SerialisedDialogue dialogue = JsonUtility.FromJson<SerialisedDialogue>(dialogueFiles[dialogueNumber].text);
        DialogueManager.Instance.StartDialogue(dialogue.Deserialised);
        displayer.StartDisplayingDialogue(callbackFunction);
    }

    public void StartDialogue(System.Action callbackFunction) => StartDialogue(0, callbackFunction);
}
