using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAsteroid : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject explosion = null;

    public int enemyHealth = 1;
    public int scoreValue = 5;
    public int spawnSide;

    public float moveSpeed = 2.5f;
    public float timer = 0.0f;

    void Awake()
    {
        //determine which side of the screen enemy will spawn
        spawnSide = Random.Range(0, 2);
        GameObject gameManagerObject = GameObject.Find("GameManager");

        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }

        if (spawnSide == 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, -140);
            transform.position = new Vector3(-10.75f, Random.Range(8.5f, 17.0f), 0);
        }
        else if (spawnSide == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, -220);
            transform.position = new Vector3(10.75f, Random.Range(8.5f, 17.0f), 0);
        }
    }

	void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

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
