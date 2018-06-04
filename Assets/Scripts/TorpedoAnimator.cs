using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoAnimator : MonoBehaviour
{
    //animate torpedos by cycling through array of images

    private Material mat;

    public Texture2D[] frames;

    public float animateSpeed = 10.0f;

    void Awake()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        int index = (int)(Time.time * animateSpeed);
        index = index % frames.Length;
        mat.mainTexture = frames[index];
    }
}