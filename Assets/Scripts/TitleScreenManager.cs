using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    //show title screen and how to play before players begin
    public GameObject howToPlayButton = null;
    public GameObject howToPlayCanvas = null;
    public GameObject mainGameButton = null;
    public GameObject mainScreenCanvas = null;

    public AudioSource warpSFX;

    void Awake()
    {
        warpSFX = GetComponent<AudioSource>();
    }

    public void HowToPlayButtonClicked()
    {
        warpSFX.Play();
        mainScreenCanvas.SetActive(false);
        howToPlayCanvas.SetActive(true);
    }

    public void MainGameButtonClicked()
    {
        howToPlayCanvas.SetActive(false);
        SceneManager.LoadSceneAsync("MainScene");
    }
}
