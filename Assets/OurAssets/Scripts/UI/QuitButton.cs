using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuitButton : MonoBehaviour
{
    void Awake() => GetComponent<Button>().onClick.AddListener(Quit);

    void OnDestroy() => GetComponent<Button>().onClick.RemoveListener(Quit);

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
