using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonTorpedo : MonoBehaviour
{
    public KeyCode manualExplosion = KeyCode.Mouse1;

    public float explosionSize = 3.75f;
    public float moveSpeed = 5.0f;

    public bool isExploding = false;

	void Update()
    {
        //move torpedo up before it is triggered to explode
        if (isExploding == false)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }

        //if torpedo hits enemy or is triggered by player, stop moving and increase size
        if (Input.GetKeyDown(manualExplosion))
        {
            isExploding = true;
            transform.localScale = new Vector3(explosionSize, explosionSize, explosionSize);
            Destroy(gameObject, 2.0f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            isExploding = true;
            transform.localScale = new Vector3(explosionSize, explosionSize, explosionSize);
            Destroy(gameObject, 2.5f);
        }
    }
}
