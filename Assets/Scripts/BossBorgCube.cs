using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBorgCube : MonoBehaviour
{
    public List<GameObject> aimedWeaponPlacements = new List<GameObject>();
    public List<GameObject> staticWeaponPlacements = new List<GameObject>();
    public GameObject bossHealthBar = null;
    public GameObject bossHealthText = null;
    public GameObject disruptorPrefab = null;
    public GameObject explosion = null;
    public GameObject player = null;

    public AudioSource weAreBorgSFX;

    public int bossCurrentHealth = 120;
    public int bossMaxHealth = 120;

    public float aimedWeaponFire = 1.50f;
    public float aimedWeaponTimer = 0.0f;
    public float staticWeaponFire = 1.1f;
    public float staticWeaponTimer = 0.0f;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        weAreBorgSFX = GetComponent<AudioSource>();
        bossHealthBar.SetActive(true);
        bossHealthText.SetActive(true);
    }

    void Start()
    {
        //play Borg SFX after all enemy explosion sounds have ended
        weAreBorgSFX.PlayDelayed(0.50f);
        for (int i = 0; i < aimedWeaponPlacements.Count; i++)
        {
            aimedWeaponPlacements[i].transform.LookAt(player.transform.position);
        }
	}
	
    void Update()
    {
        aimedWeaponTimer += Time.deltaTime;
        staticWeaponTimer += Time.deltaTime;
        bossHealthBar.transform.localScale = new Vector3((float)bossCurrentHealth / (float)bossMaxHealth, 1.0f, 1.0f);
        player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            for (int i = 0; i < aimedWeaponPlacements.Count; i++)
            {
                aimedWeaponPlacements[i].transform.LookAt(player.transform.position);
            }
        }

        if (aimedWeaponTimer >= aimedWeaponFire)
        {
            aimedWeaponTimer = 0;
            for (int i = 0; i < aimedWeaponPlacements.Count; i++)
            {
                Instantiate(disruptorPrefab, aimedWeaponPlacements[i].transform.position, aimedWeaponPlacements[i].transform.rotation);
            }
        }

        if (staticWeaponTimer >= staticWeaponFire)
        {
            staticWeaponTimer = 0;
            for (int i = 0; i < staticWeaponPlacements.Count; i++)
            {
                Instantiate(disruptorPrefab, staticWeaponPlacements[i].transform.position, staticWeaponPlacements[i].transform.rotation);
            }
        }

        if (bossCurrentHealth <= 0)
        {
            bossCurrentHealth = 0;
            bossHealthBar.transform.localScale = new Vector3(0, 0, 0);
            for (int i = 0; i < aimedWeaponPlacements.Count; i++)
            {
                Instantiate(explosion, transform.position, transform.rotation);
                Destroy(aimedWeaponPlacements[i]);
            }
            for (int i = 0; i < staticWeaponPlacements.Count; i++)
            {
                Instantiate(explosion, transform.position, transform.rotation);
                Destroy(staticWeaponPlacements[i]);
            }
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Phaser")
        {
            Destroy(other.gameObject);
            bossCurrentHealth--;
        }

        if (other.gameObject.tag == "PhotonTorpedo")
        {
            bossCurrentHealth -= 10;
        }
    }
}
