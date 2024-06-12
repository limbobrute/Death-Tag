using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour
{
    private Material EmissiveLight;
    private Light PointLight;
    private Color color;
    public float loop = 0.0f;
    private bool dimLight = true;
    private float intensity = 0.05f;
    public float PointLightIN = 2.0f;
    // Start is called before the first frame update
    void Awake()
    {
        EmissiveLight = GetComponent<Renderer>().material;
        PointLight = GetComponentInChildren<Light>();
        color = GetComponent<Renderer>().material.color;
    }

    private void Start()
    {
        StartCoroutine(Light());
    }


    private IEnumerator Light()
    {
        while(true)
        {
            yield return new WaitForSeconds(loop);
            if(dimLight == true)
            {
                PointLight.intensity -= 0.05f;
                intensity -= 0.005f;
                if(intensity <= 0.0f)
                { intensity = 0.0f; }
                EmissiveLight.SetVector("_EmissionColor", color * intensity);
                if(PointLight.intensity <= 0.0f)
                { dimLight = false; }
            }
            else if(dimLight == false)
            {
                PointLight.intensity += 0.05f;
                intensity += 0.005f;
                EmissiveLight.SetVector("_EmissionColor", color * intensity);
                if (PointLight.intensity >= PointLightIN)
                { dimLight = true; }
            }
        }
    }
}
