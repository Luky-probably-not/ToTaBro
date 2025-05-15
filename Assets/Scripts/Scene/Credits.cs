using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    public Button MenuButton;

    void Start()
    {
        if (MenuButton != null)
            MenuButton.onClick.AddListener(OnMenuClicked);
    }

    void OnMenuClicked()
    {
        GameManager.Instance.LoadAccueil();
    }
}