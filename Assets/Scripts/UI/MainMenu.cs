using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    CanvasView canvasView;

    private void Start()
    {
        canvasView = GetComponent<CanvasView>();
    }

    public void StartGame() {
        GameManager.StartGame();
    }

    public void Credits() {
        GameManager.Credits();
    }

    public void QuitGame() {
        GameManager.QuitGame();
    }

    
}
