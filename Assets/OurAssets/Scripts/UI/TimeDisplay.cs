using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TimeDisplay : MonoBehaviour
{
	[SerializeField, TextArea]
	string m_DisplayText;
	[SerializeField, TextArea]
	string m_OnBeatenText;

	public bool Beaten { get; set; } = false;

	TMP_Text m_Text;

	void Awake() => m_Text = GetComponent<TMP_Text>();

	void Update() => m_Text.text = Beaten ? m_OnBeatenText : string.Format(m_DisplayText, LevelTimer.Instance?.CurrentTime ?? 0);
}
