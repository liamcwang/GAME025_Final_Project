using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Idea is to implement the UI along the lines of a Model-View-Controller framework
/// This is the exposed controller for handling calls to the GameManager and the View
/// </summary>
public class CanvasController : MonoBehaviour
{
    private CanvasView canvasView;

    private void Awake()
    {
        GameManager.Canvas = this;
    }

    private void Start()
    {
        canvasView = GetComponent<CanvasView>();
    }

    public void VictoryScreen() {
        canvasView.ShowUIElement("Victory");
    }

    public void DefeatScreen() {
        canvasView.ShowUIElement("Defeat");
    }

    public void UpdateHealth(float n) {
        canvasView.UpdateUISlider("PlayerHUD", n);
    }

    public void RestartGame() {

    }

    public void QuitGame() {
        GameManager.QuitGame();
    }   


}
