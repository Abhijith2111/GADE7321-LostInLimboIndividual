using UnityEngine;

public class CollectableAnimation : MonoBehaviour
{
    [System.Serializable]
    public enum WaveType
    {
        Sine,
        Cosine
    }

    [Header("Rotation")]
    [SerializeField, Range(0f, 360f)]
    float m_DegreesPerSecond = 30f;
    [Header("Vertical Movement")]
    [SerializeField]
    WaveType m_WaveType = WaveType.Cosine; // By default starts at 0 y so probably best
    [SerializeField, Range(0f, 1f)]
    float m_Amplitude = 0.1f;
    [SerializeField, Range(0f, 2f)]
    float m_Frequency = 1f;

	Vector3 m_StartingLocalPosition;
	PlayerPause m_PlayerPause;
    float m_CurrentTime = 0f;

    void Awake()
    {
        m_StartingLocalPosition = transform.localPosition;
        m_PlayerPause = FindFirstObjectByType<PlayerPause>();
    }

	void Update()
    {
        if (m_PlayerPause && m_PlayerPause.Paused) return;
        m_CurrentTime += Time.unscaledDeltaTime;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + m_DegreesPerSecond * Time.unscaledDeltaTime, transform.localEulerAngles.z);
        transform.localPosition = new Vector3(m_StartingLocalPosition.x, m_StartingLocalPosition.y + Wave(), m_StartingLocalPosition.z);
    }

    float Wave()
    {
        if (m_WaveType == WaveType.Sine) return Mathf.Sin(m_CurrentTime * m_Frequency) * m_Amplitude;
        return Mathf.Cos(m_CurrentTime * m_Frequency) * m_Amplitude;
    }
}
