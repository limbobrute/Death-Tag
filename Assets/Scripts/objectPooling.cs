using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Created by William HP.
 Lasted Edited: March 19th, 2022
 Followed  tutorial at the following link:https://www.youtube.com/watch?v=tdSmKaJvCoA&t=1s
 There has been one change from the to, which is the if statement at line 59.
*/

public class objectPooling : MonoBehaviour
{
    public GameManger gameManger;
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static objectPooling Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion 

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();/*Create queue for each object*/

            for(int i = 0; i < pool.size; i++)/*Add objects for each given pool*/
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.position = transform.position;
                objectPool.Enqueue(obj);
                /*Give the Game Manger a refernce to all ghouls, regardless of weather they are active or not in the scene*/
                if (obj.name == "Ghoul(Clone)")
                { gameManger.Ghoul.Add(obj); }
                else if (obj.name == "ghoul_boss(Clone)")
                { gameManger.GhoulBoss.Add(obj); }
                else if (obj.name == "ghoul_festering(Clone)")
                { gameManger.GhoulFestering.Add(obj); }
                else if (obj.name == "ghoul_grotesque(Clone)")
                { gameManger.GhoulGrotesque.Add(obj); }
                else if (obj.name == "ghoul_scavenger(Clone)")
                { gameManger.GhoulScavenger.Add(obj); }
            }

            poolDictionary.Add(pool.tag, objectPool);//Add pool to the dictionary
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))//Check to see if the given pool exist
        { Debug.LogWarning("No pool with tag" + tag + "exist"); return null; }
        GameObject SpawnThis = poolDictionary[tag].Dequeue();//Get the specific pool that objects are to be spawned in from

        if (SpawnThis.activeSelf)//Check to see if all objects are already active in the scene
        { 
            Debug.Log("This creature is at it's max!!");
            poolDictionary[tag].Enqueue(SpawnThis);//Return the object to the queue so it won't be emptied
        }

        else
        {
            /*Grab the first object in the queue and bring it into the scene*/
            SpawnThis.SetActive(true);
            SpawnThis.transform.position = position;
            SpawnThis.transform.rotation = rotation;

            poolDictionary[tag].Enqueue(SpawnThis);/*This puts the spawned object back into the queue for later*/
        }
        return SpawnThis;//This is here so that the spawned objects can be 'retrived' later
    }
}
