using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRomulanController : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject disruptorPlacement = null;
    public GameObject disruptorPrefab = null;
    public GameObject explosion = null;
    public GameObject player = null;

    public Vector3 currentDestination;

    public int enemyHealth = 2;
    public int scoreValue = 15;

    public float moveSpeed = 2.0f;
    public float timeBetweenShots;
    public float timer = 0.0f;

	void Awake()
    {
        GameObject gameManagerObject = GameObject.Find("GameManager");
        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }

        //find player to make weapon placements aim shots
        player = GameObject.FindWithTag("Player");

        //spawn at random point along X axis above playing field
        transform.position = new Vector3(Random.Range(-8.75f, 8.75f), 10.4f, 0.0f);
    }

    void Start()
    {
        disruptorPlacement.transform.LookAt(player.transform.position);
        currentDestination = new Vector3(Random.Range(-9.0f, 9.0f), 7.5f, 0.0f);
        timeBetweenShots = Random.Range(0.9f, 1.75f);
    }

	void Update()
    {
        timer += Time.deltaTime;

        //continue to aim at player
        if (player != null)
        {
            disruptorPlacement.transform.LookAt(player.transform.position);
        }

        //float from spawn into static Y position
        if (transform.position.y > 7.5)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }

        //move randomly along X axis
        if (transform.position.y <= 7.5)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentDestination, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, currentDestination) <= 0.05f)
            {
                currentDestination = new Vector3(Random.Range(-8.5f, 8.5f), 7.5f, 0.0f);
            }
        }

        if (timer >= timeBetweenShots)
        {
            timer = 0;
            timeBetweenShots = Random.Range(0.9f, 1.75f);
            Instantiate(disruptorPrefab, disruptorPlacement.transform.position, disruptorPlacement.transform.rotation);
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
