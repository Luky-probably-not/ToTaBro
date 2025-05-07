using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuNavigation : MonoBehaviour
{
    public Button[] buttons;
    private int index = 0;

    private void OnEnable()
    {
        if (buttons.Length > 0)
        {
            index = 0;
            SelectButton(buttons[index]);
        }
    }

    public void OnNavigate(InputValue value)
    {
        Vector2 direction = value.Get<Vector2>();

        if (direction.y < 0 || direction.x < 0)
        {
            index = (index + 1) % buttons.Length;
            SelectButton(buttons[index]);
        }
        else if (direction.y > 0 || direction.x > 0)
        {
            index = (index - 1 + buttons.Length) % buttons.Length;
            SelectButton(buttons[index]);
        }
    }

    public void OnSubmit()
    {
        buttons[index].onClick.Invoke();
    }

    private void SelectButton(Button button)
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }
}