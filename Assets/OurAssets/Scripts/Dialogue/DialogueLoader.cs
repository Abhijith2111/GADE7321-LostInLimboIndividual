using System.IO;
using UnityEngine;

public class DialogueLoader : MonoBehaviour
{
    static DialogueLoader _instance;
    public static DialogueLoader Instance
    {
        get
        {
            // Lazy instantiation
            if (!_instance)
            {
                GameObject go = new GameObject("DialogueLoader");
                _instance = go.AddComponent<DialogueLoader>();
            }
            return _instance;
        }
    }

    [SerializeField]
    string dialogueFolder = "Dialogue";
    [SerializeField]
    string jsonFolder = "DialogueJSONs";
    [SerializeField]
    string iconFolder = "DialogueIcons";
    [SerializeField]
    string[] iconExtensions = new string[] { ".png", ".jpg", ".jpeg" };

    void Awake()
    {
        if (_instance && _instance != this) Destroy(gameObject);
        else _instance = this;
    }

    T LoadResource<T>(string folder, string fileName) where T : Object
    {
        string resource = Path.Combine(dialogueFolder, folder, fileName);
        return Resources.Load<T>(resource);
    }

    public Dialogue LoadDialogue(string fileName)
    {
        if (fileName.EndsWith(".json", System.StringComparison.OrdinalIgnoreCase)) fileName = fileName.Remove(fileName.Length - ".json".Length);
        TextAsset json = LoadResource<TextAsset>(jsonFolder, fileName);
        return json ? JsonUtility.FromJson<Dialogue>(json.text) : null;
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
        return LoadResource<Sprite>(iconFolder, iconFileName);
    }
}
