using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    public float destroyDelay = 1.5f;

	void Start()
    {
        //cause non-moving instantiated objects such as explosions to be removed from memory
        Destroy(gameObject, destroyDelay);
	}
}
