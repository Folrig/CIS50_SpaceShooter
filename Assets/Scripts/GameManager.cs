using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();   //0 = asteroid, 1 = Klingon, 2 = Romulan, 3 = defense station
    public List<float> spawnTimes = new List<float>();          //match above index values
    public List<float> timers = new List<float>();              //match above index values

    public GameObject bossBorgCube = null;
    public GameObject explosion = null;
    public GameObject[] livesLeft;
    public GameObject player = null;
    public GameObject playerPrefab = null;

    public Text scoreTextLabel = null;

    public bool bossIsActive = false;
    public bool screenCleared = false;

    public int livesRemaining = 3;
    public int score = 0;
    
    public float bossTimer = 0.0f;
    public float spawnBoss = 120.0f;
    public float spawnPlayer = 4.0f;
    public float spawnTimer = 0.0f;

    public string[] enemiesForScreenClear = { "Enemy", "Disruptor" };

    void Update()
    {
        bossTimer += Time.deltaTime;

        //begin normal level enemy spawning and count down until boss time
        if (bossTimer <= spawnBoss)
        {
            for (int i = 0; i < timers.Count; i++)
            {
                timers[i] += Time.deltaTime;
            }

            for (int i = 0; i < timers.Count; i++)
            {
                if (timers[i] >= spawnTimes[i])
                {
                    Instantiate(enemies[i]);
                    timers[i] = 0.0f;
                }
            }
        }
        //destroy all enemies, stop their spawning, and activate boss
        else if (bossTimer >= spawnBoss && screenCleared == false)
        {
            screenCleared = true;

            foreach (string tag in enemiesForScreenClear)
            {
                GameObject[] enemiesToDestroy = GameObject.FindGameObjectsWithTag(tag);
                foreach (GameObject enemy in enemiesToDestroy)
                {
                    Instantiate(explosion, enemy.transform.position, enemy.transform.rotation);
                    Destroy(enemy);
                }
            }
            
            enemies.Clear();
            spawnTimes.Clear();
            timers.Clear();
            bossBorgCube.SetActive(true);
            bossIsActive = true;
        }

        if (bossIsActive == true)
        {
            bossBorgCube = GameObject.Find("BossBorgCube");
        }

        //give player a couple seconds to recover before spawning character again
        if (player == null && (livesLeft[0].activeSelf == true || livesLeft[1].activeSelf == true))
        {
            spawnTimer += Time.deltaTime;
        }

        //remove a life and clear screen when player spawns a new life
        if (player == null && spawnTimer >= spawnPlayer)
        {
            foreach (string tag in enemiesForScreenClear)
            {
                GameObject[] enemiesToDestroy = GameObject.FindGameObjectsWithTag(tag);
                foreach (GameObject enemy in enemiesToDestroy)
                {
                    Instantiate(explosion, enemy.transform.position, enemy.transform.rotation);
                    Destroy(enemy);
                }
            }
            Instantiate(playerPrefab);
            player = GameObject.FindGameObjectWithTag("Player");
            spawnTimer = 0.0f;
            livesRemaining--;

            if (livesLeft[0].activeSelf == true)
            {
                livesLeft[0].SetActive(false);
            }

            else if (livesLeft[1].activeSelf == true)
            {
                livesLeft[1].SetActive(false);
            }
        }

        if (player == null && livesRemaining == 1)
        {
            StartCoroutine(WaitForNewScreen("GameOverScreen"));
        }

        if (bossBorgCube == null)
        {
            bossIsActive = false;
            StartCoroutine(WaitForNewScreen("WinScreen"));
        }
    }

    //change to appropriate scene for game end
    public IEnumerator WaitForNewScreen(string sceneName)
    {
        yield return new WaitForSecondsRealtime(3.0f);
        SceneManager.LoadSceneAsync(sceneName);
    }

    //add and update score as necessary
    public void AddToScore (int scoreValue)
    {
        score += scoreValue;
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreTextLabel.text = "Score: " + score;
    }
}

//final assignment parameters
//1. 2 unique weapons
//2. 2 unique enemies (boss doesn't count)
//3. Boss encounter
//4. Special ability (warp drive/blink ability, thruster)
//5. Score System
//6. Life system
//7. Win/loss (beat boss, lose lives)