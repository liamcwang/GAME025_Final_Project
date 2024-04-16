using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Idea is to implement the UI along the lines of a Model-View-Controller framework
/// This is the view, whose sole responsibility is to contain logic for displaying and hiding
/// UI elements
/// </summary>
public class CanvasView : MonoBehaviour
{
    [SerializeField] private UIGameObject[] UIElements;
    private Dictionary<string, UIGameObject> UIRef = new Dictionary<string, UIGameObject>();
    // Start is called before the first frame update
    void Awake() {

    }

    void Start()
    {
        
        foreach (UIGameObject UIObj in UIElements)
        {
            UIRef[UIObj.name] = UIObj;
            if (UIObj.hideOnStart) {
                HideUIElement(UIObj.name);
            }
        }
    }

    public void UpdateUIText(string name, string s) {
        TMP_Text textComponent = UIRef[name].gameObject.GetComponent<TMP_Text>();
        textComponent.text = s;
    }

    public void UpdateUISlider(string name, float n) {
        Slider sliderComponent = UIRef[name].gameObject.GetComponentInChildren<Slider>();
        sliderComponent.value = n;
    }

    public void HideUIElement(string name) {
        UIRef[name].gameObject.SetActive(false);
    }
    public void ShowUIElement(string name) {
        UIRef[name].gameObject.SetActive(true);
    }

    
}

[System.Serializable]
public struct UIGameObject
{
    public string name;
    public bool hideOnStart;
    public UIElemType type;
    public GameObject gameObject;
}

public enum UIElemType {NONE};