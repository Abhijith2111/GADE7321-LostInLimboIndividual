using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelSelectButton : MonoBehaviour
{
    [SerializeField, Min(0)]
    int m_SceneIndexToLoad = 0;
    [SerializeField]
    LoadingScreen m_LoadingScreen;

	void Awake()
	{
        GetComponent<Button>().onClick.AddListener(() => {
            if (LevelStartManager.Instance) LevelStartManager.Instance.StartingLevelIndex = m_SceneIndexToLoad; // Set the new starting scene so that don't have to keep opening menu after dying
            m_LoadingScreen.SceneIndexToLoad = m_SceneIndexToLoad;
            m_LoadingScreen.gameObject.SetActive(true);
        });
	}
}
