using System;
using System.IO;
using UnityEngine;

public class DialogueHelper : MonoBehaviour
{
	static readonly string m_ResourcesFolder = $"{Application.dataPath}/Resources";
	static readonly string m_JSONFolder = "Dialogue/DialogueJSONS";

	[SerializeField]
    bool m_ClearOnDelete = true;
	[field: SerializeField]
	public string SubDirectory { get; set; } = "";
    [field: SerializeField]

	public string FileName { get; set; } = "";
    [SerializeField]
    Dialogue m_Dialogue;

	string Sub => SubDirectory + (string.IsNullOrWhiteSpace(SubDirectory) ? "" : "/");
	public string FileAndSub => Sub + FileName;
	public string FileNameToSaveOrDelete => string.IsNullOrWhiteSpace(m_CurrentLoadedFileName) ? FileName : m_CurrentLoadedFileName;
	public string FileToSaveOrDeleteAndSub => Sub + FileNameToSaveOrDelete;

	string m_CurrentLoadedFileName;
	string m_LoadFilePath;

#if UNITY_EDITOR
	void OnValidate() => m_LoadFilePath = $"{m_JSONFolder}/{FileAndSub}";
#endif

	public void ClearValues()
	{
		SubDirectory = "";
		FileName = "";
		m_CurrentLoadedFileName = "";
		m_Dialogue.UnloadAllSprites();
		m_Dialogue = null;
	}

	public void LoadFile()
	{
		TextAsset json = Resources.Load<TextAsset>(m_LoadFilePath);
		if (json == null)
		{
			Debug.LogError($"\"{FileName}.json\" doesn't exist inside Assets/Resources/{m_JSONFolder}/{Sub}");
			return;
		}
		try
		{
			m_Dialogue = JsonUtility.FromJson<SerialisedDialogue>(json.text).Deserialised;
			m_CurrentLoadedFileName = FileName;
			Debug.Log($"Successfully loaded dialogue from \"{FileAndSub}.json\"");
		}
		catch (ArgumentException)
		{
			Debug.LogError($"Invalid data in {FileAndSub}.json");
		}
	}

	public void SaveFile(string fileToSaveAndSub)
	{
		if (!fileToSaveAndSub.EndsWith(".json")) fileToSaveAndSub += ".json";
		int indexOfSlash = fileToSaveAndSub.LastIndexOf('/');
		string sub = fileToSaveAndSub.Substring(0, indexOfSlash).TrimEnd('/');
		string file = fileToSaveAndSub.Substring(indexOfSlash, fileToSaveAndSub.Length - indexOfSlash).TrimStart('/');
		if (!string.IsNullOrWhiteSpace(sub) && !Directory.Exists($"{m_ResourcesFolder}/{m_JSONFolder}/{sub}")) Directory.CreateDirectory($"{m_ResourcesFolder}/{m_JSONFolder}/{sub}");
		string json = JsonUtility.ToJson(m_Dialogue.Serialised, prettyPrint: true);
		string writePath = $"{m_ResourcesFolder}/{m_JSONFolder}/{fileToSaveAndSub}";
		string message = $"Successfully {(File.Exists(writePath) ? "modified" : "created")} \"{fileToSaveAndSub}.json\"";
		File.WriteAllText(writePath, json);
		Debug.Log(message);
	}

	public void DeleteFile(string fileToDeleteAndSub)
	{
		if (!fileToDeleteAndSub.EndsWith(".json")) fileToDeleteAndSub += ".json";
		string deletePath = $"{m_ResourcesFolder}/{m_JSONFolder}/{fileToDeleteAndSub}";
		if (!File.Exists(deletePath))
		{
			int indexOfSlash = fileToDeleteAndSub.LastIndexOf('/');
			string sub = fileToDeleteAndSub.Substring(0, indexOfSlash).TrimEnd('/');
			if (string.IsNullOrWhiteSpace(sub) && !sub.EndsWith('/')) sub += '/';
			string file = fileToDeleteAndSub.Substring(indexOfSlash, fileToDeleteAndSub.Length - indexOfSlash).TrimStart('/');
			Debug.LogError($"\"{file}.json\" doesn't exist inside Assets/Resources/{m_JSONFolder}/{sub}");
			return;
		}
		File.Delete(deletePath);
		string message = $"Successfully deleted {fileToDeleteAndSub}.json";
		if (File.Exists($"{deletePath}.meta"))
		{
			File.Delete($"{deletePath}.meta");
			message += $" and {fileToDeleteAndSub}.json.meta";
		}
		if (m_ClearOnDelete) ClearValues();
		Debug.Log(message);
	}
}
