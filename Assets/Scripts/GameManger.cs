using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Unity.AI.Navigation;
using TMPro;

/*
 * Created on: 03/17/2022
 * Created by: William HP.
 * Last Edited on 08/10/2023
 */

public class GameManger : MonoBehaviour
{
    #region PROPERTIES
    [HideInInspector] public List<GameObject> Ghoul = new List<GameObject>();
    [HideInInspector] public List<GameObject> GhoulBoss = new List<GameObject>();
    [HideInInspector] public List<GameObject> GhoulScavenger = new List<GameObject>();
    [HideInInspector] public List<GameObject> GhoulGrotesque = new List<GameObject>();
    [HideInInspector] public List<GameObject> GhoulFestering = new List<GameObject>();
    [HideInInspector] public int Points = 0;
    [HideInInspector] public bool Dead = false;
    public List<GameObject> buildings = new List<GameObject>();
    public Gun[] gun;
    public PlayerController player;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI GameOver;
    public SpawnGhoul SpawnManger;
    public NavMeshSurface GhoulSurface;
    public NavMeshSurface BossSurface;
    public NavMeshSurface FesteringSurface;
    public NavMeshSurface GrotesqueSurface;
    public PlayerController PlayerController;
    public bool RunAway = false;
    private float seconds = 0f;
    private float minutes = 0f;
    private float time = 0f;
    private int buildingTotal = 0;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        buildingTotal = buildings.Count;
        //GhoulSurface.BuildNavMesh();
        StartCoroutine(AltarGame());
        StartCoroutine(Clock());
    }

    // Update is called once per frame
    void Update()
    {
        if (!Dead)
        {
            time += Time.deltaTime;
            minutes = Mathf.FloorToInt(time / 60f);
            seconds = Mathf.FloorToInt(time % 60f);
            if (minutes < 10 && seconds < 10)
            { timer.SetText("  0" + minutes + ":0" + seconds); }
            else if (minutes < 10 && seconds >= 10)
            { timer.SetText("  0" + minutes + ":" + seconds); }
            else if (minutes >= 10 && seconds < 10)
            { timer.SetText("  " + minutes + ":0" + seconds); }
            else
            { timer.SetText("  " + minutes + ":" + seconds); }
            Score.text = "Score: " + Points;
        }
        else
        {
            StopAllCoroutines();
            GameOver.text = "GAME OVER"; 
        }
    }

    public void Save(TextMeshProUGUI name)
    {
        SaveData data = new SaveData();
        data.name = name.text;
        data.Score = Points;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.dataPath + "/Scores.json", json);
    }

    private IEnumerator Clock()
    {
        yield return new WaitForSeconds(150f);
        Points += 10;
        float r = Random.Range(0f, buildings.Count);
        var temp = buildings[(int)Mathf.Round(r)];
        if(temp.activeSelf == true)
        { temp.SetActive(false); }
        else if(temp.activeSelf == false)
        { temp.SetActive(true); }
    }

    private IEnumerator AltarGame()
    {
        while (true)
        {
            #region RAND-NUMB-GEN
            yield return new WaitForSeconds(300f);
            int a = 0;
            int d = 0;
            float r = Random.Range(0f, 1000f);

            /*
             * For every active enemy in the scene, add 1 to the r value.
             * For every inactive enemy in the scene, minus 1 to the r value
            */
            foreach (GameObject mob in Ghoul)
            {
                if (mob.activeSelf == true)
                { a += 1; }
                else
                { d += 1; }
            }
            foreach (GameObject mob in GhoulBoss)
            {
                if (mob.activeSelf == true)
                { a += 1; }
                else
                { d += 1; }
            }
            foreach (GameObject mob in GhoulGrotesque)
            {
                if (mob.activeSelf == true)
                { a += 1; }
                else
                { d += 1; }
            }
            foreach (GameObject mob in GhoulFestering)
            {
                if (mob.activeSelf == true)
                { a += 1; }
                else
                { d += 1; }
            }
            foreach (GameObject mob in GhoulScavenger)
            {
                if (mob.activeSelf == true)
                { a += 1; }
                else
                { d += 1; }
            }

            r += a;
            r -= d;
            r = (r % 100) + 1;
            #endregion

            #region RESULTS
            #region PLAYER_BOOST
            /*if (r >= 0 && r <= 9)
            { 
                player.SprintMulti += 1; 
                //Debug.Log("Player Sprint increased");
            }
            else if (r >= 10 && r <= 15)
            {
                for (int i = 0; i < gun.Length; i++)
                { gun[i].damage += 1; } 
                //Debug.Log("Gun does more damage");
            }
            else if (r >= 16 && r <= 25)
            { 
                player.playerSpeed += 1.25f; 
                //Debug.Log("Player Base speed increased"); 
            }*/
            #endregion
            #region GHOUL_BOOST
            /*else*/ if (r >= 26 && r <= 35)
            {
                foreach (GameObject mob in Ghoul)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.HPMax += 1;
                }
                foreach (GameObject mob in GhoulBoss)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.HPMax += 1;
                }
                foreach (GameObject mob in GhoulGrotesque)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.HPMax += 1;
                }
                foreach (GameObject mob in GhoulFestering)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.HPMax += 1;
                }
                foreach (GameObject mob in GhoulScavenger)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.HPMax += 1;
                }
                //Debug.Log("Enemy Max health increased");
            }
            else if (r >= 36 && r <= 40)
            {
                foreach (GameObject mob in Ghoul)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.run += 0.25f;
                }
                foreach (GameObject mob in GhoulBoss)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.run += 0.25f;
                }
                foreach (GameObject mob in GhoulGrotesque)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.run += 0.25f;
                }
                foreach (GameObject mob in GhoulFestering)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.run += 0.25f;
                }
                foreach (GameObject mob in GhoulScavenger)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.run += 0.25f;
                }
                //Debug.Log("Enemy run faster now");
            }
            #endregion
            #region ALTAR_SPAWNER
            else if (r >= 41 && r <= 45)
            {
                SpawnManger.min += 0.5f;
                if (SpawnManger.min >= SpawnManger.max)
                { SpawnManger.min = 0f; }
                Debug.Log("Min. delay increased");
            }
            else if (r >= 46 && r <= 50)
            { 
                SpawnManger.max += 0.5f; 
                //Debug.Log("Longer possible spawn delay"); 
            }
            else if (r >= 51 && r <= 55)
            {
                SpawnManger.max -= 0.5f;
                if (SpawnManger.max <= SpawnManger.min)
                { SpawnManger.max = SpawnManger.min + 5.25f; }
                //Debug.Log("Min. spawn delay increased");
            }
            else if (r >= 56 && r <= 60)
            {
                SpawnManger.min -= 0.5f;
                if (SpawnManger.min <= 0f)
                { SpawnManger.min = 0.75f; }
                //Debug.Log("Max. delay decreasd");
            }
            #endregion
            #region GHOUL_UNBOOST
            else if (r >= 61 && r <= 65)
            {
                foreach (GameObject mob in Ghoul)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.run -= 0.25f;
                    if (temp.run <= temp.walk)
                    { temp.run = temp.walk + 2.25f; }
                }
                foreach (GameObject mob in GhoulBoss)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.run -= 0.25f;
                    if (temp.run <= temp.walk)
                    { temp.run = temp.walk + 2.25f; }
                }
                foreach (GameObject mob in GhoulGrotesque)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.run -= 0.25f;
                    if (temp.run <= temp.walk)
                    { temp.run = temp.walk + 2.25f; }
                }
                foreach (GameObject mob in GhoulFestering)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.run -= 0.25f;
                    if (temp.run <= temp.walk)
                    { temp.run = temp.walk + 2.25f; }
                }
                foreach (GameObject mob in GhoulScavenger)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.run -= 0.25f;
                    if (temp.run <= temp.walk)
                    { temp.run = temp.walk + 2.25f; }
                }
                //Debug.Log("Enemy run speed decreased");
            }
            else if (r >= 66 && r <= 75)
            {
                foreach (GameObject mob in Ghoul)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.HPMax -= 1;
                    if (temp.HPMax <= 0)
                    { temp.HPMax = temp.HP + 2; }
                }
                foreach (GameObject mob in GhoulBoss)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.HPMax -= 1;
                    if (temp.HPMax <= 0)
                    { temp.HPMax = temp.HP + 2; }
                }
                foreach (GameObject mob in GhoulGrotesque)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.HPMax -= 1;
                    if (temp.HPMax <= 0)
                    { temp.HPMax = temp.HP + 2; }
                }
                foreach (GameObject mob in GhoulFestering)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.HPMax -= 1;
                    if (temp.HPMax <= 0)
                    { temp.HPMax = temp.HP + 2; }
                }
                foreach (GameObject mob in GhoulScavenger)
                {
                    var temp = mob.GetComponent<GhoulAI>();
                    temp.HPMax -= 1;
                    if (temp.HPMax <= 0)
                    { temp.HPMax = temp.HP + 2; }
                }
                //Debug.Log("Enemy max HP decreased");
            }
            #endregion
            #region PLAYER_UNBOOST
            /*else if (r >= 76 && r <= 85)
            {
                player.playerSpeed -= 1.25f;
                if (player.playerSpeed <= 0)
                { player.playerSpeed = 3f; }
                //Debug.Log("Player base speed reduced");
            }
            else if (r >= 86 && r <= 90)
            {
                for (int i = 0; i < gun.Length; i++)
                {
                    gun[i].damage -= 1;
                    if (gun[i].damage <= 0)
                    { gun[i].damage = 2; }
                }
                //Debug.Log("Gun damage decreased");
            }
            else if (r >= 91 && r <= 95)
            {
                player.SprintMulti -= 1;
                if (player.SprintMulti <= 0)
                { player.SprintMulti = player.playerSpeed + 3; }
                //Debug.Log("Player spring decreased");
            }*/
            #endregion
            /*else if(r >= 96 && r <= 100)
            {
                int rb = Random.Range(0, buildingTotal);
                GameObject temp = buildings[rb];
                if (temp.activeSelf == true)
                {
                    temp.SetActive(false);
                    GhoulSurface.BuildNavMesh();
                    //BossSurface.BuildNavMesh();
                    FesteringSurface.BuildNavMesh();
                    GrotesqueSurface.BuildNavMesh();
                }
                else if (temp.activeSelf == false)
                {
                    temp.SetActive(true);
                    GhoulSurface.BuildNavMesh();
                    //BossSurface.BuildNavMesh();
                    FesteringSurface.BuildNavMesh();
                    GrotesqueSurface.BuildNavMesh();
                }
            }*/
            #endregion
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public int Score;
        public string name;
    }
}
