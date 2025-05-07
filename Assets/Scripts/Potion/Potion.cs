using UnityEngine;
using System.Collections;
using TMPro;

public abstract class Potion : MonoBehaviour
{
    public string type { get; set; }
    public float value { get; set; }
    public string description { get; set; }

    public Canvas popupCanvas;
    public TMP_Text nameTMP;
    public TMP_Text descTMP;

    public bool canBuy = false;
    public int cost = 1;
    public void Init(string type, float value)
    {
        this.type = type;
        this.value = value;
        this.description = description;
    }

    protected virtual void Awake() 
    {
        popupCanvas = GetComponentInChildren<Canvas>();
        popupCanvas.gameObject.SetActive(false);
    }
    public void Use()
    {
        ClosePopup();
        Destroy(gameObject);
    }

    public void ShowPopup()
    {
        popupCanvas.gameObject.SetActive(true);
        GetTMP();
        nameTMP.SetText(type);
        descTMP.SetText(fillDesc());

    }

    public void ClosePopup()
    {
        popupCanvas.gameObject.SetActive(false);
    }
    public void GetTMP()
    {
        foreach (TMP_Text tmp in GetComponentsInChildren<TMP_Text>())
        {
            switch (tmp.name)
            {
                case "Name":
                    nameTMP = tmp; break;
                case "Description":
                    descTMP = tmp; break;
                default:
                    break;
            }
        }
    }

    public string fillDesc()
    {
        return description.Replace("xxxxx", value.ToString());
    }

}