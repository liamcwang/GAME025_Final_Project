using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    CanvasView canvasView;
    bool creditsEnabled = false;

    // didn't think about this, but if I want a screen to be easily open and closeable at the press of a button, it requires me to add an update loop checking for inputs. This isn't great imo.

    private void Update()
    {
        if (Input.anyKeyDown && creditsEnabled) {
            canvasView.HideUIElement("Credits");
            creditsEnabled = false;
        }
    }

    private void Start()
    {
        canvasView = GetComponent<CanvasView>();
    }

    public void StartGame() {
        GameManager.StartGame();
    }

    public void Credits() {
        canvasView.ShowUIElement("Credits");
        creditsEnabled = true;
    }

    public void QuitGame() {
        GameManager.QuitGame();
    }

    
}
