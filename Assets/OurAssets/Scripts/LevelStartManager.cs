using UnityEngine;
using UnityEngine.SceneManagement;

// Manages the scene number to restart in so that don't have to keep opening menu if chose to start at level two and it would keep taking you back to level one
public class LevelStartManager : MonoBehaviour
{
    public static LevelStartManager Instance { get; private set; }
    
    public int StartingLevelIndex { get; set; }

	void Awake()
	{
        if (Instance && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            StartingLevelIndex = -1;
        }
	}

	void Start()
    {
        if (StartingLevelIndex < 0) StartingLevelIndex = SceneManager.GetActiveScene().buildIndex;
    }
}
