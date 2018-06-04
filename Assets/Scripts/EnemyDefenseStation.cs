using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefenseStation : MonoBehaviour
{
    public GameManager gameManager;

    public List<GameObject> disruptorPlacements = new List<GameObject>();
    public GameObject disruptorPrefab = null;
    public GameObject explosion = null;

    public int enemyHealth = 3;
    public int scoreValue = 20;
    public int shotPosition1;
    public int shotPosition2;

    public float moveSpeed = 1.75f;
    public float timeBetweenShots = 1.25f;
    public float timer = 0.0f;

    void Awake()
    {
        GameObject gameManagerObject = GameObject.Find("GameManager");

        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }

        transform.position = new Vector3(Random.Range(-8.75f, 8.75f), 10.4f, 0.0f);

        //determine which 2 of the 5 weapon placements will fire first
        shotPosition1 = Random.Range(0, 5);
        do
        {
            shotPosition2 = Random.Range(0, 5);
        } while (shotPosition2 == shotPosition1);
	}

	void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        //shoot weapon at 2 of 5 weapon placements
        if (timer >= timeBetweenShots)
        {
            timer = 0;
            Instantiate(disruptorPrefab, disruptorPlacements[shotPosition1].transform.position, disruptorPlacements[shotPosition1].transform.rotation);
            Instantiate(disruptorPrefab, disruptorPlacements[shotPosition2].transform.position, disruptorPlacements[shotPosition2].transform.rotation);

            //determine which 2 of the 5 weapon placements will fire after cooldown
            shotPosition1 = Random.Range(0, 5);
            do
            {
                shotPosition2 = Random.Range(0, 5);
            } while (shotPosition2 == shotPosition1);
        }

        if (enemyHealth <= 0)
        {
            gameManager.AddToScore(scoreValue);
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Phaser")
        {
            Destroy(other.gameObject);
            enemyHealth--;
        }
        if (other.gameObject.tag == "PhotonTorpedo")
        {
            enemyHealth -= 5;
        }
    }
}
