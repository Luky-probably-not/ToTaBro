using UnityEngine;
using UnityEngine.UI;

public class AccueilUI : MonoBehaviour
{
    public Button startButton;

    private void Start()
    {
        if (startButton == null)
        {
            Debug.LogError("Le bouton startButton n'a pas été assigné dans l'Inspector !");
        }
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        Debug.Log("Le bouton de démarrage a été cliqué !");
        GameManager.Instance.StartGame();
    }
}