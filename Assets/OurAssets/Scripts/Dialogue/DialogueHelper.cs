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
	string m_SaveDeleteFilePath;

#if UNITY_EDITOR
	void OnValidate()
	{
		m_LoadFilePath = $"{m_JSONFolder}/{FileAndSub}";
		m_SaveDeleteFilePath = $"{m_ResourcesFolder}/{m_JSONFolder}/{FileToSaveOrDeleteAndSub}.json";
	}
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

	public void SaveFile()
	{
		if (!string.IsNullOrWhiteSpace(SubDirectory) && !Directory.Exists($"{m_ResourcesFolder}/{m_JSONFolder}/{SubDirectory}")) Directory.CreateDirectory($"{m_ResourcesFolder}/{m_JSONFolder}/{SubDirectory}");
		string json = JsonUtility.ToJson(m_Dialogue.Serialised, prettyPrint: true);
		string message = $"Successfully {(File.Exists(m_SaveDeleteFilePath) ? "modified" : "created")} \"{FileToSaveOrDeleteAndSub}.json\"";
		File.WriteAllText(m_SaveDeleteFilePath, json);
		Debug.Log(message);
	}

	public void DeleteFile()
	{
		if (!File.Exists(m_SaveDeleteFilePath))
		{
			Debug.LogError($"\"{FileNameToSaveOrDelete}.json\" doesn't exist inside Assets/Resources/{m_JSONFolder}{Sub}");
			return;
		}
		File.Delete(m_SaveDeleteFilePath);
		string message = $"Successfully deleted {FileToSaveOrDeleteAndSub}.json";
		if (File.Exists($"{m_SaveDeleteFilePath}.meta"))
		{
			File.Delete($"{m_SaveDeleteFilePath}.meta");
			message += $" and {FileToSaveOrDeleteAndSub}.json.meta";
		}
		if (m_ClearOnDelete) ClearValues();
		Debug.Log(message);
	}
}
