using UnityEngine;
using UnityEngine.UI;

public class AccueilUI : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;

    void Start()
    {
        if (startButton != null)
            startButton.onClick.AddListener(OnStartClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);
    }

    void OnStartClicked()
    {
        Debug.Log("Start button clicked");
        GameManager.Instance.StartGame();
    }

    void OnQuitClicked()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}