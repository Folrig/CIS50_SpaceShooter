using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefenseStationRotate : MonoBehaviour
{
    public int rotateDirection;

    public float rotateSpeed = 75.0f;

	void Start()
    {
        //randomly choose clockwise or counterclockwise direction for enemy to spin
        rotateDirection = Random.Range(0, 2);
	}
	
	void Update()
    {
        if (rotateDirection == 0)
        {
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        }

        if (rotateDirection == 1)
        {
            transform.Rotate(Vector3.back, rotateSpeed * Time.deltaTime);
        }
	}
}
