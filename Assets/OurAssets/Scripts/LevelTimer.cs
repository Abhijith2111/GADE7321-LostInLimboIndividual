using UnityEngine;
using UnityEngine.Events;

// Add score based on how many seconds were left in the level
public class LevelTimer : MonoBehaviour
{
    public static LevelTimer Instance { get; private set; }

    [SerializeField, Range(120f, 1200f)] // 2min-20min (I doubt we need 20 but who knows)
    float m_LevelTime = 300f; // 5min by default
    [field: SerializeField]
    public UnityEvent OnBeaten;

    public int CurrentTime => Mathf.CeilToInt(m_CurrentTime);
    public int BonusScore => Mathf.CeilToInt(m_CurrentTime);

    float m_CurrentTime;

	void Awake()
	{
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
	}

	void Start() => m_CurrentTime = m_LevelTime;

    void Update()
    {
        if (m_CurrentTime > 0f) m_CurrentTime = Mathf.Max(m_CurrentTime - Time.deltaTime, 0f); // Thankfully dialogue freezes time at the start so this won't be affected
    }
}
