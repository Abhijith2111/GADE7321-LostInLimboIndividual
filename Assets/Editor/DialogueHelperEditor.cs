using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueHelper))]
public class DialogueHelperEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		DialogueHelper helper = (DialogueHelper)target;
		helper.SubDirectory = helper.SubDirectory.Trim();
		helper.FileName = helper.FileName.Trim();
		if (GUILayout.Button("Clear Inputted Data")) helper.ClearValues();
		if (helper.FileName.Length > 0) if (GUILayout.Button($"Load \"{helper.FileAndSub}.json\"")) helper.LoadFile();
		if (!string.IsNullOrWhiteSpace(helper.FileNameToSaveOrDelete))
		{
			if (GUILayout.Button($"Create/Modify \"{helper.FileToSaveOrDeleteAndSub}.json\"")) helper.SaveFile(helper.FileToSaveOrDeleteAndSub);
			if (GUILayout.Button($"Delete \"{helper.FileToSaveOrDeleteAndSub}.json\"")) helper.DeleteFile(helper.FileToSaveOrDeleteAndSub);
		}
		if (helper.FileName.Length > 0 && helper.FileName != helper.FileNameToSaveOrDelete)
		{
			if (GUILayout.Button($"Create/Modify \"{helper.FileAndSub}.json\" with values from \"{helper.FileToSaveOrDeleteAndSub}.json\"")) helper.SaveFile(helper.FileAndSub);
			if (GUILayout.Button($"Delete \"{helper.FileAndSub}.json\"")) helper.DeleteFile(helper.FileAndSub);
		}
		EditorUtility.SetDirty(target);
	}
}
