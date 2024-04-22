using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static GameManager singletonInstance;

    #region INSTANCE_VARIABLES
    private Player _player;
    public static Player Player {
        get => Instance._player;
        set => Instance._player = value;
    }
    private PlayerCamera _playerCamera;
    public static PlayerCamera PlayerCamera {
        get => Instance._playerCamera;
        set => Instance._playerCamera = value;
    }
    private CanvasController _canvas;
    public static CanvasController Canvas {
        get => Instance._canvas;
        set => Instance._canvas = value;
    }
    private bool _isPaused;
    public static bool isPaused {
        get => Instance._isPaused;
        set => Instance._isPaused = value;
    }

    #endregion


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
        Unpause();
    }

    public static void StartGame() {
        SceneManager.LoadScene(1);
        Unpause();
    }

    public static void ToMainMenu() {
        SceneManager.LoadScene("MainMenu");
        Cursor.visible = true;

    }

    public static void Pause() {
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true;
    }

    public static void Unpause(){
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false;

    }

    public static void Victory(){
        Canvas.VictoryScreen();
    }

    public static void Defeat(){
        Canvas.DefeatScreen();
    }

    public static void QuitGame(){
        Application.Quit();
    }
}
