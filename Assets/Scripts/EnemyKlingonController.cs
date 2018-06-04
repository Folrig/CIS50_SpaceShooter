using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKlingonController : MonoBehaviour
{
    public GameManager gameManager;

    public List<GameObject> disruptorPlacements = new List<GameObject>();
    public GameObject disruptorPrefab = null;
    public GameObject explosion = null;

    public int enemyHealth = 3;
    public int scoreValue = 10;

    public Vector3 evadeValue;

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

        //spawn at random point along X axis above playing field
        transform.position = new Vector3(Random.Range(-8.75f, 8.75f), Random.Range(10.25f, 10.45f), 0.0f);
    }

    void Start()
    {
        timeBetweenShots = Random.Range(0.85f, 1.5f);

        //determine diagonal flight path destination
        evadeValue = new Vector3(Random.Range(-9.0f, 9.0f), -4.5f, 0.0f);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenShots)
        {
            timer = 0;
            timeBetweenShots = Random.Range(0.85f, 1.5f);
            for (int i = 0; i < disruptorPlacements.Count; i++)
            {
                Instantiate(disruptorPrefab, disruptorPlacements[i].transform.position, disruptorPlacements[i].transform.rotation);
            }
        }

        //float straight down to Y position before beginning diagonal movement
        if (transform.position.y > 7.5)
        {
            transform.Translate(Vector3.up * 0.75f * moveSpeed * Time.deltaTime);
        }

        //start diagonal "evasive" movement
        if (transform.position.y <= 10.5)
        {
            transform.position = Vector3.MoveTowards(transform.position, evadeValue, moveSpeed * 1.1f * Time.deltaTime);
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
            Debug.Log(other.gameObject + " hit Klingon!");
        }
        if (other.gameObject.tag == "PhotonTorpedo")
        {
            enemyHealth -= 5;
            Debug.Log("Hit by torpedo");
        }
    }
}
