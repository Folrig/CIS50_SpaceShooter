using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject explosion = null;
    public GameObject healthBar = null;
    public GameObject[] phaserPlacements;
    public GameObject phaserPrefab = null;
    public GameObject photonTorpedoPlacement = null;
    public GameObject photonTorpedoPrefab = null;
    public GameObject shield = null;
    public GameObject shieldBar = null;

    public AudioSource phaserFireSFX;
    public AudioSource screenClearBombSFX;

    public KeyCode upInput = KeyCode.W;
    public KeyCode downInput = KeyCode.S;
    public KeyCode leftInput = KeyCode.A;
    public KeyCode rightInput = KeyCode.D;
    public KeyCode shootPhaser = KeyCode.Mouse0;
    public KeyCode activateShield = KeyCode.Space;
    public KeyCode shootPhotonTorpedo = KeyCode.Mouse1;
    public KeyCode screenClearBomb = KeyCode.Q;
    public KeyCode warpDrive = KeyCode.LeftShift;

    public bool isPhotonTorpedoAway = false;
    public bool screenClearBombUsed = false;
    public bool shieldActive = false;
    public bool shieldDestroyed = false;


    public int currentHealth = 50;
    public int currentShield = 50;
    public int phaserRotation = 0;
    public int maxHealth = 50;
    public int maxShield = 50;

    public float moveSpeed = 5.0f;
    public float torpedoCooldown = 2.5f;
    public float torpedoTimer = 0.0f;
    public float warpDriveCooldown = 1.5f;
    public float warpDriveTimer = 0.0f;
    public float xPosMin = -9.25f;
    public float xPosMax = 9.25f;
    public float yPosMin = -1.25f;
    public float yPosMax = 8.0f;

    public string[] enemiesForScreenClear = { "Enemy", "Disruptor" };

    void Start()
    {
        healthBar = GameObject.Find("PlayerHealthBar");
        shieldBar = GameObject.Find("ShieldBar");
        healthBar.transform.localScale = new Vector3(1, 1, 1f);
        shieldBar.transform.localScale = new Vector3(1, 1, 1);
        Mathf.Clamp(currentHealth, 0, 50);
        AudioSource[] SFX = GetComponents<AudioSource>();
        screenClearBombSFX = SFX[0];
        phaserFireSFX = SFX[1];
        shield.SetActive(false);
        warpDriveTimer = 1.5f;
    }

    void Update()
    {
        //start timers for abilities and ensure health and shield bars deplete as damage is taken
        torpedoTimer += Time.deltaTime;
        warpDriveTimer += Time.deltaTime;
        shieldBar.transform.localScale = new Vector3((float)currentShield / (float)maxShield, 1.0f, 1.0f);
        healthBar.transform.localScale = new Vector3((float)currentHealth / (float)maxHealth, 1.0f, 1.0f);

        if (Input.GetKey(upInput))
        {
            //keep player inside the playing field
            if (transform.position.y < yPosMax)
            {
                transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
                //allow player to use blink ability if not on cooldown
                if (Input.GetKeyDown(warpDrive) && warpDriveTimer > warpDriveCooldown)
                {
                    transform.position += Vector3.up * 3.0f;
                    warpDriveTimer = 0;
                }
            }
        }

        if (Input.GetKey(downInput))
        {
            if (transform.position.y > yPosMin)
            {
                transform.Translate(Vector3.down * Time.deltaTime * moveSpeed);
                if (Input.GetKeyDown(warpDrive) && warpDriveTimer > warpDriveCooldown)
                {
                    transform.position += Vector3.down * 3.0f;
                    warpDriveTimer = 0;
                }
            }

        }

        if (Input.GetKey(leftInput))
        {
            if (transform.position.x > xPosMin)
            {
                transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
                if (Input.GetKeyDown(warpDrive) && warpDriveTimer > warpDriveCooldown)
                {
                    transform.position += Vector3.left * 3.0f;
                    warpDriveTimer = 0;
                }
            }
        }

        if (Input.GetKey(rightInput))
        {
            if (transform.position.x < xPosMax)
            {
                transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
                if (Input.GetKeyDown(warpDrive) && warpDriveTimer > warpDriveCooldown)
                {
                    transform.position += Vector3.right * 3.0f;
                    warpDriveTimer = 0;
                }
            }
        }

        if (Input.GetKeyDown(shootPhaser))
        {
            //play phaser SFX when shot is fired
            phaserFireSFX.Play();
            //cycle through different weapon placements as shots are fired
            Instantiate(phaserPrefab, phaserPlacements[phaserRotation].transform.position, phaserPlacements[phaserRotation].transform.rotation);
            phaserRotation++;
            if (phaserRotation == 3)
            {
                phaserRotation = 0;
            }
        }

        if (Input.GetKeyDown(shootPhotonTorpedo) && torpedoTimer >= torpedoCooldown)
        {
            //launch torpedo if not on cooldown and one has not already been fired
            if (isPhotonTorpedoAway == false)
            {
                Instantiate(photonTorpedoPrefab, photonTorpedoPlacement.transform.position, photonTorpedoPlacement.transform.rotation);
                isPhotonTorpedoAway = true;
                torpedoTimer = 0.0f;
            }
            //detonate torpedo in torpedo controller script
            else if (isPhotonTorpedoAway == true)
            {
                isPhotonTorpedoAway = false;
            }
        }

        //use ultimate ability once per life to clear the screen of all hazards
        if (Input.GetKeyDown(screenClearBomb) && screenClearBombUsed == false)
        {
            screenClearBombSFX.Play();
            screenClearBombUsed = true;
            foreach (string tag in enemiesForScreenClear)
            {
                GameObject[] enemiesToDestroy = GameObject.FindGameObjectsWithTag(tag);
                foreach (GameObject enemy in enemiesToDestroy)
                {
                    Instantiate(explosion, enemy.transform.position, enemy.transform.rotation);
                    Destroy(enemy);
                }
            }
        }

        if (Input.GetKey(activateShield) && shieldDestroyed == false)
        {
            shieldActive = true;
            shield.SetActive(true);
        }

        if (Input.GetKeyUp(activateShield))
        {
            shieldActive = false;
            shield.SetActive(false);
        }

        if (currentShield <= 0)
        {
            currentShield = 0;
            shieldDestroyed = true;
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            healthBar.transform.localScale = new Vector3(0, 0, 0);
            Instantiate(explosion, transform.position, transform.rotation);
        }
	}

    void LateUpdate()
    {
        if (currentHealth == 0)
        {
            Destroy(gameObject);
        }
    }

    //damage triggers
    public void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Disruptor" || other.gameObject.tag == "Enemy") && shieldActive == true)
        {
            currentShield -= 10;
            Destroy(other.gameObject);
            Instantiate(explosion, other.gameObject.transform.position, other.gameObject.transform.rotation);
        }

        if (other.gameObject.tag == "Disruptor" && shieldActive == false)
        {
            currentHealth -= 9;
            Destroy(other.gameObject);
            Instantiate(explosion, other.gameObject.transform.position, other.gameObject.transform.rotation);
        }

        if (other.gameObject.tag == "Enemy" && shieldActive == false)
        {
            currentHealth -= 14;
            Destroy(other.gameObject);
            Instantiate(explosion, other.gameObject.transform.position, other.gameObject.transform.rotation);
        }
    }
}
