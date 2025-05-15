using UnityEngine;
using UnityEngine.UI;

public class AccueilUI : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public Button creditsButton;

    void Start()
    {
        if (startButton != null)
            startButton.onClick.AddListener(OnStartClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);
            
        if (creditsButton != null)
            creditsButton.onClick.AddListener(OnCreditsClicked);
    }

    void OnStartClicked()
    {
        GameManager.Instance.StartGame();
    }

    void OnQuitClicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void OnCreditsClicked()
    {
        GameManager.Instance.Credits();
    }
}