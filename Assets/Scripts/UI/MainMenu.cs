using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
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
