using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static GameManager singletonInstance;

    public Player Player;
    public PlayerCamera PlayerCamera;
    public CanvasManager Canvas;

    public GameManager() {

    }

    public static GameManager Instance {
        get {
            if(singletonInstance==null) {
                singletonInstance = new GameManager();

            }

            return singletonInstance;
        }
    }

    #if UNITY_EDITOR
    [MenuItem("GameManager/RestartGame")]
    #endif
    public static void RestartGame() {
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public static void WinGame(){
        Instance.Canvas.WinScreen();
    }
}
