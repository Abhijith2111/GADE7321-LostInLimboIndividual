using System.IO;
using UnityEngine;

public class DialogueIconLoader : MonoBehaviour
{
    static DialogueIconLoader _instance;
    public static DialogueIconLoader Instance
    {
        get
        {
            // Lazy instantiation
            if (!_instance)
            {
                GameObject go = new GameObject("DialogueIconLoader");
                _instance = go.AddComponent<DialogueIconLoader>();
            }
            return _instance;
        }
    }

    [SerializeField]
    string dialogueFolder = "Dialogue";
    [SerializeField]
    string iconFolder = "DialogueIcons";
    [SerializeField]
    string[] iconExtensions = new string[] { ".png", ".jpg", ".jpeg" };

    void Awake()
    {
        if (_instance && _instance != this) Destroy(gameObject);
        else _instance = this;
    }

    public Sprite LoadIcon(DialogueItem dialogueItem)
    {
        string iconFileName = dialogueItem.icon;
        foreach (string iconExtension in iconExtensions)
        {
            if (iconFileName.EndsWith(iconExtension, System.StringComparison.OrdinalIgnoreCase))
            {
                iconFileName = iconFileName.Remove(iconFileName.Length - iconExtension.Length);
                break; // Already removed the extension so break early
            }
        }
        string resource = Path.Combine(dialogueFolder, iconFolder, iconFileName);
        return Resources.Load<Sprite>(resource);
    }
}
