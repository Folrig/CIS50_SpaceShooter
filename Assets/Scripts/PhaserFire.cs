using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaserFire : MonoBehaviour
{
    public float moveSpeed = 10.0f;

	void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
	}
}
