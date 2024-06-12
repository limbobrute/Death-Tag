using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerGenerator : MonoBehaviour
{
    public GameObject Generator;
    public GameObject cam;
    public int GeneratorTimer = 250;
    List<GameObject> NormalLight = new List<GameObject>();
    List<GameObject> EMLight = new List<GameObject>();
    public GameObject InputText;
    public bool LightsOn = false;

    private void Start()
    {
        var lights = GameObject.FindGameObjectsWithTag("NormalLight");
        foreach(GameObject obj in lights)
        { NormalLight.Add(obj); obj.SetActive(false); }
        var EMlights = GameObject.FindGameObjectsWithTag("RedLight");
        foreach(GameObject obj in EMlights)
        { EMLight.Add(obj); }
    }

    private void Update()
    {
        if (!LightsOn)
        {
            RaycastHit hit;
            Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 5f);
            if (hit.transform.gameObject == Generator)
            {
                InputText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    LightsOn = true;
                    foreach (GameObject obj in EMLight)
                    {
                        if (obj.name == "Capsule")
                        { obj.GetComponent<MeshRenderer>().material.DisableKeyword("emission"); }
                        else if (obj.name == "Point Light")
                        { obj.SetActive(false); }
                    }
                    foreach (GameObject obj in NormalLight)
                    {
                        obj.SetActive(true);
                    }
                    StartCoroutine(ShortedGenerator());
                }
            }
            else
            { InputText.SetActive(false); }
        }
        else
        { InputText.SetActive(false); }
    }

    IEnumerator ShortedGenerator()
    {
        yield return new WaitForSeconds(GeneratorTimer);
        foreach (GameObject obj in EMLight)
        {
            if (obj.name == "Capsule")
            { obj.GetComponent<MeshRenderer>().material.EnableKeyword("emission"); }
            else if (obj.name == "Point Light")
            { obj.SetActive(true); }
        }
        foreach (GameObject obj in NormalLight)
        {
            obj.SetActive(false);
        }
        LightsOn = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = cam.transform.TransformDirection(Vector3.forward) * 5f;
        Gizmos.DrawRay(cam.transform.position, direction);
    }
}
