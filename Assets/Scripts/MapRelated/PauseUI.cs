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
        Debug.Log(gameObject);
    }

    void OnResumeClicked()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    void OnReturnClicked()
    {
        Time.timeScale = 1f;
        GameManager.Instance.LoadAccueil(1);
        Destroy(gameObject);
    }
}
