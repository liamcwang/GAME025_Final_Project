using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DEPRECATED: DO NOT USE
public enum UIType {VICTORY, LOSS};
public class CanvasManager : MonoBehaviour
{
    [SerializeField] private UIObject[] UIElements;
    private Dictionary<UIType, GameObject> UIRef = new Dictionary<UIType, GameObject>();
    void Awake()
    {
        foreach (UIObject UIObj in UIElements) {
            UIRef[UIObj.name] = UIObj.screenObject;
        }
    }

    private void Start()
    {
        foreach (UIObject UIObj in UIElements) {
            setScreen(UIObj.name, false);
        }
    }

    void setScreen(UIType key, bool isActive) {
        GameObject gObj = UIRef[key];
        gObj.SetActive(isActive);
    }

    public void WinScreen() {
        setScreen(UIType.VICTORY, true);
    }
}

[System.Serializable]
public struct UIObject {
    public UIType name;
    public GameObject screenObject;
}