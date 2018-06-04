using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroAnimation : MonoBehaviour
{
    public GameObject gameManager = null;

    public AudioSource spockGreetings;
    public AudioSource spockUnpleasantSituation;

    public Text spockTextOne = null;
    public Text spockTextTwo = null;
    
	void Start()
    {
        //get audio for "cinematic" GUI opening
        AudioSource[] spockClips = GetComponents<AudioSource>();
        spockGreetings = spockClips[0];
        spockUnpleasantSituation = spockClips[1];
        spockGreetings.Play();
	}
	
    public void NextTextActive()
    {
        //change text dialogue in GUI
        spockTextOne.gameObject.SetActive(false);
        spockTextTwo.gameObject.SetActive(true);
        spockUnpleasantSituation.Play();
    }

    public void ActivateGameManager()
    {
        //begin game
        gameManager.SetActive(true);
    }
}
