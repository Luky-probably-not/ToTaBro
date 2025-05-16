using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public Button resumeButton;
    public Button returnButton;

    void Start()
    {
        resumeButton.onClick.AddListener(OnResumeClicked);
        returnButton.onClick.AddListener(OnReturnClicked);
        Time.timeScale = 0f;
        GameManager.Instance.setInGame(false);
    }

    void OnResumeClicked()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);
        GameManager.Instance.setInGame(true);
    }

    void OnReturnClicked()
    {
        Time.timeScale = 1f;
        GameManager.Instance.GameOver();
        Destroy(gameObject);
    }
}