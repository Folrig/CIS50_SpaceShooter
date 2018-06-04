using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject restartButton = null;
    public GameObject quitButton = null;

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadSceneAsync("MainScene");
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
