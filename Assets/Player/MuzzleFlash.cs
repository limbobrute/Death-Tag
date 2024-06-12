using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public ParticleSystem muzzleFlash;
    public Light pointLight;

    // Update is called once per frame
    void Update()
    {
        if (muzzleFlash.isPlaying)
        { pointLight.enabled = true; }
        else
        { pointLight.enabled = false; }
    }
}
