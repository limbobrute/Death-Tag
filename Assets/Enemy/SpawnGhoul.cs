using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGhoul : MonoBehaviour
{
    private GameObject[] Spawner;
    private GameObject temp;
    public float delay = 10f;
    private float t;
    private int i;
    public float min = 0.5f;
    public float max = 5.5f;

    // Start is called before the first frame update
    void Start()
    { Spawner = GameObject.FindGameObjectsWithTag("Spawner"); }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t >= delay)
        {
            t = 0f;
            i = Random.Range(0, Spawner.Length-1);
            temp = Spawner[i];
            //temp = Spawner[3];
            float r = Random.Range(0f, 100f);
            //float r = 60;
            /*if (r >= 0 && r <= 4)
            { temp.GetComponent<objectPooling>().SpawnFromPool("BossGhoul", temp.transform.position, Quaternion.identity); }*/
            if (r > 4 && r <= 19)
            { temp.GetComponent<objectPooling>().SpawnFromPool("GrotesqueGhoul", temp.transform.position, Quaternion.identity); }
            else if (r > 19 && r <= 49)
            { temp.GetComponent<objectPooling>().SpawnFromPool("ScavenerGhoul", temp.transform.position, Quaternion.identity); }
            else if (r > 49 && r <= 59)
            { temp.GetComponent<objectPooling>().SpawnFromPool("FesteringGhoul", temp.transform.position, Quaternion.identity); }
            else
            { temp.GetComponent<objectPooling>().SpawnFromPool("Ghoul", temp.transform.position, Quaternion.identity); }
            delay = Random.Range(min, max);
        }
    }
}
