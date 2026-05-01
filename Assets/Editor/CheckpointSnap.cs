using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Checkpoint))]
public class CheckpointSnap : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		Checkpoint checkpoint = (Checkpoint)target;
		if (GUILayout.Button("Snap to Ground")) checkpoint.SnapToGround();
	}
}
