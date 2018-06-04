using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingField : MonoBehaviour
{
    //ensure enemies and projectiles are destroyed when they leave the playing field
    void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
